using BudgetWise.Core.Domain.Abstractions.Services;
using BudgetWise.Core.Domain.Model.Profiles;
using BudgetWise.Core.Domain.Model.Profiles.Entities;
using BudgetWise.Core.Domain.Model.Summaries;
using BudgetWise.Core.Domain.Model.Summaries.ValueObjects;
using BudgetWise.Core.Domain.Model.Transactions.ValueObjects;
using BudgetWise.Core.Persistence;
using BudgetWise.Core.Specifications;

namespace BudgetWise.Core.Domain.Services;

/// <summary>
/// This internal implementation of <see cref="ICalculationService"/> is used
/// because only it should know about <see cref="Profile"/> implementation
/// details and how to deal with them. 
/// </summary>
internal class CalculationService : ICalculationService
{
	private readonly IProfileRepository _profileRepository;
	private readonly ITransactionRepository _transactionRepository;
	private readonly ICategoryRepository _categoryRepository;
	
	public CalculationService(
		IProfileRepository profileRepository, 
		ITransactionRepository transactionRepository, 
		ICategoryRepository categoryRepository)
	{
		_profileRepository = profileRepository;
		_transactionRepository = transactionRepository;
		_categoryRepository = categoryRepository;
	}

	/// <inheritdoc />
	public async Task<Profile?> GetCurrentProfile()
	{
		var profile = await _profileRepository.GetCurrentProfile();

		if (profile is null)
		{
			return null;
		}
		
		return await PopulateAndProcessProfile(profile);
	}

	private async Task<Profile> PopulateAndProcessProfile(Profile profile)
	{
		var savingTransactions = await _transactionRepository.GetFiltered(
			new TransactionsSpecification
			{
				ProfileId = profile.Id,
				Destination = TransactionDestination.SavingsBalance,
				ToDate = profile.BillingPeriod.DateFrom,
				IsMultiCurrency = true
			});
		
		var withdrawTransactions = await _transactionRepository.GetFiltered(
			new TransactionsSpecification
			{
				ProfileId = profile.Id,
				Destination = TransactionDestination.ProfileBalance,
				ToDate = profile.BillingPeriod.DateFrom,
				IsMultiCurrency = true
			});
		
		var transactions = await _transactionRepository.GetForPeriod(
			profile.Id, 
			profile.BillingPeriod.DateFrom,
			profile.BillingPeriod.DateTo);
		
		transactions = transactions
			.Concat(savingTransactions)
			.Concat(withdrawTransactions)
			.ToList();
		
		var categories = await _categoryRepository.GetAllByProfileId(profile.Id);

		if (categories.Count > 0)
		{
			var profileCategories = categories.Select(c => new ProfileCategory(c.Id)
			{
				Name = c.Name,
				ActualAmount = 0,
				PlannedAmount = c.PlannedAmount
			});
			
			profile.AddCategories(profileCategories);
		}
		
		var currentDate = DateTime.Now;
		
		profile.HandleTransactions(transactions, currentDate);

		if (!profile.IsNewPeriod)
		{
			return profile;
		}
		
		var updatedProfile = await _profileRepository.Update(profile);
		
		// Supposed to be executed a maximum of 2 times
		return await PopulateAndProcessProfile(updatedProfile);
	}

	/// <inheritdoc />
	public async Task<Summary> GetSummaryForPeriod(SummaryCalculationType calcType)
	{
		var profile = await _profileRepository.GetCurrentProfile();

		if (profile is null)
		{
			throw new InvalidOperationException("Current profile was not found");
		}
		
		var currentDate = DateTime.Now;
		var dateTo = new DateTime(
			currentDate.Year, 
			currentDate.Month, 
			day: DateTime.DaysInMonth(
				currentDate.Year, 
				currentDate.Month));

		var dateFrom = calcType switch
		{
			SummaryCalculationType.ForMonth => new DateTime(currentDate.Year, currentDate.Month, 1),
			SummaryCalculationType.ForThreeMonths => new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-3),
			_ => new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-6) 
		};
		
		var categories = await _categoryRepository.GetAllByProfileId(profile.Id);
		var transactions = await _transactionRepository.GetForPeriod(profile.Id, dateFrom, dateTo);
		var summary = new Summary(transactions, categories, calcType, dateFrom, dateTo);
		
		summary.CalculateSummary();
		
		return summary;
	}
}
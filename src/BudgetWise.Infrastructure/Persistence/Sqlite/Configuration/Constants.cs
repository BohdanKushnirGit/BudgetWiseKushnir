namespace BudgetWise.Infrastructure.Persistence.Sqlite.Configuration;

internal class Constants
{
	public const string DatabaseFilename = "BudgetWiseDb.db3";
	public const SQLite.SQLiteOpenFlags Flags =
		SQLite.SQLiteOpenFlags.ReadWrite |
		SQLite.SQLiteOpenFlags.Create |
		SQLite.SQLiteOpenFlags.SharedCache;
}
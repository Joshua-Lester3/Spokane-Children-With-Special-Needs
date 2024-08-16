using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SpokaneChildren.Api.Data;

namespace SpokaneChildren.Api.Tests;

public abstract class DatabaseTestBase
{
	private SqliteConnection SqliteConnection { get; set; } = null!;
	protected DbContextOptions<AppDbContext> Options { get; private set; } = null!;

	[TestInitialize]
	public void InitializeDb()
	{
		SqliteConnection = new SqliteConnection("DataSource=:memory:");
		SqliteConnection.Open();

		Options = new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlite(SqliteConnection)
			.Options;

		using var context = new AppDbContext(Options);
		context.Database.EnsureCreated();
	}

	[TestCleanup]
	public void CloseDbConnection()
	{
		SqliteConnection.Close();
	}
}
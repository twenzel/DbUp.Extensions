using System.Reflection;
using DbUp;

var connectionString = "Data Source=localhost;Initial Catalog=DbUpTest;Integrated Security=SSPI;TrustServerCertificate=true";

// make sure DB exists
EnsureDatabase.For.SqlDatabase(connectionString);

var upgrader =
	DeployChanges.To
		.SqlDatabase(connectionString)
		.WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
		.JournalToSqlWithHashing("dbo", "DbChangeLog")
		.LogToConsole()
		.Build();

var result = upgrader.PerformUpgrade();

if (!result.Successful)
{
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine(result.Error);
	Console.ResetColor();

	return;
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Success!");
Console.ResetColor();

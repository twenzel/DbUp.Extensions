# DbUp.Extensions

Extensions for [DbUp](https://github.com/DbUp/DbUp).

## Usage

Install the NuGet package `DbUp.Extensions` and use the `JournalToSqlWithHashing` builder method.

```CSharp
var upgrader =
	DeployChanges.To
		.SqlDatabase(connectionString)
		.WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
		.JournalToSqlWithHashing("dbo", "DbChangeLog")
		.LogToConsole()
		.Build();

```

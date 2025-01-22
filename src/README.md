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

### Reading Liquibase changelog files

The `WithLiquibaseScriptsFromFileSystem` extension method can be used to read Liquibase changelog files.
```CSharp
var upgrader =
	DeployChanges.To
		.SqlDatabase(connectionString)
		.WithLiquibaseScriptsFromFileSystem("./dbFiles/MasterChangelog.xml")
		.LogToConsole()
		.Build();

```

This extension reads the [Liquibase changelog](https://docs.liquibase.com/concepts/changelogs/home.html) xml files and converts them to SQL scripts. The SQL scripts are then executed by DbUp.
Following tags are supported:
- include (for nested changelogs)
- sql (for inline script)
- sqlFile (for script files)

Contexts are also supported. Define your contexts using a `WithLiquibaseScriptsFromFileSystem` overload.
```CSharp
var upgrader =
	DeployChanges.To
		.SqlDatabase(connectionString)
		.WithLiquibaseScriptsFromFileSystem("./dbFiles/MasterChangelog.xml", "release", "test")
		.LogToConsole()
		.Build();

```

The "splitStatements" attribute is also supported. If set to true, the SQL script will be split into individual statements using the LiquibaseScriptOptions.SplitTerminators (defaults `[';', 'GO']`).
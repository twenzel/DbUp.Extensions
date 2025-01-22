# DbUp.Extensions

![Build](https://github.com/twenzel/DbUp.Extensions/actions/workflows/build.yml/badge.svg?branch=main)
[![NuGet Version](http://img.shields.io/nuget/v/DbUp.Extensions.svg?style=flat)](https://www.nuget.org/packages/DbUp.Extensions/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=twenzel_DbUp.Extensions&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=twenzel_DbUp.Extensions)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=twenzel_DbUp.Extensions&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=twenzel_DbUp.Extensions)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=twenzel_DbUp.Extensions&metric=security_rating)](https://sonarcloud.io/dashboard?id=twenzel_DbUp.Extensions)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=twenzel_DbUp.Extensions&metric=bugs)](https://sonarcloud.io/dashboard?id=twenzel_DbUp.Extensions)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=twenzel_DbUp.Extensions&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=twenzel_DbUp.Extensions)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=twenzel_DbUp.Extensions&metric=coverage)](https://sonarcloud.io/dashboard?id=twenzel_DbUp.Extensions)

Extensions for [DbUp](https://github.com/DbUp/DbUp).

## Usage

Install the NuGet package `DbUp.Extensions`.

### Journaling to SQL Server with hashing

Use the `JournalToSqlWithHashing` builder method to use [journaling](https://dbup.readthedocs.io/en/latest/more-info/journaling/) to a SQL Server table with hashes of the scripts instead of just the name.

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
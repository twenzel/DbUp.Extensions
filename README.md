# DbUp.Extensions

![Build](https://github.com/twenzel/DbUp.Extensions/actions/workflows/build.yml/badge.svg?branch=main)
[![NuGet Version](http://img.shields.io/nuget/v/DbUp.Extensions.svg?style=flat)](https://www.nuget.org/packages/DbUp.Extensions/)
[![License](https://img.shields.io/badge/license-APACHE-blue.svg)](LICENSE)

[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=twenzel_DbUp.Extensions&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=twenzel_DbUp.Extensions)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=twenzel_DbUp.Extensions&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=twenzel_DbUp.Extensions)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=twenzel_DbUp.Extensions&metric=security_rating)](https://sonarcloud.io/dashboard?id=twenzel_DbUp.Extensions)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=twenzel_DbUp.Extensions&metric=bugs)](https://sonarcloud.io/dashboard?id=twenzel_DbUp.Extensions)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=twenzel_DbUp.Extensions&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=twenzel_DbUp.Extensions)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=twenzel_DbUp.Extensions&metric=coverage)](https://sonarcloud.io/dashboard?id=twenzel_DbUp.Extensions)

Extensions for DbUp

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
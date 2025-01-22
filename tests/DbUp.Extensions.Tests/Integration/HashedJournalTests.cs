using DbUp.Engine;
using Shouldly;

namespace DbUp.Extensions.Tests.Integration;

internal class HashedJournalTests
{
	[Test]
	public void RunsScripts()
	{
		var store = new InMemoryStore();
		var deployer = DeployChanges.To
				.InMemoryDatabase(store)
				.WithHashing()
				.WithScripts(
					new SqlScript("script1", @"
                            CREATE TABLE Car (Make string)
                        "),
					new SqlScript("script2", @"
                            INERT INTO Car (Make) VALUES ('BMW')
                        "))
				.Build();

		deployer.PerformUpgrade().Successful.ShouldBeTrue();

		var schemaVersions = store.GetSchemaVersions();
		schemaVersions.Count.ShouldBe(2);

		var scripts = schemaVersions.Select(s => s.ScriptName).ToList();
		scripts.ShouldBe(["script1", "script2"]);
	}

	[Test]
	public void DoNotRerunScripts()
	{
		var store = new InMemoryStore();
		var deployer = DeployChanges.To
				.InMemoryDatabase(store)
				.WithHashing()
				.WithScripts(
					new SqlScript("script1", @"
                            CREATE TABLE Car (Make string)
                        "),
					new SqlScript("script2", @"
                            INERT INTO Car (Make) VALUES ('BMW')
                        "))
				.Build();

		deployer.PerformUpgrade().Successful.ShouldBeTrue();
		store.GetExecutedScripts().Count.ShouldBe(2);

		store.ClearExecutedScripts();

		deployer = DeployChanges.To
				.InMemoryDatabase(store)
				.WithHashing()
				.WithScripts(
					new SqlScript("script1", @"
                            CREATE TABLE Car (Make string)
                        "),
					new SqlScript("script2", @"
                            INERT INTO Car (Make) VALUES ('BMW')
                        "))
				.Build();

		deployer.PerformUpgrade().Successful.ShouldBeTrue();

		// no scripts should be executed
		store.GetExecutedScripts().Count.ShouldBe(0);
	}

	[Test]
	public void UpgradeFailsOnChangedScript()
	{
		var store = new InMemoryStore();
		var deployer = DeployChanges.To
				.InMemoryDatabase(store)
				.WithHashing()
				.WithScripts(
					new SqlScript("script1", @"
                            CREATE TABLE Car (Make string)
                        "),
					new SqlScript("script2", @"
                            INERT INTO Car (Make) VALUES ('BMW')
                        "))
				.Build();

		deployer.PerformUpgrade().Successful.ShouldBeTrue();
		store.GetExecutedScripts().Count.ShouldBe(2);

		store.ClearExecutedScripts();

		deployer = DeployChanges.To
				.InMemoryDatabase(store)
				.WithHashing()
				.WithScripts(
					new SqlScript("script1", @"
                            CREATE TABLE Car (Make string) -- somthing new
                        "),
					new SqlScript("script2", @"
                            INERT INTO Car (Make) VALUES ('BMW')
                        "))
				.Build();

		deployer.PerformUpgrade().Successful.ShouldBeFalse();

		// no scripts should be executed
		store.GetExecutedScripts().Count.ShouldBe(0);
	}
}

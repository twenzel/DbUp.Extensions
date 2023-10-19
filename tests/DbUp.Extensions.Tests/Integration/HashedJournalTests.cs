using DbUp.Engine;
using FluentAssertions;

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

		deployer.PerformUpgrade().Successful.Should().BeTrue();

		var schemaVersions = store.GetSchemaVersions();
		schemaVersions.Should().HaveCount(2);
		schemaVersions.Select(s => s.ScriptName).Should().ContainInOrder("script1", "script2");
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

		deployer.PerformUpgrade().Successful.Should().BeTrue();
		store.GetExecutedScripts().Should().HaveCount(2);

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

		deployer.PerformUpgrade().Successful.Should().BeTrue();

		// no scripts should be executed
		store.GetExecutedScripts().Should().HaveCount(0);
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

		deployer.PerformUpgrade().Successful.Should().BeTrue();
		store.GetExecutedScripts().Should().HaveCount(2);

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

		deployer.PerformUpgrade().Successful.Should().BeFalse();

		// no scripts should be executed
		store.GetExecutedScripts().Should().HaveCount(0);
	}
}

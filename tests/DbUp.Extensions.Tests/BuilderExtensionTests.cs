using DbUp.Extensions.Journal;
using DbUp.Extensions.Tests.Integration;
using Shouldly;

namespace DbUp.Extensions.Tests;

public class BuilderExtensionTests
{
	public class JournalToSqlWithHashingMethod : BuilderExtensionTests
	{
		[Test]
		public void AddsJournal()
		{
			var builder = DeployChanges.To
				.InMemoryDatabase(new InMemoryStore())
				.JournalToSqlWithHashing()
				.WithScript("testScript", "content");

			var config = builder.BuildConfiguration();
			config.Journal.ShouldBeOfType<SqlHashingJournal>();
			config.ScriptFilter.ShouldNotBeNull();
			config.ScriptFilter.GetType().Name.ShouldBe("HashedSqlScriptFilter");
		}
	}
}

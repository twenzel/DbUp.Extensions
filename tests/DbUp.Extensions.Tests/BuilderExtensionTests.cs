using DbUp.Extensions.Journal;
using DbUp.Extensions.Tests.Integration;
using FluentAssertions;

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
			config.Journal.Should().BeOfType<SqlHashingJournal>();
			config.ScriptFilter.Should().NotBeNull();
			config.ScriptFilter.GetType().Name.Should().Be("HashedSqlScriptFilter");
		}
	}
}

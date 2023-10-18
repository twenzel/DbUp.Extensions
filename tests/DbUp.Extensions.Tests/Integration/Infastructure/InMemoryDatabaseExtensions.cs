using DbUp.Builder;
using DbUp.Extensions.Tests.Integration.Infastructure;

namespace DbUp.Extensions.Tests.Integration;
internal static class InMemoryDatabaseExtensions
{
	public static UpgradeEngineBuilder InMemoryDatabase(this SupportedDatabases supported, InMemoryStore store)
	{
		var builder = new UpgradeEngineBuilder();
		builder.Configure(c => c.ConnectionManager = new InMemoryConnectionManager());
		builder.Configure(c => c.Journal = new InMemoryHashingJournal(store, () => c.ConnectionManager, () => c.Log));
		builder.Configure(c => c.ScriptExecutor = new InMemoryScriptExecutor(store, () => c.ConnectionManager, () => c.Log, null,
			() => c.VariablesEnabled, c.ScriptPreprocessors, () => c.Journal));

		return builder;
	}
}

using DbUp.Engine;
using DbUp.Engine.Output;
using DbUp.Engine.Transactions;
using DbUp.SqlServer;
using DbUp.Support;

namespace DbUp.Extensions.Tests.Integration.Infastructure;
internal class InMemoryScriptExecutor : ScriptExecutor
{
	private readonly InMemoryStore _store;

	public InMemoryScriptExecutor(InMemoryStore store, Func<IConnectionManager> connectionManagerFactory, Func<IUpgradeLog> log, string schema, Func<bool> variablesEnabled,
			IEnumerable<IScriptPreprocessor> scriptPreprocessors, Func<IJournal> journalFactory)
			: base(connectionManagerFactory, new SqlServerObjectParser(), log, schema, variablesEnabled, scriptPreprocessors, journalFactory)
	{
		_store = store ?? throw new ArgumentNullException(nameof(store));
	}

	protected override string GetVerifySchemaSql(string schema)
	{
		throw new NotSupportedException();
	}

	protected override void ExecuteCommandsWithinExceptionHandler(int index, SqlScript script, Action executeCallback)
	{
		try
		{
			_store.AddExecutedScript(script);
			executeCallback();
		}
		catch (Exception exception)
		{
			Log().WriteInformation("Exception has occurred in script: '{0}'", script.Name);
			Log().WriteError(exception.ToString());
			throw;
		}
	}
}

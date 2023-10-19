using System.Data;
using DbUp.Engine;
using DbUp.Engine.Output;
using DbUp.Engine.Transactions;
using DbUp.Extensions.Journal;
using DbUp.SqlServer;

namespace DbUp.Extensions.Tests.Integration.Infastructure;

/// <summary>
/// Implements <see cref="DbUp.Engine.IJournal"/> to store hashed script
/// content as well as the usual ScriptName and applied date in memory.
/// </summary>
public class InMemoryHashingJournal : HashingTableJournal, IJournal
{
	private readonly InMemoryStore _store;

	public InMemoryHashingJournal(InMemoryStore store, Func<IConnectionManager> connectionManager, Func<IUpgradeLog> logger)
		: base(connectionManager, logger, new SqlServerObjectParser(), null, "InMemory")
	{
		_store = store ?? throw new ArgumentNullException(nameof(store));
	}

	protected override string CreateSchemaTableSql(string quotedPrimaryKeyName)
	{
		throw new NotImplementedException();
	}

	protected override string GetInsertJournalEntrySql(string scriptName, string applied, string contentHash) => string.Empty;

	public override void StoreExecutedScript(SqlScript script, Func<IDbCommand> dbCommandFactory)
	{
		base.StoreExecutedScript(script, dbCommandFactory);
		_store.StoreScript(HashedSqlScript.FromScript(script));
	}

	protected override string GetJournalEntriesSql()
	{
		throw new NotImplementedException();
	}

	string[] IJournal.GetExecutedScripts()
	{
		return _store.GetSchemaVersions().Select(s => new HashedSqlScript(s.ScriptName ?? string.Empty, s.ContentHash ?? string.Empty).ToString()).ToArray();
	}

	protected override bool DoesTableExist(Func<IDbCommand> dbCommandFactory) => true;
}

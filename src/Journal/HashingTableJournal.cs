using System.Data;
using DbUp.Engine;
using DbUp.Engine.Output;
using DbUp.Engine.Transactions;
using DbUp.Support;

namespace DbUp.Extensions.Journal;

/// <summary>
/// Base class for <see cref="IJournal"/> implementations that store a hash
/// of the script's content. <see cref="SqlHashingJournal"/> is an
/// implementation that supports SQL Server.
/// </summary>
public abstract class HashingTableJournal : TableJournal
{
	protected HashingTableJournal(
		Func<IConnectionManager> connectionManager,
		Func<IUpgradeLog> logger,
		ISqlObjectParser sqlObjectParser,
		string schema,
		string table) : base(connectionManager, logger, sqlObjectParser, schema, table)
	{

	}

	/// <summary>
	/// Sql for inserting a journal entry
	/// </summary>
	/// <param name="scriptName">Name of the script name param (i.e @scriptName)</param>
	/// <param name="applied">Name of the applied param (i.e @applied)</param>
	/// <param name="contentHash">Name of the content hash param (i.e. @contentHash)</param>
	/// <returns></returns>
	protected abstract string GetInsertJournalEntrySql(string @scriptName, string @applied, string contentHash);

	protected override string GetInsertJournalEntrySql(string scriptName, string applied) => throw new NotSupportedException();

	protected override IDbCommand GetInsertScriptCommand(Func<IDbCommand> dbCommandFactory, SqlScript script)
	{
		var name = HashedSqlScript.FromScript(script);

		var command = dbCommandFactory();

		var scriptNameParam = command.CreateParameter();
		scriptNameParam.ParameterName = "scriptName";
		scriptNameParam.Value = name.PlainName;
		command.Parameters.Add(scriptNameParam);

		var appliedParam = command.CreateParameter();
		appliedParam.ParameterName = "applied";
		appliedParam.Value = DateTime.UtcNow;
		command.Parameters.Add(appliedParam);

		var contentHashParam = command.CreateParameter();
		contentHashParam.ParameterName = "contentHash";
		contentHashParam.Value = name.ContentHash;
		command.Parameters.Add(contentHashParam);

		command.CommandText = GetInsertJournalEntrySql("@scriptName", "@applied", "@contentHash");
		command.CommandType = CommandType.Text;
		return command;
	}

	public override void StoreExecutedScript(SqlScript script, Func<IDbCommand> dbCommandFactory)
	{
		EnsureTableExistsAndIsLatestVersion(dbCommandFactory);
		using (var command = GetInsertScriptCommand(dbCommandFactory, script))
		{
			command.ExecuteNonQuery();
		}

		//var name = HashedSqlScript.FromScript(script);

		//db.Execute(GetDeleteScriptSql(), new { scriptName = name.PlainName });

		//db.Execute(GetInsertScriptSql(),
		//	new
		//	{
		//		scriptName = name.PlainName,
		//		contentHash = name.ContentsHash
		//	});

		//base.StoreExecutedScript(script, dbCommandFactory);
	}


}
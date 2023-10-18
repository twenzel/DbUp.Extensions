using DbUp.Engine.Output;
using DbUp.Engine.Transactions;
using DbUp.SqlServer;

namespace DbUp.Extensions.Journal;

/// <summary>
/// Implements <see cref="DbUp.Engine.IJournal"/> to store hashed script
/// content as well as the usual ScriptName and Applied date.
/// </summary>
public class SqlHashingJournal : HashingTableJournal
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SqlHashingJournal"/> class.
	/// </summary>
	/// <param name="connectionManager">The connection manager.</param>
	/// <param name="logger">The log.</param>
	/// <param name="schema">The schema that contains the table.</param>
	/// <param name="table">The table name.</param>
	/// <example>
	/// var journal = new SqlHashingJournal("Server=server;Database=database;Trusted_Connection=True", "dbo", "MyVersionTable");
	/// </example>
	public SqlHashingJournal(Func<IConnectionManager> connectionManager, Func<IUpgradeLog> logger, string schema, string table)
		: base(connectionManager, logger, new SqlServerObjectParser(), schema, table)
	{
	}

	/// <inheritdoc/>
	protected override string CreateSchemaTableSql(string quotedPrimaryKeyName) => $@"CREATE TABLE {FqSchemaTableName} (
                    [Id] int identity(1,1) not null constraint {quotedPrimaryKeyName} primary key,
                    [ScriptName] nvarchar(255) not null,
                    [ContentHash] nvarchar(255) not null,
                    [Applied] datetime not null
                )";

	/// <inheritdoc/>
	protected override string GetInsertJournalEntrySql(string scriptName, string applied, string contentHash) => $@"INSERT INTO {FqSchemaTableName} ([ScriptName], [ContentHash], [Applied]) 
                      VALUES ({@scriptName}, {@contentHash}, {@applied})";

	/// <inheritdoc/>
	protected override string GetJournalEntriesSql() => $"SELECT CONCAT([ScriptName],'#', [ContentHash]) FROM {FqSchemaTableName} ORDER BY [ScriptName]";

}

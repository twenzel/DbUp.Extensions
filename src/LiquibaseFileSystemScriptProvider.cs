using DbUp.Engine;
using DbUp.Engine.Transactions;
using DbUp.Extensions.Liquibase;

namespace DbUp.Extensions;

/// <summary>
/// Script provider reading Liquibase changelog XML files.
/// </summary>
public class LiquibaseFileSystemScriptProvider : IScriptProvider
{
	private readonly string _changeLogFilePath;
	private readonly SqlScriptOptions _sqlScriptOptions;
	private readonly LiquibaseScriptOptions _options;

	public LiquibaseFileSystemScriptProvider(string changelogFilePath)
	   : this(changelogFilePath, new LiquibaseScriptOptions(), new SqlScriptOptions())
	{
	}

	public LiquibaseFileSystemScriptProvider(string changelogFilePath, LiquibaseScriptOptions options, SqlScriptOptions sqlScriptOptions)
	{
		_changeLogFilePath = changelogFilePath;
		_options = options ?? throw new ArgumentNullException("options");
		_sqlScriptOptions = sqlScriptOptions ?? throw new ArgumentNullException("sqlScriptOptions");
	}

	/// <inheritdoc/>
	public IEnumerable<SqlScript> GetScripts(IConnectionManager connectionManager)
	{
		if (!File.Exists(_changeLogFilePath))
			throw new InvalidOperationException($"Changelog file '{_changeLogFilePath}' does not exist.");

		var rootDir = Path.GetDirectoryName(_changeLogFilePath);
		var runGroupOrder = _sqlScriptOptions.RunGroupOrder;
		return DatabaseChangeLogSource.GetScripts(_changeLogFilePath, rootDir, _options, ref runGroupOrder);
	}
}

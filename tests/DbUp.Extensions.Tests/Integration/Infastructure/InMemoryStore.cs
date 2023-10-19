using DbUp.Engine;

namespace DbUp.Extensions.Tests.Integration;

public class InMemoryStore
{
	private readonly List<SchemaVersion> _schemaVersions = new();
	private readonly List<SqlScript> _executedScripts = new();

	public IList<SchemaVersion> GetSchemaVersions() => _schemaVersions;

	public IList<SqlScript> GetExecutedScripts() => _executedScripts;

	public void StoreScript(HashedSqlScript script)
	{
		_schemaVersions.Add(new SchemaVersion
		{
			ScriptName = script.PlainName,
			ContentHash = script.ContentHash,
			Applied = DateTime.UtcNow
		});
	}

	public void AddExecutedScript(SqlScript script)
	{
		_executedScripts.Add(script);
	}

	public void ClearExecutedScripts()
	{
		_executedScripts.Clear();
	}
}

public class SchemaVersion
{
	public string? ScriptName { get; set; }
	public string? ContentHash { get; set; }
	public DateTime Applied { get; set; }
}

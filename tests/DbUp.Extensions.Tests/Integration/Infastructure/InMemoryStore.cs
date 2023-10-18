using DbUp.Engine;

namespace DbUp.Extensions.Tests.Integration;

public class InMemoryStore
{
	private readonly List<HashedSqlScript> _scripts = new();
	private readonly List<SqlScript> _executedScripts = new();

	public IEnumerable<HashedSqlScript> GetHashedScripts() => _scripts;

	public string[] GetExecutedScriptNames()
	{
		return _executedScripts.Select(s => s.Name).ToArray();
	}

	public void StoreScript(HashedSqlScript script)
	{
		_scripts.Add(script);
	}

	public void AddExecutedScript(SqlScript script)
	{
		_executedScripts.Add(script);
	}
}

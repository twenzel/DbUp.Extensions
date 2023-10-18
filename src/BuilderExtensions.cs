using DbUp.Builder;
using DbUp.Engine;
using DbUp.Extensions;
using DbUp.Extensions.Journal;
using DbUp.Support;

namespace DbUp;

public static class BuilderExtensions
{
	/// <summary>
	/// Configures hashing script content and saving them to the journal table.
	/// This means that if a script is not changed, it won't be re-run, but if it is
	/// changed then it will. This avoids the need to treat "run always" and 
	/// "run once" scripts differently
	/// 
	/// A filter is also installed that ensures script names include the hash.
	/// You can optionally provide your own additional filtering.
	/// </summary>
	/// <returns>The to sql with hashing.</returns>
	/// <param name="builder">Builder.</param>
	/// <param name="schema">Schema name.</param>
	/// <param name="table">Table name.</param>
	public static UpgradeEngineBuilder JournalToSqlWithHashing(this UpgradeEngineBuilder builder, string? schema = null, string? table = null)
	{
		builder.Configure(configuration =>
		{
			configuration.Journal = new SqlHashingJournal(() => configuration.ConnectionManager, () => configuration.Log, schema, table);
		});
		return builder.WithHashing();
	}

	/// <summary>
	/// Configures hashing script content.
	/// </summary>
	/// <returns>The to sql with hashing.</returns>
	/// <param name="builder">Builder.</param>
	public static UpgradeEngineBuilder WithHashing(this UpgradeEngineBuilder builder)
	{
		builder.Configure(configuration =>
		{
			configuration.ScriptFilter = new HashedSqlScriptFilter();
		});
		return builder;
	}

	private sealed class HashedSqlScriptFilter : IScriptFilter
	{
		public IEnumerable<SqlScript> Filter(IEnumerable<SqlScript> sorted, HashSet<string> executedScriptNames, ScriptNameComparer comparer)
		{
			return sorted.Where(s => !executedScriptNames.Contains(HashedSqlScript.FromScript(s).ToString()));
		}
	}
}

using DbUp.Builder;
using DbUp.Engine;
using DbUp.Engine.Output;
using DbUp.Extensions;
using DbUp.Extensions.Journal;
using DbUp.Support;

namespace DbUp;

public static partial class BuilderExtensions
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
			configuration.ScriptFilter = new HashedSqlScriptFilter(() => configuration.Log, configuration.ScriptFilter);
		});

		return builder;
	}

	private sealed class HashedSqlScriptFilter : IScriptFilter
	{
		private readonly IScriptFilter? _innerFilter;
		private readonly Func<IUpgradeLog> _log;

		public HashedSqlScriptFilter(Func<IUpgradeLog> log, IScriptFilter innerFilter)
		{
			_log = log;
			_innerFilter = innerFilter;
		}

		public IEnumerable<SqlScript> Filter(IEnumerable<SqlScript> sorted, HashSet<string> executedScriptNames, ScriptNameComparer comparer)
		{
			if (_innerFilter != null)
				sorted = _innerFilter.Filter(sorted, executedScriptNames, comparer);

			var executedScripts = executedScriptNames.Select(n => HashedSqlScript.Parse(n)).ToList();

			return sorted.Where(s =>
			{
				var hashedScript = HashedSqlScript.FromScript(s);
				var executedScript = executedScripts.Find(s => s?.PlainName == hashedScript.PlainName);

				// if script was not yes executed -> fine
				if (executedScript == null)
					return true;

				// script was not changed -> fine
				if (executedScript.ContentHash == hashedScript.ContentHash)
					return s.SqlScriptOptions.ScriptType == ScriptType.RunAlways;

				// script content was changed -> not supported
				_log().LogError("Script content of {0} was changed. Original hash: {1}. New {2}", s.Name, executedScript.ContentHash, hashedScript.ContentHash);
				throw new InvalidOperationException($"Script content of {s.Name} was changed.");
			});
		}
	}
}

using DbUp.Builder;
using DbUp.Engine;
using DbUp.Extensions;
using DbUp.Extensions.Liquibase;

namespace DbUp;

public static partial class BuilderExtensions
{
	/// <summary>
	/// Reads Liquibase upgrade scripts from a path on the file system.
	/// </summary>
	/// <param name="builder">The update builder instance.</param>
	/// <param name="changelogFilePath">The path to the main Liquibase changelog file.</param>
	public static UpgradeEngineBuilder WithLiquibaseScriptsFromFileSystem(this UpgradeEngineBuilder builder, string changelogFilePath)
	{
		return builder.WithScripts(new LiquibaseFileSystemScriptProvider(changelogFilePath));
	}

	/// <summary>
	/// Reads Liquibase upgrade scripts from a path on the file system.
	/// </summary>
	/// <param name="builder">The update builder instance.</param>
	/// <param name="changelogFilePath">The path to the main Liquibase changelog file.</param>
	public static UpgradeEngineBuilder WithLiquibaseScriptsFromFileSystem(this UpgradeEngineBuilder builder, string changelogFilePath, params string[] contexts)
	{
		return builder.WithScripts(new LiquibaseFileSystemScriptProvider(changelogFilePath, new Extensions.Liquibase.LiquibaseScriptOptions { Contexts = new List<string>(contexts) }, new SqlScriptOptions()));
	}

	/// <summary>
	/// Reads Liquibase upgrade scripts from a path on the file system.
	/// </summary>
	/// <param name="builder">The update builder instance.</param>
	/// <param name="changelogFilePath">The path to the main Liquibase changelog file.</param>
	public static UpgradeEngineBuilder WithLiquibaseScriptsFromFileSystem(this UpgradeEngineBuilder builder, string changelogFilePath, SqlScriptOptions sqlScriptOptions)
	{
		return builder.WithScripts(new LiquibaseFileSystemScriptProvider(changelogFilePath, new LiquibaseScriptOptions(), sqlScriptOptions));
	}

	/// <summary>
	/// Reads Liquibase upgrade scripts from a path on the file system.
	/// </summary>
	/// <param name="builder">The update builder instance.</param>
	/// <param name="changelogFilePath">The path to the main Liquibase changelog file.</param>
	public static UpgradeEngineBuilder WithLiquibaseScriptsFromFileSystem(this UpgradeEngineBuilder builder, string changelogFilePath, LiquibaseScriptOptions options, SqlScriptOptions sqlScriptOptions)
	{
		return builder.WithScripts(new LiquibaseFileSystemScriptProvider(changelogFilePath, options, sqlScriptOptions));
	}
}

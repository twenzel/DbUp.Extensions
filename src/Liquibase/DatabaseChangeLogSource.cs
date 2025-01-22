using System.Text;
using System.Xml;
using DbUp.Engine;
using DbUp.Support;

namespace DbUp.Extensions.Liquibase;
internal static class DatabaseChangeLogSource
{
	public static List<SqlScript> GetScripts(string filePath, string rootDir, LiquibaseScriptOptions options, ref int runGroupOrder)
	{
		if (!File.Exists(filePath))
			throw new InvalidOperationException($"Changelog file '{filePath}' does not exist.");

		var result = new List<SqlScript>();
		var doc = new XmlDocument();
		doc.Load(filePath);

		var changelogFilePath = Path.GetDirectoryName(filePath) ??
			throw new ArgumentException($"Could not resolve directory name for '{filePath}'", filePath);

		if (doc.DocumentElement == null)
			return result;

		foreach (var node in doc.DocumentElement.ChildNodes)
		{
			if (node is XmlElement element)
			{
				if (string.Equals(element.Name, "include", StringComparison.OrdinalIgnoreCase))
				{
					AddChangelog(rootDir, options, result, changelogFilePath, element, ref runGroupOrder);
				}
				else if (string.Equals(element.Name, "changeset", StringComparison.OrdinalIgnoreCase))
				{
					if (!IsValidContext(element, options))
						continue;

					var id = element.GetAttribute("id");
					var author = element.GetAttribute("author");
					var sqlFiles = element.GetElementsByTagName("sqlFile");

					if (sqlFiles.Count == 1)
					{
						AddSqlFile(rootDir, filePath, options, result, (XmlElement)sqlFiles[0]!, id, author, ref runGroupOrder);
						continue;
					}

					var sqlElements = element.GetElementsByTagName("sql");

					if (sqlElements.Count == 1)
						AddInlineSql(rootDir, filePath, result, sqlElements[0]!.InnerText, id, author, ref runGroupOrder);
				}
			}
		}

		return result;
	}

	private static void AddChangelog(string rootDir, LiquibaseScriptOptions options, List<SqlScript> result, string changelogFilePath, XmlElement element, ref int runGroupOrder)
	{
		var file = FixDirectoryChar(element.GetAttribute("file"));
		var relativeToChangelogFile = element.GetAttribute("relativeToChangelogFile");

		if (relativeToChangelogFile == "true")
			file = Path.Combine(changelogFilePath, file);
		else
			file = Path.Combine(rootDir, file);

		result.AddRange(GetScripts(file, rootDir, options, ref runGroupOrder));
	}

	private static void AddInlineSql(string rootDir, string filePath, List<SqlScript> result, string script, string changeSetId, string author, ref int runGroupOrder)
	{
		var name = GenerateName(rootDir, filePath, changeSetId, author);
		var sqlScriptOptions = new SqlScriptOptions { RunGroupOrder = runGroupOrder++ };

		result.Add(new SqlScript(name, script, sqlScriptOptions));
	}

	private static void AddSqlFile(string rootDir, string changeSetFile, LiquibaseScriptOptions options, List<SqlScript> result, XmlElement sqlFile, string changeSetId, string author, ref int runGroupOrder)
	{
		var path = sqlFile.GetAttribute("path");

		if (string.IsNullOrEmpty(path))
			throw new InvalidOperationException($"Changeset {changeSetId} in {changeSetFile} does not have a valid \"path\" attribute!");

		path = FixDirectoryChar(path);

		var encodingName = sqlFile.GetAttribute("encoding");
		var splitStatements = sqlFile.GetAttribute("splitStatements");
		var runAlways = sqlFile.GetAttribute("runAlways");
		var encoding = string.IsNullOrEmpty(encodingName) ? System.Text.Encoding.UTF8 : System.Text.Encoding.GetEncoding(encodingName);

		var scriptType = runAlways == "true" ? Support.ScriptType.RunAlways : ScriptType.RunOnce;

		if (splitStatements == "true")
			result.AddRange(SplitStatement(rootDir, path, changeSetFile, changeSetId, author, encoding, options, scriptType, ref runGroupOrder));
		else
		{
			var sqlScriptOptions = new SqlScriptOptions { ScriptType = scriptType, RunGroupOrder = runGroupOrder++ };
			var name = GenerateName(rootDir, changeSetFile, changeSetId, author);
			result.Add(new SqlScript(name, File.ReadAllText(Path.Combine(rootDir, path), encoding), sqlScriptOptions));
		}
	}

	private static List<SqlScript> SplitStatement(string rootDir, string path, string changeSetFile, string changeSetId, string author, Encoding encoding, LiquibaseScriptOptions options, Support.ScriptType scriptType, ref int runGroupOrder)
	{
		var filePath = Path.Combine(rootDir, path);
		using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
		using var resourceStreamReader = new StreamReader(fileStream, encoding, true);

		var content = resourceStreamReader.ReadToEnd();
		var scripts = content.Split(options.SplitTerminators.ToArray(), StringSplitOptions.RemoveEmptyEntries);

		var result = new List<SqlScript>();
		var index = 0;
		foreach (var script in scripts)
		{
			var scriptContent = script.Trim();

			if (!string.IsNullOrEmpty(scriptContent))
			{
				var name = GenerateName(rootDir, changeSetFile, changeSetId, author) + $"-{index}";
				var sqlScriptOptions = new SqlScriptOptions { ScriptType = scriptType, RunGroupOrder = runGroupOrder++ };
				result.Add(new SqlScript(name, scriptContent, sqlScriptOptions));
				index++;
			}
		}

		return result;
	}

	private static bool IsValidContext(XmlElement element, LiquibaseScriptOptions options)
	{
		var context = element.GetAttribute("context");

		// no context required -> GO
		if (string.IsNullOrEmpty(context))
			return true;

		// context required but not provided -> NO GO
		if (options.Contexts == null || options.Contexts.Count == 0)
			return false;

		// context required and provided -> GO
		return options.Contexts.Contains(context, StringComparer.OrdinalIgnoreCase);
	}

	private static string GenerateName(string basePath, string path, string identifier, string author)
	{
		var fullPath = Path.GetFullPath(path);
		var fullBasePath = Path.GetFullPath(basePath);

		if (!fullPath.StartsWith(fullBasePath, StringComparison.OrdinalIgnoreCase))
			throw new ArgumentException("The basePath must be a parent of path");

		return fullPath
			.Substring(fullBasePath.Length)
			.Replace('\\', Path.AltDirectorySeparatorChar)
			.Trim(Path.AltDirectorySeparatorChar) + $"-{identifier}-{author}";
	}

	private static string FixDirectoryChar(string path)
	{
		return path.Replace('\\', Path.DirectorySeparatorChar);
	}
}
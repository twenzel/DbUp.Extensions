using System.Security.Cryptography;
using System.Text;
using DbUp.Engine;

namespace DbUp.Extensions;

/// <summary>
/// Defines a format for script names, "name#hash". This allows the DbUp engine
/// to pass through the hash of the script's content.
/// </summary>
public sealed class HashedSqlScript
{
	public string PlainName { get; }
	public string ContentHash { get; }

	public HashedSqlScript(string plainName, string contentHash)
	{
		PlainName = plainName;
		ContentHash = contentHash;
	}

	/// <summary>
	/// Returns the combined string of the name and hash separate by a #.
	/// </summary>
	/// <returns>The combined string.</returns>
	public override string ToString()
	{
		return $"{PlainName}#{ContentHash}";
	}

	/// <summary>
	/// Given a script, returns the parsed name. If the name was already
	/// in name#hash format, checks that the hash was consistent with
	/// the script contents.
	/// </summary>
	/// <returns>The script.</returns>
	/// <param name="script">Script.</param>
	public static HashedSqlScript FromScript(SqlScript script)
	{
		var name = TryParse(script.Name, out var result) ? result!.PlainName : script.Name;
		return new HashedSqlScript(name, GenerateHash(script.Contents));
	}

	/// <summary>
	/// Attempts to split the supplied string into name and hash parts.
	/// The parse will only succeed if the string contains a #.
	/// </summary>
	/// <returns><c>true</c>, if parse was successful, <c>false</c> otherwise.</returns>
	/// <param name="combinedName">Combined name or other string.</param>
	/// <param name="result">Parsed result.</param>
	public static bool TryParse(string combinedName, out HashedSqlScript? result)
	{
		var pos = combinedName.IndexOf('#');
		if (pos == -1)
		{
			result = null;
			return false;
		}

		result = new HashedSqlScript(combinedName.Substring(0, pos), combinedName.Substring(pos + 1));

		return true;
	}

	/// <summary>
	/// Attempts to split the supplied string into name and hash parts.
	/// The parse will only succeed if the string contains a #.
	/// </summary>
	/// <returns><c>HashedSqlScript</c>, if parse was successful, <c>null</c> otherwise.</returns>
	/// <param name="combinedName">Combined name or other string.</param>
	public static HashedSqlScript? Parse(string combinedName)
	{
		if (TryParse(combinedName, out var result))
			return result;

		return null;
	}

	/// <summary>
	/// Returns the SHA256 hash of the supplied content
	/// </summary>
	/// <returns>The hash.</returns>
	/// <param name="content">Content.</param>
	public static string GenerateHash(string content)
	{
#if NETSTANDARD2_0
		using var algorithm = MD5.Create();
		return Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(content)));
#else
		return Convert.ToBase64String(MD5.HashData(Encoding.UTF8.GetBytes(content)));
#endif
	}
}

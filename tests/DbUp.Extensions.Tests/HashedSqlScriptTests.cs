using DbUp.Engine;
using FluentAssertions;

namespace DbUp.Extensions.Tests;

public class HashedSqlScriptTests
{
	[Test]
	public void CombinesParts()
	{
		new HashedSqlScript("name", "hash").ToString().Should().Be("name#hash");
	}

	[Test]
	public void SplitsParts()
	{
		HashedSqlScript.TryParse("name#hash", out var parsed).Should().BeTrue();
		parsed!.PlainName.Should().Be("name");
		parsed.ContentHash.Should().Be("hash");
	}

	[Test]
	public void RejectsNonHashed()
	{
		HashedSqlScript.TryParse("random", out var parsed).Should().BeFalse();
		parsed.Should().BeNull();
	}

	[Test]
	public void AddsHash()
	{
		var hashed = HashedSqlScript.FromScript(new SqlScript("name", "contents"));

		hashed.PlainName.Should().Be("name");
		hashed.ContentHash.Should().Be(HashedSqlScript.GenerateHash("contents"));
	}

	[Test]
	public void CorrectsExistingHash()
	{
		var hashed = HashedSqlScript.FromScript(new SqlScript("name#blah", "contents"));

		hashed.PlainName.Should().Be("name");
		hashed.ContentHash.Should().Be(HashedSqlScript.GenerateHash("contents"));
	}
}

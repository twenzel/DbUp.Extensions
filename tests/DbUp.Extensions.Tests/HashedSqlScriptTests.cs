using DbUp.Engine;
using Shouldly;

namespace DbUp.Extensions.Tests;

public class HashedSqlScriptTests
{
	[Test]
	public void CombinesParts()
	{
		new HashedSqlScript("name", "hash").ToString().ShouldBe("name#hash");
	}

	[Test]
	public void SplitsParts()
	{
		HashedSqlScript.TryParse("name#hash", out var parsed).ShouldBeTrue();
		parsed!.PlainName.ShouldBe("name");
		parsed.ContentHash.ShouldBe("hash");
	}

	[Test]
	public void RejectsNonHashed()
	{
		HashedSqlScript.TryParse("random", out var parsed).ShouldBeFalse();
		parsed.ShouldBeNull();
	}

	[Test]
	public void AddsHash()
	{
		var hashed = HashedSqlScript.FromScript(new SqlScript("name", "contents"));

		hashed.PlainName.ShouldBe("name");
		hashed.ContentHash.ShouldBe(HashedSqlScript.GenerateHash("contents"));
	}

	[Test]
	public void CorrectsExistingHash()
	{
		var hashed = HashedSqlScript.FromScript(new SqlScript("name#blah", "contents"));

		hashed.PlainName.ShouldBe("name");
		hashed.ContentHash.ShouldBe(HashedSqlScript.GenerateHash("contents"));
	}
}

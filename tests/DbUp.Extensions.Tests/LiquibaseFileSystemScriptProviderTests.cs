using DbUp.Engine;
using DbUp.Engine.Transactions;
using DbUp.Extensions.Liquibase;
using DbUp.Extensions.Tests.Integration.Infastructure;
using Shouldly;

namespace DbUp.Extensions.Tests;
public class LiquibaseFileSystemScriptProviderTests
{
	private LiquibaseFileSystemScriptProvider _provider;
	private LiquibaseScriptOptions _options;
	private SqlScriptOptions _sqlScriptOptions;
	private IConnectionManager _connectionManager;

	[SetUp]
	public void Setup()
	{
		_options = new LiquibaseScriptOptions();
		_sqlScriptOptions = new SqlScriptOptions();
		_connectionManager = new InMemoryConnectionManager();
		_provider = new LiquibaseFileSystemScriptProvider(Path.GetFullPath("./dbFiles/MasterChangeLog.xml"), _options, _sqlScriptOptions);
	}

	[Test]
	public void CanGetScripts()
	{
		var scripts = _provider.GetScripts(_connectionManager);
		scripts.Count().ShouldBe(18);
	}

	[Test]
	public void CanGetScripts_With_Context()
	{
		_options.Contexts = ["test"];

		var scripts = _provider.GetScripts(_connectionManager);
		scripts.Count().ShouldBe(19);
	}

	[Test]
	public void Scripts_Have_Correct_Order()
	{
		var scripts = _provider.GetScripts(_connectionManager).ToList();

		scripts[0].Name.ShouldBe("v1.0/Initial/DBSchema.xml-001-init");
		scripts[0].SqlScriptOptions.RunGroupOrder.ShouldBe(_sqlScriptOptions.RunGroupOrder + 0);
		scripts[1].Name.ShouldBe("v1.0/Initial/DBSchema.xml-002-init");
		scripts[1].SqlScriptOptions.RunGroupOrder.ShouldBe(_sqlScriptOptions.RunGroupOrder + 1);
		scripts[2].Name.ShouldBe("v1.0/Initial/InitialChangeLog.xml-001-init");
		scripts[2].SqlScriptOptions.RunGroupOrder.ShouldBe(_sqlScriptOptions.RunGroupOrder + 2);
		scripts[3].Name.ShouldBe("v1.0/Second/changelog.xml-001-me-0");
		scripts[3].SqlScriptOptions.RunGroupOrder.ShouldBe(_sqlScriptOptions.RunGroupOrder + 3);
		scripts[4].Name.ShouldBe("v1.0/Second/changelog.xml-001-me-1");
		scripts[4].SqlScriptOptions.RunGroupOrder.ShouldBe(_sqlScriptOptions.RunGroupOrder + 4);
		scripts[17].Name.ShouldBe("MasterChangeLog.xml-001-fix-test");
		scripts[17].SqlScriptOptions.RunGroupOrder.ShouldBe(_sqlScriptOptions.RunGroupOrder + 17);
	}
}

using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using DbUp.Engine;
using DbUp.Engine.Output;
using DbUp.Engine.Transactions;

namespace DbUp.Extensions.Tests.Integration.Infastructure;
internal partial class InMemoryConnectionManager : IConnectionManager
{
	private bool _operationStarted;

	public TransactionMode TransactionMode { get; set; }
	public bool IsScriptOutputLogged { get; set; }
	public int? ExecutionTimeoutSeconds { get; set; }

	public InMemoryConnectionManager()
	{
		TransactionMode = TransactionMode.NoTransaction;

	}

	public void ExecuteCommandsWithManagedConnection(Action<Func<IDbCommand>> action)
	{
		action(() => CreateCommand());
	}

	public T ExecuteCommandsWithManagedConnection<T>(Func<Func<IDbCommand>, T> actionWithResult)
	{
		return actionWithResult(() => CreateCommand());
	}

	public IDisposable OperationStarting(IUpgradeLog upgradeLog, List<SqlScript> executedScripts)
	{
		if (_operationStarted)
			throw new InvalidOperationException("OperationStarting is meant to be called by DbUp and can only be called once");

		_operationStarted = true;

		return NullDisposable.Instance;
	}

	public IEnumerable<string> SplitScriptIntoCommands(string scriptContents)
	{
		var scriptStatements =
			   SplitCommands().Split(scriptContents)
				   .Select(x => x.Trim())
				   .Where(x => x.Length > 0)
				   .ToArray();

		return scriptStatements;
	}

	public bool TryConnect(IUpgradeLog upgradeLog, out string errorMessage)
	{
		errorMessage = string.Empty;
		return true;
	}

	private static IDbCommand CreateCommand() => new InternalDbCommand();

	private sealed class NullDisposable : IDisposable
	{
		public static NullDisposable Instance => new NullDisposable();

		public void Dispose()
		{
			// Do nothing
		}
	}

	private sealed class InternalDbCommand : IDbCommand
	{
		public string CommandText { get; set; }
		public int CommandTimeout { get; set; }
		public CommandType CommandType { get; set; }
		public IDbConnection? Connection { get; set; }

		public IDataParameterCollection Parameters => new DataParameterCollection();

		public IDbTransaction? Transaction { get; set; }
		public UpdateRowSource UpdatedRowSource { get; set; }

		public void Cancel()
		{
			// do nothing
		}

		public IDbDataParameter CreateParameter()
		{
			// do nothing
			return new DataParameter();
		}

		public void Dispose()
		{
			// do nothing
		}

		public int ExecuteNonQuery()
		{
			// do nothing
			return 0;
		}

		public IDataReader ExecuteReader() => throw new NotSupportedException();

		public IDataReader ExecuteReader(CommandBehavior behavior) => throw new NotSupportedException();

		public object? ExecuteScalar()
		{
			// do nothing
			return null;
		}

		public void Prepare()
		{
			// do nothing
		}
	}

	private sealed class DataParameter : DbParameter
	{
		public override DbType DbType { get; set; }
		public override ParameterDirection Direction { get; set; }
		public override bool IsNullable { get; set; }
		public override string ParameterName { get; set; }
		public override int Size { get; set; }
		public override string SourceColumn { get; set; }
		public override bool SourceColumnNullMapping { get; set; }
		public override object? Value { get; set; }

		public override void ResetDbType()
		{

		}
	}

	private sealed class DataParameterCollection : List<DataParameter>, IDataParameterCollection
	{
		public object this[string parameterName] { get => Find(p => p.ParameterName == parameterName); set => throw new NotImplementedException(); }

		public bool Contains(string parameterName)
		{
			return Exists(p => p.ParameterName == parameterName);
		}

		public int IndexOf(string parameterName)
		{
			var parameter = Find(p => p.ParameterName == parameterName);

			if (parameter == null)
				return -1;

			return IndexOf(parameter);
		}

		public void RemoveAt(string parameterName)
		{
			var parameter = Find(p => p.ParameterName == parameterName);
			if (parameter != null)
				Remove(parameter);
		}
	}

	[GeneratedRegex("^\\s*;\\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline, "de-DE")]
	private static partial Regex SplitCommands();
}
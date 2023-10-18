using System.Data;

namespace DbUp.Extensions.Tests.Integration;

internal static class DatabaseExtensions
{
	public static IDbCommand PrepareCommand(this Func<IDbCommand> commandFactory, string sql, object? args = null)
	{
		var command = commandFactory();
		command.CommandText = sql;

		if (args != null)
		{
			foreach (var prop in args.GetType().GetProperties())
			{
				var param = command.CreateParameter();
				param.ParameterName = prop.Name;
				param.Value = prop.GetValue(args);
				command.Parameters.Add(param);
			}
		}

		return command;
	}

	public static int Execute(this Func<IDbCommand> commandFactory, string sql, object? args = null)
		=> commandFactory.PrepareCommand(sql, args).ExecuteNonQuery();

	public static T? ExecuteScalar<T>(this Func<IDbCommand> commandFactory, string sql, object? args = null)
		=> (T)commandFactory.PrepareCommand(sql, args).ExecuteScalar();

	public static IReadOnlyCollection<T> Query<T>(this Func<IDbCommand> commandFactory, string sql, object? args = null)
	{
		var results = new List<T>();

		using (var reader = commandFactory.PrepareCommand(sql, args).ExecuteReader())
		{
			if (reader.FieldCount == 1)
			{
				while (reader.Read())
					results.Add((T)reader.GetValue(0));
			}
			else
			{
				var ctor = typeof(T).GetConstructor(
					Enumerable.Range(0, reader.FieldCount)
							  .Select(reader.GetFieldType)
							  .ToArray());

				if (ctor == null)
					throw new InvalidOperationException("No suitable constructor found");

				while (reader.Read())
				{
					results.Add((T)ctor.Invoke(Enumerable.Range(0, reader.FieldCount)
														 .Select(reader.GetValue)
														 .ToArray()));
				}
			}
		}

		return results;
	}
}

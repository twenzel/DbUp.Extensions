namespace DbUp.Extensions.Liquibase;

public class LiquibaseScriptOptions
{
	public List<string>? Contexts { get; set; }

	public List<string> SplitTerminators { get; set; } = [";", "\nGO", "\ngo"];
}

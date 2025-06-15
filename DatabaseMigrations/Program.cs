using DbUp;
using DbUp.Engine;

namespace DatabaseMigrations {
	internal class Program {
		static int Main(string[] args) {

			string connectionString = args.FirstOrDefault() ?? "Host=localhost;Database=piano_grades_two;Username=postgres;Password=123";

			EnsureDatabase.For.PostgresqlDatabase(connectionString);

			var upgrader =
				DeployChanges.To
					.PostgresqlDatabase(connectionString)
					.WithScriptsFromFileSystem("Scripts")
					.LogToConsole()
					.Build();

			DatabaseUpgradeResult result = upgrader.PerformUpgrade();

			if (!result.Successful) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(result.Error);
				Console.ResetColor();
#if DEBUG
				Console.ReadLine();
#endif
				return -1;
			}

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Success!");
			Console.ResetColor();
#if DEBUG
			Console.ReadLine();
#endif
			return 0;
		}
	}
}

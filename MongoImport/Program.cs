using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoImport.Column;
using MongoImport.Data;
using Spectre.Console;

namespace MongoImport;

public static class Program
{
	private static readonly MongoDbContext Context = new();

	private static readonly string[] Files = GetFiles();

	private static string DatabaseName { get; set; } = GetDatabaseName(ref Files[0]);


	public static async Task Main(string[] args)
	{
		var cliHeader = new FigletText("Initialisation de la base de données").Centered().Color(Color.Red);
		AnsiConsole.Write(cliHeader);
    Panel filesInfo = new Panel($"[blue]Fichiers : [/]\n{string.Join("\n", Files)}")
      .Header("[green]Fichiers de données[/]")
      .Border(BoxBorder.Rounded)
      .Expand();
		AnsiConsole.Write(filesInfo);

    string response = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title("[green]Que voulez-vous faire?[/]")
				.AddChoices(new[] {"Initialiser mongodb pour le projet.", "Quitter"})

		);
		if (response == "Quitter")
		{
			AnsiConsole.MarkupLine("[red]Initialisation annulée[/]");
			return;
		}

		var spinner = new SpinnerColumn();
		await AnsiConsole.Progress()
			.Columns(new ProgressColumn[]
			{
				new CustomTaskDescColumn(),
				new ProgressBarColumn(),
				new PercentageColumn(),
				new SpinnerColumn()
				{
					Spinner = Spinner.Known.Dots
				}
			}).StartAsync(async ctx =>
			{
				var dropDatabaseTask = ctx.AddTask($"Suppression de la base de données {DatabaseName}",
					new ProgressTaskSettings {MaxValue = 1});
				await DropDatabase(dropDatabaseTask);
				var createCollectionsTask = ctx.AddTask("Création des collections", new ProgressTaskSettings {MaxValue = 3});
				await CreateCollections(createCollectionsTask);
				var importDataTask = ctx.AddTask("Importation des données", new ProgressTaskSettings {MaxValue = 3});
				await ImportData(importDataTask);
			}).ContinueWith(completedTask =>
			{
				var panel = new Panel("[blue]Tâches terminées[/]")
					.Header("[green]Initialisation terminée[/]")
					.Border(BoxBorder.Rounded)
					.Expand();
				AnsiConsole.Write(panel);
				Task.Delay(1500).Wait();
			});
	}

	private static async Task DropDatabase(ProgressTask task)
	{
		try
		{
			await Context.Client.DropDatabaseAsync(DatabaseName);
			task.Increment(1);
		}
		catch (Exception e)
		{
			AnsiConsole.MarkupLine("[red]Erreur lors de la suppression de la base de données[/]");
		}
	}

	private static async Task CreateCollections(ProgressTask task)
	{
		try
		{
			foreach (var file in Files)
			{
        string collectionName = file.Split("\\")[1].Split(".")[1];
        await Context.Client.GetDatabase(DatabaseName).CreateCollectionAsync(collectionName);
        task.Increment(1);
      }
		}
		catch (Exception e)
		{
			AnsiConsole.MarkupLine("[red]Erreur lors de la création des collections[/]");
		}
	}

	private static Task ImportData(ProgressTask task)
	{
		try
		{

			if (Files.Length == 0)
			{
				AnsiConsole.MarkupLine("[red]Aucun fichier de données trouvé[/]");
			}


			Array.ForEach(Files, file =>
			{
				string json = File.ReadAllText(file);

				string collectionName = file.Split("\\")[1].Split(".")[1];
				var collection = Context.Client.GetDatabase(DatabaseName).GetCollection<BsonDocument>(collectionName);
				var documents = Deserialize<BsonDocument>(json);
				collection.InsertMany(documents);

				task.Increment(1);
			});
		}
		catch (FileNotFoundException e)
		{
			AnsiConsole.MarkupLine("[red]Fichiers de données introuvables[/]");
		}
		catch (DirectoryNotFoundException e)
		{
			AnsiConsole.MarkupLine("[red]Dossier de données introuvable[/]");
		}
		catch (Exception e)
		{
			AnsiConsole.MarkupLine("[red]Erreur lors de l'importation des données[/]");
		}

		return Task.CompletedTask;
	}

	private static List<T> Deserialize<T>(string json)
	{
		List<T> list = BsonSerializer.Deserialize<List<T>>(json);
		return list;
	}

	private static string[] GetFiles()
	{
		try
		{
      return Directory.GetFiles("Json", "*.json");
    }
    catch (FileNotFoundException e)
		{
      AnsiConsole.MarkupLine("[red]Fichiers de données introuvables[/]");
      return Array.Empty<string>();
    }
    catch (DirectoryNotFoundException e)
		{
      AnsiConsole.MarkupLine("[red]Dossier de données introuvable[/]");
      return Array.Empty<string>();
    }
    catch (Exception e)
		{
      AnsiConsole.MarkupLine("[red]Erreur lors de la récupération des fichiers de données[/]");
      return Array.Empty<string>();
		}
	}

	private static string GetDatabaseName(ref string file)
	{
		if(file is null)
      return string.Empty;

		return file.Split("\\")[1].Split(".")[0];
  }
}
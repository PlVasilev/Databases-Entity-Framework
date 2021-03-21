namespace VaporStore
{
	using System;
	using System.IO;
	using AutoMapper;
	using Data;
	using DataProcessor;
	using Microsoft.EntityFrameworkCore;
    //1 look at database
    //2 copy all from Model Definition class and copy it the implement the class
    //3 Attributes
    //4 Go true models again
    //5 Compile Solution
    //6 Check in SSMS for relations
    //7 Judje DEL - obj,bin datasets and importResult
    //8 Data Import Deserializer
    //9 add folder ImportDtos - Create ClassDTO - copy From Valid Input (Stings and Numbers(int,decimal ...))
    //10 create Validations!!! [Required] ect. int DTO CLASS
    //11 go to Deserializer implement the Method
    //12 create Private Static bool is Valid(Object entity)
    //13 if (!isValid(gameDto)(//validate Attributes) || gameDto.Tags.Count == 0(//extra validation)) 
    //14 if something is missing create it 
    //15 careful with mapping table
    //16 if we have Class in DTO we create inside it one more DTO
    //17 if (!isValid(gameDto)(//validate Attributes)
    //          || !userDto.Card.All(IsValid) (//all Must Be Valid from Class in ClassDTO)) 
    //18 Serializer (Export) We may use anonimus object in JSON
    public class StartUp
	{
		public static void Main(string[] args)
		{
			var context = new VaporStoreDbContext();

			Mapper.Initialize(config => config.AddProfile<VaporStoreProfile>());

			ResetDatabase(context, shouldDropDatabase: false);

			var projectDir = GetProjectDirectory();

			ImportEntities(context, projectDir + @"Datasets/", projectDir + @"ImportResults/");
			ExportEntities(context, projectDir + @"ImportResults/");

			using (var transaction = context.Database.BeginTransaction())
			{
				BonusTask(context);
				transaction.Rollback();
			}
		}

		private static void BonusTask(VaporStoreDbContext context)
		{
			var bonusOutput = Bonus.UpdateEmail(context, "atobin", "amontobin@gmail.com");
			Console.WriteLine(bonusOutput);
		}

		private static void ExportEntities(VaporStoreDbContext context, string exportDir)
		{
			var jsonOutput = Serializer.ExportGamesByGenres(context, new[] { "Nudity", "Violent" });
			PrintAndExportEntityToFile(jsonOutput, exportDir + "GamesByGenres.json");

			var xmlOutput = Serializer.ExportUserPurchasesByType(context, "Digital");
			PrintAndExportEntityToFile(xmlOutput, exportDir + "UserPurchases.xml");
		}

		private static void ImportEntities(VaporStoreDbContext context, string baseDir, string exportDir)
		{
			var games = Deserializer.ImportGames(context, File.ReadAllText(baseDir + "games.json"));
			PrintAndExportEntityToFile(games, exportDir + "ImportGames.txt");

			var users = Deserializer.ImportUsers(context, File.ReadAllText(baseDir + "users.json"));
			PrintAndExportEntityToFile(users, exportDir + "ImportUsers.txt");

			var purchases = Deserializer.ImportPurchases(context, File.ReadAllText(baseDir + "purchases.xml"));
			PrintAndExportEntityToFile(purchases, exportDir + "ImportPurchases.txt");
		}

		private static void ResetDatabase(DbContext context, bool shouldDropDatabase = false)
		{
			if (shouldDropDatabase)
			{
				context.Database.EnsureDeleted();
			}

			if (context.Database.EnsureCreated())
			{
				return;
			}

			var disableIntegrityChecksQuery = "EXEC sp_MSforeachtable @command1='ALTER TABLE ? NOCHECK CONSTRAINT ALL'";
			context.Database.ExecuteSqlCommand(disableIntegrityChecksQuery);

			var deleteRowsQuery = "EXEC sp_MSforeachtable @command1='SET QUOTED_IDENTIFIER ON;DELETE FROM ?'";
			context.Database.ExecuteSqlCommand(deleteRowsQuery);

			var enableIntegrityChecksQuery =
				"EXEC sp_MSforeachtable @command1='ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'";
			context.Database.ExecuteSqlCommand(enableIntegrityChecksQuery);

			var reseedQuery =
				"EXEC sp_MSforeachtable @command1='IF OBJECT_ID(''?'') IN (SELECT OBJECT_ID FROM SYS.IDENTITY_COLUMNS) DBCC CHECKIDENT(''?'', RESEED, 0)'";
			context.Database.ExecuteSqlCommand(reseedQuery);
		}

		private static void PrintAndExportEntityToFile(string entityOutput, string outputPath)
		{
			Console.WriteLine(entityOutput);
			File.WriteAllText(outputPath, entityOutput.TrimEnd());
		}

		private static string GetProjectDirectory()
		{
			var currentDirectory = Directory.GetCurrentDirectory();
			var directoryName = Path.GetFileName(currentDirectory);
			var relativePath = directoryName.StartsWith("netcoreapp") ? @"../../../" : string.Empty;
			
			return relativePath;
		}
	}
}
using System;
using SQLite;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(novemob.iOS.SQLiteService))]
namespace novemob.iOS
{
	public class SQLiteService : ISQLiteService
	{
		//reconhecer caminho e arquivo do database sqlite
		string GetPath(string databaseName)
		{
			if (string.IsNullOrWhiteSpace(databaseName))
			{
				throw new ArgumentException("Base invalida", nameof(databaseName));
			}

			//Nome do database fisico
			var sqliteFileName = $"{databaseName}.db3";
			//pegar a caminho do documents do ios
			var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			//pegar o caminho do library
			var libraryPath = Path.Combine(documentsPath, "..", "Library");
			//combine path com arquivo fisico
			var path = Path.Combine(libraryPath, sqliteFileName);
			return path;

		}

		public SQLiteService()
		{
		}

		public SQLiteConnection GetConnection(string databaseName)
		{
			return new SQLiteConnection(GetPath(databaseName));
		}

		public long GetSize(string databaseName)
		{
			var fileInfo = new FileInfo(GetPath(databaseName));
			return fileInfo != null ? fileInfo.Length : 0;
		}
	}
}

using System;
using SQLite;

namespace novemob
{
	//Interface para conexão com database SQLite
	public interface ISQLiteService
	{
		SQLiteConnection GetConnection(string databaseName);
		long GetSize(string databaseName);
	}
}

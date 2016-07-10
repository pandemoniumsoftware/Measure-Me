using System;
using SQLite;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeasureMe
{
	public static class SQLiteConnection
	{
		private static SQLiteAsyncConnection _db;

		private static string dbFileName = "MeasureMeDB";

		public static SQLiteAsyncConnection DB
		{

			get
			{
				if (_db == null)
					InitializeDB();
				return _db;
			}
			set
			{
				_db = value;
			}
		}

		private static async void InitializeDB()
		{
			try
			{
				var dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

				dbPath = Path.Combine(dbPath, dbFileName);

				_db = new SQLiteAsyncConnection(dbPath);

				var result = await _db.CreateTableAsync<User>(CreateFlags.None);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Error initializing database: " + ex.ToString());
			}
		}

		public static async Task<User> GetUserData()
		{
			var user = await DB.Table<User>().ToListAsync();

			if (user == null)
				throw new InvalidOperationException("No user data present!");

			return user.Last();
		}

		public static async Task<bool> DoesUserDataExist()
		{
			var userCount = await DB.Table<User>().CountAsync();

			return (userCount >= 1);
		}

		public static async Task CreateOrUpdateObject(object obj)
		{
			await DB.InsertOrReplaceAsync(obj);
		}



	}
}


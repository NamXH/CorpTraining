using System;
using SQLite;
using Java.Lang;

namespace CorpTraining
{
	public class UserDB
	{
		private static string folder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);

		private string dataBasePath = System.IO.Path.Combine (folder, "lessonBasket.db");

		public UserDB ()
		{
			InitDB ();
		}

		private void InitDB(){

			var conn = new SQLiteConnection (dataBasePath);

			conn.CreateTable<UserDao>();
		}

		public void InsertToken (string token){
			if (token == null)
				return;

			var conn = new SQLiteConnection (dataBasePath);

			conn.DeleteAll<UserDao>();

			UserDao userDao = new UserDao (token);

			conn.Insert (userDao);
		}

		public string GetToken (){
			
			var conn = new SQLiteConnection (dataBasePath);

			try{
				UserDao userDao = conn.Table<UserDao>().FirstOrDefault ();

				return userDao.Token;
			}catch (NullReferenceException){
				return null;
			}
		}

		public void DeleteToken(){
			
			var conn = new SQLiteConnection (dataBasePath);

			conn.DeleteAll<UserDao>();
		}
	}
}


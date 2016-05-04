using System;
using SQLite;

namespace CorpTraining
{
	public class UserDao
	{
		[PrimaryKey]
		public int Id{ get; set;}

		public string Token { get; set;}

		public UserDao(){
			
		}

		public UserDao (string token)
		{
			Token = token;
		}
	}
}


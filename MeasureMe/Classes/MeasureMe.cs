using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeasureMe
{
	public static class MeasureMe
	{
		private static bool _UserDataExists;

		public static bool UserDataExists
		{
			get
			{
				UpdateIfUserDataExists();
				return _UserDataExists;
			}
			set
			{
				_UserDataExists = value;
			}
		}

		static async void UpdateIfUserDataExists()
		{
			try
			{
				_UserDataExists = await SQLiteConnection.DoesUserDataExist();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Error retrieving UserDataExists(): " + ex.ToString());
			}
		}

		private static User _CurrentUser;

		public static User CurrentUser
		{

			get
			{
				if (_CurrentUser == null)
					 GetCurrentUser();
				return _CurrentUser;
			}
			set
			{
				_CurrentUser = value;
			}
		}

		static async void GetCurrentUser()
		{
			try
			{
				if (await SQLiteConnection.DoesUserDataExist())
				{
					_CurrentUser = await SQLiteConnection.GetUserData();
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Error retrieving User for MeasureMe: " + ex.ToString());
			}
		}
}
}


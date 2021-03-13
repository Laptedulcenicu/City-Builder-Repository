namespace GameRig.Scripts.Systems.SaveSystem
{
	public static class SaveManager
	{
		public static void Save<T>(string identifier, T value)
		{
			ES3.Save(identifier, value);
		}

		public static T Load<T>(string identifier, T defaultValue)
		{
			return ES3.Load(identifier, defaultValue);
		}

		public static void Delete(string identifier)
		{
			ES3.DeleteKey(identifier);
		}

		public static void DeleteAll()
		{
			ES3.DeleteFile();
		}

		public static bool Exists(string identifier)
		{
			return ES3.KeyExists(identifier);
		}
	}
}
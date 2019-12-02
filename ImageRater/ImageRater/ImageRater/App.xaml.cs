using ImageRater.Model;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImageRater
{
	public partial class App : Application
	{
        static Database database;

        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Model.Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "posts.db3"));
                }
                return database;
            }
        }

        public App()
		{
			InitializeComponent();

			MainPage = new View.MasterPage();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

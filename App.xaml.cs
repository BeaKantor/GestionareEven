using GestionareEven.Data;
using System.IO;
using GestionareEven.Models;
using GestionareEven.Views;

namespace GestionareEven
{
    public partial class App : Application
    {
        private static EventDatabase _database;
        public static User CurrentUser { get; set; } // Add this property

        public static EventDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    // Set the database path
                    string dbPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "GestionareEven.db3");

                    _database = new EventDatabase(dbPath);
                }
                return _database;
            }
        }



        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());

        }
    }
}

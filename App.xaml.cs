using GestionareEven.Data;
using System.IO;

namespace GestionareEven
{
    public partial class App : Application
    {
        private static EventDatabase _database;

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

            MainPage = new NavigationPage(new Views.EventsPage());
        }
    }
}

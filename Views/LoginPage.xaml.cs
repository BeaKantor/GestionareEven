namespace GestionareEven.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        async void OnLoginClicked(object sender, EventArgs e)
        {
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;

            // Fetch the user from the database
            var user = (await App.Database.GetUsersAsync())
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                await DisplayAlert("Error", "Invalid email or password.", "OK");
                return;
            }

            // Set the logged-in user
            App.CurrentUser = user;

            // Navigate to the main events page
            await Navigation.PushAsync(new EventsPage());
        }

        async void OnSignUpClicked(object sender, EventArgs e)
        {
            // Navigate to the signup page
            await Navigation.PushAsync(new SignUpPage());
        }
    }
}

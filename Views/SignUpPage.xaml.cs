using GestionareEven.Models;

namespace GestionareEven.Views
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        async void OnSignUpClicked(object sender, EventArgs e)
        {
            var name = NameEntry.Text;
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "All fields are required.", "OK");
                return;
            }

            var existingUser = (await App.Database.GetUsersAsync()).FirstOrDefault(u => u.Email == email);

            if (existingUser != null)
            {
                await DisplayAlert("Error", "Email is already registered.", "OK");
                return;
            }

            var user = new User
            {
                Name = name,
                Email = email,
                Password = password
            };

            await App.Database.SaveUserAsync(user);
            await DisplayAlert("Success", "Account created successfully.", "OK");
            await Navigation.PopAsync();
        }
    }
}

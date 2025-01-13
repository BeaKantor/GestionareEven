using GestionareEven.Models;

namespace GestionareEven.Views
{
    public partial class AddEditEventPage : ContentPage
    {
        public AddEditEventPage()
        {
            InitializeComponent();
        }

        async void OnSaveClicked(object sender, EventArgs e)
        {
            var evnt = (Event)BindingContext;
            await App.Database.SaveEventAsync(evnt);
            await Navigation.PopAsync();
        }

        async void OnDeleteClicked(object sender, EventArgs e)
        {
            var evnt = (Event)BindingContext;
            await App.Database.DeleteEventAsync(evnt);
            await Navigation.PopAsync();
        }
    }
}

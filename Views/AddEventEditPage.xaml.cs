using GestionareEven.Models;
using Plugin.LocalNotification;
using System;

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

            // Validate title and location
            if (string.IsNullOrWhiteSpace(evnt.Title))
            {
                await DisplayAlert("Error", "Please provide a title.", "OK");
                return;
            }

            // Save the event to the database
            await App.Database.SaveEventAsync(evnt);

            await Navigation.PopAsync();
        }

        async void OnDeleteClicked(object sender, EventArgs e)
        {
            var evnt = (Event)BindingContext;

            if (evnt.ID != 0)
            {
                await App.Database.DeleteEventAsync(evnt);
            }
            await Navigation.PopAsync();
        }

    }
}

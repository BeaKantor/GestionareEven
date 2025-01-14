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

            if (evnt.ID == 0) // New Event
            {
                evnt.CreatorID = App.CurrentUser.ID; // Set the current user as the creator
            }

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

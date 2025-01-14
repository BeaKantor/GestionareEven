using GestionareEven.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace GestionareEven.Views
{
    public partial class MyReservationsPage : ContentPage
    {
        public MyReservationsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (App.CurrentUser == null)
            {
                await DisplayAlert("Error", "You must log in to view your reservations.", "OK");
                return;
            }

            // Get the user's reserved events
            var participants = await App.Database.GetParticipantsAsync();
            var reservedEventIds = participants
                .Where(p => p.UserID == App.CurrentUser.ID)
                .Select(p => p.EventID);

            var events = await App.Database.GetEventsAsync();
            var reservedEvents = events.Where(e => reservedEventIds.Contains(e.ID)).ToList();

            // Bind to ListView
            ReservationsListView.ItemsSource = new ObservableCollection<Event>(reservedEvents);
        }

    }
}

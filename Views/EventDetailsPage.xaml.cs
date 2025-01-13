using GestionareEven.Models;
using System.Collections.ObjectModel;

namespace GestionareEven.Views
{
    public partial class EventDetailsPage : ContentPage
    {
        public ObservableCollection<User> Participants { get; set; }

        public EventDetailsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var evnt = (Event)BindingContext;

            // Load participants for the event
            var participants = await App.Database.GetParticipantsAsync();
            var userIds = participants.Where(p => p.EventID == evnt.ID).Select(p => p.UserID);

            // Fetch user details for the participants
            var users = await App.Database.GetUsersAsync();
            Participants = new ObservableCollection<User>(users.Where(u => userIds.Contains(u.ID)));
            ParticipantsListView.ItemsSource = Participants;
        }

        async void OnReserveSpotClicked(object sender, EventArgs e)
        {
            var evnt = (Event)BindingContext;

            if (evnt.ReservedSpots >= evnt.MaxParticipants)
            {
                await DisplayAlert("Error", "No spots available for this event.", "OK");
                return;
            }

            int currentUserId = 1; // Replace with real user ID

            var existingParticipant = (await App.Database.GetParticipantsAsync())
                .FirstOrDefault(p => p.EventID == evnt.ID && p.UserID == currentUserId);

            if (existingParticipant != null)
            {
                await DisplayAlert("Error", "You have already reserved a spot for this event.", "OK");
                return;
            }

            var participant = new Participant
            {
                EventID = evnt.ID,
                UserID = currentUserId
            };

            await App.Database.SaveParticipantAsync(participant);

            evnt.ReservedSpots++;
            await App.Database.SaveEventAsync(evnt);

            OnAppearing(); // Refresh participants list

            await DisplayAlert("Success", "You have reserved a spot!", "OK");
        }
        async void OnCancelReservationClicked(object sender, EventArgs e)
        {
            var evnt = (Event)BindingContext;

            int currentUserId = 1; // Replace with real user ID

            var participant = (await App.Database.GetParticipantsAsync())
                .FirstOrDefault(p => p.EventID == evnt.ID && p.UserID == currentUserId);

            if (participant == null)
            {
                await DisplayAlert("Error", "You do not have a reservation for this event.", "OK");
                return;
            }

            await App.Database.DeleteParticipantAsync(participant);

            evnt.ReservedSpots--;
            await App.Database.SaveEventAsync(evnt);

            OnAppearing(); // Refresh participants list

            await DisplayAlert("Success", "Your reservation has been canceled.", "OK");
        }

    }
}

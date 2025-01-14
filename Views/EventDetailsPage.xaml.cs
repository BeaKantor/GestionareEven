using GestionareEven.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace GestionareEven.Views
{
    public partial class EventDetailsPage : ContentPage
    {
        public ObservableCollection<User> Participants { get; set; }
        public string CreatorEmail { get; set; } // Display the creator's email
        public bool IsCreator { get; set; }      // Control edit/delete button visibility
        public bool CanReserve { get; set; }    // Control the visibility of Reserve Spot button
        public bool CanCancelReservation { get; set; } // Control the visibility of Cancel Reservation button

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

            // Fetch the creator's details
            var creator = users.FirstOrDefault(u => u.ID == evnt.CreatorID);
            CreatorEmail = creator?.Email ?? "Unknown";

            // Determine if the logged-in user is the creator
            IsCreator = App.CurrentUser?.ID == evnt.CreatorID;

            // Determine if the user can reserve or cancel a reservation
            var existingParticipant = participants.FirstOrDefault(p => p.EventID == evnt.ID && p.UserID == App.CurrentUser?.ID);
            CanReserve = App.CurrentUser != null && existingParticipant == null && evnt.ReservedSpots < evnt.MaxParticipants;
            CanCancelReservation = App.CurrentUser != null && existingParticipant != null;

            // Update UI bindings
            OnPropertyChanged(nameof(Participants));
            OnPropertyChanged(nameof(CreatorEmail));
            OnPropertyChanged(nameof(IsCreator));
            OnPropertyChanged(nameof(CanReserve));
            OnPropertyChanged(nameof(CanCancelReservation));
        }

        async void OnReserveSpotClicked(object sender, EventArgs e)
        {
            var evnt = (Event)BindingContext;

            if (App.CurrentUser == null)
            {
                await DisplayAlert("Error", "You must log in to reserve a spot.", "OK");
                return;
            }

            if (evnt.ReservedSpots >= evnt.MaxParticipants)
            {
                await DisplayAlert("Error", "No spots available.", "OK");
                return;
            }

            var existingParticipant = (await App.Database.GetParticipantsAsync())
                .FirstOrDefault(p => p.EventID == evnt.ID && p.UserID == App.CurrentUser.ID);

            if (existingParticipant != null)
            {
                await DisplayAlert("Error", "You have already reserved a spot for this event.", "OK");
                return;
            }

            // Create a new participant record
            var participant = new Participant
            {
                EventID = evnt.ID,
                UserID = App.CurrentUser.ID
            };

            await App.Database.SaveParticipantAsync(participant);

            // Increment the reserved spots for the event
            evnt.ReservedSpots++;
            await App.Database.SaveEventAsync(evnt);

            // Add the user to the participant list
            Participants.Add(App.CurrentUser);

            OnAppearing(); // Refresh participants list

            await DisplayAlert("Success", "Spot reserved successfully.", "OK");
        }

        async void OnCancelReservationClicked(object sender, EventArgs e)
        {
            var evnt = (Event)BindingContext;

            if (App.CurrentUser == null)
            {
                await DisplayAlert("Error", "You must log in to cancel a reservation.", "OK");
                return;
            }

            var participant = (await App.Database.GetParticipantsAsync())
                .FirstOrDefault(p => p.EventID == evnt.ID && p.UserID == App.CurrentUser.ID);

            if (participant == null)
            {
                await DisplayAlert("Error", "No reservation to cancel.", "OK");
                return;
            }

            await App.Database.DeleteParticipantAsync(participant);

            // Decrement reserved spots
            evnt.ReservedSpots--;
            await App.Database.SaveEventAsync(evnt);

            // Remove the user from the participant list
            var userToRemove = Participants.FirstOrDefault(u => u.ID == App.CurrentUser.ID);
            if (userToRemove != null)
            {
                Participants.Remove(userToRemove);
            }

            OnAppearing(); // Refresh participants list

            await DisplayAlert("Success", "Reservation canceled successfully.", "OK");
        }
    }
}

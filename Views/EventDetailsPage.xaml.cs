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

        public EventDetailsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var evnt = (Event)BindingContext; // Get the current event bound to the page

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

            // Determine if the logged-in user is the creator of the event
            IsCreator = App.CurrentUser?.ID == evnt.CreatorID;

            // Update the UI
            OnPropertyChanged(nameof(Participants));
            OnPropertyChanged(nameof(CreatorEmail));
            OnPropertyChanged(nameof(IsCreator));
        }

        async void OnReserveSpotClicked(object sender, EventArgs e)
        {
            var evnt = (Event)BindingContext;

            // Ensure the logged-in user is valid
            if (App.CurrentUser == null)
            {
                await DisplayAlert("Error", "No user is logged in. Please log in first.", "OK");
                return;
            }

            // Check if there are available spots
            if (evnt.ReservedSpots >= evnt.MaxParticipants)
            {
                await DisplayAlert("Error", "No spots available for this event.", "OK");
                return;
            }

            // Check if the current user has already reserved a spot
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

            OnAppearing(); // Refresh participants list

            await DisplayAlert("Success", "You have reserved a spot!", "OK");
        }

        async void OnCancelReservationClicked(object sender, EventArgs e)
        {
            var evnt = (Event)BindingContext;

            // Ensure the logged-in user is valid
            if (App.CurrentUser == null)
            {
                await DisplayAlert("Error", "No user is logged in. Please log in first.", "OK");
                return;
            }

            var participant = (await App.Database.GetParticipantsAsync())
                .FirstOrDefault(p => p.EventID == evnt.ID && p.UserID == App.CurrentUser.ID);

            if (participant == null)
            {
                await DisplayAlert("Error", "You do not have a reservation for this event.", "OK");
                return;
            }

            await App.Database.DeleteParticipantAsync(participant);

            // Decrement reserved spots
            evnt.ReservedSpots--;
            await App.Database.SaveEventAsync(evnt);

            OnAppearing(); // Refresh participants list

            await DisplayAlert("Success", "Your reservation has been canceled.", "OK");
        }

        async void OnEditClicked(object sender, EventArgs e)
        {
            var evnt = (Event)BindingContext;

            if (!IsCreator)
            {
                await DisplayAlert("Error", "Only the creator can edit this event.", "OK");
                return;
            }

            await Navigation.PushAsync(new AddEditEventPage
            {
                BindingContext = evnt
            });
        }

        async void OnDeleteClicked(object sender, EventArgs e)
        {
            var evnt = (Event)BindingContext;

            if (!IsCreator)
            {
                await DisplayAlert("Error", "Only the creator can delete this event.", "OK");
                return;
            }

            bool confirm = await DisplayAlert("Delete", "Are you sure you want to delete this event?", "Yes", "No");
            if (confirm)
            {
                await App.Database.DeleteEventAsync(evnt);
                await Navigation.PopAsync(); // Navigate back after deletion
            }
        }
    }
}

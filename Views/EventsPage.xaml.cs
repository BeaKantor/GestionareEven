using GestionareEven.Models;
using System.Collections.ObjectModel;

namespace GestionareEven.Views
{
    public partial class EventsPage : ContentPage
    {
        public ObservableCollection<Event> Events { get; set; }
        public ObservableCollection<Event> AllEvents { get; set; }
        public string SelectedCategory { get; set; }

        public EventsPage()
        {
            InitializeComponent();
            BindingContext = this;

            Appearing += async (sender, e) =>
            {
                var eventsFromDb = await App.Database.GetEventsAsync();
                AllEvents = new ObservableCollection<Event>(eventsFromDb);
                Events = new ObservableCollection<Event>(AllEvents);
                EventsListView.ItemsSource = Events;
            };
        }

        private void FilterEvents()
        {
            if (SelectedCategory == "All" || string.IsNullOrWhiteSpace(SelectedCategory))
            {
                Events.Clear();
                foreach (var evnt in AllEvents)
                    Events.Add(evnt);
            }
            else
            {
                var filteredEvents = AllEvents.Where(e => e.Category == SelectedCategory).ToList();
                Events.Clear();
                foreach (var evnt in filteredEvents)
                    Events.Add(evnt);
            }
        }

        async void OnAddEventClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEditEventPage
            {
                BindingContext = new Event()
            });
        }

        async void OnEventSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Event selectedEvent)
            {
                await Navigation.PushAsync(new AddEditEventPage
                {
                    BindingContext = selectedEvent
                });
            }

            EventsListView.SelectedItem = null;
        }

        async void OnDetailsClicked(object sender, EventArgs e)
        {
            if ((sender as Button)?.CommandParameter is Event selectedEvent)
            {
                await Navigation.PushAsync(new EventDetailsPage
                {
                    BindingContext = selectedEvent
                });
            }
        }
        async void OnMyReservationsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyReservationsPage());
        }
        async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Logout", "Are you sure you want to log out?", "Yes", "No");
            if (confirm)
            {
                // Clear the current user session
                App.CurrentUser = null;

                // Navigate back to the LoginPage
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }
        async void OnCategorySelected(object sender, EventArgs e)
        {
            // Get the selected category
            var picker = (Picker)sender;
            string selectedCategory = picker.SelectedItem?.ToString();

            // Fetch all events
            var allEvents = await App.Database.GetEventsAsync();

            // Filter events by the selected category
            if (!string.IsNullOrEmpty(selectedCategory) && selectedCategory != "All")
            {
                Events = new ObservableCollection<Event>(
                    allEvents.Where(e => e.Category == selectedCategory)
                );
            }
            else
            {
                Events = new ObservableCollection<Event>(allEvents); // Show all events if "All" is selected
            }

            // Update the EventsListView
            EventsListView.ItemsSource = Events;
        }



    }
}

using GestionareEven.Models;
using System.Collections.ObjectModel;

namespace GestionareEven.Views
{
    public partial class EventsPage : ContentPage
    {
        ObservableCollection<Event> events;

        public EventsPage()
        {
            InitializeComponent();

            Appearing += async (sender, e) =>
            {
                // Load events from database
                var eventsFromDb = await App.Database.GetEventsAsync();
                Console.WriteLine($"Loaded {eventsFromDb.Count} events.");
                events = new ObservableCollection<Event>(eventsFromDb);

                // Bind events to the ListView
                EventsListView.ItemsSource = events;
            };
        }

        async void OnAddEventClicked(object sender, EventArgs e)
        {
            // Navigate to AddEditEventPage with a new Event object
            await Navigation.PushAsync(new AddEditEventPage
            {
                BindingContext = new Event()
            });
        }

        async void OnEventSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Event selectedEvent)
            {
                // Navigate to AddEditEventPage with the selected Event
                await Navigation.PushAsync(new AddEditEventPage
                {
                    BindingContext = selectedEvent
                });
            }

            // Clear selection to avoid issues
            ((ListView)sender).SelectedItem = null;
        }
    }
}

using GestionareEven.Models;
using System;


namespace GestionareEven.Views;
public partial class EventDetailsPage : ContentPage
{
	public EventDetailsPage()
	{
		InitializeComponent();
	}
    async void OnReserveClicked(object sender, EventArgs e)
    {
        var evnt = (Event)BindingContext;

        // Check if spots are available
        if (evnt.ReservedSpots >= evnt.MaxParticipants)
        {
            await DisplayAlert("Error", "No spots available.", "OK");
            return;
        }

        // Increment reserved spots
        evnt.ReservedSpots++;
        await App.Database.SaveEventAsync(evnt);

        // Refresh UI
        OnPropertyChanged(nameof(evnt.ReservedSpots));

        await DisplayAlert("Success", "You have reserved a spot!", "OK");
    }

}
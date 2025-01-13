using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using GestionareEven.Views; // Ensure the namespace for your views is included
using GestionareEven.Models;
using Plugin.LocalNotification.EventArgs;
using GestionareEven.Data;

namespace GestionareEven
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Configure notifications
            builder.UseLocalNotification();

            // Handle notification taps
            LocalNotificationCenter.Current.NotificationActionTapped += OnNotificationTapped;


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void OnNotificationTapped(NotificationEventArgs e)
        {
            // Get the NotificationId, which corresponds to the Event ID
            var eventId = e.Request.NotificationId;

            // Use MainThread to safely access UI-related code
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                // Retrieve the specific event from the database using the event ID
                var selectedEvent = await App.Database.GetEventByIdAsync(eventId);

                if (selectedEvent != null)
                {
                    // Navigate to the event detail page (AddEditEventPage or another page)
                    if (Application.Current.MainPage is NavigationPage navigation)
                    {
                        await navigation.PushAsync(new AddEditEventPage
                        {
                            BindingContext = selectedEvent
                        });
                    }
                }
                else
                {
                    // Handle the case where the event is not found
                    Console.WriteLine($"Event with ID {eventId} not found.");
                }
            });
        }
    }
}

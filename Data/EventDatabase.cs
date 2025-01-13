using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestionareEven.Models;

namespace GestionareEven.Data
{
    public class EventDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public EventDatabase(string dbPath)
        {
            // Initialize SQLite connection
            _database = new SQLiteAsyncConnection(dbPath);

            // Create tables for models
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<Event>().Wait();
            _database.CreateTableAsync<Notification>().Wait();
            _database.CreateTableAsync<Participant>().Wait();
        }

        // CRUD operations for User
        public Task<List<User>> GetUsersAsync() => _database.Table<User>().ToListAsync();
        public Task<int> SaveUserAsync(User user) => user.ID != 0 ? _database.UpdateAsync(user) : _database.InsertAsync(user);
        public Task<int> DeleteUserAsync(User user) => _database.DeleteAsync(user);

        // CRUD operations for Event
        public Task<List<Event>> GetEventsAsync() => _database.Table<Event>().ToListAsync();
        public Task<int> SaveEventAsync(Event evnt) => evnt.ID != 0 ? _database.UpdateAsync(evnt) : _database.InsertAsync(evnt);
        public Task<int> DeleteEventAsync(Event evnt) => _database.DeleteAsync(evnt);

        // Get a single Event by ID
        public Task<Event> GetEventByIdAsync(int id)
        {
            return _database.Table<Event>().FirstOrDefaultAsync(e => e.ID == id);
        }

        // CRUD operations for Notification
        public Task<List<Notification>> GetNotificationsAsync() => _database.Table<Notification>().ToListAsync();
        public Task<int> SaveNotificationAsync(Notification notification) => notification.ID != 0 ? _database.UpdateAsync(notification) : _database.InsertAsync(notification);
        public Task<int> DeleteNotificationAsync(Notification notification) => _database.DeleteAsync(notification);

        // CRUD operations for Participant
        public Task<List<Participant>> GetParticipantsAsync() => _database.Table<Participant>().ToListAsync();
        public Task<int> SaveParticipantAsync(Participant participant) => participant.ID != 0 ? _database.UpdateAsync(participant) : _database.InsertAsync(participant);
        public Task<int> DeleteParticipantAsync(Participant participant) => _database.DeleteAsync(participant);
    }
}

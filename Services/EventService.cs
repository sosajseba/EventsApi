using EventApi.Interfaces;
using EventApi.Models;
using MongoDB.Driver;

namespace EventApi.Services
{
    public class EventService : IEventService
    {
        private readonly IMongoCollection<Event> _events;
        public EventService(IEventDatabaseSettings settings, IMongoClient client)
        {
            var database = client.GetDatabase(settings.DatabaseName);
            _events = database.GetCollection<Event>(settings.EventsCollectionName);
        }
        public Event Create(Event evento)
        {
            _events.InsertOne(evento);
            return evento;
        }

        public void Delete(string id)
        {
            _events.DeleteOne(item => item.Id == id);
        }

        public List<Event> Get()
        {
            return _events.Find(item => true).ToList();
        }

        public Event Get(string id)
        {
            return _events.Find(item => item.Id == id).FirstOrDefault();
        }

        public void Update(string id, Event evento)
        {
            _events.ReplaceOne(item => item.Id == id, evento);
        }
    }
}

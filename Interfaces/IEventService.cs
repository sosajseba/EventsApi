using EventApi.Models;

namespace EventApi.Interfaces
{
    public interface IEventService
    {
        List<Event> Get();
        Event Get(string id);
        Event Create(Event evento);
        void Update(string id, Event evento);
        void Delete(string id);
    }
}

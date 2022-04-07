using EventApi.Models;
using MongoDB.Driver;

namespace EventApi.Interfaces;

public interface IEventService
{
    List<Event> Get(int? page, int? size);
    Event Get(string id);
    Event Create(Event evento);
    ReplaceOneResult Update(string id, Event evento);
    DeleteResult Delete(string id);
}
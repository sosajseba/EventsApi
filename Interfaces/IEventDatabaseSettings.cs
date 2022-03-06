namespace EventApi.Interfaces;

public interface IEventDatabaseSettings
{
    string EventsCollectionName { get; set; }
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
}
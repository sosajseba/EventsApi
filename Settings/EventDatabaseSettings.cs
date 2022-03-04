using EventApi.Interfaces;

namespace EventApi.Settings
{
    public class EventDatabaseSettings : IEventDatabaseSettings
    {
        public string EventsCollectionName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }
}

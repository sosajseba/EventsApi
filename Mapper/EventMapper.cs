using AutoMapper;
using EventApi.Models;
using EventsApi.Dtos;

namespace EventsApi.Mapper;

public class EventMapper: Profile
{
    public EventMapper()
    {
        CreateMap<PostEvent, Event>();
        CreateMap<Event, GetEvent>();
    }
}
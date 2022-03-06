using AutoMapper;
using EventApi.Interfaces;
using EventApi.Models;
using EventApi.Services;
using EventApi.Settings;
using EventsApi.Dtos;
using EventsApi.Mapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<EventDatabaseSettings>(
    builder.Configuration.GetSection(nameof(EventDatabaseSettings)));

builder.Services.AddSingleton(new MapperConfiguration(mc => mc.AddProfile(new EventMapper())).CreateMapper());

builder.Services.AddSingleton<IEventDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<EventDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
    new MongoClient(builder.Configuration.GetValue<string>("EventDatabaseSettings:ConnectionString")));

builder.Services.AddScoped<IEventService, EventService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<PostEvent>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/event", (IMapper mapper, [FromServices] IEventService eventService) =>
{
    var events = eventService.Get();

    var eventDtos = mapper.Map<List<Event>, List<GetEvent>>(events);

    return Results.Ok(eventDtos);
});

app.MapGet("/event/{id}", ([FromServices] IEventService eventService, IMapper mapper, string id) =>
{
    try
    {
        var eventGet = eventService.Get(id);

        if (eventGet != null)
        {
            var eventDto = mapper.Map<GetEvent>(eventGet);
            return Results.Ok(eventDto);
        }

        return Results.NotFound();
    }
    catch (FormatException e)
    {
        return Results.BadRequest(e.Message);
    }
});

app.MapPost("/event", (IValidator<PostEvent> validator, IMapper mapper, [FromServices] IEventService eventService, PostEvent eventDto) =>
{

    var validation = validator.Validate(eventDto);

    if (!validation.IsValid)
    {
        var errors = new { errors = validation.Errors.Select(e => e.ErrorMessage) };
        return Results.BadRequest(errors);
    }
    var newEvent = mapper.Map<Event>(eventDto);

    eventService.Create(newEvent);

    return Results.Created("Created:", eventDto);
});

app.MapPut("/event/{id}", (IValidator<PostEvent> validator, IMapper mapper, [FromServices] IEventService eventService, string id, PostEvent eventDto) =>
{
    try
    {
        var validation = validator.Validate(eventDto);

        if (!validation.IsValid)
        {
            var errors = new { errors = validation.Errors.Select(e => e.ErrorMessage) };
            return Results.BadRequest(errors);
        }
        var updatedEvent = mapper.Map<Event>(eventDto);

        var result = eventService.Update(id, updatedEvent);

        return result.MatchedCount == 0 ? Results.NotFound() : Results.Ok(result);
    }
    catch (FormatException e)
    {
        return Results.BadRequest(e.Message);
    }
});

app.MapDelete("/event/{id}", ([FromServices] IEventService eventService, string id) =>
{
    try
    {
        var result = eventService.Delete(id);
        return result.DeletedCount == 0 ? Results.NotFound() : Results.Ok(result);
    }
    catch (FormatException e)
    {
        return Results.BadRequest(e.Message);
    }
});

app.Run();
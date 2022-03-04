using EventApi.Interfaces;
using EventApi.Models;
using EventApi.Services;
using EventApi.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<EventDatabaseSettings>(
    builder.Configuration.GetSection(nameof(EventDatabaseSettings)));

builder.Services.AddSingleton<IEventDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<EventDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
    new MongoClient(builder.Configuration.GetValue<string>("EventDatabaseSettings:ConnectionString")));

builder.Services.AddScoped<IEventService, EventService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/event", ([FromServices] IEventService eventService) =>
{
    return eventService.Get();
});

app.MapGet("/event/{id}", ([FromServices] IEventService eventService, string id) =>
{
    var evento = eventService.Get(id);
    return evento is not null ? Results.Ok(evento) : Results.NotFound();
});

app.MapPost("/event", ([FromServices] IEventService eventService, Event newEvent) =>
{
    eventService.Create(newEvent);
    return Results.Created("queseyo", newEvent);
});

app.MapPut("/event/{id}", ([FromServices] IEventService eventService, string id, Event newEvent) =>
{
    eventService.Update(id, newEvent);
    return Results.Ok(newEvent);
});

app.MapDelete("/event/{id}", ([FromServices] IEventService eventService, string id) =>
{
    eventService.Delete(id);
    return Results.Ok();
});

app.Run();
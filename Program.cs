using EventApi.Interfaces;
using EventApi.Services;
using EventApi.Settings;
using EventsApi.Dtos;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
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

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EventsApi", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Token Bearer",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
});

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<EventPost>());

builder.Services.AddAuthentication("Bearer")
      .AddJwtBearer("Bearer", options =>
      {
          options.RequireHttpsMetadata = false;
          options.Authority = builder.Configuration.GetValue<string>("JwtConfig:Authority");
          options.Audience = builder.Configuration.GetValue<string>("JwtConfig:Audience");
          options.SaveToken = true;
      });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/event", ([FromServices] IEventService eventService, int? page, int? size) =>
{
    var events = eventService.Get(page, size);

    var eventDtos = from e in events select e.ToDto();

    return Results.Ok(eventDtos);
}).RequireAuthorization();

app.MapGet("/event/{id}", ([FromServices] IEventService eventService, string id) =>
{
    try
    {
        var eventGet = eventService.Get(id);

        if (eventGet != null)
        {
            var eventDto = eventGet.ToDto();
            return Results.Ok(eventDto);
        }

        return Results.NotFound();
    }
    catch (FormatException e)
    {
        return Results.BadRequest(e.Message);
    }
}).RequireAuthorization();

app.MapPost("/event", (IValidator<EventPost> validator, [FromServices] IEventService eventService, EventPost eventDto) =>
{

    var validation = validator.Validate(eventDto);

    if (!validation.IsValid)
    {
        var errors = new { errors = validation.Errors.Select(e => e.ErrorMessage) };
        return Results.BadRequest(errors);
    }
    var newEvent = eventDto.ToEntity();

    eventService.Create(newEvent);

    return Results.Created("Created:", eventDto);
}).RequireAuthorization();

app.MapPut("/event/{id}", (IValidator<EventPost> validator, [FromServices] IEventService eventService, string id, EventPost eventDto) =>
{
    try
    {
        var validation = validator.Validate(eventDto);

        if (!validation.IsValid)
        {
            var errors = new { errors = validation.Errors.Select(e => e.ErrorMessage) };
            return Results.BadRequest(errors);
        }
        var updatedEvent = eventDto.ToEntity();

        var result = eventService.Update(id, updatedEvent);

        return result.MatchedCount == 0 ? Results.NotFound() : Results.Ok(result);
    }
    catch (FormatException e)
    {
        return Results.BadRequest(e.Message);
    }
}).RequireAuthorization();

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
}).RequireAuthorization();

app.Run();
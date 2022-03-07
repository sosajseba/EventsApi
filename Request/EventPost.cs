using EventApi.Models;

namespace EventsApi.Dtos;

public class EventPost
{
    public string? Name { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public DateTime Date { get; set; }
    public string? Location { get; set; }
    public decimal Price { get; set; }

    public Event ToEntity() => new()
    {
        Name = Name ?? "",
        Title = Title ?? "",
        Description = Description ?? "",
        Type = Type ?? "",
        Date = Date,
        Location = Location ?? "",
        Price = Price
    };
}
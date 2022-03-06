namespace EventsApi.Dtos;

public class GetEvent
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public DateTime Date { get; set; }
    public string? Location { get; set; }
    public decimal Price { get; set; }
}
namespace RaceDay.MVC.Models;

public class EventViewModel
{
    public int EventID { get; set; }
    public string EventName { get; set; } = "";
    public string? Description { get; set; }
    public string EventType { get; set; } = "";
    public DateTime EventDate { get; set; }
    public string Venue { get; set; } = "";
    public string City { get; set; } = "";
    public decimal DistanceKM { get; set; }
}
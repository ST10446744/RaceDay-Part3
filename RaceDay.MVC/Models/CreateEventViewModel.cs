using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace RaceDay.MVC.Models;

public class CreateEventViewModel
{
    [Required]
    [StringLength(150)]
    public string EventName { get; set; } = "";

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    public string EventType { get; set; } = "Running";

    [Required]
    [DataType(DataType.Date)]
    public DateTime EventDate { get; set; } = DateTime.Today.AddDays(30);

    [Required]
    public string StartTime { get; set; } = "08:00";

    [Required]
    [StringLength(200)]
    public string Venue { get; set; } = "";

    [Required]
    [StringLength(100)]
    public string City { get; set; } = "";

    [Required]
    [StringLength(100)]
    public string Province { get; set; } = "";

    [Range(0.1, 1000)]
    public decimal DistanceKM { get; set; }

    [Range(0, 100000)]
    public decimal EntryFee { get; set; }

    [Range(1, 100000)]
    public int? MaximumParticipants { get; set; } = 500;

    public string Status { get; set; } = "Upcoming";
    public IFormFile? BannerImage { get; set; }
    public string? BannerImagePath { get; set; }
}

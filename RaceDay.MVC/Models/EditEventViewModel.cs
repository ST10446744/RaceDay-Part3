namespace RaceDay.MVC.Models;

public class EditEventViewModel : CreateEventViewModel
{
    public string? BannerImagePath { get; set; }
    public int EventID { get; set; }
  
}

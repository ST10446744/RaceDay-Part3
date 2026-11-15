namespace RaceDay.MVC.Models;

public class EventDetailsViewModel
{
    public EditEventViewModel Event { get; set; } = new();

    public List<CategoryViewModel> Categories { get; set; } = new();

    public EnrolmentFormViewModel Enrolment { get; set; } = new();
}
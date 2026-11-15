namespace RaceDay.MVC.Models;

public class PersonalResultViewModel
{
    public int ResultID { get; set; }
    public int EnrolmentID { get; set; }
    public string EventName { get; set; } = "";
    public DateTime EventDate { get; set; }
    public string CategoryName { get; set; } = "";
    public int? FinishPosition { get; set; }
    public TimeSpan? FinishTime { get; set; }
    public string ResultStatus { get; set; } = "";
    public int FinisherCount { get; set; }
    public DateTime RecordedDate { get; set; }
}

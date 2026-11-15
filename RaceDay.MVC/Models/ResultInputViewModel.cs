using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.Models;

public class ResultInputViewModel
{
    [Range(1, int.MaxValue)]
    public int EnrolmentID { get; set; }

    [Range(1, int.MaxValue)]
    public int? FinishPosition { get; set; }

    [RegularExpression(
        @"^\d{1,2}:[0-5]\d:[0-5]\d$",
        ErrorMessage = "Enter time as hours:minutes:seconds, for example 01:25:34.")]
    public string? FinishTime { get; set; }

    [Required]
    public string ResultStatus { get; set; } = "Finished";
}

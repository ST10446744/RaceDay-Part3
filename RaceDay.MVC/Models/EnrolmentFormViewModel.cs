using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.Models;

public class EnrolmentFormViewModel
{
    public int EventID { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Select a category.")]
    public int CategoryID { get; set; }

    [Required]
    [StringLength(100)]
    public string EmergencyContactName { get; set; } = "";

    [Required]
    [StringLength(20)]
    public string EmergencyContactPhone { get; set; } = "";
}
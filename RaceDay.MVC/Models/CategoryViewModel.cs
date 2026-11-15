using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.Models;

public class CategoryViewModel
{
    public int CategoryID { get; set; }
    public int EventID { get; set; }

    [Required]
    [StringLength(100)]
    public string CategoryName { get; set; } = "";

    [StringLength(500)]
    public string? Description { get; set; }

    [Range(0, 120)]
    public int? AgeMinimum { get; set; }

    [Range(0, 120)]
    public int? AgeMaximum { get; set; }

    [Range(0, 100000)]
    public decimal CategoryFee { get; set; }
}
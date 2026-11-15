using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.Models;

public class RegisterViewModel
{
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = "";

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [StringLength(100, MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords must match.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = "";

    [Required]
    public string Role { get; set; } = "Participant";

    public string? PhoneNumber { get; set; }
}
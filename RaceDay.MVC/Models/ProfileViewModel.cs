using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace RaceDay.MVC.Models;

public class ProfileViewModel
{
    public string? ProfilePicturePath { get; set; }

    public IFormFile? ProfilePicture { get; set; }
    public int UserID { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = "";

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    public string Role { get; set; } = "";

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    public DateTime DateRegistered { get; set; }
}

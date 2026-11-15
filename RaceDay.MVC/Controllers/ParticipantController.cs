using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Filters;
using RaceDay.MVC.Models;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers;

public class ParticipantController : Controller
{
    private readonly RaceDayApiClient _apiClient;

    public ParticipantController(RaceDayApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet]
    [SessionAuthorize("Participant")]
    public async Task<IActionResult> MyEnrolments()
    {
        var enrolments = await _apiClient.GetMyEnrolmentsAsync();
        return View(enrolments);
    }

    [HttpGet]
    [SessionAuthorize("Participant")]
    public async Task<IActionResult> MyResults()
    {
        var results = await _apiClient.GetMyResultsAsync();
        return View(results);
    }

    [HttpGet]
    [SessionAuthorize("Participant")]
    public async Task<IActionResult> Profile()
    {
        var profile = await _apiClient.GetProfileAsync();

        if (profile == null)
            return NotFound();

        return View(profile);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize("Participant")]
    public async Task<IActionResult> Profile(ProfileViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        using var response = await _apiClient.UpdateProfileAsync(model);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(
                string.Empty,
                "Your profile could not be updated. Please try again.");

            return View(model);
        }

        if (model.ProfilePicture != null)
        {
            using var uploadResponse =
                await _apiClient.UploadProfilePictureAsync(model.ProfilePicture);

            if (!uploadResponse.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Your profile was updated, but the picture could not be uploaded.");

                return View(model);
            }
        }

        TempData["Success"] = "Profile updated successfully.";
        return RedirectToAction(nameof(Profile));
    }
}
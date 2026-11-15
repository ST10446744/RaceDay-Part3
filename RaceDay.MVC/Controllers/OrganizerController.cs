using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Filters;
using RaceDay.MVC.Models;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers;

public class OrganizerController : Controller
{
    private readonly RaceDayApiClient _apiClient;

    public OrganizerController(RaceDayApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [SessionAuthorize("Organiser")]
    public async Task<IActionResult> Dashboard()
    {
        var events = await _apiClient.GetMyEventsAsync();
        return View(events);
    }

    [HttpGet]
    [SessionAuthorize("Organiser")]
    public IActionResult CreateEvent()
    {
        return View(new CreateEventViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize("Organiser")]
    public async Task<IActionResult> CreateEvent(CreateEventViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        using var response = await _apiClient.CreateEventAsync(model);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(
                string.Empty,
                "The event could not be saved. Check the details and try again.");

            return View(model);
        }

        var createdEvent =
            await response.Content.ReadFromJsonAsync<EventViewModel>();

        if (model.BannerImage != null && createdEvent != null)
        {
            using var uploadResponse = await _apiClient.UploadEventBannerAsync(
                createdEvent.EventID,
                model.BannerImage);

            if (!uploadResponse.IsSuccessStatusCode)
            {
                TempData["Error"] =
                    "The event was created, but its banner image could not be uploaded.";

                return RedirectToAction(nameof(Dashboard));
            }
        }

        TempData["Success"] = "Event created successfully.";
        return RedirectToAction(nameof(Dashboard));
    }

    [HttpGet]
    [SessionAuthorize("Organiser")]
    public async Task<IActionResult> EditEvent(int id)
    {
        var model = await _apiClient.GetEventForEditAsync(id);

        if (model == null)
            return NotFound();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize("Organiser")]
    public async Task<IActionResult> EditEvent(EditEventViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        using var response = await _apiClient.UpdateEventAsync(model);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(
                string.Empty,
                "The event could not be updated. Check the details and try again.");

            return View(model);
        }

        if (model.BannerImage != null)
        {
            using var uploadResponse = await _apiClient.UploadEventBannerAsync(
                model.EventID,
                model.BannerImage);

            if (!uploadResponse.IsSuccessStatusCode)
            {
                TempData["Error"] =
                    "The event was updated, but its banner image could not be uploaded.";

                return RedirectToAction(nameof(Dashboard));
            }
        }

        TempData["Success"] = "Event updated successfully.";
        return RedirectToAction(nameof(Dashboard));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize("Organiser")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        using var response = await _apiClient.DeleteEventAsync(id);

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "The event could not be deleted.";
            return RedirectToAction(nameof(Dashboard));
        }

        TempData["Success"] = "Event deleted successfully.";
        return RedirectToAction(nameof(Dashboard));
    }

    [HttpGet]
    [SessionAuthorize("Organiser")]
    public async Task<IActionResult> ManageCategories(int eventId)
    {
        ViewBag.EventId = eventId;

        var categories = await _apiClient.GetCategoriesAsync(eventId);
        return View(categories);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize("Organiser")]
    public async Task<IActionResult> CreateCategory(CategoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Check the category details and try again.";

            return RedirectToAction(
                nameof(ManageCategories),
                new { eventId = model.EventID });
        }

        using var response = await _apiClient.CreateCategoryAsync(model);

        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "Category added successfully.";
        }
        else
        {
            var apiError = await response.Content.ReadAsStringAsync();

            TempData["Error"] =
                $"Category failed ({(int)response.StatusCode}): {apiError}";
        }

        return RedirectToAction(
            nameof(ManageCategories),
            new { eventId = model.EventID });
    }

    [HttpGet]
    [SessionAuthorize("Organiser")]
    public async Task<IActionResult> ManageEnrolments(int eventId)
    {
        ViewBag.EventId = eventId;

        var enrolments =
            await _apiClient.GetEventEnrolmentsAsync(eventId);

        return View(enrolments);
    }

    [HttpGet]
    [SessionAuthorize("Organiser")]
    public async Task<IActionResult> ManageResults(int eventId)
    {
        ViewBag.EventId = eventId;

        var enrolments =
            await _apiClient.GetEventEnrolmentsAsync(eventId);

        return View(enrolments);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize("Organiser")]
    public async Task<IActionResult> CreateResult(
        int eventId,
        ResultInputViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Check the result details and try again.";

            return RedirectToAction(
                nameof(ManageResults),
                new { eventId });
        }

        using var response = await _apiClient.CreateResultAsync(model);

        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "Result saved successfully.";
        }
        else
        {
            var apiError = await response.Content.ReadAsStringAsync();

            TempData["Error"] =
                $"Result failed ({(int)response.StatusCode}): {apiError}";
        }

        return RedirectToAction(
            nameof(ManageResults),
            new { eventId });
    }
}
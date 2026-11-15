using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Filters;
using RaceDay.MVC.Models;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly RaceDayApiClient _apiClient;

    public HomeController(
        ILogger<HomeController> logger,
        RaceDayApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public async Task<IActionResult> Index()
    {
        var events = await _apiClient.GetEventsAsync();
        return View(events);
    }

    public async Task<IActionResult> Details(int id)
    {
        var eventItem = await _apiClient.GetEventForEditAsync(id);

        if (eventItem == null)
            return NotFound();

        var categories = await _apiClient.GetCategoriesAsync(id);

        var viewModel = new EventDetailsViewModel
        {
            Event = eventItem,
            Categories = categories,
            Enrolment = new EnrolmentFormViewModel
            {
                EventID = id
            }
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [SessionAuthorize("Participant")]
    public async Task<IActionResult> Enrol(EnrolmentFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please complete the enrolment details.";
            return RedirectToAction(
                nameof(Details),
                new { id = model.EventID });
        }

        using var response =
            await _apiClient.CreateEnrolmentAsync(model);

        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "You have enrolled in the event.";
        }
        else
        {
            TempData["Error"] =
                "Enrolment could not be completed. You may already be enrolled.";
        }

        return RedirectToAction(
            nameof(Details),
            new { id = model.EventID });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id
                ?? HttpContext.TraceIdentifier
        });
    }
}
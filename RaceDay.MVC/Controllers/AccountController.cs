using System.Linq;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers;

public class AccountController : Controller
{
    private readonly RaceDayApiClient _apiClient;

    public AccountController(RaceDayApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        using var response = await _apiClient.RegisterAsync(model);

        if (!response.IsSuccessStatusCode)
        {
            var apiError = await response.Content.ReadAsStringAsync();

            ModelState.AddModelError(
                string.Empty,
                $"Registration failed ({(int)response.StatusCode}): {apiError}");

            return View(model);
        }

        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        using var response = await _apiClient.LoginAsync(model);

        if (!response.IsSuccessStatusCode)
        {
            var apiError = await response.Content.ReadAsStringAsync();

            ModelState.AddModelError(
                string.Empty,
                $"Login failed ({(int)response.StatusCode}): {apiError}");

            return View(model);
        }

        var apiResult =
            await response.Content.ReadFromJsonAsync<LoginResponse>();

        string? apiCookie = null;

        if (response.Headers.TryGetValues("Set-Cookie", out var cookieHeaders))
        {
            apiCookie = cookieHeaders
                .Select(header => header.Split(';')[0])
                .FirstOrDefault();
        }

        if (apiResult == null || string.IsNullOrWhiteSpace(apiCookie))
        {
            ModelState.AddModelError(
                string.Empty,
                "Login succeeded, but the API did not return the expected account or login cookie.");

            return View(model);
        }

        HttpContext.Session.SetString(
            "UserId",
            apiResult.UserID.ToString());

        HttpContext.Session.SetString(
            "Role",
            apiResult.Role);

        HttpContext.Session.SetString(
            "ApiCookie",
            apiCookie);


        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    private sealed class LoginResponse
    {
        public int UserID { get; set; }

        public string Role { get; set; } = string.Empty;
    }
}
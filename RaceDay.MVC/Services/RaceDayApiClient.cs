using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using RaceDay.MVC.Models;
using System.Globalization;
using System.Net.Http.Headers;

namespace RaceDay.MVC.Services;

public class RaceDayApiClient
{
    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RaceDayApiClient(
        HttpClient http,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
    {
        _http = http;
        _httpContextAccessor = httpContextAccessor;
        _http.BaseAddress = new Uri(
            configuration["Api:BaseUrl"]
            ?? throw new InvalidOperationException(
                "Set Api:BaseUrl in appsettings.json."));
    }

    public async Task<List<EventViewModel>> GetEventsAsync()
    {
        using var response = await _http.GetAsync("api/events");

        if (!response.IsSuccessStatusCode)
            return new List<EventViewModel>();

        return await response.Content
            .ReadFromJsonAsync<List<EventViewModel>>()
            ?? new List<EventViewModel>();
    }

    public async Task<List<EventViewModel>> GetMyEventsAsync()
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            "api/events/mine");

        var apiCookie = _httpContextAccessor.HttpContext?
            .Session.GetString("ApiCookie");

        if (!string.IsNullOrWhiteSpace(apiCookie))
            request.Headers.Add("Cookie", apiCookie);

        using var response = await _http.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return new List<EventViewModel>();

        return await response.Content
            .ReadFromJsonAsync<List<EventViewModel>>()
            ?? new List<EventViewModel>();
    }

    public async Task<HttpResponseMessage> RegisterAsync(
        RegisterViewModel model)
    {
        var request = new
        {
            model.FirstName,
            model.LastName,
            model.Email,
            model.Password,
            model.Role,
            model.PhoneNumber
        };

        return await _http.PostAsJsonAsync("api/auth/register", request);
    }

    public async Task<HttpResponseMessage> LoginAsync(
        LoginViewModel model)
    {
        return await _http.PostAsJsonAsync("api/auth/login", new
        {
            model.Email,
            model.Password
        });
    }
    public async Task<HttpResponseMessage> CreateEventAsync(
    CreateEventViewModel model)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "api/events");

        request.Content = System.Net.Http.Json.JsonContent.Create(new
        {
            model.EventName,
            model.Description,
            model.EventType,
            model.EventDate,
            StartTime = TimeSpan.Parse(
                model.StartTime,
                CultureInfo.InvariantCulture),
            model.Venue,
            model.City,
            model.Province,
            model.DistanceKM,
            model.EntryFee,
            model.MaximumParticipants,
            model.Status
        });

        var apiCookie = _httpContextAccessor.HttpContext?
            .Session.GetString("ApiCookie");

        if (!string.IsNullOrWhiteSpace(apiCookie))
            request.Headers.Add("Cookie", apiCookie);

        return await _http.SendAsync(request);
    }
    public async Task<EditEventViewModel?> GetEventForEditAsync(int id)
    {
        using var response = await _http.GetAsync($"api/events/{id}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content
            .ReadFromJsonAsync<EditEventViewModel>();
    }

    public async Task<HttpResponseMessage> UpdateEventAsync(
        EditEventViewModel model)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Put,
            $"api/events/{model.EventID}");

        request.Content = System.Net.Http.Json.JsonContent.Create(new
        {
            model.EventName,
            model.Description,
            model.EventType,
            model.EventDate,
            StartTime = TimeSpan.Parse(
                model.StartTime,
                CultureInfo.InvariantCulture),
            model.Venue,
            model.City,
            model.Province,
            model.DistanceKM,
            model.EntryFee,
            model.MaximumParticipants,
            model.Status
        });

        var apiCookie = _httpContextAccessor.HttpContext?
            .Session.GetString("ApiCookie");

        if (!string.IsNullOrWhiteSpace(apiCookie))
            request.Headers.Add("Cookie", apiCookie);

        return await _http.SendAsync(request);
    }
    public async Task<HttpResponseMessage> DeleteEventAsync(int id)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Delete,
            $"api/events/{id}");

        var apiCookie = _httpContextAccessor.HttpContext?
            .Session.GetString("ApiCookie");

        if (!string.IsNullOrWhiteSpace(apiCookie))
            request.Headers.Add("Cookie", apiCookie);

        return await _http.SendAsync(request);
    }
    public async Task<List<CategoryViewModel>> GetCategoriesAsync(int eventId)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/events/{eventId}/categories");

        var apiCookie = _httpContextAccessor.HttpContext?
            .Session.GetString("ApiCookie");

        if (!string.IsNullOrWhiteSpace(apiCookie))
            request.Headers.Add("Cookie", apiCookie);

        using var response = await _http.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return new List<CategoryViewModel>();

        return await response.Content
            .ReadFromJsonAsync<List<CategoryViewModel>>()
            ?? new List<CategoryViewModel>();
    }

    public async Task<HttpResponseMessage> CreateCategoryAsync(
        CategoryViewModel model)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"api/events/{model.EventID}/categories");

        request.Content = System.Net.Http.Json.JsonContent.Create(new
        {
            model.CategoryName,
            model.Description,
            model.AgeMinimum,
            model.AgeMaximum,
            model.CategoryFee
        });

        var apiCookie = _httpContextAccessor.HttpContext?
            .Session.GetString("ApiCookie");

        if (!string.IsNullOrWhiteSpace(apiCookie))
            request.Headers.Add("Cookie", apiCookie);

        return await _http.SendAsync(request);
    }
    public async Task<List<EnrolmentViewModel>> GetEventEnrolmentsAsync(
    int eventId)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/events/{eventId}/enrolments");

        var apiCookie = _httpContextAccessor.HttpContext?
            .Session.GetString("ApiCookie");

        if (!string.IsNullOrWhiteSpace(apiCookie))
            request.Headers.Add("Cookie", apiCookie);

        using var response = await _http.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return new List<EnrolmentViewModel>();

        return await response.Content
            .ReadFromJsonAsync<List<EnrolmentViewModel>>()
            ?? new List<EnrolmentViewModel>();
    }
    public async Task<HttpResponseMessage> CreateResultAsync(
    ResultInputViewModel model)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "api/results");

        TimeSpan? finishTime = string.IsNullOrWhiteSpace(model.FinishTime)
            ? null
            : TimeSpan.Parse(model.FinishTime, CultureInfo.InvariantCulture);

        request.Content = System.Net.Http.Json.JsonContent.Create(new
        {
            model.EnrolmentID,
            model.FinishPosition,
            FinishTime = finishTime,
            model.ResultStatus
        });

        var apiCookie = _httpContextAccessor.HttpContext?
            .Session.GetString("ApiCookie");

        if (!string.IsNullOrWhiteSpace(apiCookie))
            request.Headers.Add("Cookie", apiCookie);

        return await _http.SendAsync(request);
    }
    public async Task<HttpResponseMessage> CreateEnrolmentAsync(
    EnrolmentFormViewModel model)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "api/enrolments");

        request.Content = System.Net.Http.Json.JsonContent.Create(new
        {
            model.CategoryID,
            model.EmergencyContactName,
            model.EmergencyContactPhone
        });

        var apiCookie = _httpContextAccessor.HttpContext?
            .Session.GetString("ApiCookie");

        if (!string.IsNullOrWhiteSpace(apiCookie))
            request.Headers.Add("Cookie", apiCookie);

        return await _http.SendAsync(request);
    }
    public async Task<List<EnrolmentViewModel>> GetMyEnrolmentsAsync()
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            "api/enrolments/my");

        var apiCookie = _httpContextAccessor.HttpContext?
            .Session.GetString("ApiCookie");

        if (!string.IsNullOrWhiteSpace(apiCookie))
            request.Headers.Add("Cookie", apiCookie);

        using var response = await _http.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return new List<EnrolmentViewModel>();

        return await response.Content
            .ReadFromJsonAsync<List<EnrolmentViewModel>>()
            ?? new List<EnrolmentViewModel>();
    }
    public async Task<List<PersonalResultViewModel>> GetMyResultsAsync()
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            "api/results/my");

        var apiCookie = _httpContextAccessor.HttpContext?
            .Session.GetString("ApiCookie");

        if (!string.IsNullOrWhiteSpace(apiCookie))
            request.Headers.Add("Cookie", apiCookie);

        using var response = await _http.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return new List<PersonalResultViewModel>();

        return await response.Content
            .ReadFromJsonAsync<List<PersonalResultViewModel>>()
            ?? new List<PersonalResultViewModel>();
    }
    public async Task<ProfileViewModel?> GetProfileAsync()
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            "api/profile");

        var apiCookie = _httpContextAccessor.HttpContext?
            .Session.GetString("ApiCookie");

        if (!string.IsNullOrWhiteSpace(apiCookie))
            request.Headers.Add("Cookie", apiCookie);

        using var response = await _http.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<ProfileViewModel>();
    }

    public async Task<HttpResponseMessage> UpdateProfileAsync(
        ProfileViewModel model)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Put,
            "api/profile");

        request.Content = System.Net.Http.Json.JsonContent.Create(new
        {
            model.FirstName,
            model.LastName,
            model.PhoneNumber
        });

        var apiCookie = _httpContextAccessor.HttpContext?
            .Session.GetString("ApiCookie");

        if (!string.IsNullOrWhiteSpace(apiCookie))
            request.Headers.Add("Cookie", apiCookie);

        return await _http.SendAsync(request);
    }
    public async Task<HttpResponseMessage> UploadEventBannerAsync(
    int eventId,
    IFormFile file)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"api/images/events/{eventId}/banner");

        using var content = new MultipartFormDataContent();
        using var stream = file.OpenReadStream();
        using var fileContent = new StreamContent(stream);

        fileContent.Headers.ContentType =
            new MediaTypeHeaderValue(file.ContentType);

        content.Add(
            fileContent,
            "file",
            Path.GetFileName(file.FileName));

        request.Content = content;

        var apiCookie = _httpContextAccessor.HttpContext?
            .Session.GetString("ApiCookie");

        if (!string.IsNullOrWhiteSpace(apiCookie))
            request.Headers.Add("Cookie", apiCookie);

        return await _http.SendAsync(request);
    }
    public async Task<HttpResponseMessage> UploadProfilePictureAsync(
    IFormFile file)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "api/images/profile");

        using var content = new MultipartFormDataContent();
        using var stream = file.OpenReadStream();
        using var fileContent = new StreamContent(stream);

        fileContent.Headers.ContentType =
            new MediaTypeHeaderValue(file.ContentType);

        content.Add(
            fileContent,
            "file",
            Path.GetFileName(file.FileName));

        request.Content = content;

        var apiCookie = _httpContextAccessor.HttpContext?
            .Session.GetString("ApiCookie");

        if (!string.IsNullOrWhiteSpace(apiCookie))
            request.Headers.Add("Cookie", apiCookie);

        return await _http.SendAsync(request);
    }
}
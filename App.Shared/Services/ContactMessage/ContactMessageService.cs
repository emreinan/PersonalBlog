using App.Shared.Dto.ContactMessage;
using App.Shared.Models;
using App.Shared.Services.Token;
using App.Shared.Util.ExceptionHandling;
using App.Shared.Util.ExceptionHandling.Types;
using System.Net.Http.Json;

namespace App.Shared.Services.ContactMessage;

public class ContactMessageService(IHttpClientFactory httpClientFactory,ITokenService tokenService) : BaseService(httpClientFactory),IContactMessageService
{
    public async Task AddContactMessageAsync(ContactMessageAddDto contactMessageAddDto)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PostAsJsonAsync("api/ContactMessage", contactMessageAddDto);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task DeleteMessageAsync(int id)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.DeleteAsync($"api/ContactMessage/{id}");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }

    public async Task<ContactMessageViewModel> GetMessageByIdAsync(int id)
    {
        var response = await _apiHttpClient.GetAsync($"api/ContactMessage/{id}");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        return await response.Content.ReadFromJsonAsync<ContactMessageViewModel>() ??
            throw new DeserializationException("Failed to deserialize the response.");
    }

    public async Task<List<ContactMessageViewModel>> GetMessagesAsync()
    {
        var response = await _apiHttpClient.GetAsync("api/ContactMessage");
        await response.EnsureSuccessStatusCodeWithProblemDetails();
        return await response.Content.ReadFromJsonAsync<List<ContactMessageViewModel>>() ??
            throw new DeserializationException("Failed to deserialize the response.");
    }

    public async Task MarkAsReadAsync(int id)
    {
        WebApiClientGetToken(tokenService);
        var response = await _apiHttpClient.PutAsync($"api/ContactMessage/mark-as-read/{id}", null);
        await response.EnsureSuccessStatusCodeWithProblemDetails();
    }
}

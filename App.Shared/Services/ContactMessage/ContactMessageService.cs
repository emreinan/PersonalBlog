using App.Shared.Dto.ContactMessage;
using App.Shared.Models;
using App.Shared.Services.Token;
using App.Shared.Util.ExceptionHandling;
using System.Net.Http.Json;

namespace App.Shared.Services.ContactMessage;

public class ContactMessageService(IHttpClientFactory httpClientFactory,ITokenService tokenService) : BaseService(httpClientFactory),IContactMessageService
{
    public async Task AddContactMessageAsync(ContactMessageAddDto contactMessageAddDto)
    {
        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.PostAsJsonAsync("api/ContactMessage", contactMessageAddDto);
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task DeleteMessageAsync(int id)
    {
        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.DeleteAsync($"api/ContactMessage/{id}");
        await response.EnsureSuccessStatusCodeWithApiError();
    }

    public async Task<ContactMessageViewModel> GetMessageByIdAsync(int id)
    {
        var response = await _dataHttpClient.GetAsync($"api/ContactMessage/{id}");
        await response.EnsureSuccessStatusCodeWithApiError();
        return await response.Content.ReadFromJsonAsync<ContactMessageViewModel>();
    }

    public async Task<List<ContactMessageViewModel>> GetMessagesAsync()
    {
        var response = await _dataHttpClient.GetAsync("api/ContactMessage");
        await response.EnsureSuccessStatusCodeWithApiError();
        return await response.Content.ReadFromJsonAsync<List<ContactMessageViewModel>>();
    }

    public async Task MarkAsReadAsync(int id)
    {
        DataClientGetToken(tokenService);
        var response = await _dataHttpClient.PutAsync($"api/ContactMessage/mark-as-read/{id}", null);
        await response.EnsureSuccessStatusCodeWithApiError();
    }
}

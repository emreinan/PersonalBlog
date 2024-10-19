using App.Client.Models;
using App.Client.Util.ExceptionHandling;

namespace App.Client.Services.ContactMessage;

public class ContactMessageService(IHttpClientFactory httpClientFactory) : IContactMessageService
{
    private readonly HttpClient _dataHttpClient = httpClientFactory.CreateClient("DataApiClient");

    public async Task<ContactMessageViewModel> AddContactMessage(ContactMessageViewModel contactMessage)
    {
        var response = await _dataHttpClient.PostAsJsonAsync("api/contactmessage", contactMessage);
        await response.EnsureSuccessStatusCodeWithApiError();
        return await response.Content.ReadFromJsonAsync<ContactMessageViewModel>();
    }
}

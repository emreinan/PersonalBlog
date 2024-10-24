using App.Shared.Dto.ContactMessage;
using App.Shared.Models;
using App.Shared.Util.ExceptionHandling;
using System.Net.Http.Json;

namespace App.Shared.Services.ContactMessage;

public class ContactMessageService(IHttpClientFactory httpClientFactory) : IContactMessageService
{
    private readonly HttpClient _dataHttpClient = httpClientFactory.CreateClient("DataApiClient");

    public async Task<ContactMessageViewModel> AddContactMessage(ContactMessageViewModel contactMessage)
    {
        var contactMessageDto = new ContactMessageDto
        {
            Name = contactMessage.Name,
            Email = contactMessage.Email,
            Message = contactMessage.Message
        };
        var response = await _dataHttpClient.PostAsJsonAsync("api/contactmessage", contactMessageDto);
        await response.EnsureSuccessStatusCodeWithApiError();
        return await response.Content.ReadFromJsonAsync<ContactMessageViewModel>();
    }
}

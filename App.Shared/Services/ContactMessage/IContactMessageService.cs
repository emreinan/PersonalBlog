using App.Shared.Dto.ContactMessage;
using App.Shared.Models;

namespace App.Shared.Services.ContactMessage;

public interface IContactMessageService
{
    Task AddContactMessage(ContactMessageAddDto contactMessageAddDto);
    Task DeleteMessageAsync(int id);
    Task<ContactMessageViewModel> GetMessageByIdAsync(int id);
    Task<List<ContactMessageViewModel>> GetMessagesAsync();
    Task MarkAsReadAsync(int id);
}




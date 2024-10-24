using App.Shared.Models;

namespace App.Shared.Services.ContactMessage;

public interface IContactMessageService
{
    Task<ContactMessageViewModel> AddContactMessage(ContactMessageViewModel contactMessage);
}




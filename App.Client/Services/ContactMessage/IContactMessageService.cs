using App.Client.Models;

namespace App.Client.Services.ContactMessage;

public interface IContactMessageService
{
    Task<ContactMessageViewModel> AddContactMessage(ContactMessageViewModel contactMessage);
}




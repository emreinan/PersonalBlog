using App.Data.Contexts;
using App.Data.Entities.Data;
using App.Shared.Dto.ContactMessage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactMessageController(DataDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetContactMessages()
    {
        var messages = await context.ContactMessages.ToListAsync();
        return Ok(messages);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContactMessage(int id)
    {
        var message = await context.ContactMessages.FindAsync(id);

        if (message == null)
            return NotFound();

        return Ok(message);
    }

    [HttpPost]
    public async Task<IActionResult> CreateContactMessage(ContactMessageDto contactMessageDto)
    {
        var contactMessage = new ContactMessage
        {
            Name = contactMessageDto.Name,
            Email = contactMessageDto.Email,
            Subject = contactMessageDto.Subject,
            Message = contactMessageDto.Message,
            IsRead = false, // Yeni mesajların okunmamış olduğunu varsayıyoruz
            CreatedAt = DateTime.UtcNow
        };

        context.ContactMessages.Add(contactMessage);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetContactMessage), new { id = contactMessage.Id }, contactMessage);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContactMessage(int id, ContactMessageDto contactMessageDto)
    {
        var contactMessage = await context.ContactMessages.FindAsync(id);

        if (contactMessage == null)
            return NotFound();

        contactMessage.Name = contactMessageDto.Name;
        contactMessage.Email = contactMessageDto.Email;
        contactMessage.Subject = contactMessageDto.Subject;
        contactMessage.Message = contactMessageDto.Message;
        contactMessage.UpdatedAt = DateTime.Now;

        context.ContactMessages.Update(contactMessage);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContactMessage(int id)
    {
        var contactMessage = await context.ContactMessages.FindAsync(id);

        if (contactMessage == null)
            return NotFound();

        context.ContactMessages.Remove(contactMessage);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("mark-as-read/{id}")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var contactMessage = await context.ContactMessages.FindAsync(id);

        if (contactMessage == null)
            return NotFound();

        contactMessage.IsRead = true;

        context.ContactMessages.Update(contactMessage);
        await context.SaveChangesAsync();

        return NoContent();
    }
}


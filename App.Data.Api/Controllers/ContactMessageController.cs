using App.Data.Contexts;
using App.Data.Entities.Data;
using App.Shared.Dto.ContactMessage;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactMessageController(DataDbContext context,IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetContactMessages()
    {
        var messages = await context.ContactMessages.ToListAsync();
        if (messages == null)
            return NotFound("Contact message not found");

        var messagesDto = mapper.Map<List<ContactMessageDto>>(messages);
        return Ok(messagesDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContactMessage(int id)
    {
        var message = await context.ContactMessages.FindAsync(id);

        if (message == null)
            return NotFound();

        var messageDto = mapper.Map<ContactMessageDto>(message);
        return Ok(messageDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateContactMessage(ContactMessageDto contactMessageDto)
    {
        var contactMessage = mapper.Map<ContactMessage>(contactMessageDto);
        contactMessage.CreatedAt = DateTime.Now;

        context.ContactMessages.Add(contactMessage);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetContactMessage), new { id = contactMessage.Id }, contactMessageDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContactMessage(int id, ContactMessageDto contactMessageDto)
    {
        var contactMessage = await context.ContactMessages.FindAsync(id);

        if (contactMessage == null)
            return NotFound();

        var contactMessageUpdated = mapper.Map(contactMessageDto, contactMessage);
        contactMessageUpdated.CreatedAt = DateTime.Now;

        context.ContactMessages.Update(contactMessageUpdated);
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


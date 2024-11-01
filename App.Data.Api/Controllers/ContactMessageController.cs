using App.Data.Contexts;
using App.Data.Entities.Data;
using App.Shared.Dto.ContactMessage;
using App.Shared.Services.Mail;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace App.Data.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactMessageController(DataDbContext context,IMapper mapper,IMailService mailService,IConfiguration configuration) : ControllerBase
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
    public async Task<IActionResult> CreateContactMessage(ContactMessageAddDto contactMessageAddDto)
    {
        var contactMessage = mapper.Map<ContactMessage>(contactMessageAddDto);
        contactMessage.CreatedAt = DateTime.Now;

        context.ContactMessages.Add(contactMessage);
        await context.SaveChangesAsync();

        var adminEmail = "emreinannn@gmail.com";
        var subject = "Yeni Mesaj Alındı!";
        var domain = configuration["Domain"];
        var markAsReadLink = $"{domain}/api/ContactMessage/mark-as-read/{contactMessage.Id}";
        var messageBody = $"<p>Gönderen: {contactMessage.Name} ({contactMessage.Email})</p>" +
                          $"<p>Konu: {contactMessage.Subject}</p>" +
                          $"<p>Mesaj: {contactMessage.Message}</p>" +
                          $"<p><a href='{markAsReadLink}'>Mesajı okundu olarak işaretle</a></p>";

        await mailService.SendEmailAsync(adminEmail, subject, messageBody);

        return CreatedAtAction(nameof(GetContactMessage), new { id = contactMessage.Id }, contactMessageAddDto);
    }

    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
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


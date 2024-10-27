using App.Shared.Services.ContactMessage;
using App.Shared.Services.Mail;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers;

[Route("ContactMessage")]
public class ContactMessageController(IContactMessageService contactMessageService, IMailService mailService) : Controller
{
    [HttpGet("ContactMessages")]
    public async Task<IActionResult> ContactMessages()
    {
        var messages = await contactMessageService.GetMessagesAsync();
        return View(messages);
    }

    [HttpPost("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var message = await contactMessageService.GetMessageByIdAsync(id);
        if (message == null)
            return NotFound();

        await contactMessageService.DeleteMessageAsync(id);
        ViewBag.Success = "Message deleted successfully";
        return RedirectToAction(nameof(ContactMessages));
    }

    [HttpGet("MarkAsRead/{id}")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var message = await contactMessageService.GetMessageByIdAsync(id);
        if (message == null)
            return NotFound();

        string subject = "Your message has been read";
        string htmlMessage = "<p>Your message has been read by the admin. Thank you for reaching out!</p>";
        try
        {
            await mailService.SendEmailAsync(message.Email, subject, htmlMessage);

            // Email gönderimi başarılıysa, isRead'i true yap ve kaydet
            await contactMessageService.MarkAsReadAsync(id);
            ViewBag.Success = "Message marked as read and user notified";
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Email notification failed. Please try again.";
        }
        return RedirectToAction(nameof(ContactMessages));
    }
}

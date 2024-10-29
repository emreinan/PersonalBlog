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

    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var message = await contactMessageService.GetMessageByIdAsync(id);
        if (message == null)
            return NotFound();

        await contactMessageService.DeleteMessageAsync(id);

        TempData["SuccessMessage"] = "Message deleted successfully";
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
            TempData["SuccessMessage"] = "Message marked as read successfully";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An error occurred while sending the email. Please try again later.";
        }
        return RedirectToAction(nameof(ContactMessages));
    }
}

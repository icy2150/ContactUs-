using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Contact.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    [BindProperty]
    public string Name { get; set; }

    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Notes { get; set; }

    [BindProperty]
    public IFormFile Attachment { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var message = new MailMessage();
        message.To.Add("icy030804@gmail.com"); 
        message.From = new MailAddress("icy030804@gmail.com");
        message.Subject = "Contact Us Form Submission";
        message.Body = $"Name: {Name}\nEmail: {Email}\nNotes: {Notes}";

        if (Attachment != null)
        {
            var fileName = $"{Name}_{Attachment.FileName}";
            message.Attachments.Add(new Attachment(Attachment.OpenReadStream(), fileName));
        }

        using (var smtpClient = new SmtpClient())
        {
            smtpClient.Host = "smtp.gmail.com"; 
            smtpClient.Port = 587; 
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("icy030804@gmail.com", "azmzvbjsnmtbfzie"); 
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Timeout = 10000;
            smtpClient.Send(message);
        }

        return RedirectToPage("/Index");
    }
}
    
using Microsoft.AspNetCore.Mvc;

namespace PTP.WebAPI.Controllers;
public class TestingController : BaseController
{
    private readonly IEmailService emailService;
    public TestingController(IEmailService emailService)
    {
        this.emailService = emailService;
    }
    [HttpGet]
    public IActionResult Get()
    {
        var html = "<p>Welcome to Code Maze</p>";
        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }
    [HttpPost]
    public async Task<IActionResult> Post()
    {
       string mailText = System.IO.File.ReadAllText(@"./wwwroot/create-store-email.html");
       await emailService.SendEmailAsync("quangtm0152@gmail.com", "none", mailText);
       return Ok();
    }
}
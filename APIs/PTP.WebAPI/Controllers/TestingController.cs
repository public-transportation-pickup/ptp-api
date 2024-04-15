using Microsoft.AspNetCore.Mvc;

namespace PTP.WebAPI.Controllers;
public class TestingController : BaseController
{
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
}
using BookReviewApp.Business.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BookReviewApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        [Route("Home/Error")]
        public IActionResult Error()
        {
            // Optional: capture additional info if needed
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewData["ErrorPath"] = feature?.Path;
            ViewData["Exception"] = feature?.Error?.Message;

            return View();
        }
    }
}

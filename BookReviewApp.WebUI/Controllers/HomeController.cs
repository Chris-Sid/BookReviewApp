using BookReviewApp.Business.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BookReviewApp.WebUI.Controllers
{
    // Hide MVC controller actions from OpenAPI generation so Swashbuckle
    // only documents API controllers (those with [ApiController]).
    [ApiExplorerSettings(IgnoreApi = true)]
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

using Microsoft.AspNetCore.Mvc;

namespace SubjectAndClassManagement.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ILogger<NotificationController> _logger;
        public NotificationController(ILogger<NotificationController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

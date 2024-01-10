using Microsoft.AspNetCore.Mvc;

namespace SubjectAndClassManagement.Controllers
{
    public class UpdatePassController : Controller
    {
        private readonly ILogger<UpdatePassController> _logger;
        public UpdatePassController(ILogger<UpdatePassController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

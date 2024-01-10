using Microsoft.AspNetCore.Mvc;

namespace SubjectAndClassManagement.Controllers
{
    public class StuInfoController : Controller
    {
        private readonly ILogger<StuInfoController> _logger;
        public StuInfoController(ILogger<StuInfoController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

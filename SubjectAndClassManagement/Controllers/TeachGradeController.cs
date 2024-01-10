using Microsoft.AspNetCore.Mvc;

namespace SubjectAndClassManagement.Controllers
{
    public class TeachGradeController : Controller
    {
        private readonly ILogger<TeachGradeController> _logger;
        public TeachGradeController(ILogger<TeachGradeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

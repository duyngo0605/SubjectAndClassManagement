using Microsoft.AspNetCore.Mvc;

namespace SubjectAndClassManagement.Controllers
{
    public class StuGradeController : Controller
    {
        private readonly ILogger<StuGradeController> _logger;
        public StuGradeController(ILogger<StuGradeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

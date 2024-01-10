using Microsoft.AspNetCore.Mvc;

namespace SubjectAndClassManagement.Controllers
{
    
    public class TeacherInfoController : Controller
    {
        private readonly ILogger<TeacherInfoController> _logger;
        public TeacherInfoController(ILogger<TeacherInfoController> logger) 
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

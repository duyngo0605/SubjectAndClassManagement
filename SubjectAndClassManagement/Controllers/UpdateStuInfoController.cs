using Microsoft.AspNetCore.Mvc;

namespace SubjectAndClassManagement.Controllers
{
    public class UpdateStuInfoController : Controller
    {
        private readonly ILogger<UpdateStuInfoController> _logger;
        public UpdateStuInfoController(ILogger<UpdateStuInfoController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

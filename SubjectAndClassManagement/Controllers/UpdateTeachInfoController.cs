using Microsoft.AspNetCore.Mvc;

namespace SubjectAndClassManagement.Controllers
{
    public class UpdateTeachInfoController : Controller
    {
        private readonly ILogger<UpdateTeachInfoController> _logger;
        public UpdateTeachInfoController(ILogger<UpdateTeachInfoController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

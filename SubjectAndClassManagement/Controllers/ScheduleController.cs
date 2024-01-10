using Microsoft.AspNetCore.Mvc;
using SubjectAndClassManagement.Models;
using System.Diagnostics;

namespace SubjectAndClassManagement.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ILogger<ScheduleController> _logger;

        public ScheduleController(ILogger<ScheduleController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SubjectAndClassManagement.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SubjectAndClassManagement.Controllers
{
    public class AccountsController : Controller
    {
        private readonly SchoolContext _context;

        public AccountsController(SchoolContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (isValidAccount(model))
            {
                CreateAuthenticationAndSession(model.Username);
                return RedirectToAction("Index", "Home"); //Access valid
            }
            TempData["Message"] = "Login Failed!";
            return View(model); //Invalid Access
            //2 Tham số trong hàm này phải trùng tên với tên biến đã đặt ở file Access.cshtml
        }

        private async void CreateAuthenticationAndSession(string para)
        {
            List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier,para),
                    new Claim("username", para),

                    new Claim("OtherProperties","Example Role")
                };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,    
                IsPersistent = true,
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), properties);
            //Hàm này để xác thực(Authenticate) và tạo phiên(Session) đăng nhập
        }

        private bool isValidAccount(LoginViewModel model)
        {

            var user = _context.Users.FirstOrDefault(u => u.username == model.Username && u.password == model.Password);

            if (user == null) return false;
            return true;
        }
    }
}

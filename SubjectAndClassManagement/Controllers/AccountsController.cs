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
                return RedirectToAction("Index", "Dashboard"); //Access valid
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
                };

            // Thêm role vào danh sách claims dựa trên user_type
            var user = _context.Users.FirstOrDefault(u => u.username == para);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.user_type))
                    claims.Add(new Claim(ClaimTypes.Role, user.user_type));
                if (!string.IsNullOrEmpty(user.student_id))
                    claims.Add(new Claim("StudentId", user.student_id));
                if (!string.IsNullOrEmpty(user.teacher_id))
                    claims.Add(new Claim("TeacherId", user.teacher_id));
            }
            

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

        public IActionResult SomeAction()
        {
            // Lấy ra giá trị của username từ claims
            string username = User.FindFirstValue("username");

            // Lấy ra giá trị của role từ claims
            string role = User.FindFirstValue("OtherProperties");

            // Thực hiện các hành động khác với thông tin người dùng

            return View();
        }
    }
}

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
            string message;
            if (isValidAccount(model, out message))
            {
                CreateAuthenticationAndSession(model.Username);
                return RedirectToAction("Index", "Dashboard");

            }

            TempData["Message"] = message;
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
                if (!string.IsNullOrEmpty(user.status))
                    claims.Add(new Claim("Status", user.status));
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

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            // Validate and change the password
            if (ModelState.IsValid)
            {
                // Fetch user from the database using the current username
                var user = _context.Users.FirstOrDefault(u => u.username == User.FindFirstValue("username"));

                // Verify the old password
                if (user.password == model.OldPassword)
                {
                    // Update the password
                    user.password = model.NewPassword;
                    _context.SaveChanges();

                    TempData["Message"] = "Password changed successfully.";
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Incorrect old password.");
                }
            }

            return View(model);
        }


        private bool isValidAccount(LoginViewModel model, out string message)
        {
            var user = _context.Users.FirstOrDefault(u => u.username == model.Username);

            if (user.username != model.Username || user == null)
            {
                // Account does not exist
                message = "Account does not exist.";
                return false;
            }

            if (user.password != model.Password)
            {
                // Incorrect password
                message = "Incorrect password.";
                return false;
            }

            if (user.status == "locked")
            {
                // Account is locked
                message = "Account is locked.";
                return false;
            }

            // Valid account
            message = "Login successful.";
            return true;
        }


    }
}

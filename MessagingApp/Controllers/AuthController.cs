using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
         private readonly UserManager<IdentityUser> _userManager;
        
        public AuthController(AuthService authService, UserManager<IdentityUser> _userManager)
        {
            this._authService = authService;
            this._userManager = _userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            var login = new Login();
            return View(login);
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login , string? choice)
        {
            if(choice == "Register as a new user")
            {
                return RedirectToAction("Register");
            }
           
                var authResult = _authService.LoginUser(login).Result;
                if(authResult == "Success")
                {
                ViewBag.message = "Login Successful";

                var currentUser = await _userManager.FindByEmailAsync(login.Email);
                HttpContext.Session.SetString("CurrentUser", currentUser.UserName);

                return RedirectToAction("Index", "Chat");
               // return View("../Chat/Index");
                }
                else
            {
                ViewBag.message = authResult;
                return View(login);
            }

            
           
        }

        public IActionResult Register()
        {
            var register = new Register();
            return View(register);
        }
        [HttpPost]
        public async Task<IActionResult> Register(Register register)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterUser(register);
                if (result != null)
                {
                    ViewBag.message = "Registration Successful";

                    return RedirectToAction("Login");
                }
                
            }
            return View(register);
        }
    }
}

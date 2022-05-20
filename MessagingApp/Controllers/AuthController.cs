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
        public async Task<IActionResult> Login(Login login , string choice)
        {
            if(choice == "Register as a new user")
            {
                return RedirectToAction("Register");
            }
           
                var authResult = await _authService.LoginUserAsync(login);
                if(authResult == "Success")
                {
               
                return RedirectToAction("Index", "Chat");
                }
                else
                { 
                ViewBag.message = authResult;
                return View(login);
                }    
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Register register)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterUserAsync(register);
                if (result.Succeeded )
                {
                    return RedirectToAction("Login");
                }
                
            }
            return View(register);
        }

    }
}

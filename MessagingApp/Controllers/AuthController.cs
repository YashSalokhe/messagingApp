using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        
        
        public AuthController(AuthService authService)
        {
            this._authService = authService;
           
        }
        public ViewResult Index()
        {
            return View();
        }

        public ViewResult Login()
        {
           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login , string choice)
        {
            if(choice == "Register as a new user")
            {
                return RedirectToAction("Register");
            }
           
                string authResult = await _authService.LoginUserAsync(login);
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

        public ViewResult Register()
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

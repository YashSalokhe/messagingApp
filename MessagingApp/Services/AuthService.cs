
namespace MessagingApp.Services
{
    public class AuthService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
       // private readonly HttpContext _httpContext;
  

        public AuthService(SignInManager<IdentityUser> signInManager , UserManager<IdentityUser> userManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            //  this._httpContext = _httpContext;
            //, HttpContext _httpContext

        }
        //<IdentityResult>
        public async Task<IdentityResult> RegisterUser(Register register)
        {
            string temp = string.Empty;
            var registerNewUser = new IdentityUser() { UserName = register.UserName, Email = register.Email };
            // Create User
            var result = await _userManager.CreateAsync(registerNewUser, register.Password);
           
            return result;
        }

        public async Task<string> LoginUser(Login login)
        {
            string loginResult = "Success";
            var user = await _userManager.FindByEmailAsync(login.Email);
            var allUser =  _userManager.Options.User.ToString();
            if(user == null)
            {
                loginResult = "Invalid Email";
                return loginResult;
            }
            //_httpContext.Session.SetString("CurrentUser", user.UserName);
            var result = await _signInManager.PasswordSignInAsync(user.UserName, login.Password, false, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                loginResult = "Invalid Password";
            }
            return loginResult;
        } 
    }
}

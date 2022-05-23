
namespace MessagingApp.Services
{
    public class AuthService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor http;



        public AuthService(SignInManager<IdentityUser> signInManager , UserManager<IdentityUser> userManager, IHttpContextAccessor http)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this.http = http;
        }

        public async Task<IdentityResult> RegisterUserAsync(Register register)
        {
            var registerNewUser = new IdentityUser() { UserName = register.UserName, Email = register.Email };
            var result = await _userManager.CreateAsync(registerNewUser, register.Password);           
            return result;
        }

        public async Task<string> LoginUserAsync(Login login)
        {
            string loginResult = string.Empty;
            var user = await _userManager.FindByEmailAsync(login.Email);

            if(user == null)
            {
                loginResult = "Invalid Email";
                return loginResult;
            }
            
            var result = await _signInManager.PasswordSignInAsync(user.UserName, login.Password, false, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                loginResult = "Invalid Password";
                return loginResult;
            }
            else
            {
                loginResult = "Success";
                http.HttpContext.Session.SetString("currentUser", user.UserName);
                return loginResult;
            }
            
        } 


    }
}

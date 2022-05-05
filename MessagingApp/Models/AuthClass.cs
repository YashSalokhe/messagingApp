using System.ComponentModel.DataAnnotations;

namespace MessagingApp.Models
{
    public class AuthClass
    {
        public class Login
        {
            public string Email { get; set; }

            public string Password { get; set; }
        }

        public class Register
        {
            public string UserName { get; set; }
            public string Email { get; set; }
            [Required(ErrorMessage = "Password is Required")]
            [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$",
            ErrorMessage = "Passwords must be minimum 8 characters and can contain upper case, lower case, number (0-9) and special character")]
            public string Password { get; set; }
      
        }
    }
}

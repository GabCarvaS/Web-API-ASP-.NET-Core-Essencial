using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTO_s
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User name is required")]
        public string? UserName { get; set; }    

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}

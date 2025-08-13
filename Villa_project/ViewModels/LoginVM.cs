using System.ComponentModel.DataAnnotations;

namespace Villa_project.ViewModels
{
    public class LoginVM
    {
        public string Email {  get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public string? RedirectUrl {  get; set; }
    }
}

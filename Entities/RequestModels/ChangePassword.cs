using System.ComponentModel.DataAnnotations;

namespace Entities.RequestModels
{
    public class ChangePassword
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        public string NewPasswordConfirm { get; set; }

    }
}

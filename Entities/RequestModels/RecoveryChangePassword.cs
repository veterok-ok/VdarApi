using System.ComponentModel.DataAnnotations;

namespace Entities.RequestModels
{
    public class RecoveryChangePassword
    {
        public string Uri { get; set; }
        public string Password { get; set; }
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}

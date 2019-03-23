using System.ComponentModel.DataAnnotations;

namespace Entities.RequestModels
{
    public class RegistrationViewModel : ClientParameters
    {
        [Required]
        public string Phone { get; set; }
        public string Password { get; set; }
        public string SecurityCode { get; set; }

    }
}

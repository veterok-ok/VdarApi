using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VdarApi.ViewModels
{
    public class RegistrationViewModel : ClientParameters
    {
        [Required]
        public string Phone { get; set; }
        public string Password { get; set; }
        public string SecurityCode { get; set; }

    }
}

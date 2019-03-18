using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VdarApi.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }

        public string SurName { get; set; }

        public string FathersName { get; set; }

        public DateTime? Birthday { get; set; }

        [Required]
        public string Password { get; set; }

        public string Salt { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public bool PhoneIsConfirmed { get; set; }

        public string Email { get; set; }

        public bool EmailIsConfirmed { get; set; }

        public bool IsActive { get; set; }

        public DateTime? ActivatedDateUtc { get; set; }

        public DateTime CreatedDateUtc { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("Users", Schema = "Identity")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        
        [MaxLength(50)]
        public string Login { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string SurName { get; set; }

        [MaxLength(250)]
        public string FathersName { get; set; }

        public DateTime? Birthday { get; set; }
                
        public int? CityId { get; set; }
        public City City { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [MaxLength(50)]
        public string Salt { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        public bool PhoneIsConfirmed { get; set; }

        [MaxLength(320)]
        public string Email { get; set; }

        public bool EmailIsConfirmed { get; set; }

        public bool EmailIsSubscribe { get; set; }

        public string EmailKeyUnSubscribe { get; set; }

        public bool IsActive { get; set; }

        public DateTime? ActivatedDateUtc { get; set; }

        public DateTime CreatedDateUtc { get; set; }


        public List<ConfirmationKey> ConfirmationKeys { get; set; }
        public List<Token> Tokens { get; set; }

    }
}

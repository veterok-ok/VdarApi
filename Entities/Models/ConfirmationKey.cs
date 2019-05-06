using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("ConfirmationKeys", Schema = "Identity")]
    public class ConfirmationKey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConfirmationKeyId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; } 
        public User User { get; set; }

        //ключ
        [Required]
        [MaxLength(10)]
        public string Key { get; set; }

        //тип ключа (например: recovery.SMS; recovery.Email)        
        [Required]
        [MaxLength(25)]
        public string KeyType { get; set; }

        //Хэш код ключа - отправляется  на почту        
        public string HashCode { get; set; }

        //Дата создания ключа подтверждения
        [Required]
        public DateTime CreatedDateUTC { get; set; }

        //Дата окончания жизни ключа подтверждения
        public DateTime ExpireDateUTC { get; set; }

    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class ConfirmationKey
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string KeyType { get; set; }

        public string HashCode { get; set; }

        [Required]
        public DateTime CreatedDateUTC { get; set; }

        //Дата окончания жизни ключа подтверждения
        public DateTime ExpireDateUTC { get; set; }

    }
}

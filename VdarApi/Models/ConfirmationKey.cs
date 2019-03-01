using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VdarApi.Models
{
    public class ConfirmationKey
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string KeyType { get; set; }

        [Required]
        public DateTime CreatedDateUTC { get; set; }

        //Дата окончания жизни ключа подвтерждения
        public DateTime ExpireDateUTC { get; set; }

    }
}

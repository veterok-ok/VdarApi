using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    //Победители
    [Table("Winners", Schema = "Action")]
    public class Winner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WinnerId { get; set; }

        //Ссылка на акцию
        [Required]
        [ForeignKey("Stock")]
        public int StockId { get; set; }
        public Stock Stock { get; set; }

        //Ссылка на пользователя победителя
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        //Место которое занял пользователь
        [Required]
        public int Place { get; set; }

    }
}

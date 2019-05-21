using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    //Рекламы
    [Table("Ads", Schema = "Action")]
    public class Ads
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdsId { get; set; }
        
        //ссылка на рекламодателя
        [Required]
        [ForeignKey("Owner")]
        public int OwnerId { get; set; }
        public Owner Owner {get;set;}

        //ссылка на источник рекламы
        public string ContentUrl { get; set; }

        //Сообщение отображаемое в контейнере рекламы
        public string Message { get; set; }
        
        //Активность рекламы
        [Required]
        public bool IsActive { get; set; }

        //Кол-во просмотров акции в тотале
        [Required]
        public int QuantityViews { get; set; }

        //Дата добавления рекламы
        [Required]
        public DateTime CreatedDateUTC { get; set; }

    }
}

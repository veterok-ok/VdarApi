using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    //Детали акции
    [Table("StockDetails", Schema = "Action")]
    public class StockDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockDetailId { get; set; }

        //Ссылка на акцию
        [Required]
        [ForeignKey("Stock")]
        public int StockId { get; set; }
        public Stock Stock { get; set; }

        //Ссылка на рекламу
        [Required]
        [ForeignKey("Ads")]
        public int AdsId { get; set; }
        public Ads Ads { get; set; }

        //Активность детали акции
        [Required]
        public bool IsActive { get; set; }

        //Приоритет акции, применяется для определения порядка показа видео в разрезе данной акции
        [Required]
        public int Rate { get; set; }

        //Гипертекст отображаемый в описании рекламы
        public string MessageHTML { get; set; }

        //Кол-во просмотров видео в разрезе данной акции
        [Required]
        public int QuantityViews { get; set; }

    }
}

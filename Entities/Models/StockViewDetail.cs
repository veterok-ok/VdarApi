using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    //Фиксация таймлайна
    [Table("StockViewDetails", Schema = "Action")]
    public class StockViewDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockViewDetailId { get; set; }

        //Ссылка на просмотр
        [Required]
        [ForeignKey("StockView")]
        public int StockViewId { get; set; }
        public StockView StockView { get; set; }

        //Таймлайн ролика в секундах
        [Required]
        public int TimeLine { get; set; }

        [Required]
        //Дата появления подтверждения
        public DateTime DateFixationUTC { get; set; }

    }
}

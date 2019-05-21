using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    //Подтверждение просмотра акции
    [Table("StockViewConfirms", Schema = "Action")]
    public class StockViewConfirm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockViewConfirmId { get; set; }

        //Ссылка на просмотр
        [Required]
        [ForeignKey("StockView")]
        public int StockViewId { get; set; }
        public StockView StockView { get; set; }
                
        [Required]
        //Дата появления подтверждения
        public DateTime DateShowConfirmUTC { get; set; }
        //Дата подтверждения просмотра
        public DateTime DateClickConfirmUTC { get; set; }
               
    }
}

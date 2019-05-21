using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    //Просмотры акций
    [Table("StockViews", Schema = "Action")]
    public class StockView
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockViewId { get; set; }

        //Ссылка на деталь акции
        [Required]
        [ForeignKey("StockDetail")]
        public int StockDetailId { get; set; }
        public StockDetail StockDetail { get; set; }

        //Ссылка на пользователя
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        /* Статус просмотра
         * -2 - Подтверждено наружение (дисквалификация просмотра)
         * -1 - После анализа выявлено нарушение
         * 0 - не завершен
         * 1 - Завершен
         * 2 - Валидное завершение
         */
        [Required]
        public int Status { get; set; }


        [Required]
        //Дата начала просмотра акции
        public DateTime DateStartUTC { get; set; }
        //Дата завершения просмотра акции
        public DateTime DateEndUTC { get; set; }
               
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    //Призы
    [Table("Trophies", Schema = "Action")]
    public class Trophie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrophieId { get; set; }

        
        //Место от
        [Required]
        public int PlaceFrom { get; set; }
        //Место по
        public int PlaceTo { get; set; }
        //Пример: PlaceFrom = 10; Place To = 100; (Значит места с 10 по 100 (включительно), итого 90 призов)
        //Если PlaceTo = null; Значит место только одно (PlaceTo)


        //Ссылка на акцию
        [Required]
        [ForeignKey("Stock")]
        public int StockId { get; set; }
        public Stock Stock { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    //Акций
    [Table("Stocks", Schema = "Action")]
    public class Stock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockId { get; set; }

        //Заголовок акции
        [MaxLength(500)]
        public string Title { get; set; }

        //Описание акции
        public string Description { get; set; }

        /*Состояние акции
         * 0 - Новая акция
         * 1 - Акция готова к запуску (доступна для просмотра администраторам)
         * 2 - Акция запущена (все пользователи видят акцию)
         * 3 - Акция завершена
         * -1 - Акция удалена (недоступна никому)
         */
        [Required]
        public int Status { get; set; }

        /*Тип акции
         * 1 - Видео акция (несколько видеороликов)
         * 2 - Видео акция (один видеоролик)
         * 3 - Экспресс видео акция (один видеоролик, ограничено кол-во просмотров)
         * Далее возможны другие типы акций (фото реклама, реклама-анкета, реклама соц-сетей и т.д.)
         */
        [Required]
        public int Type { get; set; }

        //Дата начала акции
        [Required]
        public DateTime StartDateUTC { get; set; }

        //Дата завершения акции
        [Required]
        public DateTime EndDateUTC { get; set; }

        //Кол-во пользователь которые принимают участие в акции
        [Required]
        public int QuantityMember { get; set; }

        //Максимальное кол-во участников при экспресс акции
        public int LimiteQuantityMember { get; set; }

        //Дата добавления акции
        [Required]
        public DateTime CreatedDateUTC { get; set; }

    }
}

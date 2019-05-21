using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    //Социальные сети
    [Table("SocialNetworks", Schema = "Action")]
    public class SocialNetwork
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SocialNetworkId { get; set; }

        //Ссылка на владельца
        [Required]
        [ForeignKey("Owner")]
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }

        //Наименование аккаунта в соц сети (например vdar.kz)
        public string Name { get; set; }

        /* Вид соц сети
         * 1 - Instagramm
         * 2 - Facebook
         * 3 - Twitter
         * 4 - Google+
         * 5 - YouTube
         * 6 - VKontakte
         * 7 - Telegramm
         * 8 - Одноклассники
         * 9 - Tumblr
         * 10 - LinkedIn
         */
         [Required]
        public int Type { get; set; }

        //Ссылка на соц сеть
        [Required]
        public string Url { get; set; }

        //Активен профиль соц сети?
        [Required]
        public bool IsActive { get; set; }


    }
}

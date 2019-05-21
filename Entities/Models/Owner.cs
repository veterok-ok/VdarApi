using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    //Рекламодатели
    [Table("Owners", Schema = "Action")]
    public class Owner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OwnerId { get; set; }

        //Наименование компании
        public string CompanyName { get; set; }

        //Контактный телефон
        public string ContactMobile { get; set; }
        
        //Контактный email
        public string ContactEmail { get; set; }

        //Описание компании (HTML)
        public string AboutCompanyHTML { get; set; }

        //Логин
        public string Login { get; set; }

        //Пароль
        public string Password { get; set; }

        //Соль для пароля
        public string Sault { get; set; }

        //Активность компании
        public bool IsActive { get; set; }

        //Дата регистрации компании
        public DateTime RegistrationDate { get; set; }

        //Последняя активность компании
        public DateTime LastActivity { get; set; }

        public virtual List<SocialNetwork> SocialNetworksList { get; set; }

    }
}

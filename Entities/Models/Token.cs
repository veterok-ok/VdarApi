﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("Tokens", Schema = "Identity")]
    public class Token
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TokenId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        //JWT-токен - в нём хранится вся инфа
        [Required]
        public string AccessToken { get; set; }

        //Токен для обновления пары токенов
        [Required]
        public string RefreshToken { get; set; }

        //Хэш-слепок модели пользователя
        [Required]
        public string UpdateHashSum { get; set; }

        [Required]
        public string FingerPrint { get; set; }

        [Required]
        public DateTime CreatedDateUTC { get; set; }

        [Required]
        public DateTime LastRefreshDateUTC { get; set; }

        public string UserAgent { get; set; }

        [MaxLength(100)]
        public string IP { get; set; }

        public string Location { get; set; }

    }
}

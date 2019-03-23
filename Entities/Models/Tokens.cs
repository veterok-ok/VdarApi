using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Tokens
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public string UpdateHashSum { get; set; }

        [Required]
        public string FingerPrint { get; set; }

        [Required]
        public DateTime CreatedDateUTC { get; set; }

        [Required]
        public DateTime LastRefreshDateUTC { get; set; }

        public string UserAgent { get; set; }

        public string IP { get; set; }

        public string Location { get; set; }

    }
}

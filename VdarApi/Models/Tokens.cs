using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VdarApi.Models
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

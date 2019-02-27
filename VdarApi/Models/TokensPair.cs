using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VdarApi.Models
{
    [Table("tokens")]
    public class TokensPair
    {        
        [Column("id")]
        [Key]
        public string Id { get; set; }

        [Column("client_id")]
        [Required]
        public string ClientId { get; set; }

        [Column("access_token")]
        [Required]
        public string AccessToken { get; set; }

        [Column("refresh_token")]
        [Required]
        public string RefreshToken { get; set; }

        [Column("update_hash_sum")]
        [Required]
        public string UpdateHashSum { get; set; }

        [Column("finger_print")]
        [Required]
        public string FingerPrint { get; set; }

        [Column("created_date_utc")]
        [Required]
        public DateTime CreatedDateUTC { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace BannerWebAPI.Models
{
    public class PostBanner
    {
        [Required]
        public int Id { get; set; }
        public string Html { get; set; }
    }
}

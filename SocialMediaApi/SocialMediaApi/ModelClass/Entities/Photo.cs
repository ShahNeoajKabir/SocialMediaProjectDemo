using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaApi.ModelClass.Entities
{
    [Table("Photo")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public AppUser AppUser { get; set; }

        public int AppUserId { get; set; }
    }
}
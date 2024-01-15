using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApi.Models
{
    public class Reply
    {
        [Key]
        public int ReplyNo { get; set; }
        [Required]
        public string ReplyContent { get; set;}
        public DateTime? ReplyDate{ get; set; } = DateTime.Now;

        public int UserId { get; set; }
        public int BoardNo { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [JsonIgnore]
        [ForeignKey("BoardNo")]
        public virtual Board Board { get; set; }


    }
}

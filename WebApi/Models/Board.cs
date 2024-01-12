using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi.Models
{
    public class Board
    {
        /// <summary>
        /// 게시글 번호
        /// </summary>
        [Key]
        public int BoardNo { get; set; }

        /// <summary>
        /// 게시글 제목
        /// </summary>
        [Required]
        public string BoardTitle { get; set; }

        /// <summary>
        /// 게시글 내용
        /// </summary>
        [Required]
        public string BoardContent { get; set; }

        /// <summary>
        /// 게시글 조회수
        /// </summary>
        public int? BoardCount { get; set; } = 0;

        /// <summary>
        /// 게시글 작성일자(디폴트값 현재 일시)
        /// </summary>
        public DateTime? BoardDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 유저 번호(외래키)
        /// </summary>
        [JsonIgnore]
        public int? UserId { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}

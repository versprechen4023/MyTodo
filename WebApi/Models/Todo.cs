using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Todo
    {
        /// <summary>
        /// 할일 번호
        /// </summary>
        [Key]
        public int TodoNo { get; set; }

        /// <summary>
        /// 할일 내용
        /// </summary>
        public string TodoContent { get; set; }

        /// <summary>
        /// 할일 상태(디폴트 0; 0 = 미완료, 1 = 완료)
        /// </summary>
        public int? TodoStatus { get; set; } = 0;

        /// <summary>
        /// 유저 번호(외래키)
        /// </summary>
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}

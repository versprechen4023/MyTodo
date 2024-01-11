using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace WebApi.Models
{
    public class User
    {
        /// <summary>
        /// 유저 번호
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 유저 이름(계정 아이디)
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 유저 비밀번호
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// JWT 인가의 리프레시 토큰
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// JWT 인가의 리프레시 토큰 만료일
        /// </summary>
        public DateTime? RefreshTokenExpiry { get; set; }

        /// <summary>
        /// 권한 번호(외래키)
        /// </summary>
        public int? RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }
    }
}

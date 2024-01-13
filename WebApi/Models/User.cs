using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;

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
        [Required(ErrorMessage = "사용자 ID를 입력해주십시오")]
        [MaxLength(25, ErrorMessage = "최대 25글자 까지만 가능합니다")]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "영문자와 숫자만 허용됩니다.")]

        public string UserName { get; set; }

        /// <summary>
        /// 유저 비밀번호
        /// </summary>
        [Required(ErrorMessage = "사용자 비밀번호를 입력해주십시오")]
		public string Password { get; set; }

        /// <summary>
        /// JWT 인가의 리프레시 토큰
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// JWT 인가의 리프레시 토큰 만료일
        /// </summary>
        public DateTime? RefreshTokenExpiry { get; set; }

        [JsonIgnore]
        /// <summary>
        /// 권한 번호(외래키)
        /// </summary>
        public int? RoleId { get; set; }

        [JsonIgnore]
        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }
    }
}

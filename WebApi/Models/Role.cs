using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Role
    {
        /// <summary>
        /// 권한 번호
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 권한 이름(ex: Admin, User)
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}

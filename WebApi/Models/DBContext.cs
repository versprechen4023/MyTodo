using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    public class DBContext : DbContext // DB 컨텍스트 상속(:)
    {
        // DB 모델 클래스와 연결
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Todo> Todos { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }
    }
}

using Microsoft.EntityFrameworkCore;
using Prackticheskaya_2024.Comments;
using Prackticheskaya_2024.News;
using Prackticheskaya_2024.Users;

namespace Prackticheskaya_2024
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().HasData(new UserEntity
            {
                Id = 1,
                Family = "Цицугин",
                Name = "Данёк",
                Patronymic = "Вячеславович",
                Email = "" +
                "kazahstan4ever@example.kz",
                Password = "Kazakistan",
            });
        }
        public DbSet<CommentsEntity> Comments => Set<CommentsEntity>();
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<NewsEntity> News => Set<NewsEntity>();
    }
}

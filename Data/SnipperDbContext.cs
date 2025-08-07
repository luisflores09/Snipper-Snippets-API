using Microsoft.EntityFrameworkCore;
using Snipper_Snippets_API.Models;

namespace Snipper_Snippets_API.Data
{
    public class SnipperDbContext : DbContext
    {
        public SnipperDbContext(DbContextOptions<SnipperDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Snippet> Snippets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(u => u.Password)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasIndex(u => u.Email)
                    .IsUnique();
            });

            modelBuilder.Entity<Snippet>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Language)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(s => s.Code)
                    .IsRequired();

                entity.Property(s => s.UserId)
                    .IsRequired();

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(s => s.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

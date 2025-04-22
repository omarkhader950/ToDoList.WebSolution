using Microsoft.EntityFrameworkCore;
using System;

namespace Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        public virtual DbSet<TodoItem> TodoItems { get; set; }
        public virtual DbSet<User> Users { get; set; }  // Add DbSet for Users

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fixed GUIDs for seeded users
            var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var user1Id = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var user2Id = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var user3Id = Guid.Parse("44444444-4444-4444-4444-444444444444");

            // Configure TodoItem entity
            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Description)
                      .HasMaxLength(500);

                entity.Property(e => e.IsCompleted)
                      .HasDefaultValue(false);

                entity.Property(e => e.DueDate)
                      .HasColumnType("datetime");

                entity.Property(e => e.IsDeleted)
                      .HasDefaultValue(false);

                // Foreign key relationship
                entity.HasOne(e => e.User)
                      .WithMany(u => u.TodoItems)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Global filter for soft delete
            modelBuilder.Entity<TodoItem>()
                .HasQueryFilter(t => !t.IsDeleted);

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = adminId,
                    Username = "admin",
                    PasswordHash = "adminpasswordhash",
                    Role = "Admin"
                },
                new User
                {
                    Id = user1Id,
                    Username = "user1",
                    PasswordHash = "user1passwordhash",
                    Role = "User"
                },
                new User
                {
                    Id = user2Id,
                    Username = "user2",
                    PasswordHash = "user2passwordhash",
                    Role = "User"
                },
                new User
                {
                    Id = user3Id,
                    Username = "user3",
                    PasswordHash = "user3passwordhash",
                    Role = "User"
                }
            );

            // Seed TodoItems assigned to real users
            modelBuilder.Entity<TodoItem>().HasData(
                new TodoItem
                {
                    Id = Guid.Parse("aaaaaaa1-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Title = "First Task",
                    Description = "This is the first task.",
                    IsCompleted = false,
                    DueDate = DateTime.Now.AddDays(1),
                    IsDeleted = false,
                    UserId = user1Id
                },
                new TodoItem
                {
                    Id = Guid.Parse("aaaaaaa2-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Title = "Second Task",
                    Description = "This is the second task.",
                    IsCompleted = false,
                    DueDate = DateTime.Now.AddDays(2),
                    IsDeleted = false,
                    UserId = user2Id
                }
            );
        }

    }
}

using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure.Data;


using System;
using Entities;

namespace ToDoList.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public virtual DbSet<TodoItem> TodoItems { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

        public DbSet<TodoItemAttachment> TodoItemAttachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fixed GUIDs
            var adminId = Guid.Parse("FAC962AC-E397-47E2-996F-CC8E728A7F8F");
            var user1Id = Guid.Parse("44C091C5-BE82-4B3F-A9E0-EB195D2E62AF");
            var user2Id = Guid.Parse("3625E573-9F81-46A1-80F9-1100306169F5");
            var user3Id = Guid.Parse("59DFEC42-4C48-407F-B9DE-1AB16A845624");

            var roleAdminId = Guid.Parse("8DFC85CA-F780-43B1-B908-97EE9C90EF42");
            var roleUserId = Guid.Parse("7B858E14-D92D-43E0-AFE9-261365D067AD");

            // Hash passwords
            var adminPasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123");
            var user1PasswordHash = BCrypt.Net.BCrypt.HashPassword("User1@123");
            var user2PasswordHash = BCrypt.Net.BCrypt.HashPassword("User2@123");
            var user3PasswordHash = BCrypt.Net.BCrypt.HashPassword("User3@123");

            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = roleAdminId, Name = "Admin" },
                new Role { Id = roleUserId, Name = "User" }
            );

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = adminId, Username = "admin", PasswordHash = adminPasswordHash, RoleId = roleAdminId },
                new User { Id = user1Id, Username = "user1", PasswordHash = user1PasswordHash, RoleId = roleUserId },
                new User { Id = user2Id, Username = "user2", PasswordHash = user2PasswordHash, RoleId = roleUserId },
                new User { Id = user3Id, Username = "user3", PasswordHash = user3PasswordHash, RoleId = roleUserId }
            );

            // User-Role relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // TodoItem configuration
            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.IsCompleted).HasDefaultValue(false);
                entity.Property(e => e.DueDate).HasColumnType("datetime");
                entity.Property(e => e.CreationDate).HasColumnType("datetime").HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.User)
                      .WithMany(u => u.TodoItems)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Soft delete filter
            modelBuilder.Entity<TodoItem>().HasQueryFilter(t => t.DeleteBy == null && t.DeleteDate == null);

            // Seed TodoItems
            modelBuilder.Entity<TodoItem>().HasData(
                new TodoItem
                {
                    Id = Guid.Parse("DB2F25B9-2149-4F75-ACA0-F5BAAB2DF9F4"),
                    Title = "First Task",
                    Description = "This is the first task.",
                    IsCompleted = false,
                    DueDate = DateTime.Now.AddDays(1),
                    CreationDate = DateTime.Now,
                    UserId = user1Id
                },
                new TodoItem
                {
                    Id = Guid.Parse("4F1790DE-F460-409D-8C27-67089BCBED2D"),
                    Title = "Second Task",
                    Description = "This is the second task.",
                    IsCompleted = false,
                    DueDate = DateTime.Now.AddDays(2),
                    CreationDate = DateTime.Now,
                    UserId = user2Id
                }
            );


            modelBuilder.Entity<TodoItemAttachment>()
    .HasOne(a => a.TodoItem)
    .WithMany(t => t.Attachments)
    .HasForeignKey(a => a.TodoItemId)
    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TodoItemAttachment>()
    .HasQueryFilter(a => a.TodoItem.DeleteBy == null && a.TodoItem.DeleteDate == null);

        }
    }
}

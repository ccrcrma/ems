using System;
using ems.Areas.Identity.Models;
using ems.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ems.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, IdentityRole, string,
        IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<Notice> Notices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Department>(department =>
            {
                department.ToTable("department").HasKey(d => d.Id);
                department.Property(d => d.Name).IsRequired().HasMaxLength(100);
                department.Property(d => d.Description).IsRequired();
            });

            modelBuilder.Entity<Leave>(leave =>
            {
                leave.ToTable("leave").HasKey(l => l.Id);
                leave.Property(l => l.From).HasColumnType("date");
                leave.Property(l => l.Type).HasColumnType("tinyint");
                leave.Property(l => l.To).HasColumnType("date");
                leave.Property(l => l.Description).IsRequired();
            });

            modelBuilder.Entity<Reply>(reply =>
            {
                reply.ToTable("reply").HasKey(r => r.Id);
                reply.Property(r => r.Status).HasColumnType("tinyint");
                reply.Property(r => r.Description).IsRequired();
                reply.HasOne<Leave>().WithOne(l => l.Reply).HasForeignKey("Reply", "LeaveId").IsRequired();

            });

            modelBuilder.Entity<Notice>(notice =>
            {
                notice.ToTable("notice").HasKey(n => n.Id);
                notice.Property(n => n.Title).IsRequired().HasMaxLength(100);
                notice.Property(n => n.Description).IsRequired();
                notice.Property(n => n.CreatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<ApplicationUser>(user =>
            {
                user.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
                user.Property(u => u.LastName).IsRequired().HasMaxLength(100);
                user.Property(u => u.Address).IsRequired().HasMaxLength(200);
                user.Property(u => u.PhoneNumber).IsRequired().HasMaxLength(10);
                user.Property(u => u.Post).HasColumnType("tinyint");
                user.Property(u => u.CreatedDate).HasColumnType("date");

                user.HasOne<Department>(u => u.Department)
                    .WithMany()
                    .HasForeignKey(u => u.DepartmentId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.HasOne<ApplicationUser>(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
                userRole.HasOne<IdentityRole>(r => r.Role)
                    .WithMany()
                    .HasForeignKey(r => r.RoleId)
                    .IsRequired();
            });
        }
    }
}
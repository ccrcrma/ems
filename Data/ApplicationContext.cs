using System;
using ems.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ems.Data
{
    public class ApplicationContext : DbContext
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
        }
    }
}
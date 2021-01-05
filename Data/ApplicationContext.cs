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
        }
    }
}
using Microsoft.EntityFrameworkCore;
using ProjectManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DAL
{
    public partial class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeProject> EmployeeProjects { get; set; }
        public virtual DbSet<Project> Projects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ProjectManagerDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(MAX)");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(MAX)");

                entity.Property(e => e.PatronymicName).HasColumnType("NVARCHAR(MAX)");

                entity.Property(e => e.SurName)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(MAX)");
            });

            modelBuilder.Entity<EmployeeProject>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.ProjectId })
                    .HasName("PK_EmployeeProject");

                entity.HasIndex(e => e.EmployeeId, "EmployeeId");

                entity.HasIndex(e => e.ProjectId, "ProjectId");

                entity.Property(e => e.CreatedAd).HasColumnType("datetime");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeProjects)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Employee");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.EmployeeProjects)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_Project");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.CreatedAd).HasColumnType("datetime");

                entity.Property(e => e.DateEnd).HasColumnType("datetime");

                entity.Property(e => e.DateStart).HasColumnType("datetime");

                entity.Property(e => e.CustomerCompanyName)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(MAX)");

                entity.Property(e => e.ExecutorCompanyName).HasColumnType("NVARCHAR(MAX)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(MAX)");

                entity.Property(e => e.PriorityUpdate).HasColumnType("datetime");

                entity.HasOne(d => d.ProjectManager)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.ProjectManagerId)
                    .HasConstraintName("FK_ProjectManager");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace InternshipLogbook.API.Models;

public partial class InternshipLogbookDbContext : DbContext
{
    public InternshipLogbookDbContext()
    {
    }

    public InternshipLogbookDbContext(DbContextOptions<InternshipLogbookDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DailyActivity> DailyActivities { get; set; }

    public virtual DbSet<Student> Students { get; set; }
    
    public virtual DbSet<Company> Companies { get; set; }
    
    public virtual DbSet<Faculty> Faculties { get; set; }
    
    public virtual DbSet<StudyProgramme> StudyProgrammes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=InternshipLogbookDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DailyActivity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DailyAct__3214EC071D55DE6E");

            entity.Property(e => e.TimeFrame).HasMaxLength(50);
            entity.Property(e => e.Venue).HasMaxLength(100);

            entity.HasOne(d => d.Student).WithMany(p => p.DailyActivities)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DailyActivities_Students");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students");
            
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.HostTutor).HasMaxLength(100);
            entity.Property(e => e.InternshipDirector).HasMaxLength(100);
            entity.Property(e => e.InternshipPeriod).HasMaxLength(100);
            entity.HasOne(d => d.Company)
                .WithMany(p => p.Students)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Students_Companies");
            entity.HasOne(d => d.StudyProgramme)
                .WithMany(p => p.Students)
                .HasForeignKey(d => d.StudyProgrammeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Students_StudyProgrammes");
        });

        modelBuilder.Entity<Company>(entity => 
        {
            entity.Property(e => e.Name).HasMaxLength(200);
        });
        
        modelBuilder.Entity<StudyProgramme>(entity =>
        {
            entity.HasOne(d => d.Faculty)
                .WithMany(p => p.StudyProgrammes)
                .HasForeignKey(d => d.FacultyId)
                .HasConstraintName("FK_StudyProgrammes_Faculties");
        });
        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

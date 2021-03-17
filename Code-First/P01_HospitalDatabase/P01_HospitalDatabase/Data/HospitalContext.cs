using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext()
        {
        }

        public HospitalContext(DbContextOptions<HospitalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Medicament> Medicaments { get; set; }
        public virtual DbSet<PatientMedicament> PatientMedicaments { get; set; }
        public virtual DbSet<Diagnose> Diagnoses { get; set; }
        public virtual DbSet<Visitation> Visitations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-OAP1RAB\\SQLEXPRESS;Database=HospitalDB;Integrated Security=True;");
            }
        }


        //      FLUENT API

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Patient>(entity =>
        //    {
        //        entity.HasKey(e => e.PatientId);

        //        entity.Property(e => e.FirstName)
        //            .HasMaxLength(50)
        //            .IsUnicode(true)
        //            .IsRequired(true);

        //        entity.Property(e => e.LastName)
        //            .HasMaxLength(50)
        //            .IsUnicode(true)
        //            .IsRequired(true);

        //        entity.Property(e => e.Address)
        //            .HasMaxLength(250)
        //            .IsUnicode(true)
        //            .IsRequired(true);

        //        entity.Property(e => e.Email)
        //            .HasMaxLength(80)
        //            .IsUnicode(false)
        //            .IsRequired(true);

        //        entity.Property(e => e.HasInsurance)
        //            .IsRequired(true);
        //    });

        //    modelBuilder.Entity<Doctor>(entity =>
        //    {
        //        entity.HasKey(e => e.DoctorId);

        //        entity.Property(e => e.Name)
        //            .HasMaxLength(100)
        //            .IsUnicode(true)
        //            .IsRequired(true);

        //        entity.Property(e => e.Specialty)
        //            .HasMaxLength(100)
        //            .IsUnicode(true)
        //            .IsRequired(true);
        //    });

        //    modelBuilder.Entity<Visitation>(entity =>
        //    {
        //        entity.HasKey(e => e.VisitationId);

        //        entity.Property(e => e.Date)
        //            .IsRequired(true)
        //            .HasDefaultValueSql("GETDATE()");

        //        entity.Property(e => e.Comments)
        //            .HasMaxLength(250)
        //            .HasDefaultValue("OK")
        //            .IsUnicode(true)
        //            .IsRequired(true);

        //        entity.Property(e => e.PatientId)
        //            .IsRequired(true);

        //        entity.HasOne(e => e.Patient)
        //            .WithMany(p => p.Visitations)
        //            .HasForeignKey(e => e.PatientId);

        //        entity.Property(e => e.DoctorId)
        //            .IsRequired(false);

        //        entity.HasOne(e => e.Doctor)
        //            .WithMany(d => d.Visitations)
        //            .HasForeignKey(e => e.DoctorId);
        //    });

        //    modelBuilder.Entity<Diagnose>(entity =>
        //    {
        //        entity.HasKey(e => e.DiagnoseId);

        //        entity.Property(e => e.Name)
        //            .HasMaxLength(50)
        //            .IsUnicode(true)
        //            .IsRequired(true);

        //        entity.Property(e => e.Comments)
        //            .HasMaxLength(250)
        //            .IsUnicode(true)
        //            .HasDefaultValue("Healthy :)")
        //            .IsRequired(true);

        //        entity.Property(e => e.PatientId)
        //            .IsRequired(true);

        //        entity.HasOne(e => e.Patient)
        //            .WithMany(p => p.Diagnoses)
        //            .HasForeignKey(e => e.PatientId);
        //    });

        //    modelBuilder.Entity<Medicament>(entity =>
        //    {
        //        entity.HasKey(e => e.MedicamentId);

        //        entity.Property(e => e.Name)
        //            .HasMaxLength(50)
        //            .IsUnicode(true)
        //            .IsRequired(true);
        //    });

        //    modelBuilder.Entity<PatientMedicament>(entity =>
        //    {
        //        entity.HasKey(e => new { e.PatientId, e.MedicamentId }); COMPOSITE KEY

        //        entity.HasOne(e => e.Patient)
        //            .WithMany(p => p.Prescriptions)
        //            .HasForeignKey(e => e.PatientId);

        //        entity.HasOne(e => e.Medicament)
        //            .WithMany(m => m.Prescriptions)
        //            .HasForeignKey(e => e.MedicamentId);
        //    });
        //}
    }
}

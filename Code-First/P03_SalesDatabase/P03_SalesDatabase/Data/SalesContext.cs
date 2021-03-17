using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        public SalesContext()
        {
        }

        public SalesContext(DbContextOptions<SalesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-OAP1RAB\\SQLEXPRESS;Database=SalesDB;Integrated Security=True;");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(true)
                    .IsRequired(true);

               entity.Property(e => e.Description)
                   .HasMaxLength(250)
                   .IsUnicode(true)
                   .IsRequired(true)
                   .HasDefaultValue("No description");

                entity.Property(e => e.Quantity)
                    .IsRequired(true);

                entity.Property(e => e.Price)
                    .IsRequired(true);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(true)
                    .IsRequired(true);

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .IsRequired(true);
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.StoreId);

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(true)
                    .IsRequired(true);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(e => e.SaleId);

                entity.Property(e => e.Date)
                    .IsRequired(true)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.ProductId)
                    .IsRequired(true);

                entity.HasOne(e => e.Product)
                    .WithMany(s => s.Sales)
                    .HasForeignKey(e => e.ProductId);

                entity.Property(e => e.CustomerId)
                    .IsRequired(true);

                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Sales)
                    .HasForeignKey(e => e.CustomerId);

                entity.Property(e => e.StoreId)
                    .IsRequired(true);

                entity.HasOne(e => e.Store)
                    .WithMany(s => s.Sales)
                    .HasForeignKey(e => e.StoreId);
            });
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Product>(entity =>
        //    {
        //        entity.HasKey(e => e.ProductId);

        //        entity.Property(e => e.Name)
        //            .HasMaxLength(50)
        //            .IsUnicode()
        //            .IsRequired();

        //        entity.Property(e => e.Quantity).IsRequired();

        //        entity.Property(e => e.Price).IsRequired();
        //    });

        //    modelBuilder.Entity<Customer>(entity =>
        //    {
        //        entity.HasKey(e => e.CustomerId);

        //        entity.Property(e => e.Name)
        //            .HasMaxLength(100)
        //            .IsUnicode()
        //            .IsRequired();

        //        entity.Property(e => e.Email)
        //            .HasMaxLength(80)
        //            .IsUnicode(false)
        //            .IsRequired();

        //        entity.Property(e => e.CreditCardNumber)
        //            .HasMaxLength(20)
        //            .IsUnicode()
        //            .IsRequired();

        //    });

        //    modelBuilder.Entity<Store>(entity =>
        //    {
        //        entity.HasKey(e => e.StoreId);

        //        entity.Property(e => e.Name)
        //            .HasMaxLength(80)
        //            .IsUnicode()
        //            .IsRequired();
        //    });

        //    modelBuilder.Entity<Sale>(entity =>
        //    {
        //        entity.HasKey(e => e.SaleId);

        //        entity.Property(e => e.Date)
        //            .IsRequired()
        //            .HasDefaultValue("GETDATE()");

        //        entity.Property(e => e.CustomerId)
        //            .IsRequired();

        //        entity.HasOne(e => e.Customer)
        //            .WithMany(s => s.Sales)
        //            .HasForeignKey(e => e.CustomerId);

        //        entity.Property(e => e.ProductId)
        //            .IsRequired();

        //        entity.HasOne(e => e.Product)
        //            .WithMany(s => s.Sales)
        //            .HasForeignKey(e => e.ProductId);

        //        entity.Property(e => e.StoreId)
        //            .IsRequired();

        //        entity.HasOne(e => e.Store)
        //            .WithMany(s => s.Sales)
        //            .HasForeignKey(e => e.StoreId);
        //    });
        //}


        //protected override void onmodelcreating(modelbuilder modelbuilder)
        //{
        //    modelbuilder.entity<patient>(entity =>
        //    {
        //        entity.haskey(e => e.patientid);

        //        entity.property(e => e.firstname)
        //            .hasmaxlength(50)
        //            .isunicode(true)
        //            .isrequired(true);

        //        entity.property(e => e.lastname)
        //            .hasmaxlength(50)
        //            .isunicode(true)
        //            .isrequired(true);

        //        entity.property(e => e.address)
        //            .hasmaxlength(250)
        //            .isunicode(true)
        //            .isrequired(true);

        //        entity.property(e => e.email)
        //            .hasmaxlength(80)
        //            .isunicode(false)
        //            .isrequired(true);

        //        entity.property(e => e.hasinsurance)
        //            .isrequired(true);
        //    });

        //    modelbuilder.entity<doctor>(entity =>
        //    {
        //        entity.haskey(e => e.doctorid);

        //        entity.property(e => e.name)
        //            .hasmaxlength(100)
        //            .isunicode(true)
        //            .isrequired(true);

        //        entity.property(e => e.specialty)
        //            .hasmaxlength(100)
        //            .isunicode(true)
        //            .isrequired(true);
        //    });

        //    modelbuilder.entity<visitation>(entity =>
        //    {
        //        entity.haskey(e => e.visitationid);

        //        entity.property(e => e.date)
        //            .isrequired(true)
        //            .hasdefaultvaluesql("getdate()");

        //        entity.property(e => e.comments)
        //            .hasmaxlength(250)
        //            .hasdefaultvalue("ok")
        //            .isunicode(true)
        //            .isrequired(true);

        //        entity.property(e => e.patientid)
        //            .isrequired(true);

        //        entity.hasone(e => e.patient)
        //            .withmany(p => p.visitations)
        //            .hasforeignkey(e => e.patientid);

        //        entity.property(e => e.doctorid)
        //            .isrequired(false);

        //        entity.hasone(e => e.doctor)
        //            .withmany(d => d.visitations)
        //            .hasforeignkey(e => e.doctorid);
        //    });

        //    modelbuilder.entity<diagnose>(entity =>
        //    {
        //        entity.haskey(e => e.diagnoseid);

        //        entity.property(e => e.name)
        //            .hasmaxlength(50)
        //            .isunicode(true)
        //            .isrequired(true);

        //        entity.property(e => e.comments)
        //            .hasmaxlength(250)
        //            .isunicode(true)
        //            .hasdefaultvalue("healthy :)")
        //            .isrequired(true);

        //        entity.property(e => e.patientid)
        //            .isrequired(true);

        //        entity.hasone(e => e.patient)
        //            .withmany(p => p.diagnoses)
        //            .hasforeignkey(e => e.patientid);
        //    });

        //    modelbuilder.entity<medicament>(entity =>
        //    {
        //        entity.haskey(e => e.medicamentid);

        //        entity.property(e => e.name)
        //            .hasmaxlength(50)
        //            .isunicode(true)
        //            .isrequired(true);
        //    });

        //    modelbuilder.entity<patientmedicament>(entity =>
        //    {
        //        entity.haskey(e => new { e.patientid, e.medicamentid }); composite key

        //        entity.hasone(e => e.patient)
        //            .withmany(p => p.prescriptions)
        //            .hasforeignkey(e => e.patientid);

        //        entity.hasone(e => e.medicament)
        //            .withmany(m => m.prescriptions)
        //            .hasforeignkey(e => e.medicamentid);
        //    });
        //}
    }
}

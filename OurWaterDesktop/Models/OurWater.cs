using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace OurWaterDesktop.Models
{
    public partial class OurWater : DbContext
    {
        public OurWater()
            : base("name=OurWater2")
        {
        }

        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<ConsumptionDebitRecord> ConsumptionDebitRecords { get; set; }
        public virtual DbSet<FineRule> FineRules { get; set; }
        public virtual DbSet<Fine> Fines { get; set; }
        public virtual DbSet<ProductionDebitRecord> ProductionDebitRecords { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public User User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bill>()
                .Property(e => e.status)
                .IsUnicode(false);

            modelBuilder.Entity<Bill>()
                .Property(e => e.rejectionReason)
                .IsUnicode(false);

            modelBuilder.Entity<Bill>()
                .Property(e => e.imagePath)
                .IsUnicode(false);

            modelBuilder.Entity<Bill>()
                .HasMany(e => e.Fines)
                .WithRequired(e => e.Bill)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ConsumptionDebitRecord>()
                .Property(e => e.status)
                .IsUnicode(false);

            modelBuilder.Entity<ConsumptionDebitRecord>()
                .Property(e => e.rejectionReason)
                .IsUnicode(false);

            modelBuilder.Entity<ConsumptionDebitRecord>()
                .Property(e => e.imagePath)
                .IsUnicode(false);

            modelBuilder.Entity<ConsumptionDebitRecord>()
                .Property(e => e.location)
                .IsUnicode(false);

            modelBuilder.Entity<ConsumptionDebitRecord>()
                .HasMany(e => e.Bills)
                .WithRequired(e => e.ConsumptionDebitRecord)
                .HasForeignKey(e => e.consumptionRecordId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FineRule>()
                .HasMany(e => e.Fines)
                .WithRequired(e => e.FineRule)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductionDebitRecord>()
                .Property(e => e.location)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.fullname)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.role)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.address)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Bills)
                .WithRequired(e => e.Customer)
                .HasForeignKey(e => e.customerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ConsumptionDebitRecords)
                .WithRequired(e => e.InputtingUser)
                .HasForeignKey(e => e.inputtedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ConsumptionDebitRecords1)
                .WithRequired(e => e.Customer)
                .HasForeignKey(e => e.customerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ConsumptionDebitRecords2)
                .WithOptional(e => e.CorrectingUser)
                .HasForeignKey(e => e.correctedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ProductionDebitRecords)
                .WithRequired(e => e.InputtingUser)
                .HasForeignKey(e => e.inputtedBy)
                .WillCascadeOnDelete(false);
        }
    }
}

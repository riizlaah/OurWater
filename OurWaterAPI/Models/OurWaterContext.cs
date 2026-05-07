using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OurWaterAPI.Models;

public partial class OurWaterContext : DbContext
{
    public OurWaterContext()
    {
    }

    public OurWaterContext(DbContextOptions<OurWaterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<ConsumptionDebitRecord> ConsumptionDebitRecords { get; set; }

    public virtual DbSet<Fine> Fines { get; set; }

    public virtual DbSet<FineRule> FineRules { get; set; }

    public virtual DbSet<ProductionDebitRecord> ProductionDebitRecords { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Database=OurWater");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Bill");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.ConsumptionRecordId).HasColumnName("consumptionRecordId");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.Deadline)
                .HasColumnType("datetime")
                .HasColumnName("deadline");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("imagePath");
            entity.Property(e => e.RejectionReason)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("rejectionReason");
            entity.Property(e => e.Status)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.ConsumptionRecord).WithMany(p => p.Bills)
                .HasForeignKey(d => d.ConsumptionRecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bill_ConsumptionDebitRecord");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bills)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bill_Users");
        });

        modelBuilder.Entity<ConsumptionDebitRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ConsumptionDebitRecord");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CorrectedBy).HasColumnName("correctedBy");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Debit)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("debit");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("imagePath");
            entity.Property(e => e.InputtedBy).HasColumnName("inputtedBy");
            entity.Property(e => e.Location)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.RejectionReason)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("rejectionReason");
            entity.Property(e => e.Status)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Corrector).WithMany(p => p.ConsumptionDebitRecordCorrectedByNavigations)
                .HasForeignKey(d => d.CorrectedBy)
                .HasConstraintName("FK_ConsumptionDebitRecord_Users2");

            entity.HasOne(d => d.Customer).WithMany(p => p.ConsumptionDebitRecordCustomers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConsumptionDebitRecord_Users1");

            entity.HasOne(d => d.Creator).WithMany(p => p.ConsumptionDebitRecordInputtedByNavigations)
                .HasForeignKey(d => d.InputtedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConsumptionDebitRecord_Users");
        });

        modelBuilder.Entity<Fine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Fine");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BillId).HasColumnName("billId");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.FineRuleId).HasColumnName("fineRuleId");

            entity.HasOne(d => d.Bill).WithMany(p => p.Fines)
                .HasForeignKey(d => d.BillId)
                .HasConstraintName("FK_Fine_Bill");

            entity.HasOne(d => d.FineRule).WithMany(p => p.Fines)
                .HasForeignKey(d => d.FineRuleId)
                .HasConstraintName("FK_Fine_FineRule");
        });

        modelBuilder.Entity<FineRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_FineRule");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndDay).HasColumnName("endDay");
            entity.Property(e => e.FineAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("fineAmount");
            entity.Property(e => e.StartDay).HasColumnName("startDay");
        });

        modelBuilder.Entity<ProductionDebitRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ProductionDebitRecord");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Debit)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("debit");
            entity.Property(e => e.InputtedBy).HasColumnName("inputtedBy");
            entity.Property(e => e.Location)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("location");

            entity.HasOne(d => d.Creator).WithMany(p => p.ProductionDebitRecords)
                .HasForeignKey(d => d.InputtedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductionDebitRecord_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Fullname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fullname");
            entity.Property(e => e.Password)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

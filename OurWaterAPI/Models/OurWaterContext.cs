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

            entity.HasOne(d => d.ConsumptionRecord).WithMany(p => p.Bills)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bill_ConsumptionDebitRecord");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bills)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bill_Users");
        });

        modelBuilder.Entity<ConsumptionDebitRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ConsumptionDebitRecord");

            entity.HasOne(d => d.CorrectingUser).WithMany(p => p.ConsumptionDebitRecordCorrectedByNavigations).HasConstraintName("FK_ConsumptionDebitRecord_Users2");

            entity.HasOne(d => d.Customer).WithMany(p => p.ConsumptionDebitRecordCustomers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConsumptionDebitRecord_Users1");

            entity.HasOne(d => d.InputtingUser).WithMany(p => p.ConsumptionDebitRecordInputtedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConsumptionDebitRecord_Users");
        });

        modelBuilder.Entity<Fine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Fine");

            entity.HasOne(d => d.Bill).WithMany(p => p.Fines)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fine_Bill");

            entity.HasOne(d => d.FineRule).WithMany(p => p.Fines)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fine_FineRule");
        });

        modelBuilder.Entity<FineRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_FineRule");
        });

        modelBuilder.Entity<ProductionDebitRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ProductionDebitRecord");

            entity.HasOne(d => d.InputtedByNavigation).WithMany(p => p.ProductionDebitRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductionDebitRecord_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PaylocityPayrollApi.DataAccess.Model;

namespace PaylocityPayrollApi.DataAccess;

public partial class PayrollDbContext : DbContext
{
    public PayrollDbContext(DbContextOptions<PayrollDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Benefit> Benefits { get; set; }

    public virtual DbSet<BenefitDiscount> BenefitDiscounts { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeEnrollment> EmployeeEnrollments { get; set; }

    public virtual DbSet<EmployeePosition> EmployeePositions { get; set; }

    public virtual DbSet<EnrollmentBenefit> EnrollmentBenefits { get; set; }

    public virtual DbSet<EnrollmentDependent> EnrollmentDependents { get; set; }

    public virtual DbSet<PayRun> PayRuns { get; set; }

    public virtual DbSet<PayRunDetail> PayRunDetails { get; set; }

    public virtual DbSet<PayRunEmployee> PayRunEmployees { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Benefit>(entity =>
        {
            entity.Property(e => e.AnnualCostDependent).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.AnnualCostEmployee).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EffectiveDate).HasColumnType("date");
            entity.Property(e => e.EndDate).HasColumnType("date");

            entity.HasOne(d => d.Company).WithMany(p => p.Benefits)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Benefits_Companies");
        });

        modelBuilder.Entity<BenefitDiscount>(entity =>
        {
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.FilterByColumn)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FilterPattern).HasMaxLength(4000);

            entity.HasOne(d => d.Benefit).WithMany(p => p.BenefitDiscounts)
                .HasForeignKey(d => d.BenefitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BenefitDiscounts_Benefits");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Company).WithMany(p => p.Employees)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees_Companies");
        });

        modelBuilder.Entity<EmployeeEnrollment>(entity =>
        {
            entity.Property(e => e.EffectiveDate).HasColumnType("date");
            entity.Property(e => e.EndDate).HasColumnType("date");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeEnrollments)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeEnrollments_Employees");
        });

        modelBuilder.Entity<EmployeePosition>(entity =>
        {
            entity.Property(e => e.PayRate).HasColumnType("decimal(19, 4)");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeePositions)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeePositions_Employees");

            entity.HasOne(d => d.Position).WithMany(p => p.EmployeePositions)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeePositions_Positions");
        });

        modelBuilder.Entity<EnrollmentBenefit>(entity =>
        {
            entity.HasOne(d => d.Benefit).WithMany(p => p.EnrollmentBenefits)
                .HasForeignKey(d => d.BenefitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EnrollmentBenefits_Benefits");

            entity.HasOne(d => d.EmployeeEnrollment).WithMany(p => p.EnrollmentBenefits)
                .HasForeignKey(d => d.EmployeeEnrollmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EnrollmentBenefits_EmployeeEnrollments");
        });

        modelBuilder.Entity<EnrollmentDependent>(entity =>
        {
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.EmployeeEnrollment).WithMany(p => p.EnrollmentDependents)
                .HasForeignKey(d => d.EmployeeEnrollmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EnrollmentDependents_EmployeeEnrollments");
        });

        modelBuilder.Entity<PayRun>(entity =>
        {
            entity.Property(e => e.DeductionsTotal).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.EarningsTotal).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.NetTotal).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.PayDate).HasColumnType("date");
            entity.Property(e => e.PayPeriodFrom).HasColumnType("date");
            entity.Property(e => e.PayPeriodTo).HasColumnType("date");
            entity.Property(e => e.WithholdingsTotal).HasColumnType("decimal(19, 4)");

            entity.HasOne(d => d.Company).WithMany(p => p.PayRuns)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PayRuns_Companies");
        });

        modelBuilder.Entity<PayRunDetail>(entity =>
        {
            entity.Property(e => e.Amount).HasColumnType("decimal(19, 4)");

            entity.HasOne(d => d.PayRunEmployee).WithMany(p => p.PayRunDetails)
                .HasForeignKey(d => d.PayRunEmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PayRunDetails_PayRunEmployees");
        });

        modelBuilder.Entity<PayRunEmployee>(entity =>
        {
            entity.Property(e => e.DeductionsAmount).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.EarningsAmount).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.NetAmount).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.WithholdingsAmount).HasColumnType("decimal(19, 4)");

            entity.HasOne(d => d.Employee).WithMany(p => p.PayRunEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PayRunEmployees_Employees");

            entity.HasOne(d => d.PayRun).WithMany(p => p.PayRunEmployees)
                .HasForeignKey(d => d.PayRunId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PayRunEmployees_PayRuns");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('Employee')");

            entity.HasOne(d => d.Company).WithMany(p => p.Positions)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Positions_Companies");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using paylocity_payroll_api_test01.DataAccess.Model;

namespace paylocity_payroll_api_test01.DataAccess;

public partial class PayrollDataContext : DbContext
{
    public PayrollDataContext()
    {
    }

    public PayrollDataContext(DbContextOptions<PayrollDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TestTable> TestTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:PayrollDatabase");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestTable>(entity =>
        {
            entity.HasKey(e => e.TestId);

            entity.ToTable("TestTable");

            entity.Property(e => e.TestId).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

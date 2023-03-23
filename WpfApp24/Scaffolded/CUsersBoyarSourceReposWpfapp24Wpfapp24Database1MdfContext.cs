using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AdoBD.Scaffolded;

public partial class CUsersBoyarSourceReposWpfapp24Wpfapp24Database1MdfContext : DbContext
{
    public CUsersBoyarSourceReposWpfapp24Wpfapp24Database1MdfContext()
    {
    }

    public CUsersBoyarSourceReposWpfapp24Wpfapp24Database1MdfContext(DbContextOptions<CUsersBoyarSourceReposWpfapp24Wpfapp24Database1MdfContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\boyar\\source\\repos\\WpfApp24\\WpfApp24\\Database1.mdf;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC07046F73E9");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeleteDt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Managers__3214EC075096AD21");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeleteDt).HasColumnType("datetime");
            entity.Property(e => e.IdChief).HasColumnName("Id_chief");
            entity.Property(e => e.IdMainDep).HasColumnName("Id_main_dep");
            entity.Property(e => e.IdSecDep).HasColumnName("Id_sec_dep");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Secname).HasMaxLength(50);
            entity.Property(e => e.Surname).HasMaxLength(50);

            entity.HasOne(d => d.IdMainDepNavigation).WithMany(p => p.ManagerIdMainDepNavigations)
                .HasForeignKey(d => d.IdMainDep)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Managers__Id_mai__02FC7413");

            entity.HasOne(d => d.IdSecDepNavigation).WithMany(p => p.ManagerIdSecDepNavigations)
                .HasForeignKey(d => d.IdSecDep)
                .HasConstraintName("FK__Managers__Id_sec__03F0984C");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC070E857DDA");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeleteDt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sales__3214EC07CA3C2E2B");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeleteDt).HasColumnType("datetime");
            entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");
            entity.Property(e => e.SaleDt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Manager).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Sales__ManagerId__531856C7");

            entity.HasOne(d => d.Product).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Sales__ProductId__51300E55");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

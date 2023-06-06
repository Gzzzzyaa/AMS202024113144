using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AMS202024113144.Models;

public partial class ManageDbContext : DbContext
{
    public ManageDbContext()
    {
    }

    public ManageDbContext(DbContextOptions<ManageDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Member> Members { get; set; }

  //  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
   //     => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB; AttachDbFilename= F:\\vs.test\\AMS202024113144\\AMS202024113144\\App_Data\\ManageDB.mdf;Integrated Security=True;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.AssetId).HasName("PK_dbo.Assets");

            entity.Property(e => e.AssetId).HasColumnName("AssetID");
            entity.Property(e => e.AssetPrice).HasMaxLength(30);
            entity.Property(e => e.AssetSpecification).HasMaxLength(30);
            entity.Property(e => e.AssetTitle).HasMaxLength(30);
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ImgName).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(30);
            entity.Property(e => e.Mid).HasColumnName("MId");
            entity.Property(e => e.PurchaseTime).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Assets)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.Assets_dbo.Categories_CategoryID");

            entity.HasOne(d => d.MidNavigation).WithMany(p => p.Assets)
                .HasForeignKey(d => d.Mid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.Assets_dbo.Members_MId");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK_dbo.Categories");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(30);
            entity.Property(e => e.Description).HasMaxLength(100);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Did).HasName("PK_dbo.Departments");

            entity.Property(e => e.Did).HasColumnName("DId");
            entity.Property(e => e.Dname)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("DName");
            entity.Property(e => e.Mid).HasColumnName("MId");

            entity.HasOne(d => d.MidNavigation).WithMany(p => p.Departments)
                .HasForeignKey(d => d.Mid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.Departments_dbo.Members_MId");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Mid).HasName("PK_dbo.Members");

            entity.Property(e => e.Mid).HasColumnName("MId");
            entity.Property(e => e.Did).HasColumnName("DId");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Password)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.Members)
                .HasForeignKey(d => d.Did)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.Members_dbo.Departments_DId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

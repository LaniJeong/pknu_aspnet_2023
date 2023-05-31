using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace bookRentalShopApi.Models;

public partial class BookrentalshopContext : DbContext
{
    public BookrentalshopContext()
    {
    }

    public BookrentalshopContext(DbContextOptions<BookrentalshopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bookstbl> Bookstbls { get; set; }

    public virtual DbSet<Divtbl> Divtbls { get; set; }

    public virtual DbSet<Membertbl> Membertbls { get; set; }

    public virtual DbSet<Rentaltbl> Rentaltbls { get; set; }

    public virtual DbSet<Usertbl> Usertbls { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//// #warning To protect potentially sensitive information in your connection string,
//// you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//       => optionsBuilder.UseMySql("server=localhost;port=3306;database=bookrentalshop;user=root;password=12345;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bookstbl>(entity =>
        {
            entity.HasKey(e => e.BookIdx).HasName("PRIMARY");

            entity.ToTable("bookstbl");

            entity.HasIndex(e => e.Division, "fk_bookstbl_divtbl_idx");

            entity.Property(e => e.BookIdx).HasColumnName("bookIdx");
            entity.Property(e => e.Author).HasMaxLength(45);
            entity.Property(e => e.Division)
                .HasMaxLength(4)
                .IsFixedLength();
            entity.Property(e => e.Isbn)
                .HasMaxLength(200)
                .HasColumnName("ISBN");
            entity.Property(e => e.Names).HasMaxLength(100);
            entity.Property(e => e.Price).HasPrecision(10);
            entity.Property(e => e.ReleaseDate).HasColumnType("date");

            entity.HasOne(d => d.DivisionNavigation).WithMany(p => p.Bookstbls)
                .HasForeignKey(d => d.Division)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bookstbl_divtbl");
        });

        modelBuilder.Entity<Divtbl>(entity =>
        {
            entity.HasKey(e => e.Division).HasName("PRIMARY");

            entity.ToTable("divtbl");

            entity.Property(e => e.Division)
                .HasMaxLength(4)
                .IsFixedLength();
            entity.Property(e => e.Names).HasMaxLength(45);
        });

        modelBuilder.Entity<Membertbl>(entity =>
        {
            entity.HasKey(e => e.MemberIdx).HasName("PRIMARY");

            entity.ToTable("membertbl");

            entity.Property(e => e.MemberIdx).HasColumnName("memberIdx");
            entity.Property(e => e.Addr).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Levels)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Mobile).HasMaxLength(13);
            entity.Property(e => e.Names).HasMaxLength(45);
        });

        modelBuilder.Entity<Rentaltbl>(entity =>
        {
            entity.HasKey(e => e.RentalIdx).HasName("PRIMARY");

            entity.ToTable("rentaltbl");

            entity.HasIndex(e => e.BookIdx, "fk_rentaltbl_bookstbl1_idx");

            entity.HasIndex(e => e.MemberIdx, "fk_rentaltbl_membertbl1_idx");

            entity.Property(e => e.RentalIdx).HasColumnName("rentalIdx");
            entity.Property(e => e.BookIdx).HasColumnName("bookIdx");
            entity.Property(e => e.MemberIdx).HasColumnName("memberIdx");
            entity.Property(e => e.RentalDate)
                .HasColumnType("date")
                .HasColumnName("rentalDate");
            entity.Property(e => e.ReturnDate)
                .HasColumnType("date")
                .HasColumnName("returnDate");

            entity.HasOne(d => d.BookIdxNavigation).WithMany(p => p.Rentaltbls)
                .HasForeignKey(d => d.BookIdx)
                .HasConstraintName("fk_rentaltbl_bookstbl1");

            entity.HasOne(d => d.MemberIdxNavigation).WithMany(p => p.Rentaltbls)
                .HasForeignKey(d => d.MemberIdx)
                .HasConstraintName("fk_rentaltbl_membertbl1");
        });

        modelBuilder.Entity<Usertbl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usertbl");

            entity.HasIndex(e => e.UserId, "userId_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.UserId)
                .HasMaxLength(12)
                .HasColumnName("userId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

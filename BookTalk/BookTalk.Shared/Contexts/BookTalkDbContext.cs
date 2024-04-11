using System;
using System.Collections.Generic;
using BookTalk.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BookTalk.Shared.Contexts;

public partial class BookTalkDbContext : DbContext
{
    public BookTalkDbContext(DbContextOptions<BookTalkDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BookCategory> BookCategories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CommonCode> CommonCodes { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("book_category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .HasColumnName("category_name");
            entity.Property(e => e.Mall)
                .HasMaxLength(255)
                .HasColumnName("mall");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => new { e.ReviewId, e.CommentId }).HasName("PK__comment__0EF16AF866C2E6ED");

            entity.ToTable("comment");

            entity.Property(e => e.ReviewId).HasColumnName("review_id");
            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.Content)
                .HasColumnType("ntext")
                .HasColumnName("content");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_date");
            entity.Property(e => e.UserId)
                .HasMaxLength(20)
                .HasColumnName("user_id");

            entity.HasOne(d => d.Review).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ReviewId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__comment__review___3B0BC30C");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__comment__user_id__3BFFE745");
        });

        modelBuilder.Entity<CommonCode>(entity =>
        {
            entity.HasKey(e => new { e.Type, e.Code }).HasName("PK__common_c__70AF868685AF189F");

            entity.ToTable("common_code");

            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .HasColumnName("type");
            entity.Property(e => e.Code)
                .HasMaxLength(30)
                .HasColumnName("code");
            entity.Property(e => e.Value)
                .HasMaxLength(100)
                .HasColumnName("value");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__menu__3213E83F8AB2E143");

            entity.ToTable("menu");

            entity.HasIndex(e => e.MenuName, "UQ__menu__4505407DAA904A27").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActionName)
                .HasMaxLength(30)
                .HasColumnName("action_name");
            entity.Property(e => e.ControllerName)
                .HasMaxLength(30)
                .HasColumnName("controller_name");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.MenuName)
                .HasMaxLength(20)
                .HasColumnName("menu_name");
            entity.Property(e => e.ParentMenuId).HasColumnName("parent_menu_id");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__review__3213E83F4A18A95B");

            entity.ToTable("review");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookName)
                .HasMaxLength(200)
                .HasColumnName("book_name");
            entity.Property(e => e.Content)
                .HasColumnType("ntext")
                .HasColumnName("content");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_date");
            entity.Property(e => e.Isbn10)
                .HasMaxLength(10)
                .HasColumnName("isbn10");
            entity.Property(e => e.Isbn13)
                .HasMaxLength(13)
                .HasColumnName("isbn13");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
            entity.Property(e => e.UserId)
                .HasMaxLength(20)
                .HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__review__user_id__373B3228");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F9FB2F06B");

            entity.ToTable("users");

            entity.Property(e => e.Id)
                .HasMaxLength(20)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(500)
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

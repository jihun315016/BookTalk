using System;
using System.Collections.Generic;
using BookTalk.Shared.Common;
using BookTalk.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookTalk.Shared.Contexts;

public partial class BookTalkDbContext : DbContext
{
    public BookTalkDbContext()
    {
    }

    public BookTalkDbContext(DbContextOptions<BookTalkDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CommonCode> CommonCodes { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        string connectionString = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build()
            .GetConnectionString("BookTalkDb");

#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CommonCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__common_c__3213E83FB996CE39");

            entity.ToTable("common_code");

            entity.HasIndex(e => e.Tp, "UQ__common_c__3213E04E46835275").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .HasColumnName("code");
            entity.Property(e => e.Tp)
                .HasMaxLength(20)
                .HasColumnName("tp");
            entity.Property(e => e.Value)
                .HasMaxLength(30)
                .HasColumnName("value");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__menu__3213E83FF82038E2");

            entity.ToTable("menu");

            entity.HasIndex(e => e.MenuName, "UQ__menu__4505407D18724636").IsUnique();

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
            entity.HasKey(e => e.Id).HasName("PK__review__3213E83FB018C183");

            entity.ToTable("review");

            entity.HasIndex(e => e.Isbn, "UQ__review__99F9D0A4FC77284E").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("create_date");
            entity.Property(e => e.DislikeCount)
                .HasDefaultValue(0)
                .HasColumnName("dislike_count");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .HasColumnName("isbn");
            entity.Property(e => e.LikeCount)
                .HasDefaultValue(0)
                .HasColumnName("like_count");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__review__user_id__571DF1D5");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F8F3D0599");

            entity.ToTable("users");

            entity.Property(e => e.Id)
                .HasMaxLength(30)
                .HasColumnName("id");
            entity.Property(e => e.Follower)
                .HasDefaultValue(0)
                .HasColumnName("follower");
            entity.Property(e => e.Following)
                .HasDefaultValue(0)
                .HasColumnName("following");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .HasColumnName("password");
            entity.Property(e => e.ProfileImagePath)
                .HasMaxLength(500)
                .HasColumnName("profile_image_path");

            entity.HasMany(d => d.Followers).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Follow",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__follow__follower__5070F446"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__follow__user_id__4F7CD00D"),
                    j =>
                    {
                        j.HasKey("UserId", "FollowerId").HasName("PK__follow__5DFAD42D1744593E");
                        j.ToTable("follow");
                        j.IndexerProperty<string>("UserId")
                            .HasMaxLength(30)
                            .HasColumnName("user_id");
                        j.IndexerProperty<string>("FollowerId")
                            .HasMaxLength(30)
                            .HasColumnName("follower_id");
                    });

            entity.HasMany(d => d.Users).WithMany(p => p.Followers)
                .UsingEntity<Dictionary<string, object>>(
                    "Follow",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__follow__user_id__4F7CD00D"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__follow__follower__5070F446"),
                    j =>
                    {
                        j.HasKey("UserId", "FollowerId").HasName("PK__follow__5DFAD42D1744593E");
                        j.ToTable("follow");
                        j.IndexerProperty<string>("UserId")
                            .HasMaxLength(30)
                            .HasColumnName("user_id");
                        j.IndexerProperty<string>("FollowerId")
                            .HasMaxLength(30)
                            .HasColumnName("follower_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

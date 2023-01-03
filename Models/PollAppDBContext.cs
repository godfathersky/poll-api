using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PollAPI.Models
{
    public partial class PollAppDBContext : DbContext
    {
        public PollAppDBContext()
        {
        }

        public PollAppDBContext(DbContextOptions<PollAppDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Poll> Polls { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(local);Database=PollAppDB;Trusted_Connection=True;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Poll>(entity =>
            {
                entity.ToTable("Poll");

                entity.Property(e => e.PollAnswer)
                    .HasColumnType("text")
                    .HasColumnName("pollAnswer");

                entity.Property(e => e.PollId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pollId");

                entity.Property(e => e.PollTitle)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("pollTitle");

                entity.Property(e => e.PollType)
                    .HasColumnType("text")
                    .HasColumnName("pollType");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("roleName");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.UserAddTime)
                    .HasColumnType("time(2)")
                    .HasColumnName("userAddTime");

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("userEmail");

                entity.Property(e => e.UserLogin)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("userLogin");

                entity.Property(e => e.UserPasswordHash)
                    .HasMaxLength(64)
                    .HasColumnName("userPasswordHash")
                    .IsFixedLength();

                entity.Property(e => e.UserPasswordSalt)
                    .HasMaxLength(128)
                    .HasColumnName("userPasswordSalt")
                    .IsFixedLength();
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.UserId, e.RoleId });

                entity.ToTable("UserRole");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

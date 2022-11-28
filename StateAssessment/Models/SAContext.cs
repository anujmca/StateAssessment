using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace StateAssessment.Models
{
    public partial class SAContext : DbContext
    {
        public SAContext()
        {
        }

        public SAContext(DbContextOptions<SAContext> options)
            : base(options)
        {
        }

        public virtual DbSet<QuestionSuggestedAnswer> QuestionSuggestedAnswers { get; set; } = null!;
        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<AnswerType> AnswerTypes { get; set; } = null!;
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<Assessment> Assessments { get; set; } = null!;
        public virtual DbSet<AssessmentAnswer> AssessmentAnswers { get; set; } = null!;
        public virtual DbSet<Inventory> Inventories { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<QuestionType> QuestionTypes { get; set; } = null!;
        //public virtual DbSet<User> Users { get; set; } = null!;
        //public virtual DbSet<UserType> UserTypes { get; set; } = null!;

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. 
//                //You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=DESKTOP-6PEAPVV;Database=SA;Trusted_Connection=True;MultipleActiveResultSets=true;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuestionSuggestedAnswer>(entity =>
            {
                entity.ToTable("QuestionSuggestedAnswer");

                entity.Property(e => e.Description).HasMaxLength(3000);

                entity.Property(e => e.Score).HasColumnType("decimal(28, 8)");

                entity.Property(e => e.Title).HasMaxLength(500);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionSuggestedAnswers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_QuestionSuggestedAnswer_QuestionId");
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("Answer");

                entity.Property(e => e.AnswerTypeCode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Description).HasMaxLength(3000);

                entity.Property(e => e.Score).HasColumnType("decimal(28, 8)");

                entity.Property(e => e.Title).HasMaxLength(500);

                entity.HasOne(d => d.AnswerTypeCodeNavigation)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.AnswerTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Answer_AnswerTypeCode");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Answer_QuestionId");
            });

            modelBuilder.Entity<AnswerType>(entity =>
            {
                entity.HasKey(e => e.AnswerTypeCode)
                    .HasName("pk_AnswerType");

                entity.ToTable("AnswerType");

                entity.Property(e => e.AnswerTypeCode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.AnswerTypeName).HasMaxLength(100);
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Assessment>(entity =>
            {
                entity.ToTable("Assessment");

                entity.Property(e => e.EarnedScore).HasColumnType("decimal(28, 8)");

                entity.HasOne(d => d.Inventory)
                    .WithMany(p => p.Assessments)
                    .HasForeignKey(d => d.InventoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Assessment_QuestionId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Assessments)
                    .HasForeignKey(d => d.AssesseeUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Assessment_AssesseeUserId");
            });

            modelBuilder.Entity<AssessmentAnswer>(entity =>
            {
                entity.ToTable("AssessmentAnswer");

                entity.HasOne(d => d.Question)
                   .WithMany(p => p.AssessmentAnswers)
                   .HasForeignKey(d => d.QuestionId)
                   .HasConstraintName("fk_AssessmentAnswer_QuestionId");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.AssessmentAnswers)
                    .HasForeignKey(d => d.SuggestedAnswerId)
                    .HasConstraintName("fk_Assessment_SuggestedAnswerId");

                entity.HasOne(d => d.Assessment)
                    .WithMany(p => p.AssessmentAnswers)
                    .HasForeignKey(d => d.AssessmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AssessmentAnswer_AssessmentId");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.ToTable("Inventory");

                entity.Property(e => e.InventoryDescription).HasMaxLength(1000);

                entity.Property(e => e.InventoryName).HasMaxLength(100);

                entity.Property(e => e.SectionName).HasMaxLength(50);

                entity.HasOne(d => d.ParentInventory)
                    .WithMany(p => p.InverseParentInventory)
                    .HasForeignKey(d => d.ParentInventoryId)
                    .HasConstraintName("fk_Inventory_ParentInventoryId");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.Description).HasMaxLength(3000);

                entity.Property(e => e.QuestionTypeCode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Title).HasMaxLength(500);

                entity.HasOne(d => d.Inventory)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.InventoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Question_InventoryId");

                entity.HasOne(d => d.QuestionTypeCodeNavigation)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.QuestionTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Question_QuestionTypeCode");
            });

            modelBuilder.Entity<QuestionType>(entity =>
            {
                entity.HasKey(e => e.QuestionTypeCode)
                    .HasName("pk_QuestionType");

                entity.ToTable("QuestionType");

                entity.Property(e => e.QuestionTypeCode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.QuestionTypeName).HasMaxLength(100);
            });

            //modelBuilder.Entity<User>(entity =>
            //{
            //    entity.ToTable("User");

            //    entity.Property(e => e.UserEmail)
            //        .HasMaxLength(200)
            //        .IsUnicode(false);

            //    entity.Property(e => e.UserName)
            //        .HasMaxLength(100)
            //        .IsUnicode(false);

            //    entity.Property(e => e.UserTypeCode)
            //        .HasMaxLength(1)
            //        .IsUnicode(false)
            //        .IsFixedLength();

            //    entity.HasOne(d => d.UserTypeCodeNavigation)
            //        .WithMany(p => p.Users)
            //        .HasForeignKey(d => d.UserTypeCode)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("fk_User_UserTypeCode");
            //});

            //modelBuilder.Entity<UserType>(entity =>
            //{
            //    entity.HasKey(e => e.UserTypeCode)
            //        .HasName("pk_UserTypeCode");

            //    entity.ToTable("UserType");

            //    entity.Property(e => e.UserTypeCode)
            //        .HasMaxLength(1)
            //        .IsUnicode(false)
            //        .IsFixedLength();

            //    entity.Property(e => e.UserTypeName).HasMaxLength(100);
            //});

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

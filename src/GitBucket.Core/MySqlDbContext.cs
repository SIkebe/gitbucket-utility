using System;
using GitBucket.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GitBucket.Core
{
    public partial class MySqlDbContext : DbContext
    {
        public MySqlDbContext(string connectionString)
            => ConnectionString = connectionString;

        public MySqlDbContext(DbContextOptions<MySqlDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccessToken> AccessToken { get; set; }
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountExtraMailAddress> AccountExtraMailAddress { get; set; }
        public virtual DbSet<AccountFederation> AccountFederation { get; set; }
        public virtual DbSet<AccountWebHook> AccountWebHook { get; set; }
        public virtual DbSet<AccountWebHookEvent> AccountWebHookEvent { get; set; }
        public virtual DbSet<Collaborator> Collaborator { get; set; }
        public virtual DbSet<CommitComment> CommitComment { get; set; }
        public virtual DbSet<CommitStatus> CommitStatus { get; set; }
        public virtual DbSet<DeployKey> DeployKey { get; set; }
        public virtual DbSet<Gist> Gist { get; set; }
        public virtual DbSet<GistComment> GistComment { get; set; }
        public virtual DbSet<GpgKey> GpgKey { get; set; }
        public virtual DbSet<GroupMember> GroupMember { get; set; }
        public virtual DbSet<Issue> Issue { get; set; }
        public virtual DbSet<IssueComment> IssueComment { get; set; }
        public virtual DbSet<IssueId> IssueId { get; set; }
        public virtual DbSet<IssueLabel> IssueLabel { get; set; }
        public virtual DbSet<IssueNotification> IssueNotification { get; set; }
        public virtual DbSet<IssueOutlineView> IssueOutlineView { get; set; }
        public virtual DbSet<Label> Label { get; set; }
        public virtual DbSet<Milestone> Milestone { get; set; }
        public virtual DbSet<NotificationsAccount> NotificationsAccount { get; set; }
        public virtual DbSet<Pages> Pages { get; set; }
        public virtual DbSet<Plugin> Plugin { get; set; }
        public virtual DbSet<Priority> Priority { get; set; }
        public virtual DbSet<ProtectedBranch> ProtectedBranch { get; set; }
        public virtual DbSet<ProtectedBranchRequireContext> ProtectedBranchRequireContext { get; set; }
        public virtual DbSet<PullRequest> PullRequest { get; set; }
        public virtual DbSet<ReleaseAsset> ReleaseAsset { get; set; }
        public virtual DbSet<ReleaseTag> ReleaseTag { get; set; }
        public virtual DbSet<Repository> Repository { get; set; }
        public virtual DbSet<SshKey> SshKey { get; set; }
        public virtual DbSet<Versions> Versions { get; set; }
        public virtual DbSet<Watch> Watch { get; set; }
        public virtual DbSet<WebHook> WebHook { get; set; }
        public virtual DbSet<WebHookEvent> WebHookEvent { get; set; }

        public string ConnectionString { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder == null)
            {
                throw new ArgumentNullException(nameof(optionsBuilder));
            }

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<AccessToken>(entity =>
            {
                entity.ToTable("ACCESS_TOKEN");

                entity.HasIndex(e => e.AccessTokenId)
                    .HasName("ACCESS_TOKEN_ID")
                    .IsUnique();

                entity.HasIndex(e => e.TokenHash)
                    .HasName("IDX_ACCESS_TOKEN_TOKEN_HASH")
                    .IsUnique();

                entity.HasIndex(e => e.UserName)
                    .HasName("IDX_ACCESS_TOKEN_FK0");

                entity.Property(e => e.AccessTokenId).HasColumnName("ACCESS_TOKEN_ID");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasColumnName("NOTE")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.TokenHash)
                    .IsRequired()
                    .HasColumnName("TOKEN_HASH")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.AccessToken)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("IDX_ACCESS_TOKEN_FK0");
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("PRIMARY");

                entity.ToTable("ACCOUNT");

                entity.HasIndex(e => e.MailAddress)
                    .HasName("IDX_ACCOUNT_1")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Administrator)
                    .HasColumnName("ADMINISTRATOR")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasColumnName("FULL_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.GroupAccount)
                    .HasColumnName("GROUP_ACCOUNT")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.Image)
                    .HasColumnName("IMAGE")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.LastLoginDate)
                    .HasColumnName("LAST_LOGIN_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.MailAddress)
                    .IsRequired()
                    .HasColumnName("MAIL_ADDRESS")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Password)
                    .HasColumnName("PASSWORD")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("REGISTERED_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Removed)
                    .HasColumnName("REMOVED")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("UPDATED_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<AccountExtraMailAddress>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.ExtraMailAddress })
                    .HasName("PRIMARY");

                entity.ToTable("ACCOUNT_EXTRA_MAIL_ADDRESS");

                entity.HasIndex(e => e.ExtraMailAddress)
                    .HasName("IDX_ACCOUNT_EXTRA_MAIL_ADDRESS_1")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ExtraMailAddress)
                    .HasColumnName("EXTRA_MAIL_ADDRESS")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<AccountFederation>(entity =>
            {
                entity.HasKey(e => new { e.Issuer, e.Subject })
                    .HasName("PRIMARY");

                entity.ToTable("ACCOUNT_FEDERATION");

                entity.HasIndex(e => e.UserName)
                    .HasName("IDX_ACCOUNT_FEDERATION_FK0");

                entity.Property(e => e.Issuer)
                    .HasColumnName("ISSUER")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Subject)
                    .HasColumnName("SUBJECT")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.AccountFederation)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_ACCOUNT_FEDERATION_FK0");
            });

            modelBuilder.Entity<AccountWebHook>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.Url })
                    .HasName("PRIMARY");

                entity.ToTable("ACCOUNT_WEB_HOOK");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Ctype)
                    .HasColumnName("CTYPE")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Token)
                    .HasColumnName("TOKEN")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.AccountWebHook)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_ACCOUNT_WEB_HOOK_FK0");
            });

            modelBuilder.Entity<AccountWebHookEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ACCOUNT_WEB_HOOK_EVENT");

                entity.Property(e => e.Event)
                    .IsRequired()
                    .HasColumnName("EVENT")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("URL")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Collaborator>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.CollaboratorName })
                    .HasName("PRIMARY");

                entity.ToTable("COLLABORATOR");

                entity.HasIndex(e => e.CollaboratorName)
                    .HasName("IDX_COLLABORATOR_FK1");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CollaboratorName)
                    .HasColumnName("COLLABORATOR_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasColumnName("ROLE")
                    .HasColumnType("varchar(10)")
                    .HasDefaultValueSql("'ADMIN'")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.CollaboratorNameNavigation)
                    .WithMany(p => p.Collaborator)
                    .HasForeignKey(d => d.CollaboratorName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_COLLABORATOR_FK1");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Collaborator)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_COLLABORATOR_FK0");
            });

            modelBuilder.Entity<CommitComment>(entity =>
            {
                entity.HasKey(e => e.CommentId)
                    .HasName("PRIMARY");

                entity.ToTable("COMMIT_COMMENT");

                entity.HasIndex(e => e.CommentId)
                    .HasName("COMMENT_ID")
                    .IsUnique();

                entity.HasIndex(e => new { e.UserName, e.RepositoryName })
                    .HasName("IDX_COMMIT_COMMENT_FK0");

                entity.Property(e => e.CommentId).HasColumnName("COMMENT_ID");

                entity.Property(e => e.CommentedUserName)
                    .IsRequired()
                    .HasColumnName("COMMENTED_USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CommitId)
                    .IsRequired()
                    .HasColumnName("COMMIT_ID")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("CONTENT")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FileName)
                    .HasColumnName("FILE_NAME")
                    .HasColumnType("varchar(260)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IssueId).HasColumnName("ISSUE_ID");

                entity.Property(e => e.NewLineNumber).HasColumnName("NEW_LINE_NUMBER");

                entity.Property(e => e.OldLineNumber).HasColumnName("OLD_LINE_NUMBER");

                entity.Property(e => e.OriginalCommitId)
                    .IsRequired()
                    .HasColumnName("ORIGINAL_COMMIT_ID")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.OriginalNewLine).HasColumnName("ORIGINAL_NEW_LINE");

                entity.Property(e => e.OriginalOldLine).HasColumnName("ORIGINAL_OLD_LINE");

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("REGISTERED_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("UPDATED_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.CommitComment)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_COMMIT_COMMENT_FK0");
            });

            modelBuilder.Entity<CommitStatus>(entity =>
            {
                entity.ToTable("COMMIT_STATUS");

                entity.HasIndex(e => e.CommitStatusId)
                    .HasName("COMMIT_STATUS_ID")
                    .IsUnique();

                entity.HasIndex(e => e.Creator)
                    .HasName("IDX_COMMIT_STATUS_FK3");

                entity.HasIndex(e => new { e.UserName, e.RepositoryName, e.CommitId, e.Context })
                    .HasName("IDX_COMMIT_STATUS_1")
                    .IsUnique();

                entity.Property(e => e.CommitStatusId).HasColumnName("COMMIT_STATUS_ID");

                entity.Property(e => e.CommitId)
                    .IsRequired()
                    .HasColumnName("COMMIT_ID")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Context)
                    .IsRequired()
                    .HasColumnName("CONTEXT")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Creator)
                    .IsRequired()
                    .HasColumnName("CREATOR")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("REGISTERED_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("STATE")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.TargetUrl)
                    .HasColumnName("TARGET_URL")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("UPDATED_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.CreatorNavigation)
                    .WithMany(p => p.CommitStatusCreatorNavigation)
                    .HasForeignKey(d => d.Creator)
                    .HasConstraintName("IDX_COMMIT_STATUS_FK3");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.CommitStatusUserNameNavigation)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("IDX_COMMIT_STATUS_FK2");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.CommitStatus)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .HasConstraintName("IDX_COMMIT_STATUS_FK1");
            });

            modelBuilder.Entity<DeployKey>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.DeployKeyId })
                    .HasName("PRIMARY");

                entity.ToTable("DEPLOY_KEY");

                entity.HasIndex(e => e.DeployKeyId)
                    .HasName("DEPLOY_KEY_ID")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.DeployKeyId)
                    .HasColumnName("DEPLOY_KEY_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AllowWrite)
                    .HasColumnName("ALLOW_WRITE")
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.PublicKey)
                    .IsRequired()
                    .HasColumnName("PUBLIC_KEY")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("TITLE")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.DeployKey)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_DEPLOY_KEY_FK0");
            });

            modelBuilder.Entity<Gist>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName })
                    .HasName("PRIMARY");

                entity.ToTable("GIST");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Mode)
                    .IsRequired()
                    .HasColumnName("MODE")
                    .HasColumnType("varchar(10)")
                    .HasDefaultValueSql("'PUBLIC'")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.OriginRepositoryName)
                    .HasColumnName("ORIGIN_REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.OriginUserName)
                    .HasColumnName("ORIGIN_USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("REGISTERED_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("TITLE")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("UPDATED_DATE")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Gist)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_GIST_FK0");
            });

            modelBuilder.Entity<GistComment>(entity =>
            {
                entity.HasKey(e => e.CommentId)
                    .HasName("PRIMARY");

                entity.ToTable("GIST_COMMENT");

                entity.HasIndex(e => e.CommentId)
                    .HasName("COMMENT_ID")
                    .IsUnique();

                entity.HasIndex(e => e.CommentedUserName)
                    .HasName("IDX_GIST_COMMENT_FK1");

                entity.HasIndex(e => new { e.UserName, e.RepositoryName, e.CommentId })
                    .HasName("IDX_GIST_COMMENT_1")
                    .IsUnique();

                entity.Property(e => e.CommentId).HasColumnName("COMMENT_ID");

                entity.Property(e => e.CommentedUserName)
                    .IsRequired()
                    .HasColumnName("COMMENTED_USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("CONTENT")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("REGISTERED_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("UPDATED_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.CommentedUserNameNavigation)
                    .WithMany(p => p.GistComment)
                    .HasForeignKey(d => d.CommentedUserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_GIST_COMMENT_FK1");

                entity.HasOne(d => d.Gist)
                    .WithMany(p => p.GistComment)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_GIST_COMMENT_FK0");
            });

            modelBuilder.Entity<GpgKey>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.GpgKeyId })
                    .HasName("PRIMARY");

                entity.ToTable("GPG_KEY");

                entity.HasIndex(e => e.KeyId)
                    .HasName("KEY_ID")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.GpgKeyId).HasColumnName("GPG_KEY_ID");

                entity.Property(e => e.KeyId)
                    .HasColumnName("KEY_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.PublicKey)
                    .IsRequired()
                    .HasColumnName("PUBLIC_KEY")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("TITLE")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.GpgKey)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_GPG_KEY_FK0");
            });

            modelBuilder.Entity<GroupMember>(entity =>
            {
                entity.HasKey(e => new { e.GroupName, e.UserName })
                    .HasName("PRIMARY");

                entity.ToTable("GROUP_MEMBER");

                entity.HasIndex(e => e.UserName)
                    .HasName("IDX_GROUP_MEMBER_FK1");

                entity.Property(e => e.GroupName)
                    .HasColumnName("GROUP_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Manager)
                    .HasColumnName("MANAGER")
                    .HasColumnType("bit(1)");

                entity.HasOne(d => d.GroupNameNavigation)
                    .WithMany(p => p.GroupMemberGroupNameNavigation)
                    .HasForeignKey(d => d.GroupName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_GROUP_MEMBER_FK0");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.GroupMemberUserNameNavigation)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_GROUP_MEMBER_FK1");
            });

            modelBuilder.Entity<Issue>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.IssueId })
                    .HasName("PRIMARY");

                entity.ToTable("ISSUE");

                entity.HasIndex(e => e.MilestoneId)
                    .HasName("IDX_ISSUE_FK2");

                entity.HasIndex(e => e.OpenedUserName)
                    .HasName("IDX_ISSUE_FK1");

                entity.HasIndex(e => e.PriorityId)
                    .HasName("IDX_ISSUE_FK3");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IssueId).HasColumnName("ISSUE_ID");

                entity.Property(e => e.AssignedUserName)
                    .HasColumnName("ASSIGNED_USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Closed)
                    .HasColumnName("CLOSED")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.Content)
                    .HasColumnName("CONTENT")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.MilestoneId).HasColumnName("MILESTONE_ID");

                entity.Property(e => e.OpenedUserName)
                    .IsRequired()
                    .HasColumnName("OPENED_USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.PriorityId).HasColumnName("PRIORITY_ID");

                entity.Property(e => e.PullRequest)
                    .HasColumnName("PULL_REQUEST")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("REGISTERED_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("TITLE")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("UPDATED_DATE")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Milestone)
                    .WithMany(p => p.Issue)
                    .HasPrincipalKey(p => p.MilestoneId)
                    .HasForeignKey(d => d.MilestoneId)
                    .HasConstraintName("IDX_ISSUE_FK2");

                entity.HasOne(d => d.OpenedUserNameNavigation)
                    .WithMany(p => p.Issue)
                    .HasForeignKey(d => d.OpenedUserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_ISSUE_FK1");

                entity.HasOne(d => d.Priority)
                    .WithMany(p => p.Issue)
                    .HasPrincipalKey(p => p.PriorityId)
                    .HasForeignKey(d => d.PriorityId)
                    .HasConstraintName("IDX_ISSUE_FK3");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Issue)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_ISSUE_FK0");
            });

            modelBuilder.Entity<IssueComment>(entity =>
            {
                entity.HasKey(e => e.CommentId)
                    .HasName("PRIMARY");

                entity.ToTable("ISSUE_COMMENT");

                entity.HasIndex(e => e.CommentId)
                    .HasName("COMMENT_ID")
                    .IsUnique();

                entity.HasIndex(e => new { e.UserName, e.RepositoryName, e.IssueId })
                    .HasName("IDX_ISSUE_COMMENT_FK0");

                entity.Property(e => e.CommentId).HasColumnName("COMMENT_ID");

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasColumnName("ACTION")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CommentedUserName)
                    .IsRequired()
                    .HasColumnName("COMMENTED_USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("CONTENT")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IssueId).HasColumnName("ISSUE_ID");

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("REGISTERED_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("UPDATED_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.IssueComment)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName, d.IssueId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_ISSUE_COMMENT_FK0");
            });

            modelBuilder.Entity<IssueId>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName })
                    .HasName("PRIMARY");

                entity.ToTable("ISSUE_ID");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IssueId1).HasColumnName("ISSUE_ID");

                entity.HasOne(d => d.Repository)
                    .WithOne(p => p.IssueId)
                    .HasForeignKey<IssueId>(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_ISSUE_ID_FK1");
            });

            modelBuilder.Entity<IssueLabel>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.IssueId, e.LabelId })
                    .HasName("PRIMARY");

                entity.ToTable("ISSUE_LABEL");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IssueId).HasColumnName("ISSUE_ID");

                entity.Property(e => e.LabelId).HasColumnName("LABEL_ID");

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.IssueLabel)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName, d.IssueId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_ISSUE_LABEL_FK0");
            });

            modelBuilder.Entity<IssueNotification>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.IssueId, e.NotificationUserName })
                    .HasName("PRIMARY");

                entity.ToTable("ISSUE_NOTIFICATION");

                entity.HasIndex(e => e.NotificationUserName)
                    .HasName("IDX_ISSUE_NOTIFICATION_FK1");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IssueId).HasColumnName("ISSUE_ID");

                entity.Property(e => e.NotificationUserName)
                    .HasColumnName("NOTIFICATION_USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Subscribed)
                    .HasColumnName("SUBSCRIBED")
                    .HasColumnType("bit(1)");
            });

            modelBuilder.Entity<IssueOutlineView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ISSUE_OUTLINE_VIEW");

                entity.Property(e => e.CommentCount).HasColumnName("COMMENT_COUNT");

                entity.Property(e => e.IssueId).HasColumnName("ISSUE_ID");

                entity.Property(e => e.Priority).HasColumnName("PRIORITY");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Label>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.LabelId })
                    .HasName("PRIMARY");

                entity.ToTable("LABEL");

                entity.HasIndex(e => e.LabelId)
                    .HasName("LABEL_ID")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.LabelId)
                    .HasColumnName("LABEL_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasColumnName("COLOR")
                    .HasColumnType("char(6)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.LabelName)
                    .IsRequired()
                    .HasColumnName("LABEL_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Label)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_LABEL_FK0");
            });

            modelBuilder.Entity<Milestone>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.MilestoneId })
                    .HasName("PRIMARY");

                entity.ToTable("MILESTONE");

                entity.HasIndex(e => e.MilestoneId)
                    .HasName("MILESTONE_ID")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.MilestoneId)
                    .HasColumnName("MILESTONE_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ClosedDate)
                    .HasColumnName("CLOSED_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.DueDate)
                    .HasColumnName("DUE_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("TITLE")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Milestone)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_MILESTONE_FK0");
            });

            modelBuilder.Entity<NotificationsAccount>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("PRIMARY");

                entity.ToTable("NOTIFICATIONS_ACCOUNT");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.DisableEmail)
                    .HasColumnName("DISABLE_EMAIL")
                    .HasColumnType("bit(1)");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithOne(p => p.NotificationsAccount)
                    .HasForeignKey<NotificationsAccount>(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_NOTIFICATIONS_ACCOUNT_FK0");
            });

            modelBuilder.Entity<Pages>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName })
                    .HasName("PRIMARY");

                entity.ToTable("PAGES");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("SOURCE")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Plugin>(entity =>
            {
                entity.ToTable("PLUGIN");

                entity.Property(e => e.PluginId)
                    .HasColumnName("PLUGIN_ID")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasColumnName("VERSION")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Priority>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.PriorityId })
                    .HasName("PRIMARY");

                entity.ToTable("PRIORITY");

                entity.HasIndex(e => e.PriorityId)
                    .HasName("PRIORITY_ID")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.PriorityId)
                    .HasColumnName("PRIORITY_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasColumnName("COLOR")
                    .HasColumnType("char(6)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IsDefault)
                    .HasColumnName("IS_DEFAULT")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.Ordering).HasColumnName("ORDERING");

                entity.Property(e => e.PriorityName)
                    .IsRequired()
                    .HasColumnName("PRIORITY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Priority)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_PRIORITY_FK0");
            });

            modelBuilder.Entity<ProtectedBranch>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.Branch })
                    .HasName("PRIMARY");

                entity.ToTable("PROTECTED_BRANCH");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Branch)
                    .HasColumnName("BRANCH")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.StatusCheckAdmin)
                    .HasColumnName("STATUS_CHECK_ADMIN")
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.ProtectedBranch)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .HasConstraintName("IDX_PROTECTED_BRANCH_FK0");
            });

            modelBuilder.Entity<ProtectedBranchRequireContext>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.Branch, e.Context })
                    .HasName("PRIMARY");

                entity.ToTable("PROTECTED_BRANCH_REQUIRE_CONTEXT");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Branch)
                    .HasColumnName("BRANCH")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Context)
                    .HasColumnName("CONTEXT")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.ProtectedBranch)
                    .WithMany(p => p.ProtectedBranchRequireContext)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName, d.Branch })
                    .HasConstraintName("IDX_PROTECTED_BRANCH_REQUIRE_CONTEXT_FK0");
            });

            modelBuilder.Entity<PullRequest>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.IssueId })
                    .HasName("PRIMARY");

                entity.ToTable("PULL_REQUEST");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IssueId).HasColumnName("ISSUE_ID");

                entity.Property(e => e.Branch)
                    .IsRequired()
                    .HasColumnName("BRANCH")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CommitIdFrom)
                    .IsRequired()
                    .HasColumnName("COMMIT_ID_FROM")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CommitIdTo)
                    .IsRequired()
                    .HasColumnName("COMMIT_ID_TO")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IsDraft)
                    .HasColumnName("IS_DRAFT")
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.RequestBranch)
                    .IsRequired()
                    .HasColumnName("REQUEST_BRANCH")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RequestRepositoryName)
                    .IsRequired()
                    .HasColumnName("REQUEST_REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RequestUserName)
                    .IsRequired()
                    .HasColumnName("REQUEST_USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Issue)
                    .WithOne(p => p.PullRequestNavigation)
                    .HasForeignKey<PullRequest>(d => new { d.UserName, d.RepositoryName, d.IssueId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_PULL_REQUEST_FK0");
            });

            modelBuilder.Entity<ReleaseAsset>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.Tag, e.FileName })
                    .HasName("PRIMARY");

                entity.ToTable("RELEASE_ASSET");

                entity.HasIndex(e => e.ReleaseAssetId)
                    .HasName("RELEASE_ASSET_ID")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Tag)
                    .HasColumnName("TAG")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FileName)
                    .HasColumnName("FILE_NAME")
                    .HasColumnType("varchar(260)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Label)
                    .HasColumnName("LABEL")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("REGISTERED_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.ReleaseAssetId)
                    .HasColumnName("RELEASE_ASSET_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Size).HasColumnName("SIZE");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("UPDATED_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Uploader)
                    .IsRequired()
                    .HasColumnName("UPLOADER")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.ReleaseTag)
                    .WithMany(p => p.ReleaseAsset)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName, d.Tag })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_RELEASE_ASSET_FK0");
            });

            modelBuilder.Entity<ReleaseTag>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.Tag })
                    .HasName("PRIMARY");

                entity.ToTable("RELEASE_TAG");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Tag)
                    .HasColumnName("TAG")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasColumnName("AUTHOR")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Content)
                    .HasColumnName("CONTENT")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("REGISTERED_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("UPDATED_DATE")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.ReleaseTag)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_RELEASE_TAG_FK0");
            });

            modelBuilder.Entity<Repository>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName })
                    .HasName("PRIMARY");

                entity.ToTable("REPOSITORY");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.AllowFork)
                    .HasColumnName("ALLOW_FORK")
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'1'");

                entity.Property(e => e.DefaultBranch)
                    .HasColumnName("DEFAULT_BRANCH")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.DefaultMergeOption)
                    .IsRequired()
                    .HasColumnName("DEFAULT_MERGE_OPTION")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("'merge-commit'")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ExternalIssuesUrl)
                    .HasColumnName("EXTERNAL_ISSUES_URL")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ExternalWikiUrl)
                    .HasColumnName("EXTERNAL_WIKI_URL")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IssuesOption)
                    .IsRequired()
                    .HasColumnName("ISSUES_OPTION")
                    .HasColumnType("varchar(10)")
                    .HasDefaultValueSql("'DISABLE'")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.LastActivityDate)
                    .HasColumnName("LAST_ACTIVITY_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.MergeOptions)
                    .IsRequired()
                    .HasColumnName("MERGE_OPTIONS")
                    .HasColumnType("varchar(200)")
                    .HasDefaultValueSql("'merge-commit,squash,rebase'")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.OriginRepositoryName)
                    .HasColumnName("ORIGIN_REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.OriginUserName)
                    .HasColumnName("ORIGIN_USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ParentRepositoryName)
                    .HasColumnName("PARENT_REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.ParentUserName)
                    .HasColumnName("PARENT_USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Private)
                    .HasColumnName("PRIVATE")
                    .HasColumnType("bit(1)");

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("REGISTERED_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("UPDATED_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.WikiOption)
                    .IsRequired()
                    .HasColumnName("WIKI_OPTION")
                    .HasColumnType("varchar(10)")
                    .HasDefaultValueSql("'DISABLE'")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Repository)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_REPOSITORY_FK0");
            });

            modelBuilder.Entity<SshKey>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.SshKeyId })
                    .HasName("PRIMARY");

                entity.ToTable("SSH_KEY");

                entity.HasIndex(e => e.SshKeyId)
                    .HasName("SSH_KEY_ID")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SshKeyId)
                    .HasColumnName("SSH_KEY_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.PublicKey)
                    .IsRequired()
                    .HasColumnName("PUBLIC_KEY")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("TITLE")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.SshKey)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_SSH_KEY_FK0");
            });

            modelBuilder.Entity<Versions>(entity =>
            {
                entity.HasKey(e => e.ModuleId)
                    .HasName("PRIMARY");

                entity.ToTable("VERSIONS");

                entity.Property(e => e.ModuleId)
                    .HasColumnName("MODULE_ID")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasColumnName("VERSION")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Watch>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.NotificationUserName })
                    .HasName("PRIMARY");

                entity.ToTable("WATCH");

                entity.HasIndex(e => e.NotificationUserName)
                    .HasName("IDX_WATCH_FK1");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.NotificationUserName)
                    .HasColumnName("NOTIFICATION_USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Notification)
                    .IsRequired()
                    .HasColumnName("NOTIFICATION")
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<WebHook>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.Url })
                    .HasName("PRIMARY");

                entity.ToTable("WEB_HOOK");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Ctype)
                    .HasColumnName("CTYPE")
                    .HasColumnType("varchar(10)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Token)
                    .HasColumnName("TOKEN")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.WebHook)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("IDX_WEB_HOOK_FK0");
            });

            modelBuilder.Entity<WebHookEvent>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.Url, e.Event })
                    .HasName("PRIMARY");

                entity.ToTable("WEB_HOOK_EVENT");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("REPOSITORY_NAME")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Event)
                    .HasColumnName("EVENT")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.WebHook)
                    .WithMany(p => p.WebHookEvent)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName, d.Url })
                    .HasConstraintName("IDX_WEB_HOOK_EVENT_FK0");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

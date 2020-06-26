using System;
using GitBucket.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GitBucket.Core
{
    /// <summary>
    /// DBContext class against GitBucket DB.
    /// Use scaffold.bat to regenerate.
    /// </summary>
    public partial class GitBucketDbContext : DbContext
    {
        public GitBucketDbContext(string connectionString)
            => ConnectionString = connectionString;

        public GitBucketDbContext(DbContextOptions<GitBucketDbContext> options)
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

        // The column 'public.repository.allow_fork' would normally be mapped to a non-nullable bool property, but it has a default constraint. Such a column is mapped to a nullable bool property to allow a difference between setting the property to false and invoking the default constraint. See https://go.microsoft.com/fwlink/?linkid=851278 for details.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder == null)
            {
                throw new ArgumentNullException(nameof(optionsBuilder));
            }

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(ConnectionString);
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
                entity.ToTable("access_token");

                entity.HasIndex(x => x.AccessTokenId)
                    .HasName("access_token_access_token_id_key")
                    .IsUnique();

                entity.HasIndex(x => x.TokenHash)
                    .HasName("idx_access_token_token_hash")
                    .IsUnique();

                entity.Property(e => e.AccessTokenId)
                    .HasColumnName("access_token_id")
                    .HasViewColumnName("access_token_id");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasColumnName("note")
                    .HasViewColumnName("note");

                entity.Property(e => e.TokenHash)
                    .IsRequired()
                    .HasColumnName("token_hash")
                    .HasViewColumnName("token_hash")
                    .HasMaxLength(40);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.AccessToken)
                    .HasForeignKey(x => x.UserName)
                    .HasConstraintName("idx_access_token_fk0");
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(x => x.UserName)
                    .HasName("idx_account_pk");

                entity.ToTable("account");

                entity.HasIndex(x => x.MailAddress)
                    .HasName("idx_account_1")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Administrator)
                    .HasColumnName("administrator")
                    .HasViewColumnName("administrator");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasViewColumnName("description");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasColumnName("full_name")
                    .HasViewColumnName("full_name")
                    .HasMaxLength(100);

                entity.Property(e => e.GroupAccount)
                    .HasColumnName("group_account")
                    .HasViewColumnName("group_account");

                entity.Property(e => e.Image)
                    .HasColumnName("image")
                    .HasViewColumnName("image")
                    .HasMaxLength(100);

                entity.Property(e => e.LastLoginDate)
                    .HasColumnName("last_login_date")
                    .HasViewColumnName("last_login_date");

                entity.Property(e => e.MailAddress)
                    .IsRequired()
                    .HasColumnName("mail_address")
                    .HasViewColumnName("mail_address")
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasViewColumnName("password")
                    .HasMaxLength(200);

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("registered_date")
                    .HasViewColumnName("registered_date");

                entity.Property(e => e.Removed)
                    .HasColumnName("removed")
                    .HasViewColumnName("removed");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("updated_date")
                    .HasViewColumnName("updated_date");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasViewColumnName("url")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<AccountExtraMailAddress>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.ExtraMailAddress })
                    .HasName("idx_account_extra_mail_address_pk");

                entity.ToTable("account_extra_mail_address");

                entity.HasIndex(x => x.ExtraMailAddress)
                    .HasName("idx_account_extra_mail_address_1")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.ExtraMailAddress)
                    .HasColumnName("extra_mail_address")
                    .HasViewColumnName("extra_mail_address")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<AccountFederation>(entity =>
            {
                entity.HasKey(x => new { x.Issuer, x.Subject })
                    .HasName("idx_account_federation_pk");

                entity.ToTable("account_federation");

                entity.Property(e => e.Issuer)
                    .HasColumnName("issuer")
                    .HasViewColumnName("issuer")
                    .HasMaxLength(100);

                entity.Property(e => e.Subject)
                    .HasColumnName("subject")
                    .HasViewColumnName("subject")
                    .HasMaxLength(100);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.AccountFederation)
                    .HasForeignKey(x => x.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_account_federation_fk0");
            });

            modelBuilder.Entity<AccountWebHook>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.Url })
                    .HasName("idx_account_web_hook_pk");

                entity.ToTable("account_web_hook");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasViewColumnName("url")
                    .HasMaxLength(200);

                entity.Property(e => e.Ctype)
                    .HasColumnName("ctype")
                    .HasViewColumnName("ctype")
                    .HasMaxLength(10);

                entity.Property(e => e.Token)
                    .HasColumnName("token")
                    .HasViewColumnName("token")
                    .HasMaxLength(100);

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.AccountWebHook)
                    .HasForeignKey(x => x.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_account_web_hook_fk0");
            });

            modelBuilder.Entity<AccountWebHookEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("account_web_hook_event");

                entity.Property(e => e.Event)
                    .IsRequired()
                    .HasColumnName("event")
                    .HasViewColumnName("event")
                    .HasMaxLength(30);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url")
                    .HasViewColumnName("url")
                    .HasMaxLength(200);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.ToTable("activity");

                entity.HasIndex(x => x.ActivityId)
                    .HasName("activity_activity_id_key")
                    .IsUnique();

                entity.Property(e => e.ActivityId)
                    .HasColumnName("activity_id")
                    .HasViewColumnName("activity_id");

                entity.Property(e => e.ActivityDate)
                    .HasColumnName("activity_date")
                    .HasViewColumnName("activity_date");

                entity.Property(e => e.ActivityType)
                    .IsRequired()
                    .HasColumnName("activity_type")
                    .HasViewColumnName("activity_type")
                    .HasMaxLength(100);

                entity.Property(e => e.ActivityUserName)
                    .IsRequired()
                    .HasColumnName("activity_user_name")
                    .HasViewColumnName("activity_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.AdditionalInfo)
                    .HasColumnName("additional_info")
                    .HasViewColumnName("additional_info");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message")
                    .HasViewColumnName("message");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.HasOne(d => d.ActivityUserNameNavigation)
                    .WithMany(p => p.Activity)
                    .HasForeignKey(x => x.ActivityUserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_activity_fk1");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Activity)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_activity_fk0");
            });

            modelBuilder.Entity<Collaborator>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.CollaboratorName })
                    .HasName("idx_collaborator_pk");

                entity.ToTable("collaborator");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.CollaboratorName)
                    .HasColumnName("collaborator_name")
                    .HasViewColumnName("collaborator_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasColumnName("role")
                    .HasViewColumnName("role")
                    .HasMaxLength(10)
                    .HasDefaultValueSql("'ADMIN'::character varying");

                entity.HasOne(d => d.CollaboratorNameNavigation)
                    .WithMany(p => p.Collaborator)
                    .HasForeignKey(x => x.CollaboratorName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_collaborator_fk1");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Collaborator)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_collaborator_fk0");
            });

            modelBuilder.Entity<CommitComment>(entity =>
            {
                entity.HasKey(x => x.CommentId)
                    .HasName("idx_commit_comment_pk");

                entity.ToTable("commit_comment");

                entity.HasIndex(x => x.CommentId)
                    .HasName("commit_comment_comment_id_key")
                    .IsUnique();

                entity.Property(e => e.CommentId)
                    .HasColumnName("comment_id")
                    .HasViewColumnName("comment_id");

                entity.Property(e => e.CommentedUserName)
                    .IsRequired()
                    .HasColumnName("commented_user_name")
                    .HasViewColumnName("commented_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.CommitId)
                    .IsRequired()
                    .HasColumnName("commit_id")
                    .HasViewColumnName("commit_id")
                    .HasMaxLength(100);

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasViewColumnName("content");

                entity.Property(e => e.FileName)
                    .HasColumnName("file_name")
                    .HasViewColumnName("file_name")
                    .HasMaxLength(260);

                entity.Property(e => e.IssueId)
                    .HasColumnName("issue_id")
                    .HasViewColumnName("issue_id");

                entity.Property(e => e.NewLineNumber)
                    .HasColumnName("new_line_number")
                    .HasViewColumnName("new_line_number");

                entity.Property(e => e.OldLineNumber)
                    .HasColumnName("old_line_number")
                    .HasViewColumnName("old_line_number");

                entity.Property(e => e.OriginalCommitId)
                    .IsRequired()
                    .HasColumnName("original_commit_id")
                    .HasViewColumnName("original_commit_id")
                    .HasMaxLength(100);

                entity.Property(e => e.OriginalNewLine)
                    .HasColumnName("original_new_line")
                    .HasViewColumnName("original_new_line");

                entity.Property(e => e.OriginalOldLine)
                    .HasColumnName("original_old_line")
                    .HasViewColumnName("original_old_line");

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("registered_date")
                    .HasViewColumnName("registered_date");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("updated_date")
                    .HasViewColumnName("updated_date");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.CommitComment)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_commit_comment_fk0");
            });

            modelBuilder.Entity<CommitStatus>(entity =>
            {
                entity.ToTable("commit_status");


                entity.HasIndex(x => new { x.UserName, x.RepositoryName, x.CommitId, x.Context })
                    .HasName("idx_commit_status_1")
                    .IsUnique();

                entity.Property(e => e.CommitStatusId)
                    .HasColumnName("commit_status_id")
                    .HasViewColumnName("commit_status_id");

                entity.Property(e => e.CommitId)
                    .IsRequired()
                    .HasColumnName("commit_id")
                    .HasViewColumnName("commit_id")
                    .HasMaxLength(40);

                entity.Property(e => e.Context)
                    .IsRequired()
                    .HasColumnName("context")
                    .HasViewColumnName("context")
                    .HasMaxLength(255);

                entity.Property(e => e.Creator)
                    .IsRequired()
                    .HasColumnName("creator")
                    .HasViewColumnName("creator")
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasViewColumnName("description");

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("registered_date")
                    .HasViewColumnName("registered_date");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasViewColumnName("state")
                    .HasMaxLength(10);

                entity.Property(e => e.TargetUrl)
                    .HasColumnName("target_url")
                    .HasViewColumnName("target_url")
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("updated_date")
                    .HasViewColumnName("updated_date");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.HasOne(d => d.CreatorNavigation)
                    .WithMany(p => p.CommitStatusCreatorNavigation)
                    .HasForeignKey(x => x.Creator)
                    .HasConstraintName("idx_commit_status_fk3");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.CommitStatusUserNameNavigation)
                    .HasForeignKey(x => x.UserName)
                    .HasConstraintName("idx_commit_status_fk2");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.CommitStatus)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .HasConstraintName("idx_commit_status_fk1");
            });

            modelBuilder.Entity<DeployKey>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.DeployKeyId })
                    .HasName("idx_deploy_key_pk");

                entity.ToTable("deploy_key");

                entity.HasIndex(x => x.DeployKeyId)
                    .HasName("deploy_key_deploy_key_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.DeployKeyId)
                    .HasColumnName("deploy_key_id")
                    .HasViewColumnName("deploy_key_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AllowWrite)
                    .HasColumnName("allow_write")
                    .HasViewColumnName("allow_write");

                entity.Property(e => e.PublicKey)
                    .IsRequired()
                    .HasColumnName("public_key")
                    .HasViewColumnName("public_key");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasViewColumnName("title")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.DeployKey)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_deploy_key_fk0");
            });

            modelBuilder.Entity<Gist>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName })
                    .HasName("idx_gist_pk");

                entity.ToTable("gist");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasViewColumnName("description");

                entity.Property(e => e.Mode)
                    .IsRequired()
                    .HasColumnName("mode")
                    .HasViewColumnName("mode")
                    .HasMaxLength(10)
                    .HasDefaultValueSql("'PUBLIC'::character varying");

                entity.Property(e => e.OriginRepositoryName)
                    .HasColumnName("origin_repository_name")
                    .HasViewColumnName("origin_repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.OriginUserName)
                    .HasColumnName("origin_user_name")
                    .HasViewColumnName("origin_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("registered_date")
                    .HasViewColumnName("registered_date");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasViewColumnName("title")
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("updated_date")
                    .HasViewColumnName("updated_date");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Gist)
                    .HasForeignKey(x => x.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_gist_fk0");
            });

            modelBuilder.Entity<GistComment>(entity =>
            {
                entity.HasKey(x => x.CommentId)
                    .HasName("idx_gist_comment_pk");

                entity.ToTable("gist_comment");

                entity.HasIndex(x => x.CommentId)
                    .HasName("gist_comment_comment_id_key")
                    .IsUnique();

                entity.HasIndex(x => new { x.UserName, x.RepositoryName, x.CommentId })
                    .HasName("idx_gist_comment_1")
                    .IsUnique();

                entity.Property(e => e.CommentId)
                    .HasColumnName("comment_id")
                    .HasViewColumnName("comment_id");

                entity.Property(e => e.CommentedUserName)
                    .IsRequired()
                    .HasColumnName("commented_user_name")
                    .HasViewColumnName("commented_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasViewColumnName("content");

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("registered_date")
                    .HasViewColumnName("registered_date");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("updated_date")
                    .HasViewColumnName("updated_date");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.HasOne(d => d.CommentedUserNameNavigation)
                    .WithMany(p => p.GistComment)
                    .HasForeignKey(x => x.CommentedUserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_gist_comment_fk1");

                entity.HasOne(d => d.Gist)
                    .WithMany(p => p.GistComment)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_gist_comment_fk0");
            });

            modelBuilder.Entity<GpgKey>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.GpgKeyId })
                    .HasName("idx_gpg_key_pk");

                entity.ToTable("gpg_key");

                entity.HasIndex(x => x.KeyId)
                    .HasName("gpg_key_key_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.GpgKeyId)
                    .HasColumnName("gpg_key_id")
                    .HasViewColumnName("gpg_key_id");

                entity.Property(e => e.KeyId)
                    .HasColumnName("key_id")
                    .HasViewColumnName("key_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.PublicKey)
                    .IsRequired()
                    .HasColumnName("public_key")
                    .HasViewColumnName("public_key");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasViewColumnName("title")
                    .HasMaxLength(100);

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.GpgKey)
                    .HasForeignKey(x => x.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_gpg_key_fk0");
            });

            modelBuilder.Entity<GroupMember>(entity =>
            {
                entity.HasKey(x => new { x.GroupName, x.UserName })
                    .HasName("idx_group_member_pk");

                entity.ToTable("group_member");

                entity.Property(e => e.GroupName)
                    .HasColumnName("group_name")
                    .HasViewColumnName("group_name")
                    .HasMaxLength(100);

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Manager)
                    .HasColumnName("manager")
                    .HasViewColumnName("manager");

                entity.HasOne(d => d.GroupNameNavigation)
                    .WithMany(p => p.GroupMemberGroupNameNavigation)
                    .HasForeignKey(x => x.GroupName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_group_member_fk0");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.GroupMemberUserNameNavigation)
                    .HasForeignKey(x => x.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_group_member_fk1");
            });

            modelBuilder.Entity<Issue>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.IssueId })
                    .HasName("idx_issue_pk");

                entity.ToTable("issue");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.IssueId)
                    .HasColumnName("issue_id")
                    .HasViewColumnName("issue_id");

                entity.Property(e => e.AssignedUserName)
                    .HasColumnName("assigned_user_name")
                    .HasViewColumnName("assigned_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Closed)
                    .HasColumnName("closed")
                    .HasViewColumnName("closed");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasViewColumnName("content");

                entity.Property(e => e.MilestoneId)
                    .HasColumnName("milestone_id")
                    .HasViewColumnName("milestone_id");

                entity.Property(e => e.OpenedUserName)
                    .IsRequired()
                    .HasColumnName("opened_user_name")
                    .HasViewColumnName("opened_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.PriorityId)
                    .HasColumnName("priority_id")
                    .HasViewColumnName("priority_id");

                entity.Property(e => e.PullRequest)
                    .HasColumnName("pull_request")
                    .HasViewColumnName("pull_request");

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("registered_date")
                    .HasViewColumnName("registered_date");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasViewColumnName("title");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("updated_date")
                    .HasViewColumnName("updated_date");

                entity.HasOne(d => d.Milestone)
                    .WithMany(p => p.Issue)
                    .HasPrincipalKey(x => x.MilestoneId)
                    .HasForeignKey(x => x.MilestoneId)
                    .HasConstraintName("idx_issue_fk2");

                entity.HasOne(d => d.OpenedUserNameNavigation)
                    .WithMany(p => p.Issue)
                    .HasForeignKey(x => x.OpenedUserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_issue_fk1");

                entity.HasOne(d => d.Priority)
                    .WithMany(p => p.Issue)
                    .HasPrincipalKey(x => x.PriorityId)
                    .HasForeignKey(x => x.PriorityId)
                    .HasConstraintName("idx_issue_fk3");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Issue)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_issue_fk0");
            });

            modelBuilder.Entity<IssueComment>(entity =>
            {
                entity.HasKey(x => x.CommentId)
                    .HasName("idx_issue_comment_pk");

                entity.ToTable("issue_comment");

                entity.HasIndex(x => x.CommentId)
                    .HasName("issue_comment_comment_id_key")
                    .IsUnique();

                entity.Property(e => e.CommentId)
                    .HasColumnName("comment_id")
                    .HasViewColumnName("comment_id");

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasColumnName("action")
                    .HasViewColumnName("action")
                    .HasMaxLength(20);

                entity.Property(e => e.CommentedUserName)
                    .IsRequired()
                    .HasColumnName("commented_user_name")
                    .HasViewColumnName("commented_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasViewColumnName("content");

                entity.Property(e => e.IssueId)
                    .HasColumnName("issue_id")
                    .HasViewColumnName("issue_id");

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("registered_date")
                    .HasViewColumnName("registered_date");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("updated_date")
                    .HasViewColumnName("updated_date");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.IssueComment)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName, x.IssueId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_issue_comment_fk0");
            });

            modelBuilder.Entity<IssueId>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName })
                    .HasName("idx_issue_id_pk");

                entity.ToTable("issue_id");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.IssueId1)
                    .HasColumnName("issue_id")
                    .HasViewColumnName("issue_id");

                entity.HasOne(d => d.Repository)
                    .WithOne(p => p.IssueId)
                    .HasForeignKey<IssueId>(x => new { x.UserName, x.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_issue_id_fk1");
            });

            modelBuilder.Entity<IssueLabel>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.IssueId, x.LabelId })
                    .HasName("idx_issue_label_pk");

                entity.ToTable("issue_label");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.IssueId)
                    .HasColumnName("issue_id")
                    .HasViewColumnName("issue_id");

                entity.Property(e => e.LabelId)
                    .HasColumnName("label_id")
                    .HasViewColumnName("label_id");

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.IssueLabel)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName, x.IssueId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_issue_label_fk0");
            });

            modelBuilder.Entity<IssueNotification>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.IssueId, x.NotificationUserName })
                    .HasName("idx_issue_notification_pk");

                entity.ToTable("issue_notification");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.IssueId)
                    .HasColumnName("issue_id")
                    .HasViewColumnName("issue_id");

                entity.Property(e => e.NotificationUserName)
                    .HasColumnName("notification_user_name")
                    .HasViewColumnName("notification_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Subscribed)
                    .HasColumnName("subscribed")
                    .HasViewColumnName("subscribed");
            });

            modelBuilder.Entity<IssueOutlineView>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("issue_outline_view");

                entity.Property(e => e.CommentCount)
                    .HasColumnName("comment_count")
                    .HasViewColumnName("comment_count");

                entity.Property(e => e.IssueId)
                    .HasColumnName("issue_id")
                    .HasViewColumnName("issue_id");

                entity.Property(e => e.Priority)
                    .HasColumnName("priority")
                    .HasViewColumnName("priority");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Label>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.LabelId })
                    .HasName("idx_label_pk");

                entity.ToTable("label");

                entity.HasIndex(x => x.LabelId)
                    .HasName("label_label_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.LabelId)
                    .HasColumnName("label_id")
                    .HasViewColumnName("label_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasColumnName("color")
                    .HasViewColumnName("color")
                    .HasMaxLength(6)
                    .IsFixedLength();

                entity.Property(e => e.LabelName)
                    .IsRequired()
                    .HasColumnName("label_name")
                    .HasViewColumnName("label_name")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Label)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_label_fk0");
            });

            modelBuilder.Entity<Milestone>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.MilestoneId })
                    .HasName("idx_milestone_pk");

                entity.ToTable("milestone");

                entity.HasIndex(x => x.MilestoneId)
                    .HasName("milestone_milestone_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.MilestoneId)
                    .HasColumnName("milestone_id")
                    .HasViewColumnName("milestone_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ClosedDate)
                    .HasColumnName("closed_date")
                    .HasViewColumnName("closed_date");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasViewColumnName("description");

                entity.Property(e => e.DueDate)
                    .HasColumnName("due_date")
                    .HasViewColumnName("due_date");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasViewColumnName("title")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Milestone)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_milestone_fk0");
            });

            modelBuilder.Entity<NotificationsAccount>(entity =>
            {
                entity.HasKey(x => x.UserName)
                    .HasName("idx_notifications_account_pk");

                entity.ToTable("notifications_account");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.DisableEmail)
                    .HasColumnName("disable_email")
                    .HasViewColumnName("disable_email");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithOne(p => p.NotificationsAccount)
                    .HasForeignKey<NotificationsAccount>(x => x.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_notifications_account_fk0");
            });

            modelBuilder.Entity<Pages>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName })
                    .HasName("idx_pages_pk");

                entity.ToTable("pages");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("source")
                    .HasViewColumnName("source")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Plugin>(entity =>
            {
                entity.ToTable("plugin");

                entity.Property(e => e.PluginId)
                    .HasColumnName("plugin_id")
                    .HasViewColumnName("plugin_id")
                    .HasMaxLength(100);

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasViewColumnName("version")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Priority>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.PriorityId })
                    .HasName("idx_priority_pk");

                entity.ToTable("priority");

                entity.HasIndex(x => x.PriorityId)
                    .HasName("priority_priority_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.PriorityId)
                    .HasColumnName("priority_id")
                    .HasViewColumnName("priority_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasColumnName("color")
                    .HasViewColumnName("color")
                    .HasMaxLength(6)
                    .IsFixedLength();

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasViewColumnName("description")
                    .HasMaxLength(255);

                entity.Property(e => e.IsDefault)
                    .HasColumnName("is_default")
                    .HasViewColumnName("is_default");

                entity.Property(e => e.Ordering)
                    .HasColumnName("ordering")
                    .HasViewColumnName("ordering");

                entity.Property(e => e.PriorityName)
                    .IsRequired()
                    .HasColumnName("priority_name")
                    .HasViewColumnName("priority_name")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Priority)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_priority_fk0");
            });

            modelBuilder.Entity<ProtectedBranch>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.Branch })
                    .HasName("idx_protected_branch_pk");

                entity.ToTable("protected_branch");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Branch)
                    .HasColumnName("branch")
                    .HasViewColumnName("branch")
                    .HasMaxLength(100);

                entity.Property(e => e.StatusCheckAdmin)
                    .HasColumnName("status_check_admin")
                    .HasViewColumnName("status_check_admin");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.ProtectedBranch)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .HasConstraintName("idx_protected_branch_fk0");
            });

            modelBuilder.Entity<ProtectedBranchRequireContext>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.Branch, x.Context })
                    .HasName("idx_protected_branch_require_context_pk");

                entity.ToTable("protected_branch_require_context");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Branch)
                    .HasColumnName("branch")
                    .HasViewColumnName("branch")
                    .HasMaxLength(100);

                entity.Property(e => e.Context)
                    .HasColumnName("context")
                    .HasViewColumnName("context")
                    .HasMaxLength(255);

                entity.HasOne(d => d.ProtectedBranch)
                    .WithMany(p => p.ProtectedBranchRequireContext)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName, x.Branch })
                    .HasConstraintName("idx_protected_branch_require_context_fk0");
            });

            modelBuilder.Entity<PullRequest>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.IssueId })
                    .HasName("idx_pull_request_pk");

                entity.ToTable("pull_request");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.IssueId)
                    .HasColumnName("issue_id")
                    .HasViewColumnName("issue_id");

                entity.Property(e => e.Branch)
                    .IsRequired()
                    .HasColumnName("branch")
                    .HasViewColumnName("branch")
                    .HasMaxLength(100);

                entity.Property(e => e.CommitIdFrom)
                    .IsRequired()
                    .HasColumnName("commit_id_from")
                    .HasViewColumnName("commit_id_from")
                    .HasMaxLength(40);

                entity.Property(e => e.CommitIdTo)
                    .IsRequired()
                    .HasColumnName("commit_id_to")
                    .HasViewColumnName("commit_id_to")
                    .HasMaxLength(40);

                entity.Property(e => e.IsDraft)
                    .HasColumnName("is_draft")
                    .HasViewColumnName("is_draft");

                entity.Property(e => e.RequestBranch)
                    .IsRequired()
                    .HasColumnName("request_branch")
                    .HasViewColumnName("request_branch")
                    .HasMaxLength(100);

                entity.Property(e => e.RequestRepositoryName)
                    .IsRequired()
                    .HasColumnName("request_repository_name")
                    .HasViewColumnName("request_repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RequestUserName)
                    .IsRequired()
                    .HasColumnName("request_user_name")
                    .HasViewColumnName("request_user_name")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Issue)
                    .WithOne(p => p.PullRequestNavigation)
                    .HasForeignKey<PullRequest>(x => new { x.UserName, x.RepositoryName, x.IssueId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_pull_request_fk0");
            });

            modelBuilder.Entity<ReleaseAsset>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.Tag, x.FileName })
                    .HasName("idx_release_asset_pk");

                entity.ToTable("release_asset");

                entity.HasIndex(x => x.ReleaseAssetId)
                    .HasName("release_asset_release_asset_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Tag)
                    .HasColumnName("tag")
                    .HasViewColumnName("tag")
                    .HasMaxLength(100);

                entity.Property(e => e.FileName)
                    .HasColumnName("file_name")
                    .HasViewColumnName("file_name")
                    .HasMaxLength(260);

                entity.Property(e => e.Label)
                    .HasColumnName("label")
                    .HasViewColumnName("label")
                    .HasMaxLength(100);

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("registered_date")
                    .HasViewColumnName("registered_date");

                entity.Property(e => e.ReleaseAssetId)
                    .HasColumnName("release_asset_id")
                    .HasViewColumnName("release_asset_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Size)
                    .HasColumnName("size")
                    .HasViewColumnName("size");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("updated_date")
                    .HasViewColumnName("updated_date");

                entity.Property(e => e.Uploader)
                    .IsRequired()
                    .HasColumnName("uploader")
                    .HasViewColumnName("uploader")
                    .HasMaxLength(100);

                entity.HasOne(d => d.ReleaseTag)
                    .WithMany(p => p.ReleaseAsset)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName, x.Tag })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_release_asset_fk0");
            });

            modelBuilder.Entity<ReleaseTag>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.Tag })
                    .HasName("idx_release_tag_pk");

                entity.ToTable("release_tag");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Tag)
                    .HasColumnName("tag")
                    .HasViewColumnName("tag")
                    .HasMaxLength(100);

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasColumnName("author")
                    .HasViewColumnName("author")
                    .HasMaxLength(100);

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasViewColumnName("content");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasViewColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("registered_date")
                    .HasViewColumnName("registered_date");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("updated_date")
                    .HasViewColumnName("updated_date");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.ReleaseTag)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_release_tag_fk0");
            });

            modelBuilder.Entity<Repository>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName })
                    .HasName("idx_repository_pk");

                entity.ToTable("repository");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.AllowFork)
                    .IsRequired()
                    .HasColumnName("allow_fork")
                    .HasViewColumnName("allow_fork")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.DefaultBranch)
                    .HasColumnName("default_branch")
                    .HasViewColumnName("default_branch")
                    .HasMaxLength(100);

                entity.Property(e => e.DefaultMergeOption)
                    .IsRequired()
                    .HasColumnName("default_merge_option")
                    .HasViewColumnName("default_merge_option")
                    .HasMaxLength(100)
                    .HasDefaultValueSql("'merge-commit'::character varying");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasViewColumnName("description");

                entity.Property(e => e.ExternalIssuesUrl)
                    .HasColumnName("external_issues_url")
                    .HasViewColumnName("external_issues_url")
                    .HasMaxLength(200);

                entity.Property(e => e.ExternalWikiUrl)
                    .HasColumnName("external_wiki_url")
                    .HasViewColumnName("external_wiki_url")
                    .HasMaxLength(200);

                entity.Property(e => e.IssuesOption)
                    .IsRequired()
                    .HasColumnName("issues_option")
                    .HasViewColumnName("issues_option")
                    .HasMaxLength(10)
                    .HasDefaultValueSql("'DISABLE'::character varying");

                entity.Property(e => e.LastActivityDate)
                    .HasColumnName("last_activity_date")
                    .HasViewColumnName("last_activity_date");

                entity.Property(e => e.MergeOptions)
                    .IsRequired()
                    .HasColumnName("merge_options")
                    .HasViewColumnName("merge_options")
                    .HasMaxLength(200)
                    .HasDefaultValueSql("'merge-commit,squash,rebase'::character varying");

                entity.Property(e => e.OriginRepositoryName)
                    .HasColumnName("origin_repository_name")
                    .HasViewColumnName("origin_repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.OriginUserName)
                    .HasColumnName("origin_user_name")
                    .HasViewColumnName("origin_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.ParentRepositoryName)
                    .HasColumnName("parent_repository_name")
                    .HasViewColumnName("parent_repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.ParentUserName)
                    .HasColumnName("parent_user_name")
                    .HasViewColumnName("parent_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Private)
                    .HasColumnName("private")
                    .HasViewColumnName("private");

                entity.Property(e => e.RegisteredDate)
                    .HasColumnName("registered_date")
                    .HasViewColumnName("registered_date");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("updated_date")
                    .HasViewColumnName("updated_date");

                entity.Property(e => e.WikiOption)
                    .IsRequired()
                    .HasColumnName("wiki_option")
                    .HasViewColumnName("wiki_option")
                    .HasMaxLength(10)
                    .HasDefaultValueSql("'DISABLE'::character varying");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Repository)
                    .HasForeignKey(x => x.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_repository_fk0");
            });

            modelBuilder.Entity<SshKey>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.SshKeyId })
                    .HasName("idx_ssh_key_pk");

                entity.ToTable("ssh_key");

                entity.HasIndex(x => x.SshKeyId)
                    .HasName("ssh_key_ssh_key_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.SshKeyId)
                    .HasColumnName("ssh_key_id")
                    .HasViewColumnName("ssh_key_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.PublicKey)
                    .IsRequired()
                    .HasColumnName("public_key")
                    .HasViewColumnName("public_key");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasViewColumnName("title")
                    .HasMaxLength(100);

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.SshKey)
                    .HasForeignKey(x => x.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_ssh_key_fk0");
            });

            modelBuilder.Entity<Versions>(entity =>
            {
                entity.HasKey(x => x.ModuleId)
                    .HasName("versions_pk");

                entity.ToTable("versions");

                entity.Property(e => e.ModuleId)
                    .HasColumnName("module_id")
                    .HasViewColumnName("module_id")
                    .HasMaxLength(100);

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasViewColumnName("version")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Watch>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.NotificationUserName })
                    .HasName("idx_watch_pk");

                entity.ToTable("watch");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.NotificationUserName)
                    .HasColumnName("notification_user_name")
                    .HasViewColumnName("notification_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Notification)
                    .IsRequired()
                    .HasColumnName("notification")
                    .HasViewColumnName("notification")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<WebHook>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.Url })
                    .HasName("idx_web_hook_pk");

                entity.ToTable("web_hook");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasViewColumnName("url")
                    .HasMaxLength(200);

                entity.Property(e => e.Ctype)
                    .HasColumnName("ctype")
                    .HasViewColumnName("ctype")
                    .HasMaxLength(10);

                entity.Property(e => e.Token)
                    .HasColumnName("token")
                    .HasViewColumnName("token")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.WebHook)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_web_hook_fk0");
            });

            modelBuilder.Entity<WebHookEvent>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.Url, x.Event })
                    .HasName("idx_web_hook_event_pk");

                entity.ToTable("web_hook_event");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasViewColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasViewColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasViewColumnName("url")
                    .HasMaxLength(200);

                entity.Property(e => e.Event)
                    .HasColumnName("event")
                    .HasViewColumnName("event")
                    .HasMaxLength(30);

                entity.HasOne(d => d.WebHook)
                    .WithMany(p => p.WebHookEvent)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName, x.Url })
                    .HasConstraintName("idx_web_hook_event_fk0");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

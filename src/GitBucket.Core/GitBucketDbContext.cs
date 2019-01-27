using GitBucket.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GitBucket.Core
{
    public partial class GitBucketDbContext : DbContext
    {
        public GitBucketDbContext(string connectionString)
            => ConnectionString = connectionString;

        public virtual DbSet<AccessToken> AccessToken { get; set; }
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountExtraMailAddress> AccountExtraMailAddress { get; set; }
        public virtual DbSet<AccountFederation> AccountFederation { get; set; }
        public virtual DbSet<AccountWebHook> AccountWebHook { get; set; }
        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<Collaborator> Collaborator { get; set; }
        public virtual DbSet<CommitComment> CommitComment { get; set; }
        public virtual DbSet<CommitStatus> CommitStatus { get; set; }
        public virtual DbSet<DeployKey> DeployKey { get; set; }
        public virtual DbSet<GroupMember> GroupMember { get; set; }
        public virtual DbSet<Issue> Issue { get; set; }
        public virtual DbSet<IssueComment> IssueComment { get; set; }
        public virtual DbSet<IssueId> IssueId { get; set; }
        public virtual DbSet<IssueLabel> IssueLabel { get; set; }
        public virtual DbSet<IssueNotification> IssueNotification { get; set; }
        public virtual DbSet<Label> Label { get; set; }
        public virtual DbSet<Milestone> Milestone { get; set; }
        public virtual DbSet<NotificationsAccount> NotificationsAccount { get; set; }
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

        // Unable to generate entity type for table 'public.account_web_hook_event'. Please see the warning messages.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<AccessToken>(entity =>
            {
                entity.ToTable("access_token");

                entity.HasIndex(e => e.AccessTokenId)
                    .HasName("access_token_access_token_id_key")
                    .IsUnique();

                entity.HasIndex(e => e.TokenHash)
                    .HasName("idx_access_token_token_hash")
                    .IsUnique();

                entity.Property(e => e.AccessTokenId).HasColumnName("access_token_id");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasColumnName("note");

                entity.Property(e => e.TokenHash)
                    .IsRequired()
                    .HasColumnName("token_hash")
                    .HasMaxLength(40);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.AccessToken)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("idx_access_token_fk0");
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("idx_account_pk");

                entity.ToTable("account");

                entity.HasIndex(e => e.MailAddress)
                    .HasName("idx_account_1")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100)
                    .ValueGeneratedNever();

                entity.Property(e => e.Administrator).HasColumnName("administrator");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasColumnName("full_name")
                    .HasMaxLength(100);

                entity.Property(e => e.GroupAccount).HasColumnName("group_account");

                entity.Property(e => e.Image)
                    .HasColumnName("image")
                    .HasMaxLength(100);

                entity.Property(e => e.LastLoginDate).HasColumnName("last_login_date");

                entity.Property(e => e.MailAddress)
                    .IsRequired()
                    .HasColumnName("mail_address")
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(200);

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.Removed).HasColumnName("removed");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<AccountExtraMailAddress>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.ExtraMailAddress })
                    .HasName("idx_account_extra_mail_address_pk");

                entity.ToTable("account_extra_mail_address");

                entity.HasIndex(e => e.ExtraMailAddress)
                    .HasName("idx_account_extra_mail_address_1")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.ExtraMailAddress)
                    .HasColumnName("extra_mail_address")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<AccountFederation>(entity =>
            {
                entity.HasKey(e => new { e.Issuer, e.Subject })
                    .HasName("idx_account_federation_pk");

                entity.ToTable("account_federation");

                entity.Property(e => e.Issuer)
                    .HasColumnName("issuer")
                    .HasMaxLength(100);

                entity.Property(e => e.Subject)
                    .HasColumnName("subject")
                    .HasMaxLength(100);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.AccountFederation)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_account_federation_fk0");
            });

            modelBuilder.Entity<AccountWebHook>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.Url })
                    .HasName("idx_account_web_hook_pk");

                entity.ToTable("account_web_hook");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(200);

                entity.Property(e => e.Ctype)
                    .HasColumnName("ctype")
                    .HasMaxLength(10);

                entity.Property(e => e.Token)
                    .HasColumnName("token")
                    .HasMaxLength(100);

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.AccountWebHook)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_account_web_hook_fk0");
            });

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.ToTable("activity");

                entity.HasIndex(e => e.ActivityId)
                    .HasName("activity_activity_id_key")
                    .IsUnique();

                entity.Property(e => e.ActivityId).HasColumnName("activity_id");

                entity.Property(e => e.ActivityDate).HasColumnName("activity_date");

                entity.Property(e => e.ActivityType)
                    .IsRequired()
                    .HasColumnName("activity_type")
                    .HasMaxLength(100);

                entity.Property(e => e.ActivityUserName)
                    .IsRequired()
                    .HasColumnName("activity_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.AdditionalInfo).HasColumnName("additional_info");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.HasOne(d => d.ActivityUserNameNavigation)
                    .WithMany(p => p.Activity)
                    .HasForeignKey(d => d.ActivityUserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_activity_fk1");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Activity)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_activity_fk0");
            });

            modelBuilder.Entity<Collaborator>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.CollaboratorName })
                    .HasName("idx_collaborator_pk");

                entity.ToTable("collaborator");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.CollaboratorName)
                    .HasColumnName("collaborator_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasColumnName("role")
                    .HasMaxLength(10)
                    .HasDefaultValueSql("'ADMIN'::character varying");

                entity.HasOne(d => d.CollaboratorNameNavigation)
                    .WithMany(p => p.Collaborator)
                    .HasForeignKey(d => d.CollaboratorName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_collaborator_fk1");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Collaborator)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_collaborator_fk0");
            });

            modelBuilder.Entity<CommitComment>(entity =>
            {
                entity.HasKey(e => e.CommentId)
                    .HasName("idx_commit_comment_pk");

                entity.ToTable("commit_comment");

                entity.HasIndex(e => e.CommentId)
                    .HasName("commit_comment_comment_id_key")
                    .IsUnique();

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.CommentedUserName)
                    .IsRequired()
                    .HasColumnName("commented_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.CommitId)
                    .IsRequired()
                    .HasColumnName("commit_id")
                    .HasMaxLength(100);

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content");

                entity.Property(e => e.FileName)
                    .HasColumnName("file_name")
                    .HasMaxLength(260);

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.NewLineNumber).HasColumnName("new_line_number");

                entity.Property(e => e.OldLineNumber).HasColumnName("old_line_number");

                entity.Property(e => e.OriginalCommitId)
                    .IsRequired()
                    .HasColumnName("original_commit_id")
                    .HasMaxLength(100);

                entity.Property(e => e.OriginalNewLine).HasColumnName("original_new_line");

                entity.Property(e => e.OriginalOldLine).HasColumnName("original_old_line");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.CommitComment)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_commit_comment_fk0");
            });

            modelBuilder.Entity<CommitStatus>(entity =>
            {
                entity.ToTable("commit_status");

                entity.HasIndex(e => e.CommitStatusId)
                    .HasName("commit_status_commit_status_id_key")
                    .IsUnique();

                entity.HasIndex(e => new { e.UserName, e.RepositoryName, e.CommitId, e.Context })
                    .HasName("idx_commit_status_1")
                    .IsUnique();

                entity.Property(e => e.CommitStatusId).HasColumnName("commit_status_id");

                entity.Property(e => e.CommitId)
                    .IsRequired()
                    .HasColumnName("commit_id")
                    .HasMaxLength(40);

                entity.Property(e => e.Context)
                    .IsRequired()
                    .HasColumnName("context")
                    .HasMaxLength(255);

                entity.Property(e => e.Creator)
                    .IsRequired()
                    .HasColumnName("creator")
                    .HasMaxLength(100);

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasMaxLength(10);

                entity.Property(e => e.TargetUrl)
                    .HasColumnName("target_url")
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.HasOne(d => d.CreatorNavigation)
                    .WithMany(p => p.CommitStatusCreatorNavigation)
                    .HasForeignKey(d => d.Creator)
                    .HasConstraintName("idx_commit_status_fk3");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.CommitStatusUserNameNavigation)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("idx_commit_status_fk2");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.CommitStatus)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .HasConstraintName("idx_commit_status_fk1");
            });

            modelBuilder.Entity<DeployKey>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.DeployKeyId })
                    .HasName("idx_deploy_key_pk");

                entity.ToTable("deploy_key");

                entity.HasIndex(e => e.DeployKeyId)
                    .HasName("deploy_key_deploy_key_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.DeployKeyId)
                    .HasColumnName("deploy_key_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AllowWrite).HasColumnName("allow_write");

                entity.Property(e => e.PublicKey)
                    .IsRequired()
                    .HasColumnName("public_key");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.DeployKey)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_deploy_key_fk0");
            });

            modelBuilder.Entity<GroupMember>(entity =>
            {
                entity.HasKey(e => new { e.GroupName, e.UserName })
                    .HasName("idx_group_member_pk");

                entity.ToTable("group_member");

                entity.Property(e => e.GroupName)
                    .HasColumnName("group_name")
                    .HasMaxLength(100);

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Manager).HasColumnName("manager");

                entity.HasOne(d => d.GroupNameNavigation)
                    .WithMany(p => p.GroupMemberGroupNameNavigation)
                    .HasForeignKey(d => d.GroupName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_group_member_fk0");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.GroupMemberUserNameNavigation)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_group_member_fk1");
            });

            modelBuilder.Entity<Issue>(entity =>
            {
                entity.HasKey(e => new { e.IssueId, e.UserName, e.RepositoryName })
                    .HasName("idx_issue_pk");

                entity.ToTable("issue");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.AssignedUserName)
                    .HasColumnName("assigned_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Closed).HasColumnName("closed");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.MilestoneId).HasColumnName("milestone_id");

                entity.Property(e => e.OpenedUserName)
                    .IsRequired()
                    .HasColumnName("opened_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.PriorityId).HasColumnName("priority_id");

                entity.Property(e => e.PullRequest).HasColumnName("pull_request");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.HasOne(d => d.Milestone)
                    .WithMany(p => p.Issue)
                    .HasPrincipalKey(p => p.MilestoneId)
                    .HasForeignKey(d => d.MilestoneId)
                    .HasConstraintName("idx_issue_fk2");

                entity.HasOne(d => d.OpenedUserNameNavigation)
                    .WithMany(p => p.Issue)
                    .HasForeignKey(d => d.OpenedUserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_issue_fk1");

                entity.HasOne(d => d.Priority)
                    .WithMany(p => p.Issue)
                    .HasPrincipalKey(p => p.PriorityId)
                    .HasForeignKey(d => d.PriorityId)
                    .HasConstraintName("idx_issue_fk3");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Issue)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_issue_fk0");
            });

            modelBuilder.Entity<IssueComment>(entity =>
            {
                entity.HasKey(e => e.CommentId)
                    .HasName("idx_issue_comment_pk");

                entity.ToTable("issue_comment");

                entity.HasIndex(e => e.CommentId)
                    .HasName("issue_comment_comment_id_key")
                    .IsUnique();

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasColumnName("action")
                    .HasMaxLength(20);

                entity.Property(e => e.CommentedUserName)
                    .IsRequired()
                    .HasColumnName("commented_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<IssueId>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName })
                    .HasName("idx_issue_id_pk");

                entity.ToTable("issue_id");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.IssueId1).HasColumnName("issue_id");

                entity.HasOne(d => d.Repository)
                    .WithOne(p => p.IssueId)
                    .HasForeignKey<IssueId>(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_issue_id_fk1");
            });

            modelBuilder.Entity<IssueLabel>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.IssueId, e.LabelId })
                    .HasName("idx_issue_label_pk");

                entity.ToTable("issue_label");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.LabelId).HasColumnName("label_id");
            });

            modelBuilder.Entity<IssueNotification>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.IssueId, e.NotificationUserName })
                    .HasName("idx_issue_notification_pk");

                entity.ToTable("issue_notification");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.NotificationUserName)
                    .HasColumnName("notification_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Subscribed).HasColumnName("subscribed");
            });

            modelBuilder.Entity<Label>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.LabelId })
                    .HasName("idx_label_pk");

                entity.ToTable("label");

                entity.HasIndex(e => e.LabelId)
                    .HasName("label_label_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.LabelId)
                    .HasColumnName("label_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasColumnName("color")
                    .HasColumnType("character(6)");

                entity.Property(e => e.LabelName)
                    .IsRequired()
                    .HasColumnName("label_name")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Label)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_label_fk0");
            });

            modelBuilder.Entity<Milestone>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.MilestoneId })
                    .HasName("idx_milestone_pk");

                entity.ToTable("milestone");

                entity.HasIndex(e => e.MilestoneId)
                    .HasName("milestone_milestone_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.MilestoneId)
                    .HasColumnName("milestone_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ClosedDate).HasColumnName("closed_date");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.DueDate).HasColumnName("due_date");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Milestone)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_milestone_fk0");
            });

            modelBuilder.Entity<NotificationsAccount>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("idx_notifications_account_pk");

                entity.ToTable("notifications_account");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100)
                    .ValueGeneratedNever();

                entity.Property(e => e.DisableEmail).HasColumnName("disable_email");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithOne(p => p.NotificationsAccount)
                    .HasForeignKey<NotificationsAccount>(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_notifications_account_fk0");
            });

            modelBuilder.Entity<Plugin>(entity =>
            {
                entity.ToTable("plugin");

                entity.Property(e => e.PluginId)
                    .HasColumnName("plugin_id")
                    .HasMaxLength(100)
                    .ValueGeneratedNever();

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Priority>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.PriorityId })
                    .HasName("idx_priority_pk");

                entity.ToTable("priority");

                entity.HasIndex(e => e.PriorityId)
                    .HasName("priority_priority_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.PriorityId)
                    .HasColumnName("priority_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasColumnName("color")
                    .HasColumnType("character(6)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(255);

                entity.Property(e => e.IsDefault).HasColumnName("is_default");

                entity.Property(e => e.Ordering).HasColumnName("ordering");

                entity.Property(e => e.PriorityName)
                    .IsRequired()
                    .HasColumnName("priority_name")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Priority)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_priority_fk0");
            });

            modelBuilder.Entity<ProtectedBranch>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.Branch })
                    .HasName("idx_protected_branch_pk");

                entity.ToTable("protected_branch");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Branch)
                    .HasColumnName("branch")
                    .HasMaxLength(100);

                entity.Property(e => e.StatusCheckAdmin).HasColumnName("status_check_admin");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.ProtectedBranch)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .HasConstraintName("idx_protected_branch_fk0");
            });

            modelBuilder.Entity<ProtectedBranchRequireContext>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.Branch, e.Context })
                    .HasName("idx_protected_branch_require_context_pk");

                entity.ToTable("protected_branch_require_context");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Branch)
                    .HasColumnName("branch")
                    .HasMaxLength(100);

                entity.Property(e => e.Context)
                    .HasColumnName("context")
                    .HasMaxLength(255);

                entity.HasOne(d => d.ProtectedBranch)
                    .WithMany(p => p.ProtectedBranchRequireContext)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName, d.Branch })
                    .HasConstraintName("idx_protected_branch_require_context_fk0");
            });

            modelBuilder.Entity<PullRequest>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.IssueId })
                    .HasName("idx_pull_request_pk");

                entity.ToTable("pull_request");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.Branch)
                    .IsRequired()
                    .HasColumnName("branch")
                    .HasMaxLength(100);

                entity.Property(e => e.CommitIdFrom)
                    .IsRequired()
                    .HasColumnName("commit_id_from")
                    .HasMaxLength(40);

                entity.Property(e => e.CommitIdTo)
                    .IsRequired()
                    .HasColumnName("commit_id_to")
                    .HasMaxLength(40);

                entity.Property(e => e.RequestBranch)
                    .IsRequired()
                    .HasColumnName("request_branch")
                    .HasMaxLength(100);

                entity.Property(e => e.RequestRepositoryName)
                    .IsRequired()
                    .HasColumnName("request_repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RequestUserName)
                    .IsRequired()
                    .HasColumnName("request_user_name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ReleaseAsset>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.Tag, e.FileName })
                    .HasName("idx_release_asset_pk");

                entity.ToTable("release_asset");

                entity.HasIndex(e => e.ReleaseAssetId)
                    .HasName("release_asset_release_asset_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Tag)
                    .HasColumnName("tag")
                    .HasMaxLength(100);

                entity.Property(e => e.FileName)
                    .HasColumnName("file_name")
                    .HasMaxLength(260);

                entity.Property(e => e.Label)
                    .HasColumnName("label")
                    .HasMaxLength(100);

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.ReleaseAssetId)
                    .HasColumnName("release_asset_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Size).HasColumnName("size");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.Uploader)
                    .IsRequired()
                    .HasColumnName("uploader")
                    .HasMaxLength(100);

                entity.HasOne(d => d.ReleaseTag)
                    .WithMany(p => p.ReleaseAsset)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName, d.Tag })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_release_asset_fk0");
            });

            modelBuilder.Entity<ReleaseTag>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.Tag })
                    .HasName("idx_release_tag_pk");

                entity.ToTable("release_tag");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Tag)
                    .HasColumnName("tag")
                    .HasMaxLength(100);

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasColumnName("author")
                    .HasMaxLength(100);

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.ReleaseTag)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_release_tag_fk0");
            });

            modelBuilder.Entity<Repository>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName })
                    .HasName("idx_repository_pk");

                entity.ToTable("repository");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.AllowFork)
                    .IsRequired()
                    .HasColumnName("allow_fork")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.DefaultBranch)
                    .HasColumnName("default_branch")
                    .HasMaxLength(100);

                entity.Property(e => e.DefaultMergeOption)
                    .IsRequired()
                    .HasColumnName("default_merge_option")
                    .HasMaxLength(100)
                    .HasDefaultValueSql("'merge-commit'::character varying");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ExternalIssuesUrl)
                    .HasColumnName("external_issues_url")
                    .HasMaxLength(200);

                entity.Property(e => e.ExternalWikiUrl)
                    .HasColumnName("external_wiki_url")
                    .HasMaxLength(200);

                entity.Property(e => e.IssuesOption)
                    .IsRequired()
                    .HasColumnName("issues_option")
                    .HasMaxLength(10)
                    .HasDefaultValueSql("'DISABLE'::character varying");

                entity.Property(e => e.LastActivityDate).HasColumnName("last_activity_date");

                entity.Property(e => e.MergeOptions)
                    .IsRequired()
                    .HasColumnName("merge_options")
                    .HasMaxLength(200)
                    .HasDefaultValueSql("'merge-commit,squash,rebase'::character varying");

                entity.Property(e => e.OriginRepositoryName)
                    .HasColumnName("origin_repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.OriginUserName)
                    .HasColumnName("origin_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.ParentRepositoryName)
                    .HasColumnName("parent_repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.ParentUserName)
                    .HasColumnName("parent_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Private).HasColumnName("private");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.WikiOption)
                    .IsRequired()
                    .HasColumnName("wiki_option")
                    .HasMaxLength(10)
                    .HasDefaultValueSql("'DISABLE'::character varying");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Repository)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_repository_fk0");
            });

            modelBuilder.Entity<SshKey>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.SshKeyId })
                    .HasName("idx_ssh_key_pk");

                entity.ToTable("ssh_key");

                entity.HasIndex(e => e.SshKeyId)
                    .HasName("ssh_key_ssh_key_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.SshKeyId)
                    .HasColumnName("ssh_key_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.PublicKey)
                    .IsRequired()
                    .HasColumnName("public_key");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(100);

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.SshKey)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_ssh_key_fk0");
            });

            modelBuilder.Entity<Versions>(entity =>
            {
                entity.HasKey(e => e.ModuleId)
                    .HasName("versions_pk");

                entity.ToTable("versions");

                entity.Property(e => e.ModuleId)
                    .HasColumnName("module_id")
                    .HasMaxLength(100)
                    .ValueGeneratedNever();

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Watch>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.NotificationUserName })
                    .HasName("idx_watch_pk");

                entity.ToTable("watch");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.NotificationUserName)
                    .HasColumnName("notification_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Notification)
                    .IsRequired()
                    .HasColumnName("notification")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<WebHook>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.Url })
                    .HasName("idx_web_hook_pk");

                entity.ToTable("web_hook");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(200);

                entity.Property(e => e.Ctype)
                    .HasColumnName("ctype")
                    .HasMaxLength(10);

                entity.Property(e => e.Token)
                    .HasColumnName("token")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.WebHook)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_web_hook_fk0");
            });

            modelBuilder.Entity<WebHookEvent>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.Url, e.Event })
                    .HasName("idx_web_hook_event_pk");

                entity.ToTable("web_hook_event");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(200);

                entity.Property(e => e.Event)
                    .HasColumnName("event")
                    .HasMaxLength(30);

                entity.HasOne(d => d.WebHook)
                    .WithMany(p => p.WebHookEvent)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName, d.Url })
                    .HasConstraintName("idx_web_hook_event_fk0");
            });
        }
    }
}

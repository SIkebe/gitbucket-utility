using GitBucket.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GitBucket.Core
{
    public partial class GitBucketDbContext : DbContext
    {
        public GitBucketDbContext()
        {
        }

        public GitBucketDbContext(DbContextOptions<GitBucketDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccessToken> AccessTokens { get; set; } = null!;
        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<AccountExtraMailAddress> AccountExtraMailAddresses { get; set; } = null!;
        public virtual DbSet<AccountFederation> AccountFederations { get; set; } = null!;
        public virtual DbSet<AccountPreference> AccountPreferences { get; set; } = null!;
        public virtual DbSet<AccountWebHook> AccountWebHooks { get; set; } = null!;
        public virtual DbSet<AccountWebHookEvent> AccountWebHookEvents { get; set; } = null!;
        public virtual DbSet<Collaborator> Collaborators { get; set; } = null!;
        public virtual DbSet<CommitComment> CommitComments { get; set; } = null!;
        public virtual DbSet<CommitStatus> CommitStatuses { get; set; } = null!;
        public virtual DbSet<DeployKey> DeployKeys { get; set; } = null!;
        public virtual DbSet<Gist> Gists { get; set; } = null!;
        public virtual DbSet<GistComment> GistComments { get; set; } = null!;
        public virtual DbSet<GpgKey> GpgKeys { get; set; } = null!;
        public virtual DbSet<GroupMember> GroupMembers { get; set; } = null!;
        public virtual DbSet<Issue> Issues { get; set; } = null!;
        public virtual DbSet<IssueComment> IssueComments { get; set; } = null!;
        public virtual DbSet<IssueId> IssueIds { get; set; } = null!;
        public virtual DbSet<IssueLabel> IssueLabels { get; set; } = null!;
        public virtual DbSet<IssueNotification> IssueNotifications { get; set; } = null!;
        public virtual DbSet<IssueOutlineView> IssueOutlineViews { get; set; } = null!;
        public virtual DbSet<Label> Labels { get; set; } = null!;
        public virtual DbSet<Milestone> Milestones { get; set; } = null!;
        public virtual DbSet<NotificationsAccount> NotificationsAccounts { get; set; } = null!;
        public virtual DbSet<Page> Pages { get; set; } = null!;
        public virtual DbSet<Plugin> Plugins { get; set; } = null!;
        public virtual DbSet<Priority> Priorities { get; set; } = null!;
        public virtual DbSet<ProtectedBranch> ProtectedBranches { get; set; } = null!;
        public virtual DbSet<ProtectedBranchRequireContext> ProtectedBranchRequireContexts { get; set; } = null!;
        public virtual DbSet<PullRequest> PullRequests { get; set; } = null!;
        public virtual DbSet<ReleaseAsset> ReleaseAssets { get; set; } = null!;
        public virtual DbSet<ReleaseTag> ReleaseTags { get; set; } = null!;
        public virtual DbSet<Repository> Repositories { get; set; } = null!;
        public virtual DbSet<SshKey> SshKeys { get; set; } = null!;
        public virtual DbSet<Models.Version> Versions { get; set; } = null!;
        public virtual DbSet<Watch> Watches { get; set; } = null!;
        public virtual DbSet<WebHook> WebHooks { get; set; } = null!;
        public virtual DbSet<WebHookBk> WebHookBks { get; set; } = null!;
        public virtual DbSet<WebHookEvent> WebHookEvents { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.HasAnnotation("Relational:Collation", "C");

            modelBuilder.Entity<AccessToken>(entity =>
            {
                entity.ToTable("access_token");

                entity.HasIndex(e => e.TokenHash, "idx_access_token_token_hash")
                    .IsUnique();

                entity.Property(e => e.AccessTokenId).HasColumnName("access_token_id");

                entity.Property(e => e.Note).HasColumnName("note");

                entity.Property(e => e.TokenHash)
                    .HasMaxLength(40)
                    .HasColumnName("token_hash");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.AccessTokens)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("idx_access_token_fk0");
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("idx_account_pk");

                entity.ToTable("account");

                entity.HasIndex(e => e.MailAddress, "idx_account_1")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.Administrator).HasColumnName("administrator");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .HasColumnName("full_name");

                entity.Property(e => e.GroupAccount).HasColumnName("group_account");

                entity.Property(e => e.Image)
                    .HasMaxLength(100)
                    .HasColumnName("image");

                entity.Property(e => e.LastLoginDate).HasColumnName("last_login_date");

                entity.Property(e => e.MailAddress)
                    .HasMaxLength(100)
                    .HasColumnName("mail_address");

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .HasColumnName("password");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.Removed).HasColumnName("removed");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.Url)
                    .HasMaxLength(200)
                    .HasColumnName("url");
            });

            modelBuilder.Entity<AccountExtraMailAddress>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.ExtraMailAddress })
                    .HasName("idx_account_extra_mail_address_pk");

                entity.ToTable("account_extra_mail_address");

                entity.HasIndex(e => e.ExtraMailAddress, "idx_account_extra_mail_address_1")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.ExtraMailAddress)
                    .HasMaxLength(100)
                    .HasColumnName("extra_mail_address");
            });

            modelBuilder.Entity<AccountFederation>(entity =>
            {
                entity.HasKey(e => new { e.Issuer, e.Subject })
                    .HasName("idx_account_federation_pk");

                entity.ToTable("account_federation");

                entity.Property(e => e.Issuer)
                    .HasMaxLength(100)
                    .HasColumnName("issuer");

                entity.Property(e => e.Subject)
                    .HasMaxLength(100)
                    .HasColumnName("subject");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.AccountFederations)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_account_federation_fk0");
            });

            modelBuilder.Entity<AccountPreference>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("idx_account_preference_pk");

                entity.ToTable("account_preference");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.HighlighterTheme)
                    .HasMaxLength(100)
                    .HasColumnName("highlighter_theme")
                    .HasDefaultValueSql("'prettify'::character varying");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithOne(p => p.AccountPreference)
                    .HasForeignKey<AccountPreference>(d => d.UserName)
                    .HasConstraintName("idx_account_preference_fk0");
            });

            modelBuilder.Entity<AccountWebHook>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.Url })
                    .HasName("idx_account_web_hook_pk");

                entity.ToTable("account_web_hook");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.Url)
                    .HasMaxLength(200)
                    .HasColumnName("url");

                entity.Property(e => e.Ctype)
                    .HasMaxLength(10)
                    .HasColumnName("ctype");

                entity.Property(e => e.Token)
                    .HasMaxLength(100)
                    .HasColumnName("token");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.AccountWebHooks)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_account_web_hook_fk0");
            });

            modelBuilder.Entity<AccountWebHookEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("account_web_hook_event");

                entity.Property(e => e.Event)
                    .HasMaxLength(30)
                    .HasColumnName("event");

                entity.Property(e => e.Url)
                    .HasMaxLength(200)
                    .HasColumnName("url");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");
            });

            modelBuilder.Entity<Collaborator>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.CollaboratorName })
                    .HasName("idx_collaborator_pk");

                entity.ToTable("collaborator");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.CollaboratorName)
                    .HasMaxLength(100)
                    .HasColumnName("collaborator_name");

                entity.Property(e => e.Role)
                    .HasMaxLength(10)
                    .HasColumnName("role")
                    .HasDefaultValueSql("'ADMIN'::character varying");

                entity.HasOne(d => d.CollaboratorNameNavigation)
                    .WithMany(p => p.Collaborators)
                    .HasForeignKey(d => d.CollaboratorName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_collaborator_fk1");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Collaborators)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_collaborator_fk0");
            });

            modelBuilder.Entity<CommitComment>(entity =>
            {
                entity.HasKey(e => e.CommentId)
                    .HasName("idx_commit_comment_pk");

                entity.ToTable("commit_comment");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.CommentedUserName)
                    .HasMaxLength(100)
                    .HasColumnName("commented_user_name");

                entity.Property(e => e.CommitId)
                    .HasMaxLength(100)
                    .HasColumnName("commit_id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.FileName)
                    .HasMaxLength(260)
                    .HasColumnName("file_name");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.NewLineNumber).HasColumnName("new_line_number");

                entity.Property(e => e.OldLineNumber).HasColumnName("old_line_number");

                entity.Property(e => e.OriginalCommitId)
                    .HasMaxLength(100)
                    .HasColumnName("original_commit_id");

                entity.Property(e => e.OriginalNewLine).HasColumnName("original_new_line");

                entity.Property(e => e.OriginalOldLine).HasColumnName("original_old_line");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.CommitComments)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_commit_comment_fk0");
            });

            modelBuilder.Entity<CommitStatus>(entity =>
            {
                entity.ToTable("commit_status");

                entity.HasIndex(e => new { e.UserName, e.RepositoryName, e.CommitId, e.Context }, "idx_commit_status_1")
                    .IsUnique();

                entity.Property(e => e.CommitStatusId).HasColumnName("commit_status_id");

                entity.Property(e => e.CommitId)
                    .HasMaxLength(40)
                    .HasColumnName("commit_id");

                entity.Property(e => e.Context)
                    .HasMaxLength(255)
                    .HasColumnName("context");

                entity.Property(e => e.Creator)
                    .HasMaxLength(100)
                    .HasColumnName("creator");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.State)
                    .HasMaxLength(10)
                    .HasColumnName("state");

                entity.Property(e => e.TargetUrl)
                    .HasMaxLength(200)
                    .HasColumnName("target_url");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.HasOne(d => d.CreatorNavigation)
                    .WithMany(p => p.CommitStatusCreatorNavigations)
                    .HasForeignKey(d => d.Creator)
                    .HasConstraintName("idx_commit_status_fk3");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.CommitStatusUserNameNavigations)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("idx_commit_status_fk2");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.CommitStatuses)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .HasConstraintName("idx_commit_status_fk1");
            });

            modelBuilder.Entity<DeployKey>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.DeployKeyId })
                    .HasName("idx_deploy_key_pk");

                entity.ToTable("deploy_key");

                entity.HasIndex(e => e.DeployKeyId, "deploy_key_deploy_key_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.DeployKeyId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("deploy_key_id");

                entity.Property(e => e.AllowWrite).HasColumnName("allow_write");

                entity.Property(e => e.PublicKey).HasColumnName("public_key");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.DeployKeys)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_deploy_key_fk0");
            });

            modelBuilder.Entity<Gist>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName })
                    .HasName("idx_gist_pk");

                entity.ToTable("gist");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Mode)
                    .HasMaxLength(10)
                    .HasColumnName("mode")
                    .HasDefaultValueSql("'PUBLIC'::character varying");

                entity.Property(e => e.OriginRepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("origin_repository_name");

                entity.Property(e => e.OriginUserName)
                    .HasMaxLength(100)
                    .HasColumnName("origin_user_name");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Gists)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_gist_fk0");
            });

            modelBuilder.Entity<GistComment>(entity =>
            {
                entity.HasKey(e => e.CommentId)
                    .HasName("idx_gist_comment_pk");

                entity.ToTable("gist_comment");

                entity.HasIndex(e => new { e.UserName, e.RepositoryName, e.CommentId }, "idx_gist_comment_1")
                    .IsUnique();

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.CommentedUserName)
                    .HasMaxLength(100)
                    .HasColumnName("commented_user_name");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.HasOne(d => d.CommentedUserNameNavigation)
                    .WithMany(p => p.GistComments)
                    .HasForeignKey(d => d.CommentedUserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_gist_comment_fk1");

                entity.HasOne(d => d.Gist)
                    .WithMany(p => p.GistComments)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_gist_comment_fk0");
            });

            modelBuilder.Entity<GpgKey>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.GpgKeyId })
                    .HasName("idx_gpg_key_pk");

                entity.ToTable("gpg_key");

                entity.HasIndex(e => e.KeyId, "gpg_key_key_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.GpgKeyId).HasColumnName("gpg_key_id");

                entity.Property(e => e.KeyId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("key_id");

                entity.Property(e => e.PublicKey).HasColumnName("public_key");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.GpgKeys)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_gpg_key_fk0");
            });

            modelBuilder.Entity<GroupMember>(entity =>
            {
                entity.HasKey(e => new { e.GroupName, e.UserName })
                    .HasName("idx_group_member_pk");

                entity.ToTable("group_member");

                entity.Property(e => e.GroupName)
                    .HasMaxLength(100)
                    .HasColumnName("group_name");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.Manager).HasColumnName("manager");

                entity.HasOne(d => d.GroupNameNavigation)
                    .WithMany(p => p.GroupMemberGroupNameNavigations)
                    .HasForeignKey(d => d.GroupName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_group_member_fk0");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.GroupMemberUserNameNavigations)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_group_member_fk1");
            });

            modelBuilder.Entity<Issue>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.IssueId })
                    .HasName("idx_issue_pk");

                entity.ToTable("issue");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.AssignedUserName)
                    .HasMaxLength(100)
                    .HasColumnName("assigned_user_name");

                entity.Property(e => e.Closed).HasColumnName("closed");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.MilestoneId).HasColumnName("milestone_id");

                entity.Property(e => e.OpenedUserName)
                    .HasMaxLength(100)
                    .HasColumnName("opened_user_name");

                entity.Property(e => e.PriorityId).HasColumnName("priority_id");

                entity.Property(e => e.PullRequest).HasColumnName("pull_request");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.Title).HasColumnName("title");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.HasOne(d => d.Milestone)
                    .WithMany(p => p.Issues)
                    .HasPrincipalKey(p => p.MilestoneId)
                    .HasForeignKey(d => d.MilestoneId)
                    .HasConstraintName("idx_issue_fk2");

                entity.HasOne(d => d.OpenedUserNameNavigation)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => d.OpenedUserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_issue_fk1");

                entity.HasOne(d => d.Priority)
                    .WithMany(p => p.Issues)
                    .HasPrincipalKey(p => p.PriorityId)
                    .HasForeignKey(d => d.PriorityId)
                    .HasConstraintName("idx_issue_fk3");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_issue_fk0");
            });

            modelBuilder.Entity<IssueComment>(entity =>
            {
                entity.HasKey(e => e.CommentId)
                    .HasName("idx_issue_comment_pk");

                entity.ToTable("issue_comment");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.Action)
                    .HasMaxLength(20)
                    .HasColumnName("action");

                entity.Property(e => e.CommentedUserName)
                    .HasMaxLength(100)
                    .HasColumnName("commented_user_name");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.IssueComments)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName, d.IssueId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_issue_comment_fk0");
            });

            modelBuilder.Entity<IssueId>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName })
                    .HasName("idx_issue_id_pk");

                entity.ToTable("issue_id");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

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
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.LabelId).HasColumnName("label_id");

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.IssueLabels)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName, d.IssueId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_issue_label_fk0");
            });

            modelBuilder.Entity<IssueNotification>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.IssueId, e.NotificationUserName })
                    .HasName("idx_issue_notification_pk");

                entity.ToTable("issue_notification");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.NotificationUserName)
                    .HasMaxLength(100)
                    .HasColumnName("notification_user_name");

                entity.Property(e => e.Subscribed).HasColumnName("subscribed");
            });

            modelBuilder.Entity<IssueOutlineView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("issue_outline_view");

                entity.Property(e => e.CommentCount).HasColumnName("comment_count");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.Priority).HasColumnName("priority");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");
            });

            modelBuilder.Entity<Label>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.LabelId })
                    .HasName("idx_label_pk");

                entity.ToTable("label");

                entity.HasIndex(e => e.LabelId, "label_label_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.LabelId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("label_id");

                entity.Property(e => e.Color)
                    .HasMaxLength(6)
                    .HasColumnName("color")
                    .IsFixedLength();

                entity.Property(e => e.LabelName)
                    .HasMaxLength(100)
                    .HasColumnName("label_name");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Labels)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_label_fk0");
            });

            modelBuilder.Entity<Milestone>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.MilestoneId })
                    .HasName("idx_milestone_pk");

                entity.ToTable("milestone");

                entity.HasIndex(e => e.MilestoneId, "milestone_milestone_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.MilestoneId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("milestone_id");

                entity.Property(e => e.ClosedDate).HasColumnName("closed_date");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.DueDate).HasColumnName("due_date");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Milestones)
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
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.DisableEmail).HasColumnName("disable_email");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithOne(p => p.NotificationsAccount)
                    .HasForeignKey<NotificationsAccount>(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_notifications_account_fk0");
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName })
                    .HasName("idx_pages_pk");

                entity.ToTable("pages");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.Source)
                    .HasMaxLength(100)
                    .HasColumnName("source");
            });

            modelBuilder.Entity<Plugin>(entity =>
            {
                entity.ToTable("plugin");

                entity.Property(e => e.PluginId)
                    .HasMaxLength(100)
                    .HasColumnName("plugin_id");

                entity.Property(e => e.Version)
                    .HasMaxLength(100)
                    .HasColumnName("version");
            });

            modelBuilder.Entity<Priority>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.PriorityId })
                    .HasName("idx_priority_pk");

                entity.ToTable("priority");

                entity.HasIndex(e => e.PriorityId, "priority_priority_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.PriorityId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("priority_id");

                entity.Property(e => e.Color)
                    .HasMaxLength(6)
                    .HasColumnName("color")
                    .IsFixedLength();

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.IsDefault).HasColumnName("is_default");

                entity.Property(e => e.Ordering).HasColumnName("ordering");

                entity.Property(e => e.PriorityName)
                    .HasMaxLength(100)
                    .HasColumnName("priority_name");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Priorities)
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
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.Branch)
                    .HasMaxLength(100)
                    .HasColumnName("branch");

                entity.Property(e => e.StatusCheckAdmin).HasColumnName("status_check_admin");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.ProtectedBranches)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .HasConstraintName("idx_protected_branch_fk0");
            });

            modelBuilder.Entity<ProtectedBranchRequireContext>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.Branch, e.Context })
                    .HasName("idx_protected_branch_require_context_pk");

                entity.ToTable("protected_branch_require_context");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.Branch)
                    .HasMaxLength(100)
                    .HasColumnName("branch");

                entity.Property(e => e.Context)
                    .HasMaxLength(255)
                    .HasColumnName("context");

                entity.HasOne(d => d.ProtectedBranch)
                    .WithMany(p => p.ProtectedBranchRequireContexts)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName, d.Branch })
                    .HasConstraintName("idx_protected_branch_require_context_fk0");
            });

            modelBuilder.Entity<PullRequest>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.IssueId })
                    .HasName("idx_pull_request_pk");

                entity.ToTable("pull_request");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.Branch)
                    .HasMaxLength(100)
                    .HasColumnName("branch");

                entity.Property(e => e.CommitIdFrom)
                    .HasMaxLength(40)
                    .HasColumnName("commit_id_from");

                entity.Property(e => e.CommitIdTo)
                    .HasMaxLength(40)
                    .HasColumnName("commit_id_to");

                entity.Property(e => e.IsDraft).HasColumnName("is_draft");

                entity.Property(e => e.RequestBranch)
                    .HasMaxLength(100)
                    .HasColumnName("request_branch");

                entity.Property(e => e.RequestRepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("request_repository_name");

                entity.Property(e => e.RequestUserName)
                    .HasMaxLength(100)
                    .HasColumnName("request_user_name");

                entity.HasOne(d => d.Issue)
                    .WithOne(p => p.PullRequestNavigation)
                    .HasForeignKey<PullRequest>(d => new { d.UserName, d.RepositoryName, d.IssueId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_pull_request_fk0");
            });

            modelBuilder.Entity<ReleaseAsset>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.Tag, e.FileName })
                    .HasName("idx_release_asset_pk");

                entity.ToTable("release_asset");

                entity.HasIndex(e => e.ReleaseAssetId, "release_asset_release_asset_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.Tag)
                    .HasMaxLength(100)
                    .HasColumnName("tag");

                entity.Property(e => e.FileName)
                    .HasMaxLength(260)
                    .HasColumnName("file_name");

                entity.Property(e => e.Label)
                    .HasMaxLength(100)
                    .HasColumnName("label");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.ReleaseAssetId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("release_asset_id");

                entity.Property(e => e.Size).HasColumnName("size");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.Uploader)
                    .HasMaxLength(100)
                    .HasColumnName("uploader");

                entity.HasOne(d => d.ReleaseTag)
                    .WithMany(p => p.ReleaseAssets)
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
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.Tag)
                    .HasMaxLength(100)
                    .HasColumnName("tag");

                entity.Property(e => e.Author)
                    .HasMaxLength(100)
                    .HasColumnName("author");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.ReleaseTags)
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
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.AllowFork)
                    .IsRequired()
                    .HasColumnName("allow_fork")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.DefaultBranch)
                    .HasMaxLength(100)
                    .HasColumnName("default_branch");

                entity.Property(e => e.DefaultMergeOption)
                    .HasMaxLength(100)
                    .HasColumnName("default_merge_option")
                    .HasDefaultValueSql("'merge-commit'::character varying");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ExternalIssuesUrl)
                    .HasMaxLength(200)
                    .HasColumnName("external_issues_url");

                entity.Property(e => e.ExternalWikiUrl)
                    .HasMaxLength(200)
                    .HasColumnName("external_wiki_url");

                entity.Property(e => e.IssuesOption)
                    .HasMaxLength(10)
                    .HasColumnName("issues_option")
                    .HasDefaultValueSql("'DISABLE'::character varying");

                entity.Property(e => e.LastActivityDate).HasColumnName("last_activity_date");

                entity.Property(e => e.MergeOptions)
                    .HasMaxLength(200)
                    .HasColumnName("merge_options")
                    .HasDefaultValueSql("'merge-commit,squash,rebase'::character varying");

                entity.Property(e => e.OriginRepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("origin_repository_name");

                entity.Property(e => e.OriginUserName)
                    .HasMaxLength(100)
                    .HasColumnName("origin_user_name");

                entity.Property(e => e.ParentRepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("parent_repository_name");

                entity.Property(e => e.ParentUserName)
                    .HasMaxLength(100)
                    .HasColumnName("parent_user_name");

                entity.Property(e => e.Private).HasColumnName("private");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.WikiOption)
                    .HasMaxLength(10)
                    .HasColumnName("wiki_option")
                    .HasDefaultValueSql("'DISABLE'::character varying");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Repositories)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_repository_fk0");
            });

            modelBuilder.Entity<SshKey>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.SshKeyId })
                    .HasName("idx_ssh_key_pk");

                entity.ToTable("ssh_key");

                entity.HasIndex(e => e.SshKeyId, "ssh_key_ssh_key_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.SshKeyId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ssh_key_id");

                entity.Property(e => e.PublicKey).HasColumnName("public_key");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.SshKeys)
                    .HasForeignKey(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_ssh_key_fk0");
            });

            modelBuilder.Entity<Models.Version>(entity =>
            {
                entity.HasKey(e => e.ModuleId)
                    .HasName("versions_pk");

                entity.ToTable("versions");

                entity.Property(e => e.ModuleId)
                    .HasMaxLength(100)
                    .HasColumnName("module_id");

                entity.Property(e => e.Version1)
                    .HasMaxLength(100)
                    .HasColumnName("version");
            });

            modelBuilder.Entity<Watch>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.NotificationUserName })
                    .HasName("idx_watch_pk");

                entity.ToTable("watch");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.NotificationUserName)
                    .HasMaxLength(100)
                    .HasColumnName("notification_user_name");

                entity.Property(e => e.Notification)
                    .HasMaxLength(20)
                    .HasColumnName("notification");
            });

            modelBuilder.Entity<WebHook>(entity =>
            {
                entity.HasKey(e => e.HookId)
                    .HasName("idx_web_hook_pk");

                entity.ToTable("web_hook");

                entity.HasIndex(e => new { e.UserName, e.RepositoryName, e.Url }, "idx_web_hook_1")
                    .IsUnique();

                entity.Property(e => e.HookId)
                    .HasColumnName("hook_id")
                    .HasDefaultValueSql("nextval('web_hook_2_hook_id_seq'::regclass)");

                entity.Property(e => e.Ctype)
                    .HasMaxLength(10)
                    .HasColumnName("ctype");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.Token)
                    .HasMaxLength(100)
                    .HasColumnName("token");

                entity.Property(e => e.Url)
                    .HasMaxLength(200)
                    .HasColumnName("url");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.WebHooks)
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_web_hook_fk0");
            });

            modelBuilder.Entity<WebHookBk>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("web_hook_bk");

                entity.Property(e => e.Ctype)
                    .HasMaxLength(10)
                    .HasColumnName("ctype");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.Token)
                    .HasMaxLength(100)
                    .HasColumnName("token");

                entity.Property(e => e.Url)
                    .HasMaxLength(200)
                    .HasColumnName("url");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");
            });

            modelBuilder.Entity<WebHookEvent>(entity =>
            {
                entity.HasKey(e => new { e.UserName, e.RepositoryName, e.Url, e.Event })
                    .HasName("idx_web_hook_event_pk");

                entity.ToTable("web_hook_event");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.Url)
                    .HasMaxLength(200)
                    .HasColumnName("url");

                entity.Property(e => e.Event)
                    .HasMaxLength(30)
                    .HasColumnName("event");

                entity.HasOne(d => d.WebHook)
                    .WithMany(p => p.WebHookEvents)
                    .HasPrincipalKey(p => new { p.UserName, p.RepositoryName, p.Url })
                    .HasForeignKey(d => new { d.UserName, d.RepositoryName, d.Url })
                    .HasConstraintName("idx_web_hook_event_fk0");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

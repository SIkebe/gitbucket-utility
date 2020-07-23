using GitBucket.Core.Models;
using Microsoft.EntityFrameworkCore;

#nullable disable

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

        public virtual DbSet<AccessToken> AccessTokens { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountExtraMailAddress> AccountExtraMailAddresses { get; set; }
        public virtual DbSet<AccountFederation> AccountFederations { get; set; }
        public virtual DbSet<AccountWebHook> AccountWebHooks { get; set; }
        public virtual DbSet<AccountWebHookEvent> AccountWebHookEvents { get; set; }
        public virtual DbSet<Collaborator> Collaborators { get; set; }
        public virtual DbSet<CommitComment> CommitComments { get; set; }
        public virtual DbSet<CommitStatus> CommitStatuses { get; set; }
        public virtual DbSet<DeployKey> DeployKeys { get; set; }
        public virtual DbSet<Gist> Gists { get; set; }
        public virtual DbSet<GistComment> GistComments { get; set; }
        public virtual DbSet<GpgKey> GpgKeys { get; set; }
        public virtual DbSet<GroupMember> GroupMembers { get; set; }
        public virtual DbSet<Issue> Issues { get; set; }
        public virtual DbSet<IssueComment> IssueComments { get; set; }
        public virtual DbSet<IssueId> IssueIds { get; set; }
        public virtual DbSet<IssueLabel> IssueLabels { get; set; }
        public virtual DbSet<IssueNotification> IssueNotifications { get; set; }
        public virtual DbSet<IssueOutlineView> IssueOutlineViews { get; set; }
        public virtual DbSet<Label> Labels { get; set; }
        public virtual DbSet<Milestone> Milestones { get; set; }
        public virtual DbSet<NotificationsAccount> NotificationsAccounts { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<Plugin> Plugins { get; set; }
        public virtual DbSet<Priority> Priorities { get; set; }
        public virtual DbSet<ProtectedBranch> ProtectedBranches { get; set; }
        public virtual DbSet<ProtectedBranchRequireContext> ProtectedBranchRequireContexts { get; set; }
        public virtual DbSet<PullRequest> PullRequests { get; set; }
        public virtual DbSet<ReleaseAsset> ReleaseAssets { get; set; }
        public virtual DbSet<ReleaseTag> ReleaseTags { get; set; }
        public virtual DbSet<Repository> Repositories { get; set; }
        public virtual DbSet<SshKey> SshKeys { get; set; }
        public virtual DbSet<Version> Versions { get; set; }
        public virtual DbSet<Watch> Watches { get; set; }
        public virtual DbSet<WebHook> WebHooks { get; set; }
        public virtual DbSet<WebHookEvent> WebHookEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<AccessToken>(entity =>
            {
                entity.ToTable("access_token");

                entity.HasIndex(x => x.AccessTokenId, "access_token_access_token_id_key")
                    .IsUnique();

                entity.HasIndex(x => x.TokenHash, "idx_access_token_token_hash")
                    .IsUnique();

                entity.Property(e => e.AccessTokenId).HasColumnName("access_token_id");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasColumnName("note");

                entity.Property(e => e.TokenHash)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("token_hash");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.AccessTokens)
                    .HasForeignKey(x => x.UserName)
                    .HasConstraintName("idx_access_token_fk0");
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(x => x.UserName)
                    .HasName("idx_account_pk");

                entity.ToTable("account");

                entity.HasIndex(x => x.MailAddress, "idx_account_1")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.Administrator).HasColumnName("administrator");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("full_name");

                entity.Property(e => e.GroupAccount).HasColumnName("group_account");

                entity.Property(e => e.Image)
                    .HasMaxLength(100)
                    .HasColumnName("image");

                entity.Property(e => e.LastLoginDate).HasColumnName("last_login_date");

                entity.Property(e => e.MailAddress)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("mail_address");

                entity.Property(e => e.Password)
                    .IsRequired()
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
                entity.HasKey(x => new { x.UserName, x.ExtraMailAddress })
                    .HasName("idx_account_extra_mail_address_pk");

                entity.ToTable("account_extra_mail_address");

                entity.HasIndex(x => x.ExtraMailAddress, "idx_account_extra_mail_address_1")
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
                entity.HasKey(x => new { x.Issuer, x.Subject })
                    .HasName("idx_account_federation_pk");

                entity.ToTable("account_federation");

                entity.Property(e => e.Issuer)
                    .HasMaxLength(100)
                    .HasColumnName("issuer");

                entity.Property(e => e.Subject)
                    .HasMaxLength(100)
                    .HasColumnName("subject");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.AccountFederations)
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
                    .HasMaxLength(30)
                    .HasColumnName("event");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("url");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("user_name");
            });

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.ToTable("activity");

                entity.HasIndex(x => x.ActivityId, "activity_activity_id_key")
                    .IsUnique();

                entity.Property(e => e.ActivityId).HasColumnName("activity_id");

                entity.Property(e => e.ActivityDate).HasColumnName("activity_date");

                entity.Property(e => e.ActivityType)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("activity_type");

                entity.Property(e => e.ActivityUserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("activity_user_name");

                entity.Property(e => e.AdditionalInfo).HasColumnName("additional_info");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.HasOne(d => d.ActivityUserNameNavigation)
                    .WithMany(p => p.Activities)
                    .HasForeignKey(x => x.ActivityUserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_activity_fk1");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Activities)
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
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.CollaboratorName)
                    .HasMaxLength(100)
                    .HasColumnName("collaborator_name");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("role")
                    .HasDefaultValueSql("'ADMIN'::character varying");

                entity.HasOne(d => d.CollaboratorNameNavigation)
                    .WithMany(p => p.Collaborators)
                    .HasForeignKey(x => x.CollaboratorName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_collaborator_fk1");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Collaborators)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_collaborator_fk0");
            });

            modelBuilder.Entity<CommitComment>(entity =>
            {
                entity.HasKey(x => x.CommentId)
                    .HasName("idx_commit_comment_pk");

                entity.ToTable("commit_comment");

                entity.HasIndex(x => x.CommentId, "commit_comment_comment_id_key")
                    .IsUnique();

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.CommentedUserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("commented_user_name");

                entity.Property(e => e.CommitId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("commit_id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content");

                entity.Property(e => e.FileName)
                    .HasMaxLength(260)
                    .HasColumnName("file_name");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.NewLineNumber).HasColumnName("new_line_number");

                entity.Property(e => e.OldLineNumber).HasColumnName("old_line_number");

                entity.Property(e => e.OriginalCommitId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("original_commit_id");

                entity.Property(e => e.OriginalNewLine).HasColumnName("original_new_line");

                entity.Property(e => e.OriginalOldLine).HasColumnName("original_old_line");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.CommitComments)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_commit_comment_fk0");
            });

            modelBuilder.Entity<CommitStatus>(entity =>
            {
                entity.ToTable("commit_status");

                entity.HasIndex(x => x.CommitStatusId, "commit_status_commit_status_id_key")
                    .IsUnique();

                entity.HasIndex(x => new { x.UserName, x.RepositoryName, x.CommitId, x.Context }, "idx_commit_status_1")
                    .IsUnique();

                entity.Property(e => e.CommitStatusId).HasColumnName("commit_status_id");

                entity.Property(e => e.CommitId)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("commit_id");

                entity.Property(e => e.Context)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("context");

                entity.Property(e => e.Creator)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("creator");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("state");

                entity.Property(e => e.TargetUrl)
                    .HasMaxLength(200)
                    .HasColumnName("target_url");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.HasOne(d => d.CreatorNavigation)
                    .WithMany(p => p.CommitStatusCreatorNavigations)
                    .HasForeignKey(x => x.Creator)
                    .HasConstraintName("idx_commit_status_fk3");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.CommitStatusUserNameNavigations)
                    .HasForeignKey(x => x.UserName)
                    .HasConstraintName("idx_commit_status_fk2");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.CommitStatuses)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .HasConstraintName("idx_commit_status_fk1");
            });

            modelBuilder.Entity<DeployKey>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.DeployKeyId })
                    .HasName("idx_deploy_key_pk");

                entity.ToTable("deploy_key");

                entity.HasIndex(x => x.DeployKeyId, "deploy_key_deploy_key_id_key")
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

                entity.Property(e => e.PublicKey)
                    .IsRequired()
                    .HasColumnName("public_key");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.DeployKeys)
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
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Mode)
                    .IsRequired()
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
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Gists)
                    .HasForeignKey(x => x.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_gist_fk0");
            });

            modelBuilder.Entity<GistComment>(entity =>
            {
                entity.HasKey(x => x.CommentId)
                    .HasName("idx_gist_comment_pk");

                entity.ToTable("gist_comment");

                entity.HasIndex(x => x.CommentId, "gist_comment_comment_id_key")
                    .IsUnique();

                entity.HasIndex(x => new { x.UserName, x.RepositoryName, x.CommentId }, "idx_gist_comment_1")
                    .IsUnique();

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.CommentedUserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("commented_user_name");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.HasOne(d => d.CommentedUserNameNavigation)
                    .WithMany(p => p.GistComments)
                    .HasForeignKey(x => x.CommentedUserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_gist_comment_fk1");

                entity.HasOne(d => d.Gist)
                    .WithMany(p => p.GistComments)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_gist_comment_fk0");
            });

            modelBuilder.Entity<GpgKey>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.GpgKeyId })
                    .HasName("idx_gpg_key_pk");

                entity.ToTable("gpg_key");

                entity.HasIndex(x => x.KeyId, "gpg_key_key_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.GpgKeyId).HasColumnName("gpg_key_id");

                entity.Property(e => e.KeyId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("key_id");

                entity.Property(e => e.PublicKey)
                    .IsRequired()
                    .HasColumnName("public_key");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.GpgKeys)
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
                    .HasMaxLength(100)
                    .HasColumnName("group_name");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.Manager).HasColumnName("manager");

                entity.HasOne(d => d.GroupNameNavigation)
                    .WithMany(p => p.GroupMemberGroupNameNavigations)
                    .HasForeignKey(x => x.GroupName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_group_member_fk0");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.GroupMemberUserNameNavigations)
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
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("opened_user_name");

                entity.Property(e => e.PriorityId).HasColumnName("priority_id");

                entity.Property(e => e.PullRequest).HasColumnName("pull_request");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.HasOne(d => d.Milestone)
                    .WithMany(p => p.Issues)
                    .HasPrincipalKey(x => x.MilestoneId)
                    .HasForeignKey(x => x.MilestoneId)
                    .HasConstraintName("idx_issue_fk2");

                entity.HasOne(d => d.OpenedUserNameNavigation)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(x => x.OpenedUserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_issue_fk1");

                entity.HasOne(d => d.Priority)
                    .WithMany(p => p.Issues)
                    .HasPrincipalKey(x => x.PriorityId)
                    .HasForeignKey(x => x.PriorityId)
                    .HasConstraintName("idx_issue_fk3");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_issue_fk0");
            });

            modelBuilder.Entity<IssueComment>(entity =>
            {
                entity.HasKey(x => x.CommentId)
                    .HasName("idx_issue_comment_pk");

                entity.ToTable("issue_comment");

                entity.HasIndex(x => x.CommentId, "issue_comment_comment_id_key")
                    .IsUnique();

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("action");

                entity.Property(e => e.CommentedUserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("commented_user_name");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.RepositoryName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.IssueComments)
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
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.IssueId1).HasColumnName("issue_id");

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
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.LabelId).HasColumnName("label_id");

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.IssueLabels)
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

                entity.ToTable("issue_outline_view");

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
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.LabelId })
                    .HasName("idx_label_pk");

                entity.ToTable("label");

                entity.HasIndex(x => x.LabelId, "label_label_id_key")
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
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("color")
                    .IsFixedLength(true);

                entity.Property(e => e.LabelName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("label_name");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Labels)
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_label_fk0");
            });

            modelBuilder.Entity<Milestone>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.MilestoneId })
                    .HasName("idx_milestone_pk");

                entity.ToTable("milestone");

                entity.HasIndex(x => x.MilestoneId, "milestone_milestone_id_key")
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
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Milestones)
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
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.DisableEmail).HasColumnName("disable_email");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithOne(p => p.NotificationsAccount)
                    .HasForeignKey<NotificationsAccount>(x => x.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_notifications_account_fk0");
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName })
                    .HasName("idx_pages_pk");

                entity.ToTable("pages");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.Source)
                    .IsRequired()
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
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("version");
            });

            modelBuilder.Entity<Priority>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.PriorityId })
                    .HasName("idx_priority_pk");

                entity.ToTable("priority");

                entity.HasIndex(x => x.PriorityId, "priority_priority_id_key")
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
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("color")
                    .IsFixedLength(true);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.IsDefault).HasColumnName("is_default");

                entity.Property(e => e.Ordering).HasColumnName("ordering");

                entity.Property(e => e.PriorityName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("priority_name");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.Priorities)
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
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName })
                    .HasConstraintName("idx_protected_branch_fk0");
            });

            modelBuilder.Entity<ProtectedBranchRequireContext>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.Branch, x.Context })
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
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName, x.Branch })
                    .HasConstraintName("idx_protected_branch_require_context_fk0");
            });

            modelBuilder.Entity<PullRequest>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.IssueId })
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
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("branch");

                entity.Property(e => e.CommitIdFrom)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("commit_id_from");

                entity.Property(e => e.CommitIdTo)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("commit_id_to");

                entity.Property(e => e.IsDraft).HasColumnName("is_draft");

                entity.Property(e => e.RequestBranch)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("request_branch");

                entity.Property(e => e.RequestRepositoryName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("request_repository_name");

                entity.Property(e => e.RequestUserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("request_user_name");

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

                entity.HasIndex(x => x.ReleaseAssetId, "release_asset_release_asset_id_key")
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
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("uploader");

                entity.HasOne(d => d.ReleaseTag)
                    .WithMany(p => p.ReleaseAssets)
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
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.Tag)
                    .HasMaxLength(100)
                    .HasColumnName("tag");

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("author");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.ReleaseTags)
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
                    .IsRequired()
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
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("issues_option")
                    .HasDefaultValueSql("'DISABLE'::character varying");

                entity.Property(e => e.LastActivityDate).HasColumnName("last_activity_date");

                entity.Property(e => e.MergeOptions)
                    .IsRequired()
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
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("wiki_option")
                    .HasDefaultValueSql("'DISABLE'::character varying");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Repositories)
                    .HasForeignKey(x => x.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_repository_fk0");
            });

            modelBuilder.Entity<SshKey>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.SshKeyId })
                    .HasName("idx_ssh_key_pk");

                entity.ToTable("ssh_key");

                entity.HasIndex(x => x.SshKeyId, "ssh_key_ssh_key_id_key")
                    .IsUnique();

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.SshKeyId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ssh_key_id");

                entity.Property(e => e.PublicKey)
                    .IsRequired()
                    .HasColumnName("public_key");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.SshKeys)
                    .HasForeignKey(x => x.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idx_ssh_key_fk0");
            });

            modelBuilder.Entity<Version>(entity =>
            {
                entity.HasKey(x => x.ModuleId)
                    .HasName("versions_pk");

                entity.ToTable("versions");

                entity.Property(e => e.ModuleId)
                    .HasMaxLength(100)
                    .HasColumnName("module_id");

                entity.Property(e => e.Version1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("version");
            });

            modelBuilder.Entity<Watch>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.NotificationUserName })
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
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("notification");
            });

            modelBuilder.Entity<WebHook>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.Url })
                    .HasName("idx_web_hook_pk");

                entity.ToTable("web_hook");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .HasColumnName("user_name");

                entity.Property(e => e.RepositoryName)
                    .HasMaxLength(100)
                    .HasColumnName("repository_name");

                entity.Property(e => e.Url)
                    .HasMaxLength(200)
                    .HasColumnName("url");

                entity.Property(e => e.Ctype)
                    .HasMaxLength(10)
                    .HasColumnName("ctype");

                entity.Property(e => e.Token)
                    .HasMaxLength(100)
                    .HasColumnName("token");

                entity.HasOne(d => d.Repository)
                    .WithMany(p => p.WebHooks)
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
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName, x.Url })
                    .HasConstraintName("idx_web_hook_event_fk0");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

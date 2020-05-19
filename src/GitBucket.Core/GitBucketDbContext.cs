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
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.HasAnnotation("Npgsql:CollationDefinition:pg_catalog.af-NA-x-icu", "af-NA,af-NA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.af-x-icu", "af,af,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.af-ZA-x-icu", "af-ZA,af-ZA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.agq-CM-x-icu", "agq-CM,agq-CM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.agq-x-icu", "agq,agq,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ak-GH-x-icu", "ak-GH,ak-GH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ak-x-icu", "ak,ak,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.am-ET-x-icu", "am-ET,am-ET,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.am-x-icu", "am,am,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-001-x-icu", "ar-001,ar-001,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-AE-x-icu", "ar-AE,ar-AE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-BH-x-icu", "ar-BH,ar-BH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-DJ-x-icu", "ar-DJ,ar-DJ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-DZ-x-icu", "ar-DZ,ar-DZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-EG-x-icu", "ar-EG,ar-EG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-EH-x-icu", "ar-EH,ar-EH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-ER-x-icu", "ar-ER,ar-ER,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-IL-x-icu", "ar-IL,ar-IL,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-IQ-x-icu", "ar-IQ,ar-IQ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-JO-x-icu", "ar-JO,ar-JO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-KM-x-icu", "ar-KM,ar-KM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-KW-x-icu", "ar-KW,ar-KW,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-LB-x-icu", "ar-LB,ar-LB,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-LY-x-icu", "ar-LY,ar-LY,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-MA-x-icu", "ar-MA,ar-MA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-MR-x-icu", "ar-MR,ar-MR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-OM-x-icu", "ar-OM,ar-OM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-PS-x-icu", "ar-PS,ar-PS,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-QA-x-icu", "ar-QA,ar-QA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-SA-x-icu", "ar-SA,ar-SA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-SD-x-icu", "ar-SD,ar-SD,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-SO-x-icu", "ar-SO,ar-SO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-SS-x-icu", "ar-SS,ar-SS,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-SY-x-icu", "ar-SY,ar-SY,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-TD-x-icu", "ar-TD,ar-TD,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-TN-x-icu", "ar-TN,ar-TN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-x-icu", "ar,ar,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ar-YE-x-icu", "ar-YE,ar-YE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.as-IN-x-icu", "as-IN,as-IN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.as-x-icu", "as,as,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.asa-TZ-x-icu", "asa-TZ,asa-TZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.asa-x-icu", "asa,asa,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ast-ES-x-icu", "ast-ES,ast-ES,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ast-x-icu", "ast,ast,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.az-Cyrl-AZ-x-icu", "az-Cyrl-AZ,az-Cyrl-AZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.az-Cyrl-x-icu", "az-Cyrl,az-Cyrl,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.az-Latn-AZ-x-icu", "az-Latn-AZ,az-Latn-AZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.az-Latn-x-icu", "az-Latn,az-Latn,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.az-x-icu", "az,az,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bas-CM-x-icu", "bas-CM,bas-CM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bas-x-icu", "bas,bas,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.be-BY-x-icu", "be-BY,be-BY,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.be-x-icu", "be,be,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bem-x-icu", "bem,bem,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bem-ZM-x-icu", "bem-ZM,bem-ZM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bez-TZ-x-icu", "bez-TZ,bez-TZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bez-x-icu", "bez,bez,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bg-BG-x-icu", "bg-BG,bg-BG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bg-x-icu", "bg,bg,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bm-ML-x-icu", "bm-ML,bm-ML,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bm-x-icu", "bm,bm,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bn-BD-x-icu", "bn-BD,bn-BD,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bn-IN-x-icu", "bn-IN,bn-IN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bn-x-icu", "bn,bn,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bo-CN-x-icu", "bo-CN,bo-CN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bo-IN-x-icu", "bo-IN,bo-IN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bo-x-icu", "bo,bo,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.br-FR-x-icu", "br-FR,br-FR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.br-x-icu", "br,br,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.brx-IN-x-icu", "brx-IN,brx-IN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.brx-x-icu", "brx,brx,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bs-Cyrl-BA-x-icu", "bs-Cyrl-BA,bs-Cyrl-BA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bs-Cyrl-x-icu", "bs-Cyrl,bs-Cyrl,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bs-Latn-BA-x-icu", "bs-Latn-BA,bs-Latn-BA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bs-Latn-x-icu", "bs-Latn,bs-Latn,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.bs-x-icu", "bs,bs,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.C", "C,C,libc,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ca-AD-x-icu", "ca-AD,ca-AD,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ca-ES-x-icu", "ca-ES,ca-ES,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ca-FR-x-icu", "ca-FR,ca-FR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ca-IT-x-icu", "ca-IT,ca-IT,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ca-x-icu", "ca,ca,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ccp-BD-x-icu", "ccp-BD,ccp-BD,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ccp-IN-x-icu", "ccp-IN,ccp-IN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ccp-x-icu", "ccp,ccp,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ce-RU-x-icu", "ce-RU,ce-RU,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ce-x-icu", "ce,ce,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ceb-PH-x-icu", "ceb-PH,ceb-PH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ceb-x-icu", "ceb,ceb,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.cgg-UG-x-icu", "cgg-UG,cgg-UG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.cgg-x-icu", "cgg,cgg,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.chr-US-x-icu", "chr-US,chr-US,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.chr-x-icu", "chr,chr,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ckb-IQ-x-icu", "ckb-IQ,ckb-IQ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ckb-IR-x-icu", "ckb-IR,ckb-IR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ckb-x-icu", "ckb,ckb,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.cs-CZ-x-icu", "cs-CZ,cs-CZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.cs-x-icu", "cs,cs,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.cy-GB-x-icu", "cy-GB,cy-GB,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.cy-x-icu", "cy,cy,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.da-DK-x-icu", "da-DK,da-DK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.da-GL-x-icu", "da-GL,da-GL,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.da-x-icu", "da,da,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.dav-KE-x-icu", "dav-KE,dav-KE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.dav-x-icu", "dav,dav,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.de-AT-x-icu", "de-AT,de-AT,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.de-BE-x-icu", "de-BE,de-BE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.de-CH-x-icu", "de-CH,de-CH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.de-DE-x-icu", "de-DE,de-DE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.de-IT-x-icu", "de-IT,de-IT,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.de-LI-x-icu", "de-LI,de-LI,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.de-LU-x-icu", "de-LU,de-LU,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.de-x-icu", "de,de,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.default", ",,,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.dje-NE-x-icu", "dje-NE,dje-NE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.dje-x-icu", "dje,dje,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.dsb-DE-x-icu", "dsb-DE,dsb-DE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.dsb-x-icu", "dsb,dsb,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.dua-CM-x-icu", "dua-CM,dua-CM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.dua-x-icu", "dua,dua,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.dyo-SN-x-icu", "dyo-SN,dyo-SN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.dyo-x-icu", "dyo,dyo,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.dz-BT-x-icu", "dz-BT,dz-BT,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.dz-x-icu", "dz,dz,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ebu-KE-x-icu", "ebu-KE,ebu-KE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ebu-x-icu", "ebu,ebu,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ee-GH-x-icu", "ee-GH,ee-GH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ee-TG-x-icu", "ee-TG,ee-TG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ee-x-icu", "ee,ee,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.el-CY-x-icu", "el-CY,el-CY,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.el-GR-x-icu", "el-GR,el-GR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.el-x-icu", "el,el,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-001-x-icu", "en-001,en-001,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-150-x-icu", "en-150,en-150,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-AE-x-icu", "en-AE,en-AE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-AG-x-icu", "en-AG,en-AG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-AI-x-icu", "en-AI,en-AI,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-AS-x-icu", "en-AS,en-AS,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-AT-x-icu", "en-AT,en-AT,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-AU-x-icu", "en-AU,en-AU,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-BB-x-icu", "en-BB,en-BB,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-BE-x-icu", "en-BE,en-BE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-BI-x-icu", "en-BI,en-BI,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-BM-x-icu", "en-BM,en-BM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-BS-x-icu", "en-BS,en-BS,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-BW-x-icu", "en-BW,en-BW,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-BZ-x-icu", "en-BZ,en-BZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-CA-x-icu", "en-CA,en-CA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-CC-x-icu", "en-CC,en-CC,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-CH-x-icu", "en-CH,en-CH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-CK-x-icu", "en-CK,en-CK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-CM-x-icu", "en-CM,en-CM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-CX-x-icu", "en-CX,en-CX,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-CY-x-icu", "en-CY,en-CY,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-DE-x-icu", "en-DE,en-DE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-DG-x-icu", "en-DG,en-DG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-DK-x-icu", "en-DK,en-DK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-DM-x-icu", "en-DM,en-DM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-ER-x-icu", "en-ER,en-ER,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-FI-x-icu", "en-FI,en-FI,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-FJ-x-icu", "en-FJ,en-FJ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-FK-x-icu", "en-FK,en-FK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-FM-x-icu", "en-FM,en-FM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-GB-x-icu", "en-GB,en-GB,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-GD-x-icu", "en-GD,en-GD,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-GG-x-icu", "en-GG,en-GG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-GH-x-icu", "en-GH,en-GH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-GI-x-icu", "en-GI,en-GI,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-GM-x-icu", "en-GM,en-GM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-GU-x-icu", "en-GU,en-GU,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-GY-x-icu", "en-GY,en-GY,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-HK-x-icu", "en-HK,en-HK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-IE-x-icu", "en-IE,en-IE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-IL-x-icu", "en-IL,en-IL,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-IM-x-icu", "en-IM,en-IM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-IN-x-icu", "en-IN,en-IN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-IO-x-icu", "en-IO,en-IO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-JE-x-icu", "en-JE,en-JE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-JM-x-icu", "en-JM,en-JM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-KE-x-icu", "en-KE,en-KE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-KI-x-icu", "en-KI,en-KI,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-KN-x-icu", "en-KN,en-KN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-KY-x-icu", "en-KY,en-KY,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-LC-x-icu", "en-LC,en-LC,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-LR-x-icu", "en-LR,en-LR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-LS-x-icu", "en-LS,en-LS,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-MG-x-icu", "en-MG,en-MG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-MH-x-icu", "en-MH,en-MH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-MO-x-icu", "en-MO,en-MO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-MP-x-icu", "en-MP,en-MP,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-MS-x-icu", "en-MS,en-MS,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-MT-x-icu", "en-MT,en-MT,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-MU-x-icu", "en-MU,en-MU,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-MW-x-icu", "en-MW,en-MW,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-MY-x-icu", "en-MY,en-MY,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-NA-x-icu", "en-NA,en-NA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-NF-x-icu", "en-NF,en-NF,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-NG-x-icu", "en-NG,en-NG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-NL-x-icu", "en-NL,en-NL,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-NR-x-icu", "en-NR,en-NR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-NU-x-icu", "en-NU,en-NU,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-NZ-x-icu", "en-NZ,en-NZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-PG-x-icu", "en-PG,en-PG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-PH-x-icu", "en-PH,en-PH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-PK-x-icu", "en-PK,en-PK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-PN-x-icu", "en-PN,en-PN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-PR-x-icu", "en-PR,en-PR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-PW-x-icu", "en-PW,en-PW,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-RW-x-icu", "en-RW,en-RW,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-SB-x-icu", "en-SB,en-SB,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-SC-x-icu", "en-SC,en-SC,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-SD-x-icu", "en-SD,en-SD,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-SE-x-icu", "en-SE,en-SE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-SG-x-icu", "en-SG,en-SG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-SH-x-icu", "en-SH,en-SH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-SI-x-icu", "en-SI,en-SI,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-SL-x-icu", "en-SL,en-SL,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-SS-x-icu", "en-SS,en-SS,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-SX-x-icu", "en-SX,en-SX,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-SZ-x-icu", "en-SZ,en-SZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-TC-x-icu", "en-TC,en-TC,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-TK-x-icu", "en-TK,en-TK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-TO-x-icu", "en-TO,en-TO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-TT-x-icu", "en-TT,en-TT,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-TV-x-icu", "en-TV,en-TV,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-TZ-x-icu", "en-TZ,en-TZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-UG-x-icu", "en-UG,en-UG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-UM-x-icu", "en-UM,en-UM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-US-u-va-posix-x-icu", "en-US-u-va-posix,en-US-u-va-posix,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-US-x-icu", "en-US,en-US,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-VC-x-icu", "en-VC,en-VC,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-VG-x-icu", "en-VG,en-VG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-VI-x-icu", "en-VI,en-VI,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-VU-x-icu", "en-VU,en-VU,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-WS-x-icu", "en-WS,en-WS,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-x-icu", "en,en,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-ZA-x-icu", "en-ZA,en-ZA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-ZM-x-icu", "en-ZM,en-ZM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.en-ZW-x-icu", "en-ZW,en-ZW,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.eo-001-x-icu", "eo-001,eo-001,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.eo-x-icu", "eo,eo,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-419-x-icu", "es-419,es-419,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-AR-x-icu", "es-AR,es-AR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-BO-x-icu", "es-BO,es-BO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-BR-x-icu", "es-BR,es-BR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-BZ-x-icu", "es-BZ,es-BZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-CL-x-icu", "es-CL,es-CL,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-CO-x-icu", "es-CO,es-CO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-CR-x-icu", "es-CR,es-CR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-CU-x-icu", "es-CU,es-CU,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-DO-x-icu", "es-DO,es-DO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-EA-x-icu", "es-EA,es-EA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-EC-x-icu", "es-EC,es-EC,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-ES-x-icu", "es-ES,es-ES,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-GQ-x-icu", "es-GQ,es-GQ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-GT-x-icu", "es-GT,es-GT,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-HN-x-icu", "es-HN,es-HN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-IC-x-icu", "es-IC,es-IC,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-MX-x-icu", "es-MX,es-MX,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-NI-x-icu", "es-NI,es-NI,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-PA-x-icu", "es-PA,es-PA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-PE-x-icu", "es-PE,es-PE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-PH-x-icu", "es-PH,es-PH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-PR-x-icu", "es-PR,es-PR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-PY-x-icu", "es-PY,es-PY,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-SV-x-icu", "es-SV,es-SV,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-US-x-icu", "es-US,es-US,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-UY-x-icu", "es-UY,es-UY,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-VE-x-icu", "es-VE,es-VE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.es-x-icu", "es,es,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.et-EE-x-icu", "et-EE,et-EE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.et-x-icu", "et,et,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.eu-ES-x-icu", "eu-ES,eu-ES,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.eu-x-icu", "eu,eu,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ewo-CM-x-icu", "ewo-CM,ewo-CM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ewo-x-icu", "ewo,ewo,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fa-AF-x-icu", "fa-AF,fa-AF,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fa-IR-x-icu", "fa-IR,fa-IR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fa-x-icu", "fa,fa,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ff-Latn-BF-x-icu", "ff-Latn-BF,ff-Latn-BF,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ff-Latn-CM-x-icu", "ff-Latn-CM,ff-Latn-CM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ff-Latn-GH-x-icu", "ff-Latn-GH,ff-Latn-GH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ff-Latn-GM-x-icu", "ff-Latn-GM,ff-Latn-GM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ff-Latn-GN-x-icu", "ff-Latn-GN,ff-Latn-GN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ff-Latn-GW-x-icu", "ff-Latn-GW,ff-Latn-GW,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ff-Latn-LR-x-icu", "ff-Latn-LR,ff-Latn-LR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ff-Latn-MR-x-icu", "ff-Latn-MR,ff-Latn-MR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ff-Latn-NE-x-icu", "ff-Latn-NE,ff-Latn-NE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ff-Latn-NG-x-icu", "ff-Latn-NG,ff-Latn-NG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ff-Latn-SL-x-icu", "ff-Latn-SL,ff-Latn-SL,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ff-Latn-SN-x-icu", "ff-Latn-SN,ff-Latn-SN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ff-Latn-x-icu", "ff-Latn,ff-Latn,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ff-x-icu", "ff,ff,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fi-FI-x-icu", "fi-FI,fi-FI,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fi-x-icu", "fi,fi,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fil-PH-x-icu", "fil-PH,fil-PH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fil-x-icu", "fil,fil,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fo-DK-x-icu", "fo-DK,fo-DK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fo-FO-x-icu", "fo-FO,fo-FO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fo-x-icu", "fo,fo,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-BE-x-icu", "fr-BE,fr-BE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-BF-x-icu", "fr-BF,fr-BF,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-BI-x-icu", "fr-BI,fr-BI,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-BJ-x-icu", "fr-BJ,fr-BJ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-BL-x-icu", "fr-BL,fr-BL,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-CA-x-icu", "fr-CA,fr-CA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-CD-x-icu", "fr-CD,fr-CD,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-CF-x-icu", "fr-CF,fr-CF,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-CG-x-icu", "fr-CG,fr-CG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-CH-x-icu", "fr-CH,fr-CH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-CI-x-icu", "fr-CI,fr-CI,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-CM-x-icu", "fr-CM,fr-CM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-DJ-x-icu", "fr-DJ,fr-DJ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-DZ-x-icu", "fr-DZ,fr-DZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-FR-x-icu", "fr-FR,fr-FR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-GA-x-icu", "fr-GA,fr-GA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-GF-x-icu", "fr-GF,fr-GF,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-GN-x-icu", "fr-GN,fr-GN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-GP-x-icu", "fr-GP,fr-GP,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-GQ-x-icu", "fr-GQ,fr-GQ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-HT-x-icu", "fr-HT,fr-HT,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-KM-x-icu", "fr-KM,fr-KM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-LU-x-icu", "fr-LU,fr-LU,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-MA-x-icu", "fr-MA,fr-MA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-MC-x-icu", "fr-MC,fr-MC,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-MF-x-icu", "fr-MF,fr-MF,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-MG-x-icu", "fr-MG,fr-MG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-ML-x-icu", "fr-ML,fr-ML,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-MQ-x-icu", "fr-MQ,fr-MQ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-MR-x-icu", "fr-MR,fr-MR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-MU-x-icu", "fr-MU,fr-MU,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-NC-x-icu", "fr-NC,fr-NC,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-NE-x-icu", "fr-NE,fr-NE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-PF-x-icu", "fr-PF,fr-PF,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-PM-x-icu", "fr-PM,fr-PM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-RE-x-icu", "fr-RE,fr-RE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-RW-x-icu", "fr-RW,fr-RW,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-SC-x-icu", "fr-SC,fr-SC,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-SN-x-icu", "fr-SN,fr-SN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-SY-x-icu", "fr-SY,fr-SY,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-TD-x-icu", "fr-TD,fr-TD,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-TG-x-icu", "fr-TG,fr-TG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-TN-x-icu", "fr-TN,fr-TN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-VU-x-icu", "fr-VU,fr-VU,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-WF-x-icu", "fr-WF,fr-WF,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-x-icu", "fr,fr,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fr-YT-x-icu", "fr-YT,fr-YT,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fur-IT-x-icu", "fur-IT,fur-IT,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fur-x-icu", "fur,fur,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fy-NL-x-icu", "fy-NL,fy-NL,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.fy-x-icu", "fy,fy,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ga-IE-x-icu", "ga-IE,ga-IE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ga-x-icu", "ga,ga,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.gd-GB-x-icu", "gd-GB,gd-GB,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.gd-x-icu", "gd,gd,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.gl-ES-x-icu", "gl-ES,gl-ES,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.gl-x-icu", "gl,gl,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.gsw-CH-x-icu", "gsw-CH,gsw-CH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.gsw-FR-x-icu", "gsw-FR,gsw-FR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.gsw-LI-x-icu", "gsw-LI,gsw-LI,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.gsw-x-icu", "gsw,gsw,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.gu-IN-x-icu", "gu-IN,gu-IN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.gu-x-icu", "gu,gu,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.guz-KE-x-icu", "guz-KE,guz-KE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.guz-x-icu", "guz,guz,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.gv-IM-x-icu", "gv-IM,gv-IM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.gv-x-icu", "gv,gv,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ha-GH-x-icu", "ha-GH,ha-GH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ha-NE-x-icu", "ha-NE,ha-NE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ha-NG-x-icu", "ha-NG,ha-NG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ha-x-icu", "ha,ha,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.haw-US-x-icu", "haw-US,haw-US,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.haw-x-icu", "haw,haw,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.he-IL-x-icu", "he-IL,he-IL,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.he-x-icu", "he,he,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.hi-IN-x-icu", "hi-IN,hi-IN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.hi-x-icu", "hi,hi,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.hr-BA-x-icu", "hr-BA,hr-BA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.hr-HR-x-icu", "hr-HR,hr-HR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.hr-x-icu", "hr,hr,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.hsb-DE-x-icu", "hsb-DE,hsb-DE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.hsb-x-icu", "hsb,hsb,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.hu-HU-x-icu", "hu-HU,hu-HU,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.hu-x-icu", "hu,hu,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.hy-AM-x-icu", "hy-AM,hy-AM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.hy-x-icu", "hy,hy,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ia-001-x-icu", "ia-001,ia-001,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ia-x-icu", "ia,ia,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.id-ID-x-icu", "id-ID,id-ID,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.id-x-icu", "id,id,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ig-NG-x-icu", "ig-NG,ig-NG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ig-x-icu", "ig,ig,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ii-CN-x-icu", "ii-CN,ii-CN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ii-x-icu", "ii,ii,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.is-IS-x-icu", "is-IS,is-IS,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.is-x-icu", "is,is,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.it-CH-x-icu", "it-CH,it-CH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.it-IT-x-icu", "it-IT,it-IT,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.it-SM-x-icu", "it-SM,it-SM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.it-VA-x-icu", "it-VA,it-VA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.it-x-icu", "it,it,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ja-JP-x-icu", "ja-JP,ja-JP,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ja-x-icu", "ja,ja,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.jgo-CM-x-icu", "jgo-CM,jgo-CM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.jgo-x-icu", "jgo,jgo,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.jmc-TZ-x-icu", "jmc-TZ,jmc-TZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.jmc-x-icu", "jmc,jmc,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.jv-ID-x-icu", "jv-ID,jv-ID,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.jv-x-icu", "jv,jv,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ka-GE-x-icu", "ka-GE,ka-GE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ka-x-icu", "ka,ka,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kab-DZ-x-icu", "kab-DZ,kab-DZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kab-x-icu", "kab,kab,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kam-KE-x-icu", "kam-KE,kam-KE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kam-x-icu", "kam,kam,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kde-TZ-x-icu", "kde-TZ,kde-TZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kde-x-icu", "kde,kde,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kea-CV-x-icu", "kea-CV,kea-CV,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kea-x-icu", "kea,kea,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.khq-ML-x-icu", "khq-ML,khq-ML,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.khq-x-icu", "khq,khq,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ki-KE-x-icu", "ki-KE,ki-KE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ki-x-icu", "ki,ki,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kk-KZ-x-icu", "kk-KZ,kk-KZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kk-x-icu", "kk,kk,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kkj-CM-x-icu", "kkj-CM,kkj-CM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kkj-x-icu", "kkj,kkj,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kl-GL-x-icu", "kl-GL,kl-GL,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kl-x-icu", "kl,kl,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kln-KE-x-icu", "kln-KE,kln-KE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kln-x-icu", "kln,kln,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.km-KH-x-icu", "km-KH,km-KH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.km-x-icu", "km,km,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kn-IN-x-icu", "kn-IN,kn-IN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kn-x-icu", "kn,kn,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ko-KP-x-icu", "ko-KP,ko-KP,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ko-KR-x-icu", "ko-KR,ko-KR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ko-x-icu", "ko,ko,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kok-IN-x-icu", "kok-IN,kok-IN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kok-x-icu", "kok,kok,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ks-IN-x-icu", "ks-IN,ks-IN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ks-x-icu", "ks,ks,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ksb-TZ-x-icu", "ksb-TZ,ksb-TZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ksb-x-icu", "ksb,ksb,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ksf-CM-x-icu", "ksf-CM,ksf-CM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ksf-x-icu", "ksf,ksf,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ksh-DE-x-icu", "ksh-DE,ksh-DE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ksh-x-icu", "ksh,ksh,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ku-TR-x-icu", "ku-TR,ku-TR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ku-x-icu", "ku,ku,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kw-GB-x-icu", "kw-GB,kw-GB,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.kw-x-icu", "kw,kw,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ky-KG-x-icu", "ky-KG,ky-KG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ky-x-icu", "ky,ky,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.lag-TZ-x-icu", "lag-TZ,lag-TZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.lag-x-icu", "lag,lag,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.lb-LU-x-icu", "lb-LU,lb-LU,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.lb-x-icu", "lb,lb,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.lg-UG-x-icu", "lg-UG,lg-UG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.lg-x-icu", "lg,lg,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.lkt-US-x-icu", "lkt-US,lkt-US,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.lkt-x-icu", "lkt,lkt,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ln-AO-x-icu", "ln-AO,ln-AO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ln-CD-x-icu", "ln-CD,ln-CD,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ln-CF-x-icu", "ln-CF,ln-CF,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ln-CG-x-icu", "ln-CG,ln-CG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ln-x-icu", "ln,ln,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.lo-LA-x-icu", "lo-LA,lo-LA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.lo-x-icu", "lo,lo,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.lrc-IQ-x-icu", "lrc-IQ,lrc-IQ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.lrc-IR-x-icu", "lrc-IR,lrc-IR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.lrc-x-icu", "lrc,lrc,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.lt-LT-x-icu", "lt-LT,lt-LT,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.lt-x-icu", "lt,lt,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.lu-CD-x-icu", "lu-CD,lu-CD,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.lu-x-icu", "lu,lu,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.luo-KE-x-icu", "luo-KE,luo-KE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.luo-x-icu", "luo,luo,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.luy-KE-x-icu", "luy-KE,luy-KE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.luy-x-icu", "luy,luy,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.lv-LV-x-icu", "lv-LV,lv-LV,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.lv-x-icu", "lv,lv,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mas-KE-x-icu", "mas-KE,mas-KE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mas-TZ-x-icu", "mas-TZ,mas-TZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mas-x-icu", "mas,mas,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mer-KE-x-icu", "mer-KE,mer-KE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mer-x-icu", "mer,mer,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mfe-MU-x-icu", "mfe-MU,mfe-MU,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mfe-x-icu", "mfe,mfe,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mg-MG-x-icu", "mg-MG,mg-MG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mg-x-icu", "mg,mg,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mgh-MZ-x-icu", "mgh-MZ,mgh-MZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mgh-x-icu", "mgh,mgh,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mgo-CM-x-icu", "mgo-CM,mgo-CM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mgo-x-icu", "mgo,mgo,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mi-NZ-x-icu", "mi-NZ,mi-NZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mi-x-icu", "mi,mi,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mk-MK-x-icu", "mk-MK,mk-MK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mk-x-icu", "mk,mk,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ml-IN-x-icu", "ml-IN,ml-IN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ml-x-icu", "ml,ml,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mn-MN-x-icu", "mn-MN,mn-MN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mn-x-icu", "mn,mn,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mr-IN-x-icu", "mr-IN,mr-IN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mr-x-icu", "mr,mr,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ms-BN-x-icu", "ms-BN,ms-BN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ms-MY-x-icu", "ms-MY,ms-MY,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ms-SG-x-icu", "ms-SG,ms-SG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ms-x-icu", "ms,ms,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mt-MT-x-icu", "mt-MT,mt-MT,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mt-x-icu", "mt,mt,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mua-CM-x-icu", "mua-CM,mua-CM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mua-x-icu", "mua,mua,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.my-MM-x-icu", "my-MM,my-MM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.my-x-icu", "my,my,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mzn-IR-x-icu", "mzn-IR,mzn-IR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.mzn-x-icu", "mzn,mzn,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.naq-NA-x-icu", "naq-NA,naq-NA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.naq-x-icu", "naq,naq,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nb-NO-x-icu", "nb-NO,nb-NO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nb-SJ-x-icu", "nb-SJ,nb-SJ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nb-x-icu", "nb,nb,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nd-x-icu", "nd,nd,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nd-ZW-x-icu", "nd-ZW,nd-ZW,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nds-DE-x-icu", "nds-DE,nds-DE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nds-NL-x-icu", "nds-NL,nds-NL,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nds-x-icu", "nds,nds,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ne-IN-x-icu", "ne-IN,ne-IN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ne-NP-x-icu", "ne-NP,ne-NP,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ne-x-icu", "ne,ne,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nl-AW-x-icu", "nl-AW,nl-AW,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nl-BE-x-icu", "nl-BE,nl-BE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nl-BQ-x-icu", "nl-BQ,nl-BQ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nl-CW-x-icu", "nl-CW,nl-CW,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nl-NL-x-icu", "nl-NL,nl-NL,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nl-SR-x-icu", "nl-SR,nl-SR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nl-SX-x-icu", "nl-SX,nl-SX,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nl-x-icu", "nl,nl,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nmg-CM-x-icu", "nmg-CM,nmg-CM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nmg-x-icu", "nmg,nmg,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nn-NO-x-icu", "nn-NO,nn-NO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nn-x-icu", "nn,nn,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nnh-CM-x-icu", "nnh-CM,nnh-CM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nnh-x-icu", "nnh,nnh,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nus-SS-x-icu", "nus-SS,nus-SS,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nus-x-icu", "nus,nus,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nyn-UG-x-icu", "nyn-UG,nyn-UG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.nyn-x-icu", "nyn,nyn,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.om-ET-x-icu", "om-ET,om-ET,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.om-KE-x-icu", "om-KE,om-KE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.om-x-icu", "om,om,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.or-IN-x-icu", "or-IN,or-IN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.or-x-icu", "or,or,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.os-GE-x-icu", "os-GE,os-GE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.os-RU-x-icu", "os-RU,os-RU,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.os-x-icu", "os,os,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pa-Arab-PK-x-icu", "pa-Arab-PK,pa-Arab-PK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pa-Arab-x-icu", "pa-Arab,pa-Arab,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pa-Guru-IN-x-icu", "pa-Guru-IN,pa-Guru-IN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pa-Guru-x-icu", "pa-Guru,pa-Guru,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pa-x-icu", "pa,pa,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pl-PL-x-icu", "pl-PL,pl-PL,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pl-x-icu", "pl,pl,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.POSIX", "POSIX,POSIX,libc,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ps-AF-x-icu", "ps-AF,ps-AF,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ps-PK-x-icu", "ps-PK,ps-PK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ps-x-icu", "ps,ps,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pt-AO-x-icu", "pt-AO,pt-AO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pt-BR-x-icu", "pt-BR,pt-BR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pt-CH-x-icu", "pt-CH,pt-CH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pt-CV-x-icu", "pt-CV,pt-CV,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pt-GQ-x-icu", "pt-GQ,pt-GQ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pt-GW-x-icu", "pt-GW,pt-GW,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pt-LU-x-icu", "pt-LU,pt-LU,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pt-MO-x-icu", "pt-MO,pt-MO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pt-MZ-x-icu", "pt-MZ,pt-MZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pt-PT-x-icu", "pt-PT,pt-PT,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pt-ST-x-icu", "pt-ST,pt-ST,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pt-TL-x-icu", "pt-TL,pt-TL,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.pt-x-icu", "pt,pt,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.qu-BO-x-icu", "qu-BO,qu-BO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.qu-EC-x-icu", "qu-EC,qu-EC,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.qu-PE-x-icu", "qu-PE,qu-PE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.qu-x-icu", "qu,qu,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.rm-CH-x-icu", "rm-CH,rm-CH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.rm-x-icu", "rm,rm,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.rn-BI-x-icu", "rn-BI,rn-BI,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.rn-x-icu", "rn,rn,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ro-MD-x-icu", "ro-MD,ro-MD,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ro-RO-x-icu", "ro-RO,ro-RO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ro-x-icu", "ro,ro,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.rof-TZ-x-icu", "rof-TZ,rof-TZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.rof-x-icu", "rof,rof,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ru-BY-x-icu", "ru-BY,ru-BY,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ru-KG-x-icu", "ru-KG,ru-KG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ru-KZ-x-icu", "ru-KZ,ru-KZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ru-MD-x-icu", "ru-MD,ru-MD,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ru-RU-x-icu", "ru-RU,ru-RU,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ru-UA-x-icu", "ru-UA,ru-UA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ru-x-icu", "ru,ru,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.rw-RW-x-icu", "rw-RW,rw-RW,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.rw-x-icu", "rw,rw,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.rwk-TZ-x-icu", "rwk-TZ,rwk-TZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.rwk-x-icu", "rwk,rwk,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sah-RU-x-icu", "sah-RU,sah-RU,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sah-x-icu", "sah,sah,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.saq-KE-x-icu", "saq-KE,saq-KE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.saq-x-icu", "saq,saq,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sbp-TZ-x-icu", "sbp-TZ,sbp-TZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sbp-x-icu", "sbp,sbp,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sd-PK-x-icu", "sd-PK,sd-PK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sd-x-icu", "sd,sd,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.se-FI-x-icu", "se-FI,se-FI,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.se-NO-x-icu", "se-NO,se-NO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.se-SE-x-icu", "se-SE,se-SE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.se-x-icu", "se,se,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.seh-MZ-x-icu", "seh-MZ,seh-MZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.seh-x-icu", "seh,seh,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ses-ML-x-icu", "ses-ML,ses-ML,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ses-x-icu", "ses,ses,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sg-CF-x-icu", "sg-CF,sg-CF,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sg-x-icu", "sg,sg,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.shi-Latn-MA-x-icu", "shi-Latn-MA,shi-Latn-MA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.shi-Latn-x-icu", "shi-Latn,shi-Latn,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.shi-Tfng-MA-x-icu", "shi-Tfng-MA,shi-Tfng-MA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.shi-Tfng-x-icu", "shi-Tfng,shi-Tfng,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.shi-x-icu", "shi,shi,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.si-LK-x-icu", "si-LK,si-LK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.si-x-icu", "si,si,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sk-SK-x-icu", "sk-SK,sk-SK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sk-x-icu", "sk,sk,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sl-SI-x-icu", "sl-SI,sl-SI,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sl-x-icu", "sl,sl,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.smn-FI-x-icu", "smn-FI,smn-FI,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.smn-x-icu", "smn,smn,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sn-x-icu", "sn,sn,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sn-ZW-x-icu", "sn-ZW,sn-ZW,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.so-DJ-x-icu", "so-DJ,so-DJ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.so-ET-x-icu", "so-ET,so-ET,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.so-KE-x-icu", "so-KE,so-KE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.so-SO-x-icu", "so-SO,so-SO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.so-x-icu", "so,so,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sq-AL-x-icu", "sq-AL,sq-AL,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sq-MK-x-icu", "sq-MK,sq-MK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sq-x-icu", "sq,sq,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sq-XK-x-icu", "sq-XK,sq-XK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sr-Cyrl-BA-x-icu", "sr-Cyrl-BA,sr-Cyrl-BA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sr-Cyrl-ME-x-icu", "sr-Cyrl-ME,sr-Cyrl-ME,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sr-Cyrl-RS-x-icu", "sr-Cyrl-RS,sr-Cyrl-RS,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sr-Cyrl-x-icu", "sr-Cyrl,sr-Cyrl,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sr-Cyrl-XK-x-icu", "sr-Cyrl-XK,sr-Cyrl-XK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sr-Latn-BA-x-icu", "sr-Latn-BA,sr-Latn-BA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sr-Latn-ME-x-icu", "sr-Latn-ME,sr-Latn-ME,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sr-Latn-RS-x-icu", "sr-Latn-RS,sr-Latn-RS,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sr-Latn-x-icu", "sr-Latn,sr-Latn,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sr-Latn-XK-x-icu", "sr-Latn-XK,sr-Latn-XK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sr-x-icu", "sr,sr,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sv-AX-x-icu", "sv-AX,sv-AX,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sv-FI-x-icu", "sv-FI,sv-FI,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sv-SE-x-icu", "sv-SE,sv-SE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sv-x-icu", "sv,sv,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sw-CD-x-icu", "sw-CD,sw-CD,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sw-KE-x-icu", "sw-KE,sw-KE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sw-TZ-x-icu", "sw-TZ,sw-TZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sw-UG-x-icu", "sw-UG,sw-UG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.sw-x-icu", "sw,sw,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ta-IN-x-icu", "ta-IN,ta-IN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ta-LK-x-icu", "ta-LK,ta-LK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ta-MY-x-icu", "ta-MY,ta-MY,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ta-SG-x-icu", "ta-SG,ta-SG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ta-x-icu", "ta,ta,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.te-IN-x-icu", "te-IN,te-IN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.te-x-icu", "te,te,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.teo-KE-x-icu", "teo-KE,teo-KE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.teo-UG-x-icu", "teo-UG,teo-UG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.teo-x-icu", "teo,teo,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.tg-TJ-x-icu", "tg-TJ,tg-TJ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.tg-x-icu", "tg,tg,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.th-TH-x-icu", "th-TH,th-TH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.th-x-icu", "th,th,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ti-ER-x-icu", "ti-ER,ti-ER,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ti-ET-x-icu", "ti-ET,ti-ET,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ti-x-icu", "ti,ti,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.tk-TM-x-icu", "tk-TM,tk-TM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.tk-x-icu", "tk,tk,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.to-TO-x-icu", "to-TO,to-TO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.to-x-icu", "to,to,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.tr-CY-x-icu", "tr-CY,tr-CY,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.tr-TR-x-icu", "tr-TR,tr-TR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.tr-x-icu", "tr,tr,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.tt-RU-x-icu", "tt-RU,tt-RU,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.tt-x-icu", "tt,tt,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.twq-NE-x-icu", "twq-NE,twq-NE,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.twq-x-icu", "twq,twq,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.tzm-MA-x-icu", "tzm-MA,tzm-MA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.tzm-x-icu", "tzm,tzm,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ucs_basic", "C,C,libc,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ug-CN-x-icu", "ug-CN,ug-CN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ug-x-icu", "ug,ug,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.uk-UA-x-icu", "uk-UA,uk-UA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.uk-x-icu", "uk,uk,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.und-x-icu", "und,und,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ur-IN-x-icu", "ur-IN,ur-IN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ur-PK-x-icu", "ur-PK,ur-PK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.ur-x-icu", "ur,ur,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.uz-Arab-AF-x-icu", "uz-Arab-AF,uz-Arab-AF,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.uz-Arab-x-icu", "uz-Arab,uz-Arab,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.uz-Cyrl-UZ-x-icu", "uz-Cyrl-UZ,uz-Cyrl-UZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.uz-Cyrl-x-icu", "uz-Cyrl,uz-Cyrl,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.uz-Latn-UZ-x-icu", "uz-Latn-UZ,uz-Latn-UZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.uz-Latn-x-icu", "uz-Latn,uz-Latn,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.uz-x-icu", "uz,uz,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.vai-Latn-LR-x-icu", "vai-Latn-LR,vai-Latn-LR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.vai-Latn-x-icu", "vai-Latn,vai-Latn,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.vai-Vaii-LR-x-icu", "vai-Vaii-LR,vai-Vaii-LR,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.vai-Vaii-x-icu", "vai-Vaii,vai-Vaii,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.vai-x-icu", "vai,vai,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.vi-VN-x-icu", "vi-VN,vi-VN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.vi-x-icu", "vi,vi,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.vun-TZ-x-icu", "vun-TZ,vun-TZ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.vun-x-icu", "vun,vun,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.wae-CH-x-icu", "wae-CH,wae-CH,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.wae-x-icu", "wae,wae,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.wo-SN-x-icu", "wo-SN,wo-SN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.wo-x-icu", "wo,wo,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.xh-x-icu", "xh,xh,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.xh-ZA-x-icu", "xh-ZA,xh-ZA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.xog-UG-x-icu", "xog-UG,xog-UG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.xog-x-icu", "xog,xog,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.yav-CM-x-icu", "yav-CM,yav-CM,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.yav-x-icu", "yav,yav,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.yi-001-x-icu", "yi-001,yi-001,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.yi-x-icu", "yi,yi,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.yo-BJ-x-icu", "yo-BJ,yo-BJ,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.yo-NG-x-icu", "yo-NG,yo-NG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.yo-x-icu", "yo,yo,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.yue-Hans-CN-x-icu", "yue-Hans-CN,yue-Hans-CN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.yue-Hans-x-icu", "yue-Hans,yue-Hans,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.yue-Hant-HK-x-icu", "yue-Hant-HK,yue-Hant-HK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.yue-Hant-x-icu", "yue-Hant,yue-Hant,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.yue-x-icu", "yue,yue,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.zgh-MA-x-icu", "zgh-MA,zgh-MA,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.zgh-x-icu", "zgh,zgh,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.zh-Hans-CN-x-icu", "zh-Hans-CN,zh-Hans-CN,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.zh-Hans-HK-x-icu", "zh-Hans-HK,zh-Hans-HK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.zh-Hans-MO-x-icu", "zh-Hans-MO,zh-Hans-MO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.zh-Hans-SG-x-icu", "zh-Hans-SG,zh-Hans-SG,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.zh-Hans-x-icu", "zh-Hans,zh-Hans,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.zh-Hant-HK-x-icu", "zh-Hant-HK,zh-Hant-HK,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.zh-Hant-MO-x-icu", "zh-Hant-MO,zh-Hant-MO,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.zh-Hant-TW-x-icu", "zh-Hant-TW,zh-Hant-TW,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.zh-Hant-x-icu", "zh-Hant,zh-Hant,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.zh-x-icu", "zh,zh,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.zu-x-icu", "zu,zu,icu,True")
                .HasAnnotation("Npgsql:CollationDefinition:pg_catalog.zu-ZA-x-icu", "zu-ZA,zu-ZA,icu,True");

            modelBuilder.Entity<AccessToken>(entity =>
            {
                entity.ToTable("access_token");

                entity.HasIndex(x => x.AccessTokenId)
                    .HasName("access_token_access_token_id_key")
                    .IsUnique();

                entity.HasIndex(x => x.TokenHash)
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
                    .HasMaxLength(100);

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
                entity.HasKey(x => new { x.UserName, x.ExtraMailAddress })
                    .HasName("idx_account_extra_mail_address_pk");

                entity.ToTable("account_extra_mail_address");

                entity.HasIndex(x => x.ExtraMailAddress)
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
                entity.HasKey(x => new { x.Issuer, x.Subject })
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
                    .HasMaxLength(30);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url")
                    .HasMaxLength(200);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.ToTable("activity");

                entity.HasIndex(x => x.ActivityId)
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
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Mode)
                    .IsRequired()
                    .HasColumnName("mode")
                    .HasMaxLength(10)
                    .HasDefaultValueSql("'PUBLIC'::character varying");

                entity.Property(e => e.OriginRepositoryName)
                    .HasColumnName("origin_repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.OriginUserName)
                    .HasColumnName("origin_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RegisteredDate).HasColumnName("registered_date");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

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

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.CommentedUserName)
                    .IsRequired()
                    .HasColumnName("commented_user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content");

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
                    .HasMaxLength(100);

                entity.Property(e => e.GpgKeyId).HasColumnName("gpg_key_id");

                entity.Property(e => e.KeyId)
                    .HasColumnName("key_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.PublicKey)
                    .IsRequired()
                    .HasColumnName("public_key");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
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
                    .HasMaxLength(100);

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Manager).HasColumnName("manager");

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
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

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
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

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
                    .HasColumnName("user_name")
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.LabelId).HasColumnName("label_id");

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

            modelBuilder.Entity<IssueOutlineView>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("issue_outline_view");

                entity.Property(e => e.CommentCount).HasColumnName("comment_count");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.Priority).HasColumnName("priority");

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
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
                    .HasMaxLength(6)
                    .IsFixedLength();

                entity.Property(e => e.LabelName)
                    .IsRequired()
                    .HasColumnName("label_name")
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
                    .HasMaxLength(100);

                entity.Property(e => e.DisableEmail).HasColumnName("disable_email");

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
                    .HasMaxLength(100);

                entity.Property(e => e.RepositoryName)
                    .HasColumnName("repository_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasColumnName("source")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Plugin>(entity =>
            {
                entity.ToTable("plugin");

                entity.Property(e => e.PluginId)
                    .HasColumnName("plugin_id")
                    .HasMaxLength(100);

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasColumnName("version")
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
                    .HasMaxLength(6)
                    .IsFixedLength();

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

                entity.Property(e => e.IsDraft).HasColumnName("is_draft");

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
                    .HasMaxLength(100);

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasColumnName("version")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Watch>(entity =>
            {
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.NotificationUserName })
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
                entity.HasKey(x => new { x.UserName, x.RepositoryName, x.Url })
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
                    .HasForeignKey(x => new { x.UserName, x.RepositoryName, x.Url })
                    .HasConstraintName("idx_web_hook_event_fk0");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

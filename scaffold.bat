dotnet ef dbcontext scaffold "Host=localhost;Port=3306;Username=gitbucket;Password=gitbucket;Database=gitbucket" Npgsql.EntityFrameworkCore.PostgreSQL -c GitBucketDbContext -f --context-dir .\ -o Models\ -p src\GitBucket.Core\GitBucket.Core.csproj -s src\GbUtil\GbUtil.csproj --no-onconfiguring
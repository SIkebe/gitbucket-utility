dotnet ef dbcontext scaffold "Host=localhost;Username=user;Password=password;Database=gitbucket" Npgsql.EntityFrameworkCore.PostgreSQL -c GitBucketDbContext -f --context-dir .\ -o Models\ -p src\GitBucket.Core\GitBucket.Core.csproj -s src\GbUtil\GbUtil.csproj
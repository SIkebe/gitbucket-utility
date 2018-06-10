# gitbucket-utility
Utilities for GitBucket

## Requirements
* [.NET Core 2.1 SDK](https://www.microsoft.com/net/download/windows)
* GitBucket 4.25.0 (using PostgreSQL as backend DB)

## Preparation
```cmd
dotnet tool install --global gbutil --version 0.1.1
setx ConnectionStrings:GitBucketConnection Host=host;Username=username;Password=password;Database=gitbucket
```

## Usage
### Output release notes
Output release notes as markdown which list issues of the repository related to the milistone.  
`gbutil release -o {owner} -r {repository} -m {miliestone}`

```powershell
gbutil release -o ikebe -r RepeatableTimer -m v0.1.0

### Bug
* Crash when paused  #5
* CurrentPeriod reset not working when timer stopped #6

### Enhancement
* Allow one time/repeat options #1
* Add validation #3
```
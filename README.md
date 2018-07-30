# gitbucket-utility
Utilities for GitBucket  

[![Build status](https://ci.appveyor.com/api/projects/status/q1hfisqpa09662l5/?svg=true)](https://ci.appveyor.com/project/SIkebe/gitbucket-utility/)


## Requirements
* [.NET Core 2.1 SDK](https://www.microsoft.com/net/download/windows)
* GitBucket 4.25.0 (using PostgreSQL as backend DB)

## Preparation
```cmd
dotnet tool install --global gbutil --version 0.2.0
setx ConnectionStrings:GitBucketConnection Host=host;Username=username;Password=password;Database=gitbucket
```

## Usage
### `gbutil release`
```powershell
gbutil [-o|--owner] [-r|--repository] [-m|--miliestone] [-t|--target]
```
Output a release note in markdown which lists issues or pull requests of the repository related to the milistone.

```powershell
> gbutil release -o ikebe -r RepeatableTimer -m v0.1.0
As part of this release we had 4 issues closed.
The highest priority among them is "very high".

### Bug
* Crash when paused  #5
* CurrentPeriod reset not working when timer stopped #6

### Enhancement
* Allow one time/repeat options #1
* Add validation #3
```

### Options
|Short name|Long name|Required|Abstract|
|:-|:-|:-:|:-|
|`-o`|`--owner`|`true`|The owner name of the repository.|
|`-r`|`--repository`|`true`|The repository name.|
|`-m`|`--milestone`|`true`|The milestone to publish a release note.|
|`-t`|`--target`|`false`|The switch whether publish a release note based on issues or pull requests.<br>Predefined values are "issues" or "pullrequests".<br> Default value is "issues".|

-----

### `gbutil milestone`
```powershell
gbutil [-o|--owner] [-r|--repository] [-c|--includeClosed]
```
Show unclosed (by default) milestones.

```
> gbutil milestone -o ikebe:root
There are 3 open milestones.

* ikebe/RepeatableTimer, v0.2.0, 2018/08/01, Implement xxx feature
* ikebe/RepeatableTimer, v0.3.0, 
* root/test, v1.0.0, 2018/07/01, Bugfix for #123
```

### Options
|Short name|Long name|Required|Abstract|
|:-|:-|:-:|:-|
|`-o`|`--owner`|`true`|The owner names of the repositories to show milestones. Use ":" for separator.|
|`-r`|`--repository`|`false`|The repository names to show milestones. Use ":" for separator.|
|`-c`|`--includeClosed`|`false`|Whether show closed milestones.|

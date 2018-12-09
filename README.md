# gitbucket-utility
Utilities for GitBucket  

[![Build status](https://ci.appveyor.com/api/projects/status/q1hfisqpa09662l5/?svg=true)](https://ci.appveyor.com/project/SIkebe/gitbucket-utility/)


## Requirements
* [.NET Core 2.2 SDK](https://www.microsoft.com/net/download/windows)
* GitBucket 4.27.0+ (using PostgreSQL as backend DB)

## Preparation
```cmd
dotnet tool install --global gbutil --version 0.4.0
setx ConnectionStrings:GitBucketConnection Host=host;Username=username;Password=password;Database=gitbucket
setx GitBucketUri http://localhost:8080/gitbucket/api/v3/
```

## Usage
### `gbutil issue -t move`

```powershell
gbutil issue -t move [-s|--source] [-d|--destination] [-n|--number]
```

Move an issue between repositories.

```
> gbutil issue -t move -s root/test1 -d root/test2 -n 1
Enter your Username: root
Enter your Password: ****
The issue has been successfully moved to http://localhost:8080/gitbucket/root/test2/issues/35.
Close the original one manually.
```

### Options
|Short name|Long name|Required|Abstract|
|:-|:-|:-:|:-|
|`-t`|`--type`|`false`|The type of issue options. Default value is "move".|
|`-s`|`--source`|`true`|The source owner and repository to move from. Use "/" for separator like "root/repository1".|
|`-d`|`--destination`|`true`|The destination owner and repository to move to. Use "/" for separator like "root/repository2".|
|`-n`|`--number`|`false`|The issue numbers to move. Use ":" for separator.|

-----


### `gbutil issue -t copy`

```powershell
gbutil issue -t copy [-s|--source] [-d|--destination] [-n|--number]
```

Copy an issue between repositories.

```
> gbutil issue -t copy -s root/test1 -d root/test2 -n 1
Enter your Username: root
Enter your Password: ****
The issue has been successfully copied to http://localhost:8080/gitbucket/root/test2/issues/35.
```

### Options
|Short name|Long name|Required|Abstract|
|:-|:-|:-:|:-|
|`-t`|`--type`|`true`|The type of issue options. Default value is "move".|
|`-s`|`--source`|`true`|The source owner and repository to copy from. Use "/" for separator like "root/repository1".|
|`-d`|`--destination`|`true`|The destination owner and repository to copy to. Use "/" for separator like "root/repository2".|
|`-n`|`--number`|`false`|The issue numbers to copy. Use ":" for separator.|

-----

### `gbutil milestone`
```powershell
gbutil milestone [-o|--owner] [-r|--repository] [-c|--includeClosed]
```
Show unclosed (by default) milestones.

```
> gbutil milestone -o ikebe:root
There are 3 open milestones.

* root/test, v1.0.0, 2018/07/01, Bugfix for #123
* ikebe/RepeatableTimer, v0.2.0, 2018/08/01, Implement xxx feature
* ikebe/RepeatableTimer, v0.3.0, 
```

### Options
|Short name|Long name|Required|Abstract|
|:-|:-|:-:|:-|
|`-o`|`--owner`|`true`|The owner names of the repositories to show milestones. Use ":" for separator.|
|`-r`|`--repository`|`false`|The repository names to show milestones. Use ":" for separator.|
|`-c`|`--includeClosed`|`false`|Whether show closed milestones.|

-----

### `gbutil release`
```powershell
gbutil release [-o|--owner] [-r|--repository] [-m|--miliestone] [-t|--target]
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

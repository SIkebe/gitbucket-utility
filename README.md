# gitbucket-utility
Utilities for GitBucket  
[![Build status](https://github.com/SIkebe/gitbucket-utility/workflows/CI/badge.svg)](https://github.com/SIkebe/gitbucket-utility/actions?query=workflow%3A%22CI%22)
[![NuGet version](https://badge.fury.io/nu/gbutil.svg)](https://badge.fury.io/nu/gbutil)

## Requirements
* [.NET 6.X SDK](https://www.microsoft.com/net/download/windows) ※Required only if you use dotnet tool version.
* GitBucket 4.36.X+ (using PostgreSQL as backend DB)

## Preparation
```cmd
# [Required]

# Install as a dotnet tool or download exe from GitHub Release page.
dotnet tool install --global gbutil --version 0.11.0

setx GbUtil_ConnectionStrings Host=host;Username=username;Password=password;Database=gitbucket;port=XXXX
setx GbUtil_GitBucketUri http://localhost:8080/gitbucket/api/v3/

# [Optional]
# You can use your personal access token for authentication/authorization...
setx GbUtil_AccessToken <personal access token>

# ...or username and password credentials.
setx GbUtil_UserName <user name>
setx GbUtil_Password <password>
```

## Usage
### `gbutil issue -t move`

```powershell
gbutil issue -s|--source -d|--destination -n|--number [-t move]
```

Move issues between repositories.

```
> gbutil issue -s root/test1 -d root/test2 -n 1 -t move
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
|`-n`|`--number`|`true`|The issue numbers to move. Use ":" for separator.|

-----

### `gbutil issue -t copy`

```powershell
gbutil issue -t copy -s|--source -d|--destination -n|--number
```

Copy issues between repositories.

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
|`-n`|`--number`|`true`|The issue numbers to copy. Use ":" for separator.|

-----

### `gbutil milestone`
```powershell
gbutil milestone -o|--owner -r|--repository [-c|--includeClosed]
```
Show unclosed (by default) milestones.

```
> gbutil milestone -o ikebe:root
There are 3 open milestones.

* [root/test], [v1.0.0], [2018/07/01], [Bugfix for #123], [ikebe]
* [ikebe/RepeatableTimer], [v0.2.0], [2018/08/01], [Implement xxx feature], [ikebe]
* [ikebe/RepeatableTimer], [v0.3.0], [2019/12/31], [vNext], [user1, user2]
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
gbutil release -o|--owner -r|--repository -m|--miliestone [--from-pr] [--create-pr]
```
Output a release note in markdown which lists issues of the repository related to the milistone.  
If `--create-pr` option is specified, gbutil automatically creates Pull Request instead of console output.

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

> gbutil release -o ikebe -r RepeatableTimer -m v0.1.0 --create-pr
A new pull request has been successfully created!
```

### Options
|Short name|Long name|Required|Abstract|
|:-|:-|:-:|:-|
|`-o`|`--owner`|`true`|The owner name of the repository.|
|`-r`|`--repository`|`true`|The repository name.|
|`-m`|`--milestone`|`true`|The milestone to publish a release note.|
|-|`--from-pr`|`false`|If specified, gbutil publish a release note based on pull requests.|
|-|`--create-pr`|`false`|If specified, gbutil automatically creates a pull request.|
|`-b`|`--base`|`false`|The name of the branch you want the changes pulled into. Default value is "master".|
|`-h`|`--head`|`false`|The name of the branch where your changes are implemented. Default value is "develop".|
|-|`--title`|`false`|The title of the new pull request. Default value is the same as milestone.|
|`-f`|`--force`|`false`|if specified, force the output/creating PR.|
|-|`--draft`|`false`|If specified, create draft pull request.|

-----

### `gbutil backup`

```powershell
gbutil backup --home -d|--dest [--pgdump]
```
Backup GitBucket repositories and metadata.

```powershell
> gbutil backup --home "C:\gitbucket" --dest "C:\gitbucket-bk" --pgdump "C:\Program Files\PostgreSQL\12\bin\pg_dump.exe"
```

### Options
|Short name|Long name|Required|Abstract|
|:-|:-|:-:|:-|
|-|`--home`|`true`|GITBUCKET_HOME|
|`-d`|`--dest`|`true`|The backup destination folder path.|
|-|`--pgdump`|`false`|The path to the pg_dump executable. If pd_dump is not on your PATH, you need to specify this.|

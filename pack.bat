dotnet pack -c release .\src\gbutil\gbutil.csproj
mkdir packages
copy .\src\gbutil\bin\release\*.nupkg packages
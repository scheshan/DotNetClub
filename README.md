# DotNetClub
A tiny club written in Asp.Net Core

How to Build and Run:

1. Clone the repository
2. Edit database connection string in appsettings.json
3. Go to root directory and run following commands

```
dotnet restore
dotnet ef database update
cd src\DotNetClub.Web
bower install
dotnet run
```
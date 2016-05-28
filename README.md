# DotNetClub
A Tiny Club Written in Asp.Net Core

How to Build and Run:

1. Clone the repository
2. Go to DotNetClub.Web directory
3. Restore the dependencies
```
dotnet restore
```
4. Edit database connection string by SecretManagerTools
```
dotnet user-secrets set ConnectionString YourConnectionString
``` 
5. Install Database
```
dotnet ef database update
```
6. Install client libraries
```
bower install
```
7. Run the project
```
dotnet run
```
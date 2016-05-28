# DotNetClub
A Tiny Club Written in Asp.Net Core

How to Build and Run:

*   Clone the repository
*   Restore the dependencies
    ```
    dotnet restore
    ```
*   Go to DotNetClub.Web directory
    ```
    cd src\DotNetClub.Web
    ```
*   Edit database connection string by SecretManagerTools
    ```
    dotnet user-secrets set ConnectionString YourConnectionString
    ```
*   Install Database
    ```
    dotnet ef database update
    ```
*   Install client libraries
    ```
    bower install
    ```
*   Run the project
    ```
    dotnet run
    ```
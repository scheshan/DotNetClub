# DotNetClub
A Tiny Club Written in Asp.Net Core

Upgrade:

1. Add redis support
2. Use new UnitOfWork pattern to access database

Begin You Run:

*   Install client libraries

		Go to DotNetClub.Web/wwwroot directory, and run npm install

*   Set UserSecrets in VisualStudio

		```
		{
			"ConnectionString": "Your Connection String",
			"Redis": {
			"Host": "Your Redis Host",
			"Port": 6379,
			"Password": "",
			"Db": 1
			}
		}
		```
*		Create database

		The script to create the tables is under the database folder

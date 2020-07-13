# Introduction

This is the API for a TODO list project that I am currently building. When completed, the project will consist of:

 * A web UI built in Angular
 * A mobile app built with React Native; and
 * A back-end written in .NET Core
 
For the Angular UI, please see:

https://bitbucket.org/JakeM123/todo-list-angular-ui

The aim of this project is to demonstrate my capabilities in the above-listed stack, as well as in the following areas:

 * SSO and user/session management with JWT
 * Secure password management
 * Pushing notifications and updates from server-to-client via websockets
 * Responsive user interface design
 * CI/CD setup
 * Serverless architecture (Azure will be integrated at a later stage)
 * Scalability (the app will be designed to handle a high volume of users)
 * Unit testing
 * Functional and reactive programming
 * Object-oriented programming
 * SOLID principals

# Debugging

Install the .NET Core CLI or Visual Studio.

Use `dotnet run` to launch the server from the `todo-list-backend` project directory, or debug the solution with Visual Studio.

## User secrets

Set the following user secrets in your environment:

| Key       				  | Value									 |
|-----------------------------|------------------------------------------|
| TodoListApp:JWTSecret       | The secret to use for JWT authentication |
| TodoListApp:google:secret   | Google client secret                     |
| TodoListApp:google:clientId | Google client ID            			 |
|                             |                                          |

Secrets can be set from the `todo-list-backend` project folder with `dotnet user-secrets set [key] [value]`. See more at https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=windows

## Google app (optional)

A Google app is required for the SSO to work. To use:

* Create a Google developer account at https://console.cloud.google.com/
* Create a new project
* Create an OAuth consent screen under `APIs & Services`
* Create new credentials under `APIs & Services` > `Credentials`
	* Set the application type to `Web application`
	* Add the provided client ID and secret to your user-secrets store
* On the credentials settings page, under `Authorized JavaScript origins`, add the following addresses:
	* http://127.0.0.1:4200
	* http://localhost:4200
* Under `Authorized redirect URIs`, add:
	* http://127.0.0.1:4200/assert
	* http://localhost:4200/assert
	
## Data storage

The app currently uses a SQLite database for storage. On launch, a `todo-list-backend/bin/app.db` file will be created if it does not exist.

Delete `app.db` to reset the app storage.
	
# Testing

Unit tests are located in the `todo-list-backend-tests` project. Use `dotnet test` to run.
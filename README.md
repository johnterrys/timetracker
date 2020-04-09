# Student Time Tracker
## General Information
* This project is configured using .NET Core 3.1
* We use git as our version control system
* This project requires that your computer **MUST** support virtualization
* The ORM we are using is Entity Framework Core (the standard for .NET)
  * The ORM is configured to use Code First Migrations for the database PostgreSQL
    * If you need to switch which database you are using refer to the Microsoft docs
      on setting up entity framework core. You should just have to change the provider (avoid this if possible as it deletes
      migration history).
  * Refer to the Microsoft Documentation on Migrating to update your database locally as needed
    * New migration: `dotnet ef migrations add "MigrationName"`
      * Generally it is a good practice to keep migrations small as it makes it easy to name them and deal with merge conflicts
    * Update command: `dotnet ef database update [MigrationName]`
      * If you need to go down a migration you will need to specify the version you would like to go to
      * Before developing on any branch make sure your migrations are up-to-date with your new dev branch that
        typically means updating the migrations to be at the same level as master before switching branches, then 
        updating the migrations after you have switched branches.
      * As for dealing with merge conflicts in migrations, it is best to avoid them but if you must your just going to have to do a lot of research on EF Core
* The front end is currently written using AngularJS (deprecated in 2021)
    * There is a work in progress branch `refactor/update-angularjs-to-angular8` (there are also several other 
      branches that may be branches for individual developers based on the mentioned branch) which is working on updating the
      entire project using angular 9 (9.x was released before the branch got merged) this branch contains the angular
      project for the front end. We have striven to follow general angular best practices outlined in the [angular style guide](https://angular.io/guide/styleguide)
* The project backend database is configured using docker and the `.env` file for providing environments variables
  * Docker-compose is configured using the `docker-compose.yml` file. The following commands must be ran in the same directory as the `docker-compose.yml` 
    file (typically the root of the project):
    * To build the backend run `docker-compose build`
    * To run the backend run `docker-compose up`
    * To build and run the backend run `docker-compose up --build`
    * To disable the container run `docker-compose down`
    * To destroy the backend and clear data run `docker-compose down -v`
  * For more advanced docker configuration see the docker/docker-compose docs
  
## Vocabulary
* Project = Generally refers to the project as a whole unless referring to a specific directory with regards to .NET
* Solution Directory / Repository Directory / Root Directory = The top level folder where your project initially begins, typically contains an `.sln` file and any `docker-compose.yml` files.
* Project Directory = The directory for a specific project, ie. A C# project or `.csproj`, this is a .NET specific thing; other languages have their equivalents though.
* ORM = Object Relational Mapper
* EF Core = Entity Framework Core, this is also our ORM
* Docker = Container Management Tool
* Postgres = Database

## Project Setup
The following steps assume you:
* Have already cloned the repository
* Are using Postgres as your Database
* Are using docker as your database environment
* Have a general understanding of servers and software development
* Have an understanding of how to use your personal dev machine (windows, mac, linux)
* Your repository is on the master branch

### Windows
#### System Requirements
On windows you will need to make sure your system supports virtualization. This can be a setting in the bios and or a setting in the 
`Turn Windows Features On or Off` setting of the control panel and enabling **ALL** Hyper-V settings. Another thing to consider is what
Edition of windows you are running, if you are not running **Windows 10 Pro** you may run into issues with docker and virtualization. 

You must also install the microsoft development tools on your system, if you have visual studio installed you should have most of these.
If you do not the best place to find the cli tools is by looking in the microsoft tools documentation. 
The following commands will install the required development tools for: 
* Entity Framework Core: `dotnet tool install dotnet-ef --global`

In the case that you do **not** have **Windows 10 Pro** you have several options:
1. Purchase Windows 10 Pro (I personally would recommend this if you are planning on actively developing in the future using a personal 
   machine, you can transfer licenses between machines but it can be a hassle sometimes)
2. Setup Virtual Box on your system (instead of Hyper-V) and run a linux or database instance in the virtual machine
3. Setup a remote server (sql instance, **NOT** a vm) on a service such as one of the following (this would cost some money more than likely (generally cheap, sub $10/month))
  * Amazon Web Services (AWS)
  * Microsoft Azure (Azure)
  * Other ie. [Kimsufi](https://www.kimsufi.com/us/en/) self hosted
  
Of the above options number 1 is the easiest and fastest to get setup, unless you are experienced with linux servers or servers in general this is the route I would recommend.

#### Setup
Navigate to your repository root directory and locate the `.env.default` file. 
Duplicate this file in the repository root directory with the name `.env`.
The `.env` file should **NEVER** be checked into git as it could contain application secrets (passwords for production, api keys, etc...). 
The `.env.default` file should be checked into git and should **NEVER** contain application secrets.
Edit the `.env` file and fill out the variables to meet what information you want your postgres database to use.

Open your repository root directory in powershell and run the docker command: `docker-compose up`.
This command will pull all repository data from `Dockerhub` and build the related containers, then launch the containers.
After the containers are built you should have a postgres database that has the localhost port exposed that is in the `docker-compose.yml` file under the section:
```yaml
services:
  ports:
    - internalPort:exposedPort
```
We must now configure your `appsettings.Development.json` file in your project directory.
Under the connection strings object make sure to add your database name variable, used by environment variables in c#/.net.
Configure your connection string, example:
```json
{
  "ConnectionString": {
    "TimeTrackerDB": "User ID=postgres;Password=example;Host=localhost;Port=5432;Database=StudentTimeTrackerDB;"
  }
}
```
Make sure your `User ID`, `Password`, `Host`, etc. all corispond with the information entered in your `.env` file as well as where your container is located.
If you are using docker like this setup guide assumes then your host should be `localhost`.

You will now need to run migrations on your project to bring your database up to the current schema.
We do this using Entity Framework Core Code First Migrations.
In the project directory enter the following command: `dotnet ef database update` this will bring the database located at the above connection string up-to-date with the migrations you have.

You should now be able to launch the application.

## Angular Documentation
The following is information about Angular (not angularJS).

Main documentation websites:
* [How to setup your system](https://angular.io/guide/setup-local)
* [Angular Official Tutorial](https://angular.io/tutorial/toh-pt0)
* [Angular Google Material Design](https://material.angular.io/)
* [Angular Style Guide and Best Practices](https://angular.io/guide/styleguide)

## Entity Framework Documentation
Best way to learn Entity Framework is just to start reading the docs on microsoft's web site and if you have access to Pluralsite check out courses by `Julie Lerman` (Leading Expert) or `Mosh Hamedani`
* [EF Tutorial Site](https://www.entityframeworktutorial.net/efcore/entity-framework-core.aspx)
* [General Microsoft Documentation](https://docs.microsoft.com/en-us/ef/core/)
* [EF Migrations Documentation](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli)
  
## Migration Documentation
### Dealing with dates
If you need to upgrade a string type to a `DateTime` in the dotnet entities refer to the following migrations and documentation for examples.

* Migration: `TimeCats.web/Migrations/20200407192310_UpdateTimeCardToUseDateTime.cs`
* [to_timestamp documentation](https://www.postgresqltutorial.com/postgresql-to_timestamp/)
* [to_char documentation](https://www.postgresqltutorial.com/postgresql-to_char/)
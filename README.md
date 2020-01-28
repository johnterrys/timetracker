# Student Time Tracker
## General Information
LAST UPDATED: `1/27/2020`

* This project is configured using .NET Core 3.1
* The ORM we are using is Entity Framework Core (the standard for .NET)
  * The ORM is configured to use Code First Migrations for the database PostgreSQL
  * If you need to switch which database you are using refer to the Microsoft docs
  on setting up entity framework core. You should just have to change the provider (avoid this if possible as it deletes migration history).
  * Refer to MS Docs on Migrating to update your database localy as needed
    * New migration: `dotnet ef migrations add "MigrationName"`
    * Update command: `dotnet ef database update [MigrationName]`
* The front end is currently written using AngularJS (deprecated in 2021)
* The project backend database is configured using docker and the `.env` file for providing environments variables
  * Docker-compose is configured using the `docker-compose.yml` file
    * To build the backend run `docker-compose up`
    * To disable the container run `docker-compose down`
    * To destroy the backend and clear data run `docker-compose down -v`
  * For more advanced docker configuration see the docker/docker-compose docs
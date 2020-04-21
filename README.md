# Student Time Tracker
If you need more information about how to setup the project or context on specific tools see our wiki on github.
 We recommend that you become familiar with the wiki and the information contained in it before starting the project.
 Especially, if you are new to C# or any of its best practices. We have done everything we can to provide information
 on how the project is laid out, how the tools used work, and why we chose them. 
 
If anything in this readme does not make sense or appear to be out of date please refer to the wiki for the most accurate info.

[Student Time Tracker Wiki](https://github.com/bradleypeterson/timetracker/wiki)

## Contact Information
I know this project has been setup in a somewhat complicated manner so if there are any problems with getting setup or if you have any questions feel free to reach out to me `Kayden Miller` at any of the following contact options. I will do my best to get back to you within 24 hours. However, please keep in mind I do work as a Software Engineer and Team Lead with my company `Guru Technologies` as well as run my own company so it make take may some time especially if I have client meetings. 

Contact Info:
    Name: `Kayden Miller`  
    Email: `kaydenmiller1@gmail.com` (personal email)  
    Discord: `death2142zerg#3250` (personal discord)
    Phone: I am not going to put my phone number on here but Brad should be able to get it if needed.
    GitHub: You can also contact me via GitHub.

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

## Project Setup
### Assumptions
The following steps assume you:
* YOU HAVE READ THE WIKI!
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
3. Setup a remote server (sql instance, **NOT** a vm) on a service such as one of the following (this would cost some money, more than likely (generally cheap, sub $10/month))
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
    - hostPort:containerPort
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

Three default accounts should be made with the `dotnet ef database update` command.
* Admin User
  * Username: admin
  * Password: Password!
* Instructor User
  * Username: instructor
  * Password: Password!
* Standard User
  * Username: user
  * Password: Password!

You should now be able to launch the application.

## Production Information
If you go to use the `docker-compose.production.yml` file on linux you will need to make sure you are at least running docker-compose version `1.25.5`.
See [How to install docker from the github repository for the latest version](https://www.digitalocean.com/community/tutorials/how-to-install-docker-compose-on-ubuntu-18-04). 
You may be able to install using `apt install docker-compose` however, the linux repos are sometimes several minor versions out of date.

**!!IMPORTANT!!**  
Do not actually use the database in a docker container that is bad for a production environment as you could instantly loose all of your data. The only reason it is
 currently setup as a docker container is to ease development on a remote system and test deployment procedures. For an actual production environment the database should
 be hosted via Azure, AWS, Google, or some other database hosting service (unless absolutely needed). 

This includes for COPA compliant databases, it may be more expensive to run on any of these services but it is still a better choice than hosting them yourself. The reason
 this is better is due to how much security is involved in a database like that a it should be maintained by a company that has the proper systems and services setup to work
 with a dataset that requires compliance. Bottom line is don't host the production databases yourself, have someone else do it.

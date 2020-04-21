# This docker file is uesd to build the production image of the "TimeCats.web" project
# It contains a base image from microsofts dockerhub repository with all of the needed dotnet
# runtime and build components. It will then be built locally and stored in your local docker
# repository (read up on how docker works for mor information). This could be off loaded to 
# a remote repository such as Nexus Package Manager or DockerHub. However, until the project
# is closer to an actual release I would recommend not worrying so much about that side of things.

# Use base dotnet core sdk for base image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS Base
COPY . temp/
WORKDIR temp/
RUN ["dotnet", "publish", "-c", "Staging"]

# Dotnet Runtime: this is the portion of the dockerfile that actually builds the running container image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS Runtime
COPY --from=Base temp/bin/Staging/netcoreapp3.1/publish/ app/
#This is very important to have as it changes the working directory of the .net application
WORKDIR /app 
ENTRYPOINT ["dotnet", "TimeCats.dll"]
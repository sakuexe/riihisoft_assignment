# Riihisoft Application Technical Assignment

By Saku Karttunen - 2.2025

## Tech used

- Vue.js v3.5.13
- .NET Core 8 (minimal APIs)
- Sql Server (Express edition)
- Docker

## Running the code

I added a docker compose to the repo to (hopefully) increase the ease of use.
Just run one of the following command and it should _just work ™_

```bash
# in the foreground
docker compose up --build
# or detached
docker compose up --build -d
```

## Running the code without Docker compose

To run the backend, I used a docker image of [mssql/server:2022-latest](https://hub.docker.com/r/microsoft/mssql-server)
with the Express edition. It made it very easy to test the seeding and migrations.

Here is the command that I used to get it up and running:

```bash
docker run -e 'ACCEPT_EULA=Y' \
  -e 'SA_PASSWORD=Salasana1' \
  -e "MSSQL_PID=Express" \
  -p 1433:1433 --name sql-express \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

## Extra Assignment

The plan was to design a potential deployment strategy for this project using
services provided by Microsoft Azure.

(Image 1. A diagram of a hypothetical deployment pipeline)
![Flow diagram of a hypothetical project pipeline](./riihisoft_azure_diagram.svg)

(Image 2. Same diagram but in light theme)
![Flow diagram of a hypothetical project pipeline](./riihisoft_azure_diagram_light.svg)

### The services

#### [Azure App Service](https://learn.microsoft.com/en-us/azure/app-service/)

The project is already containerized, so it would be quite easy to put it into 
App Service. There Azure would take care of load balancing, automatic scaling 
and many more parts of managing a web application. The other idea was to use the 
[Azure Container Apps](https://azure.microsoft.com/en-us/products/app-service/web)
service, but it seemed to be more geared towards micro services and quick jobs.

I have some prior experience with the free tier of this service, where I created 
a CI/CD pipeline to it with automatic updates. This was only for development 
though, but I liked the service and its versatility.

It would be possible to use the frontend as a static website in a separate
App Service as well, so that the containers could be scaled more independently.
For this App Service provides deployment of static websites. [What is Azure Static Web Apps?](https://learn.microsoft.com/en-us/azure/static-web-apps/overview)

**Positives**

- Automatic horizontal and/or vertical scaling
- Load balancing, and other tools make it very easy to deploy dockerized 
projects to it
- Versatility on the details of how to deploy the project

#### [Azure SQL Database](https://azure.microsoft.com/en-us/products/azure-sql/database)

Azure SQL Database would allow the Docker containers to be "stateless" in a 
sense, since all the data would be stored somewhere else. This would help with
for example the horizontal scaling of the backend.

**Positives**

- Easy backups
- Fully managed SQL database
- Elastic scaling based on demand

#### [Azure Container Registry](https://azure.microsoft.com/en-us/products/container-registry)

Lastly, the docker images need to be stored somewhere. For this the Azure 
Container Registry seemed like an obvious choice. It does not cost much and 
includes quite a generous free tier.

From my quick checks, the price for keeping a registry would be less than 6
euros a month and most likely would stay within the free tier for data transfer.

**Positives**

- Good value for money
- Works nicely with Web App Services
- Keeps the docker images safe

#### [Github Actions](https://github.com/features/actions)

Github Actions would create initial part of the CI/CD pipeline between the
developer and the deployment. It provides a large amount of compute even with
the free tier and it is already used for version control.

The docker image would be built there every time something new is pushed to the
main branch. It also would allow for testing before building the image, to make
sure the most simple bugs don't get any further.

I have quite a bit of experience with using Github Actions for my own projects
and I think that it is a great tool that can save so much time when it comes
to pushing out updates and whatnot.

**Positives**

- Most likely the free tier would suffice
- Comes with Github already
- Automatic image updates to the container registry

#### Extra: [Azure Blob Storage](https://learn.microsoft.com/en-us/azure/storage/blobs/storage-blobs-introduction)

To make the container images more "stateless", the most important files could be
saved to the Azure Blob Storage. This could include files like logs and user
generated files.

**Positives**

- Cheaper than many other file storage services
- Can be accessed from anywhere

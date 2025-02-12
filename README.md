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

To run the backend, I used a docker image of the sqlserver with the Express edition.
It made it very easy to test the seeding and migrations.

Here is the command that I used to get it up and running:

```bash
docker run -e 'ACCEPT_EULA=Y' \
  -e 'SA_PASSWORD=Salasana1' \
  -e "MSSQL_PID=Express" \
  -p 1433:1433 --name sql-express \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

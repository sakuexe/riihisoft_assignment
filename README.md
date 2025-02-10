# Riihisoft Technical Assignment

By Saku Karttunen - 2.2025

## Running the code

I added a docker compose to the repo to (hopefully) increase the ease of use.
Just run one of the following command and it should _just work ™_

```bash
# in the foreground
docker compose up --build
# or detached
docker compose up -d
```

## Running the code without Docker compose

To run the backend, I used a docker image of the sqlserver, because I couldn't yet
find a way to use sql server express.

Here is the command that you should use to have it up and running:

```bash
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Salasana1' \
  -p 1433:1433 --name sql-express \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

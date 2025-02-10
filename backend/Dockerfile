FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app/riihisoft

# Copy the most needed files first
# this is so that the caching will work nicely
COPY backend/Program.cs ./backend/Program.cs
COPY backend/riihisoft.csproj ./backend/riihisoft.csproj
COPY riihisoft.sln ./riihisoft.sln

# Restore as distinct layers
RUN dotnet restore
RUN dotnet tool install --global dotnet-ef 
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy everything, caching ends here
COPY backend/ ./backend
# make tailwindcss to generate the main css file (minified)
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app/riihisoft
COPY --from=build-env /app/riihisoft/out .
ENTRYPOINT ["dotnet", "riihisoft.dll"]

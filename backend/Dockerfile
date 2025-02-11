FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app/riihisoft

# Copy the most needed files first
# this is so that the caching will work nicely
COPY backend/riihisoft.csproj ./backend/riihisoft.csproj
COPY riihisoft.sln ./riihisoft.sln
RUN dotnet restore

# Copy everything, caching usually ends here
COPY backend/ ./backend
# Create a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app/riihisoft
COPY --from=build-env /app/riihisoft/out .
ENTRYPOINT ["dotnet", "riihisoft.dll"]

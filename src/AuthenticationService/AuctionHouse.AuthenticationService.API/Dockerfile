# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the rest of the application code
COPY . .
WORKDIR /src/AuthenticationService/AuctionHouse.AuthenticationService.API
RUN dotnet restore AuctionHouse.AuthenticationService.API.csproj

# Build the application
RUN dotnet publish AuctionHouse.AuthenticationService.API.csproj -c Release -o /app

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "AuctionHouse.AuthenticationService.API.dll"]

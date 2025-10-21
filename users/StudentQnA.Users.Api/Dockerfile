# ========================
# 1. Build stage
# ========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the source and build the app
COPY . ./
RUN dotnet publish -c Release -o /app/out

# ========================
# 2. Runtime stage
# ========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/out ./

# EXPOSE 8080 is standaard voor Kestrel
EXPOSE 8080

ENTRYPOINT ["dotnet", "StudentQnA.Users.Api.dll"]

# ========================
# 1. Build stage
# ========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy entire project so restore can find everything
COPY . .

# Restore using the correct project file
RUN dotnet restore "StudentQnA.Users.Api.csproj"

# Build and publish
RUN dotnet publish "StudentQnA.Users.Api.csproj" -c Release -o /app/out

# ========================
# 2. Runtime stage
# ========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/out ./

# EXPOSE 8080 is standaard voor Kestrel
EXPOSE 8080

ENTRYPOINT ["dotnet", "StudentQnA.Users.Api.dll"]

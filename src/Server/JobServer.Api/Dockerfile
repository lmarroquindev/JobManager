# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution file
COPY JobManager.sln ./

# Copy individual project files
COPY src/Server/JobServer.Api/JobServer.Api.csproj ./src/JobServer.Api/
COPY src/Server/JobServer.Application/JobServer.Application.csproj ./src/JobServer.Application/
COPY src/Server/JobServer.Domain/JobServer.Domain.csproj ./src/JobServer.Domain/
COPY src/Server/JobServer.Infrastructure/JobServer.Infrastructure.csproj ./src/JobServer.Infrastructure/

# Restore dependencies
RUN for file in src/**/*.csproj; do dotnet restore "$file"; done

# Copy all source code
COPY src/ ./src/

# Set working directory to the API project
WORKDIR /app/src/Server/JobServer.Api

# Publish the application
RUN dotnet publish -c Release -o /out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published output
COPY --from=build /out .

# Run the application
ENTRYPOINT ["dotnet", "JobServer.Api.dll"]

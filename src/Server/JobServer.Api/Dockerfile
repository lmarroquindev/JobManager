# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivo de solución
COPY JobManager.sln ./

# Copiar archivos de proyecto individualmente
COPY src/Server/JobServer.Api/JobServer.Api.csproj ./src/JobServer.Api/
COPY src/Server/JobServer.Application/JobServer.Application.csproj ./src/JobServer.Application/
COPY src/Server/JobServer.Domain/JobServer.Domain.csproj ./src/JobServer.Domain/
COPY src/Server/JobServer.Infrastructure/JobServer.Infrastructure.csproj ./src/JobServer.Infrastructure/

# Restaurar dependencias
RUN for file in src/**/*.csproj; do dotnet restore "$file"; done

# Copiar todo el código fuente
COPY src/ ./src/

# Establecer el directorio de trabajo en el proyecto API
WORKDIR /app/src/Server/JobServer.Api

# Publicar la aplicación
RUN dotnet publish -c Release -o /out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiar la salida publicada
COPY --from=build /out .

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "JobServer.Api.dll"]

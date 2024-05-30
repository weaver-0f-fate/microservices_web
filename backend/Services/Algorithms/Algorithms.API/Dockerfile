FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Algorithms/Algorithms.API/Algorithms.API.csproj", "Services/Algorithms/Algorithms.API/"]
COPY ["Services/Algorithms/Algorithms.Infrastructure/Algorithms.Infrastructure.csproj", "Services/Algorithms/Algorithms.Infrastructure/"]
COPY ["Services/Algorithms/Algorithms.Application/Algorithms.Application.csproj", "Services/Algorithms/Algorithms.Application/"]
COPY ["Services/Algorithms/Algorithms.Application.Core/Algorithms.Application.Core.csproj", "Services/Algorithms/Algorithms.Application.Core/"]
COPY ["Services/Algorithms/Algorithms.Domain/Algorithms.Domain.csproj", "Services/Algorithms/Algorithms.Domain/"]
RUN dotnet restore "./Services/Algorithms/Algorithms.API/Algorithms.API.csproj"
COPY . .
WORKDIR "/src/Services/Algorithms/Algorithms.API"
RUN dotnet build "./Algorithms.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Algorithms.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Algorithms.API.dll"]
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Events/Events.API/Events.API.csproj", "Services/Events/Events.API/"]
COPY ["Services/Events/Events.Infrastructure/Events.Infrastructure.csproj", "Services/Events/Events.Infrastructure/"]
COPY ["Services/Events/Events.Application/Events.Application.csproj", "Services/Events/Events.Application/"]
COPY ["Services/Events/Events.Application.Core/Events.Application.Core.csproj", "Services/Events/Events.Application.Core/"]
COPY ["Services/Events/Events.Domain/Events.Domain.csproj", "Services/Events/Events.Domain/"]
RUN dotnet restore "Services/Events/Events.API/Events.API.csproj"
COPY . .
WORKDIR "/src/Services/Events/Events.API"
RUN dotnet build "Events.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Events.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Events.API.dll"]
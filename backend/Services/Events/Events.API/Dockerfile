FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Events/API/Events.API.csproj", "Services/Events/API/"]
COPY ["Services/Events/Infrastructure/Events.Infrastructure.csproj", "Services/Events/Infrastructure/"]
COPY ["Services/Events/Application/Events.Application.csproj", "Services/Events/Application/"]
COPY ["Services/Events/Application.Core/Events.Application.Core.csproj", "Services/Events/Application.Core/"]
COPY ["Services/Events/Domain/Events.Domain.csproj", "Services/Events/Domain/"]
RUN dotnet restore "Services/Events/API/Events.API.csproj"
COPY . .
WORKDIR "/src/Services/Events/API"
RUN dotnet build "Events.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Events.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Events.API.dll"]
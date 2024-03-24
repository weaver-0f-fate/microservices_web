FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Notification/Notification.API/Notification.API.csproj", "Services/Notification/Notification.API/"]
COPY ["Services/Notification/Notification.Infrastructure/Notification.Infrastructure.csproj", "Services/Notification/Notification.Infrastructure/"]
COPY ["Services/Notification/Notification.Application/Notification.Application.csproj", "Services/Notification/Notification.Application/"]
COPY ["Services/Notification/Notification.Application.Core/Notification.Application.Core.csproj", "Services/Notification/Notification.Application.Core/"]
COPY ["Services/Notification/Notification.Domain/Notification.Domain.csproj", "Services/Notification/Notification.Domain/"]
RUN dotnet restore "Services/Notification/Notification.API/Notification.API.csproj"
COPY . .
WORKDIR "/src/Services/Notification/Notification.API"
RUN dotnet build "Notification.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Notification.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notification.API.dll"]
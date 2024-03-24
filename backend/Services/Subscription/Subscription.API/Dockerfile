FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Subscription/Subscription.API/Subscription.API.csproj", "Services/Subscription/Subscription.API/"]
COPY ["Services/Subscription/Subscription.Infrastructure/Subscription.Infrastructure.csproj", "Services/Subscription/Subscription.Infrastructure/"]
COPY ["Services/Subscription/Subscription.Application/Subscription.Application.csproj", "Services/Subscription/Subscription.Application/"]
COPY ["Services/Subscription/Subscription.Application.Core/Subscription.Application.Core.csproj", "Services/Subscription/Subscription.Application.Core/"]
COPY ["Services/Subscription/Subscription.Domain/Subscription.Domain.csproj", "Services/Subscription/Subscription.Domain/"]
RUN dotnet restore "Services/Subscription/Subscription.API/Subscription.API.csproj"
COPY . .
WORKDIR "/src/Services/Subscription/Subscription.API"
RUN dotnet build "Subscription.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Subscription.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Subscription.API.dll"]
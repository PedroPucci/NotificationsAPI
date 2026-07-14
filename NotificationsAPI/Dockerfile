FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["nuget.config", "/src/nuget.config"]
COPY ["nuget-packages/", "/src/nuget-packages/"]

COPY ["NotificationsAPI/NotificationsAPI.csproj", "NotificationsAPI/"]
COPY ["NotificationsAPI.Application/NotificationsAPI.Application.csproj", "NotificationsAPI.Application/"]
COPY ["NotificationsAPI.Infrastructure/NotificationsAPI.Infrastructure.csproj", "NotificationsAPI.Infrastructure/"]
COPY ["NotificationsAPI.Shared/NotificationsAPI.Shared.csproj", "NotificationsAPI.Shared/"]

RUN dotnet restore "NotificationsAPI/NotificationsAPI.csproj" \
    --configfile "/src/nuget.config"

COPY . .

WORKDIR "/src/NotificationsAPI"

RUN dotnet publish "NotificationsAPI.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

USER app
WORKDIR /app

EXPOSE 8080

ENV ASPNETCORE_HTTP_PORTS=8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "NotificationsAPI.dll"]
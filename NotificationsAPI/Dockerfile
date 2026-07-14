FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["NotificationsAPI/NotificationsAPI.csproj", "NotificationsAPI/"]
COPY ["NotificationsAPI.Application/NotificationsAPI.Application.csproj", "NotificationsAPI.Application/"]
COPY ["NotificationsAPI.Domain/NotificationsAPI.Domain.csproj", "NotificationsAPI.Domain/"]
COPY ["NotificationsAPI.Infrastructure/NotificationsAPI.Infrastructure.csproj", "NotificationsAPI.Infrastructure/"]
COPY ["NotificationsAPI.Shared/NotificationsAPI.Shared.csproj", "NotificationsAPI.Shared/"]

RUN dotnet restore "NotificationsAPI/NotificationsAPI.csproj"

COPY . .

WORKDIR "/src/NotificationsAPI"

RUN dotnet build "NotificationsAPI.csproj" \
    -c Release \
    -o /app/build

FROM build AS publish

RUN dotnet publish "NotificationsAPI.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "NotificationsAPI.dll"]
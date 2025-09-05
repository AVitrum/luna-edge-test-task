FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

RUN apt-get update && apt-get install -y gosu && rm -rf /var/lib/apt/lists/*

FROM base AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["MyProject.WebApi/MyProject.WebApi.csproj", "MyProject.WebApi/"]
COPY ["MyProject.Application/MyProject.Application.csproj", "MyProject.Application/"]
COPY ["MyProject.Domain/MyProject.Domain.csproj", "MyProject.Domain/"]
COPY ["MyProject.Infrastructure/MyProject.Infrastructure.csproj", "MyProject.Infrastructure/"]
RUN dotnet restore "MyProject.WebApi/MyProject.WebApi.csproj"

COPY . .
WORKDIR "/src/MyProject.WebApi"
RUN dotnet build "MyProject.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MyProject.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/MyProject.WebApi/entrypoint.sh .

RUN chmod +x ./entrypoint.sh
RUN chown -R $APP_UID:$APP_UID /app

ENTRYPOINT ["./entrypoint.sh"]
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

RUN useradd -m appuser
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["VotepucApp.sln", "."]
COPY ["VotepucApp.WebAPI/VotepucApp.WebAPI.csproj", "VotepucApp.WebAPI/"]
COPY ["VotepucApp.Application/VotepucApp.Application.csproj", "VotepucApp.Application/"]
COPY ["VotepucApp.Domain/VotepucApp.Domain.csproj", "VotepucApp.Domain/"]
COPY ["VotepucApp.Services/VotepucApp.Services.csproj", "VotepucApp.Services/"]
COPY ["VotepucApp.Persistence/VotepucApp.Persistence.csproj", "VotepucApp.Persistence/"]

RUN dotnet restore "VotepucApp.sln"

COPY . .

WORKDIR "/src"
RUN dotnet build "VotepucApp.sln" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "VotepucApp.WebAPI/VotepucApp.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS="http://+:8080"

ENTRYPOINT ["dotnet", "VotepucApp.WebAPI.dll"]

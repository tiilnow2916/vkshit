# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Сначала копируем sln и csproj, чтобы кэшировались restore
COPY *.sln .
COPY vkshit/*.csproj vkshit/
RUN dotnet restore

# Теперь копируем весь проект
COPY . .
RUN dotnet publish -c Release -o /out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /out .

# Render запускает контейнер на порту 10000–19999, но мы сами указываем 8080
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "vkshit.dll"]

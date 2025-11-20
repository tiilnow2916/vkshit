FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 10000
ENV ASPNETCORE_URLS=http://+:10000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["vkshit/vkshit.csproj", "vkshit/"]
RUN dotnet restore "vkshit/vkshit.csproj"
COPY . .
WORKDIR "/src/vkshit"
RUN dotnet build "vkshit.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "vkshit.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "vkshit.dll"]

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["ShoeStore.API/ShoeStore.API.csproj", "ShoeStore.API/"]
RUN dotnet restore "ShoeStore.API/ShoeStore.API.csproj"
COPY . .
WORKDIR "/src/ShoeStore.API"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "ShoeStore.API.dll"]
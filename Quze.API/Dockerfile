FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY Quze.API/Quze.API.csproj Quze.API/
RUN dotnet restore Quze.API/Quze.API.csproj
COPY . .
WORKDIR /src/Quze.API
RUN dotnet build Quze.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Quze.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Quze.API.dll"]

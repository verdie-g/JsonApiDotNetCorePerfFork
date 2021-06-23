FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY app/app.csproj ./dotnetapp/
WORKDIR /app/dotnetapp
RUN dotnet restore

# copy and publish app and libraries
WORKDIR /app/
COPY app/. ./dotnetapp/
WORKDIR /app/dotnetapp
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/dotnetapp/out ./
ENTRYPOINT ["dotnet", "app.dll"]

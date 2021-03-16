FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY CommandManager/CommandManager.csproj ./CommandManager/
COPY CommandManagerTests/CommandManagerTests.csproj ./CommandManagerTests/
RUN dotnet restore

# copy everything else and build app
COPY CommandManager/. ./CommandManager/.
COPY CommandManagerTests/. ./CommandManagerTests/.
WORKDIR /app/CommandManager
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/CommandManager/out ./
RUN mkdir /app/CommandResult/
ENTRYPOINT ["dotnet", "CommandManager.dll"]
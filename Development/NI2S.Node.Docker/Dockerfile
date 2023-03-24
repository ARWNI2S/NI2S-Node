#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/NI2S.Node.Docker/NI2S.Node.Docker.csproj", "src/NI2S.Node.Docker/"]
RUN dotnet restore "src/NI2S.Node.Docker/NI2S.Node.Docker.csproj"
COPY . .
WORKDIR "/src/src/NI2S.Node.Docker"
RUN dotnet build "NI2S.Node.Docker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NI2S.Node.Docker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NI2S.Node.Docker.dll"]
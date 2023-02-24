#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/NI2SService/NI2SService.csproj", "src/NI2SService/"]
RUN dotnet restore "src/NI2SService/NI2SService.csproj"
COPY . .
WORKDIR "/src/src/NI2SService"
RUN dotnet build "NI2SService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NI2SService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NI2SService.dll"]
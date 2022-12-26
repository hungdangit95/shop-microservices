#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Basket.API/Basket.API/Basket.API.csproj", "src/Services/Basket.API/Basket.API/"]
COPY ["src/Services/Customer.API/Customer.API.csproj", "src/Services/Customer.API/"]
COPY ["src/BuildingBlocks/Shared/Shared.csproj", "src/BuildingBlocks/Shared/"]
COPY ["src/BuildingBlocks/Common.Logging/Common.Logging.csproj", "src/BuildingBlocks/Common.Logging/"]
COPY ["src/BuildingBlocks/Infrastructures/Infrastructures.csproj", "src/BuildingBlocks/Infrastructures/"]
COPY ["src/BuildingBlocks/Contracts/Contracts.csproj", "src/BuildingBlocks/Contracts/"]
RUN dotnet restore "src/Services/Basket.API/Basket.API/Basket.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Basket.API/Basket.API"
RUN dotnet build "Basket.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Basket.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.API.dll"]
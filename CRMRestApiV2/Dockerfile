#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CRMRestApiV2/CRMRestApiV2.csproj", "CRMRestApiV2/"]
COPY ["CRMRepository/CRMRepository.csproj", "CRMRepository/"]
RUN dotnet restore "CRMRestApiV2/CRMRestApiV2.csproj"
COPY . .
WORKDIR "/src/CRMRestApiV2"
RUN dotnet build "CRMRestApiV2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CRMRestApiV2.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CRMRestApiV2.dll"]


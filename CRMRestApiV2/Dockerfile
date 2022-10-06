#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#WORKDIR /app
#EXPOSE 80
#
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /src
#COPY ["CRMRestApiV2/CRMRestApiV2.csproj", "CRMRestApiV2/"]
#COPY ["CRMRepository/CRMRepository.csproj", "CRMRepository/"]
#RUN dotnet restore "CRMRestApiV2/CRMRestApiV2.csproj"
#COPY . .
#WORKDIR "/src/CRMRestApiV2"
#RUN dotnet build "CRMRestApiV2.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "CRMRestApiV2.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "CRMRestApiV2.dll"]

# Set the base image as the .NET 6.0 SDK (this includes the runtime)
FROM mcr.microsoft.com/dotnet/runtime:6.0 as build-env

# Copy everything and publish the release (publish implicitly restores and builds)
WORKDIR /app
COPY . ./
RUN dotnet publish "CRMRestApiV2.csproj"-c Release -o out --no-self-contained

# Label the container
LABEL maintainer="Steeves Saillant <steeves.saillant@alithya.com>"
LABEL repository="https://github.com/steevessaillant/SimpleCRM"


# Label as GitHub action
LABEL com.github.actions.name="GitHub Action"
# Limit to 160 characters
LABEL com.github.actions.description="GitHub Action."
# See branding:
# https://docs.github.com/actions/creating-actions/metadata-syntax-for-github-actions#branding
LABEL com.github.actions.icon="activity"
LABEL com.github.actions.color="orange"

# Relayer the .NET SDK, anew with the build output
FROM mcr.microsoft.com/dotnet/runtime:6.0
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "CRMRestApiV2.dll" ]
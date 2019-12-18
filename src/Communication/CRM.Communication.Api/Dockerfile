FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder

ARG feed='--source "https://api.nuget.org/v3/index.json"'

WORKDIR /src
COPY ["src/BuildingBlocks/CRM.Shared/*.csproj", "src/BuildingBlocks/CRM.Shared/"]
COPY ["src/BuildingBlocks/CRM.Tracing.Jaeger/*.csproj", "src/BuildingBlocks/CRM.Tracing.Jaeger/"]
COPY ["src/BuildingBlocks/CRM.MassTransit/*.csproj", "src/BuildingBlocks/CRM.MassTransit/"]
COPY ["src/BuildingBlocks/CRM.Configuration.Vault/*.csproj", "src/BuildingBlocks/CRM.Configuration.Vault/"]
COPY ["src/BuildingBlocks/CRM.Metrics/*.csproj", "src/BuildingBlocks/CRM.Metrics/"]
COPY ["src/Communication/CRM.Communication.Api/*.csproj", "src/Communication/CRM.Communication.Api/"]
COPY ["src/Communication/CRM.Communication/*.csproj", "src/Communication/CRM.Communication/"]
COPY ["src/Communication/CRM.Communication.FunctionalTests/*.csproj", "src/Communication/CRM.Communication.FunctionalTests/"]

RUN dotnet restore src/Communication/CRM.Communication.Api/ /property:Configuration=Release ${feed} -nowarn:msb3202,nu1503
RUN dotnet restore src/Communication/CRM.Communication.FunctionalTests/ /property:Configuration=Release ${feed} -nowarn:msb3202,nu1503

COPY ["src/BuildingBlocks/CRM.Shared/.", "src/BuildingBlocks/CRM.Shared/"]
COPY ["src/BuildingBlocks/CRM.Tracing.Jaeger/.", "src/BuildingBlocks/CRM.Tracing.Jaeger/"]
COPY ["src/BuildingBlocks/CRM.MassTransit/.", "src/BuildingBlocks/CRM.MassTransit/"]
COPY ["src/BuildingBlocks/CRM.Configuration.Vault/.", "src/BuildingBlocks/CRM.Configuration.Vault/"]
COPY ["src/BuildingBlocks/CRM.Metrics/.", "src/BuildingBlocks/CRM.Metrics/"]
COPY ["src/Communication/CRM.Communication.Api/.", "src/Communication/CRM.Communication.Api/"]
COPY ["src/Communication/CRM.Communication/.", "src/Communication/CRM.Communication/"]
COPY ["src/Communication/CRM.Communication.FunctionalTests/.", "src/Communication/CRM.Communication.FunctionalTests/"]
RUN dotnet publish src/Communication/CRM.Communication.Api/ -c Release -o /app --no-restore

FROM builder AS functionaltest
WORKDIR /src/src/Communication/CRM.Communication.FunctionalTests

FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT docker

COPY --from=builder /app .
EXPOSE 80
ENTRYPOINT [ "dotnet",  "CRM.Communication.Api.dll"]

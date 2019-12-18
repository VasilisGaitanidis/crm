FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder

ARG feed='--source "https://api.nuget.org/v3/index.json"'

WORKDIR /src
COPY ["src/BuildingBlocks/CRM.Shared/*.csproj", "src/BuildingBlocks/CRM.Shared/"]
COPY ["src/BuildingBlocks/CRM.Tracing.Jaeger/*.csproj", "src/BuildingBlocks/CRM.Tracing.Jaeger/"]
COPY ["src/BuildingBlocks/CRM.Configuration.Vault/*.csproj", "src/BuildingBlocks/CRM.Configuration.Vault/"]
COPY ["src/BuildingBlocks/CRM.Metrics/*.csproj", "src/BuildingBlocks/CRM.Metrics/"]
COPY ["src/Contact/CRM.Contact.Api/*.csproj", "src/Contact/CRM.Contact.Api/"]
COPY ["src/Gateways/CRM.Graph.Gateway/*.csproj", "src/Gateways/CRM.Graph.Gateway/"]

RUN dotnet restore src/Gateways/CRM.Graph.Gateway/ /property:Configuration=Release ${feed} -nowarn:msb3202,nu1503

COPY ["src/proto/.", "src/proto/"]
COPY ["src/BuildingBlocks/CRM.Shared/.", "src/BuildingBlocks/CRM.Shared/"]
COPY ["src/BuildingBlocks/CRM.Tracing.Jaeger/.", "src/BuildingBlocks/CRM.Tracing.Jaeger/"]
COPY ["src/BuildingBlocks/CRM.Configuration.Vault/", "src/BuildingBlocks/CRM.Configuration.Vault/"]
COPY ["src/BuildingBlocks/CRM.Metrics/.", "src/BuildingBlocks/CRM.Metrics/"]
COPY ["src/Contact/CRM.Contact.Api/.", "src/Contact/CRM.Contact.Api/"]
COPY ["src/Gateways/CRM.Graph.Gateway/.", "src/Gateways/CRM.Graph.Gateway/"]

RUN dotnet publish src/Gateways/CRM.Graph.Gateway/ -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR /app

ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT docker

COPY --from=builder /app .

EXPOSE 80
ENTRYPOINT [ "dotnet",  "CRM.Graph.Gateway.dll"]

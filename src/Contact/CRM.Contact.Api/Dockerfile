FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder

ARG feed='--source "https://api.nuget.org/v3/index.json"'

WORKDIR /src
COPY ["src/BuildingBlocks/CRM.Shared/*.csproj", "src/BuildingBlocks/CRM.Shared/"]
COPY ["src/BuildingBlocks/CRM.Tracing.Jaeger/*.csproj", "src/BuildingBlocks/CRM.Tracing.Jaeger/"]
COPY ["src/BuildingBlocks/CRM.MassTransit/*.csproj", "src/BuildingBlocks/CRM.MassTransit/"]
COPY ["src/BuildingBlocks/CRM.Configuration.Vault/*.csproj", "src/BuildingBlocks/CRM.Configuration.Vault/"]
COPY ["src/BuildingBlocks/CRM.Metrics/*.csproj", "src/BuildingBlocks/CRM.Metrics/"]
COPY ["src/BuildingBlocks/CRM.Dapper/*.csproj", "src/BuildingBlocks/CRM.Dapper/"]
COPY ["src/Contact/CRM.Contact.Api/*.csproj", "src/Contact/CRM.Contact.Api/"]
COPY ["src/Contact/CRM.Contact/*.csproj", "src/Contact/CRM.Contact/"]
COPY ["src/Contact/CRM.Contact.FunctionalTests/*.csproj", "src/Contact/CRM.Contact.FunctionalTests/"]

RUN dotnet restore src/Contact/CRM.Contact.Api/ /property:Configuration=Release ${feed} -nowarn:msb3202,nu1503
RUN dotnet restore src/Contact/CRM.Contact.FunctionalTests/ /property:Configuration=Debug ${feed} -nowarn:msb3202,nu1503

COPY ["src/proto/.", "src/proto/"]
COPY ["src/BuildingBlocks/CRM.Shared/.", "src/BuildingBlocks/CRM.Shared/"]
COPY ["src/BuildingBlocks/CRM.Tracing.Jaeger/.", "src/BuildingBlocks/CRM.Tracing.Jaeger/"]
COPY ["src/BuildingBlocks/CRM.MassTransit/.", "src/BuildingBlocks/CRM.MassTransit/"]
COPY ["src/BuildingBlocks/CRM.Configuration.Vault/.", "src/BuildingBlocks/CRM.Configuration.Vault/"]
COPY ["src/BuildingBlocks/CRM.Metrics/.", "src/BuildingBlocks/CRM.Metrics/"]
COPY ["src/BuildingBlocks/CRM.Dapper/.", "src/BuildingBlocks/CRM.Dapper/"]
COPY ["src/Contact/CRM.Contact.Api/.", "src/Contact/CRM.Contact.Api/"]
COPY ["src/Contact/CRM.Contact/.", "src/Contact/CRM.Contact/"]
COPY ["src/Contact/CRM.Contact.FunctionalTests/.", "src/Contact/CRM.Contact.FunctionalTests/"]
RUN dotnet publish src/Contact/CRM.Contact.Api/ -c Release -o /app --no-restore

FROM builder AS functionaltest
WORKDIR /src/src/Contact/CRM.Contact.FunctionalTests

FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT docker

COPY --from=builder /app .

EXPOSE 80
ENTRYPOINT [ "dotnet",  "CRM.Contact.Api.dll"]

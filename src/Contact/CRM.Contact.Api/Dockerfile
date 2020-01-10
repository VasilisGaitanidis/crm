FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder

WORKDIR /src
COPY ["nuget.config", "./"]
COPY ["src/BuildingBlocks/CRM.Shared/*.csproj", "src/BuildingBlocks/CRM.Shared/"]
COPY ["src/BuildingBlocks/CRM.OpenTelemetry/*.csproj", "src/BuildingBlocks/CRM.OpenTelemetry/"]
COPY ["src/BuildingBlocks/CRM.MassTransit/*.csproj", "src/BuildingBlocks/CRM.MassTransit/"]
COPY ["src/BuildingBlocks/CRM.Configuration.Vault/*.csproj", "src/BuildingBlocks/CRM.Configuration.Vault/"]
COPY ["src/BuildingBlocks/CRM.Metrics/*.csproj", "src/BuildingBlocks/CRM.Metrics/"]
COPY ["src/BuildingBlocks/CRM.Dapper/*.csproj", "src/BuildingBlocks/CRM.Dapper/"]
COPY ["src/BuildingBlocks/CRM.GraphQL/*.csproj", "src/BuildingBlocks/CRM.GraphQL/"]
COPY ["src/Contact/CRM.Contact.Api/*.csproj", "src/Contact/CRM.Contact.Api/"]
COPY ["src/Contact/CRM.Contact/*.csproj", "src/Contact/CRM.Contact/"]
COPY ["src/Contact/CRM.Contact.FunctionalTests/*.csproj", "src/Contact/CRM.Contact.FunctionalTests/"]

RUN dotnet restore src/Contact/CRM.Contact.Api/ /property:Configuration=Release -nowarn:msb3202,nu1503 --configfile nuget.config
RUN dotnet restore src/Contact/CRM.Contact.FunctionalTests/ /property:Configuration=Debug -nowarn:msb3202,nu1503 --configfile nuget.config

COPY ["src/proto/.", "src/proto/"]
COPY ["src/BuildingBlocks/CRM.Shared/.", "src/BuildingBlocks/CRM.Shared/"]
COPY ["src/BuildingBlocks/CRM.OpenTelemetry/.", "src/BuildingBlocks/CRM.OpenTelemetry/"]
COPY ["src/BuildingBlocks/CRM.MassTransit/.", "src/BuildingBlocks/CRM.MassTransit/"]
COPY ["src/BuildingBlocks/CRM.Configuration.Vault/.", "src/BuildingBlocks/CRM.Configuration.Vault/"]
COPY ["src/BuildingBlocks/CRM.Metrics/.", "src/BuildingBlocks/CRM.Metrics/"]
COPY ["src/BuildingBlocks/CRM.Dapper/.", "src/BuildingBlocks/CRM.Dapper/"]
COPY ["src/BuildingBlocks/CRM.GraphQL/", "src/BuildingBlocks/CRM.GraphQL/"]
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

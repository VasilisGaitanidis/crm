FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder

WORKDIR /src
COPY ["nuget.config", "./"]
COPY ["src/BuildingBlocks/CRM.Shared/*.csproj", "src/BuildingBlocks/CRM.Shared/"]
COPY ["src/BuildingBlocks/CRM.OpenTelemetry/*.csproj", "src/BuildingBlocks/CRM.OpenTelemetry/"]
COPY ["src/BuildingBlocks/CRM.MassTransit/*.csproj", "src/BuildingBlocks/CRM.MassTransit/"]
COPY ["src/BuildingBlocks/CRM.Metrics/*.csproj", "src/BuildingBlocks/CRM.Metrics/"]
COPY ["src/BuildingBlocks/CRM.GraphQL/*.csproj", "src/BuildingBlocks/CRM.GraphQL/"]
COPY ["src/Personal/CRM.Personal.Api/*.csproj", "src/Personal/CRM.Personal.Api/"]
COPY ["src/Personal/CRM.Personal/*.csproj", "src/Personal/CRM.Personal/"]
COPY ["src/Personal/CRM.Personal.FunctionalTests/*.csproj", "src/Personal/CRM.Personal.FunctionalTests/"]

RUN dotnet restore src/Personal/CRM.Personal.Api/ /property:Configuration=Release -nowarn:msb3202,nu1503 --configfile nuget.config
RUN dotnet restore src/Personal/CRM.Personal.FunctionalTests/ /property:Configuration=Debug -nowarn:msb3202,nu1503 --configfile nuget.config

COPY ["src/proto/.", "src/proto/"]
COPY ["src/BuildingBlocks/CRM.Shared/.", "src/BuildingBlocks/CRM.Shared/"]
COPY ["src/BuildingBlocks/CRM.OpenTelemetry/.", "src/BuildingBlocks/CRM.OpenTelemetry/"]
COPY ["src/BuildingBlocks/CRM.MassTransit/.", "src/BuildingBlocks/CRM.MassTransit/"]
COPY ["src/BuildingBlocks/CRM.Metrics/.", "src/BuildingBlocks/CRM.Metrics/"]
COPY ["src/BuildingBlocks/CRM.GraphQL/.", "src/BuildingBlocks/CRM.GraphQL/"]
COPY ["src/Personal/CRM.Personal.Api/.", "src/Personal/CRM.Personal.Api/"]
COPY ["src/Personal/CRM.Personal/.", "src/Personal/CRM.Personal/"]
COPY ["src/Personal/CRM.Personal.FunctionalTests/.", "src/Personal/CRM.Personal.FunctionalTests/"]
RUN dotnet publish src/Personal/CRM.Personal.Api/ -c Release -o /app --no-restore

FROM builder AS functionaltest
WORKDIR /src/src/Personal/CRM.Personal.FunctionalTests

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.0-buster-slim
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT docker

COPY --from=builder /app .

ENTRYPOINT [ "dotnet",  "CRM.Personal.Api.dll"]


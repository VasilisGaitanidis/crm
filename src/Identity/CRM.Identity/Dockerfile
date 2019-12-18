FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder

ARG feed='--source "https://api.nuget.org/v3/index.json"'

WORKDIR /src
COPY ["src/BuildingBlocks/CRM.Shared/*.csproj", "src/BuildingBlocks/CRM.Shared/"]
COPY ["src/BuildingBlocks/CRM.Tracing.Jaeger/*.csproj", "src/BuildingBlocks/CRM.Tracing.Jaeger/"]
COPY ["src/BuildingBlocks/CRM.Metrics/*.csproj", "src/BuildingBlocks/CRM.Metrics/"]
COPY ["src/Identity/CRM.Identity/*.csproj", "src/Identity/CRM.Identity/"]

RUN dotnet restore src/Identity/CRM.Identity/ /property:Configuration=Release ${feed} -nowarn:msb3202,nu1503

COPY ["src/BuildingBlocks/CRM.Shared/.", "src/BuildingBlocks/CRM.Shared/"]
COPY ["src/BuildingBlocks/CRM.Tracing.Jaeger/.", "src/BuildingBlocks/CRM.Tracing.Jaeger/"]
COPY ["src/BuildingBlocks/CRM.Metrics/.", "src/BuildingBlocks/CRM.Metrics/"]
COPY ["src/Identity/CRM.Identity/.", "src/Identity/CRM.Identity/"]
RUN dotnet publish src/Identity/CRM.Identity/ -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR /app

ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT docker

COPY --from=builder /app .

EXPOSE 80
ENTRYPOINT [ "dotnet",  "CRM.Identity.dll"]

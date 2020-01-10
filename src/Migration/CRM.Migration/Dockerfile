FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder

ARG feed='--source "https://api.nuget.org/v3/index.json"'

WORKDIR /src
COPY ["src/Migration/CRM.Migration/*.csproj", "src/Migration/CRM.Migration/"]
RUN dotnet restore src/Migration/CRM.Migration/ /property:Configuration=Release ${feed} -nowarn:msb3202,nu1503

COPY ["src/Migration/CRM.Migration/.", "src/Migration/CRM.Migration/"]
RUN dotnet publish src/Migration/CRM.Migration/ -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.0-buster-slim
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT docker

COPY --from=builder /app .
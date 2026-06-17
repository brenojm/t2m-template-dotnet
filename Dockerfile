FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY T2MTemplate.Api/T2MTemplate.Api.csproj T2MTemplate.Api/
COPY T2MTemplate.Application/T2MTemplate.Application.csproj T2MTemplate.Application/
COPY T2MTemplate.Domain/T2MTemplate.Domain.csproj T2MTemplate.Domain/
COPY T2MTemplate.Infra/T2MTemplate.Infra.csproj T2MTemplate.Infra/

RUN dotnet restore ./T2MTemplate.Api/T2MTemplate.Api.csproj

COPY T2MTemplate.Api/ T2MTemplate.Api/
COPY T2MTemplate.Application/ T2MTemplate.Application/
COPY T2MTemplate.Domain/ T2MTemplate.Domain/
COPY T2MTemplate.Infra/ T2MTemplate.Infra/
COPY . .

RUN dotnet build ./T2MTemplate.Api/T2MTemplate.Api.csproj -c Release -o /app/build

RUN dotnet publish ./T2MTemplate.Api/T2MTemplate.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "T2MTemplate.Api.dll"]
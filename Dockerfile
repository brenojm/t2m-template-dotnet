FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY src/T2MTemplate.Api/T2MTemplate.Api.csproj src/T2MTemplate.Api/
COPY src/T2MTemplate.Application/T2MTemplate.Application.csproj src/T2MTemplate.Application/
COPY src/T2MTemplate.Domain/T2MTemplate.Domain.csproj src/T2MTemplate.Domain/
COPY src/T2MTemplate.Infra/T2MTemplate.Infra.csproj src/T2MTemplate.Infra/

RUN dotnet restore ./src/T2MTemplate.Api/T2MTemplate.Api.csproj

COPY src/T2MTemplate.Api/ src/T2MTemplate.Api/
COPY src/T2MTemplate.Application/ src/T2MTemplate.Application/
COPY src/T2MTemplate.Domain/ src/T2MTemplate.Domain/
COPY src/T2MTemplate.Infra/ src/T2MTemplate.Infra/
COPY . .

RUN dotnet build ./src/T2MTemplate.Api/T2MTemplate.Api.csproj -c Release -o /app/build

RUN dotnet publish ./src/T2MTemplate.Api/T2MTemplate.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "T2MTemplate.Api.dll"]

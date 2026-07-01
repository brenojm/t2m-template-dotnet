# T2MTemplate - Template Padrão de Projetos .NET

Template oficial da T2M para criação de projetos .NET internos, seguindo arquitetura em camadas com autenticação integrada ao SGId.

## Pré-requisitos

- .NET SDK 8.0 ou superior
- PostgreSQL
- Acesso ao SGId para obter as configurações de JWT
- Docker (opcional)

Verifique a versão do SDK:

```bash
dotnet --version
```

---

## Instalação do Template

```bash
dotnet new install <caminho-ou-repositorio>
```

---

## Criando um Novo Projeto

```bash
dotnet new t2m-dotnet -n NomeDoProjeto
```

O comando substitui automaticamente `T2MTemplate` pelo nome informado em todos os namespaces, classes e arquivos de configuração.

---

## Estrutura do Projeto

O template segue Clean Architecture com separação clara de responsabilidades. Os projetos de produção ficam em `src/` e os projetos de teste em `tests/`:

```
NomeDoProjeto.sln
├── Dockerfile
├── .gitignore
│
├── src/
│   ├── NomeDoProjeto.Api            (Presentation)
│   │   ├── Controllers/
│   │   ├── Extensions/
│   │   │   ├── SGIdAuthenticationExtensions.cs
│   │   │   ├── ServiceCollectionExtensions.cs
│   │   │   └── SwaggerExtensions.cs
│   │   └── Middlewares/
│   │       └── GlobalExceptionHandler.cs
│   │
│   ├── NomeDoProjeto.Application     (Application)
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   ├── Mappings/
│   │   └── Services/
│   │
│   ├── NomeDoProjeto.Domain          (Domain)
│   │   ├── Entities/
│   │   ├── Exceptions/
│   │   └── Interfaces/
│   │
│   └── NomeDoProjeto.Infra           (Infra)
│       ├── Data/
│       │   └── AppDbContext.cs
│       └── Repositories/
│
└── tests/
    └── NomeDoProjeto.Tests           (Tests)
        └── Services/
            └── CollaboratorServiceTests.cs
```

> No arquivo de solução (`.sln`) os projetos aparecem organizados em pastas virtuais — `1 - Presentation`, `2 - Application`, `3 - Domain`, `4 - Infra`, `5 - Tests` e `0 - Solution Items` — independentemente do local físico em `src/` e `tests/`.

### Camadas

| Camada | Responsabilidade |
|--------|-----------------|
| **Presentation** | Controllers, autenticação SGId, configuração do Swagger, middleware de exceções |
| **Application** | Services, DTOs, AutoMapper profiles |
| **Domain** | Entidades, interfaces de repositório, exceptions de domínio |
| **Infra** | AppDbContext, implementação dos repositories (EF Core + PostgreSQL) |
| **Tests** | Testes unitários (xUnit + Moq) |

---

## Autenticação SGId

O template já inclui a integração com o SGId via JWT RSA-256. Configure o `appsettings.json` com os valores fornecidos pelo SGId:

```json
{
  "JwtSettings": {
    "Issuer": "<SGId-TenantId>",
    "Audience": "<SGId-ClientId>",
    "PublicKey": "<Base64-RSA-PublicKey>",
    "SystemId": "<SGId-SystemId>"
  }
}
```

O token JWT emitido pelo SGId contém a claim `Sistemas` no formato `"systemId:role1,role2"`. O template extrai automaticamente as roles do sistema configurado e as disponibiliza via `[Authorize(Roles = "NomeDaRole")]`.

Para proteger um endpoint:

```csharp
[Authorize]                            // requer token válido
[Authorize(Roles = "ADMIN")]           // requer role específica
```

---

## Configuração

### Connection String

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=NomeDoProjeto;Username=postgres;Password=postgres;"
  }
}
```

### Ambientes disponíveis

| Arquivo | Ambiente |
|---------|----------|
| `appsettings.json` | Base / padrão |

---

## Pacotes Incluídos

| Camada | Pacote | Versão |
|--------|--------|--------|
| Api | Microsoft.AspNetCore.Authentication.JwtBearer | 8.0.14 |
| Api | Microsoft.IdentityModel.Tokens | 8.6.1 |
| Api | System.IdentityModel.Tokens.Jwt | 8.6.1 |
| Api | Swashbuckle.AspNetCore | 6.6.2 |
| Application | AutoMapper | 14.0.0 |
| Infra | Microsoft.EntityFrameworkCore | 9.0.5 |
| Infra | Npgsql.EntityFrameworkCore.PostgreSQL | 9.0.4 |

---

## Padrões Adotados

- **Autenticação** via SGId com JWT RSA-256 (`SGIdAuthenticationExtensions`)
- **Global Exception Handler** com mapeamento de exceções de domínio para HTTP status codes
- **Repository Pattern** com EF Core puro (sem abstração extra)
- **Service Layer** com interfaces no Application
- **AutoMapper** para mapeamento Entity ↔ DTO
- **Interfaces de Repository** definidas na camada Domain
- **Injeção de Dependência** centralizada em `ServiceCollectionExtensions`
- **Swagger** documentado com autenticação Bearer JWT, habilitado apenas em ambiente de desenvolvimento

---

## Docker

```bash
docker build -t nome-do-projeto .
docker run -p 8080:8080 nome-do-projeto
```

Portas expostas: `8080` (HTTP) e `8081` (HTTPS).

---

## Contribuindo com o Template

1. Crie uma branch a partir de `main`
2. Faça as alterações necessárias
3. Abra um Pull Request descrevendo o que mudou e por que

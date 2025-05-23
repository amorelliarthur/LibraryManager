# LibraryManager

Projeto desenvolvido em C# com .NET 8, composto por uma API RESTful, uma aplicação de console e suporte a banco de dados com Entity Framework Core. O sistema gerencia livros, leitores, gêneros e editoras, utilizando relações 1:N e N:N, migrations, DTOs e proxies para lazy loading.

## Estrutura da Solução

A solução é composta por quatro projetos:

- **LibraryManager.Shared.Data**: Contém o `DbContext`, as Migrations e a classe genérica `DAL<T>`.
- **LibraryManager.Shared.Models**: Contém as entidades do domínio: `Book`, `Genre`, `Reader`, `Publisher`.
- **LibraryManager.API**: API com endpoints organizados por entidade e uso de DTOs.
- **LibraryManager.Console**: Interface simples via terminal para interagir com o banco.

## Diagrama de Classes

Abaixo está o diagrama de classes que representa as principais entidades e relacionamentos do projeto:

![Diagrama de Classes](doc/Diagram_Class_LibraryManager.png)

## Requisitos

- .NET 8 SDK
- SQL Server LocalDB
- Entity Framework Core 8.x
- Visual Studio 2022+

## Funcionalidades

### API RESTful

Disponível via Swagger: `https://localhost:{porta}/swagger`

Funcionalidades por entidade:

#### Book

- `GET /books`: Lista todos os livros
- `GET /books/{id}`: Retorna um livro por ID
- `POST /books`: Cadastra um novo livro
- `PUT /books`: Edita um livro existente
- `DELETE /books/{id}`: Exclui um livro
- `GET /books/{id}/readers`: Lista leitores de um livro

#### Genre

- `GET /genres`: Lista todos os gêneros
- `GET /genres/{id}`: Retorna um gênero
- `POST /genres`: Cadastra um novo gênero
- `PUT /genres`: Edita um gênero
- `DELETE /genres/{id}`: Remove um gênero
- `GET /genres/{id}/books`: Lista livros de um gênero

#### Publisher

- `GET /publishers`: Lista todas as editoras
- `GET /publishers/{id}`: Retorna uma editora
- `POST /publishers`: Cadastra uma nova editora
- `PUT /publishers`: Edita uma editora
- `DELETE /publishers/{id}`: Remove uma editora
- `GET /publishers/{id}/books`: Lista livros de uma editora

#### Reader

- `GET /readers`: Lista todos os leitores
- `GET /readers/{id}`: Retorna um leitor
- `POST /readers`: Cadastra um novo leitor
- `PUT /readers`: Edita um leitor
- `DELETE /readers/{id}`: Remove um leitor
- `POST /readers/{readerId}/books/{bookId}`: Associa um livro a um leitor
- `GET /readers/{readerId}/books`: Lista livros lidos por um leitor

## Aplicação Console

O projeto `LibraryManager.Console` permite:

- Registrar livros com gênero e editora
- Cadastrar leitores
- Associar livros a leitores
- Listar livros, leitores e suas relações
- Visualizar livros por gênero

O menu principal é exibido no terminal com opções numéricas.

## Banco de Dados

- A base `LibraryManager_DB` é gerenciada por Migrations.
- O contexto está configurado com `UseLazyLoadingProxies` para navegação de propriedades relacionadas.
- Há uma migration `DataEntry` que popula as tabelas `Genres`, `Publishers`, `Books` e `Readers`, incluindo registros de associação N:N em `BookReader`.

### Como aplicar as Migrations

1. Defina `LibraryManager.Shared.Data` como projeto padrão da `Package Manager Console`.
2. Execute os comandos:

```bash
Add-Migration NomeDaMigration
Update-Database
```

## Proxies e Lazy Loading

- As entidades possuem propriedades `virtual` para habilitar carregamento automático (lazy loading).
- O contexto foi configurado com `UseLazyLoadingProxies()` no método `OnConfiguring`.

## DTOs (Data Transfer Objects)

Foram criados DTOs separados para `Request` e `Response` nos endpoints, como:

- `BookRequest`, `BookEditRequest`, `BookResponse`
- `GenreRequest`, `GenreEditRequest`, `GenreResponse`
- `ReaderRequest`, `ReaderEditRequest`, `ReaderResponse`
- `PublisherRequest`, `PublisherEditRequest`, `PublisherResponse`

## Autenticação

O projeto possui autenticação completa utilizando `ASP.NET Core Identity`.

- A base suporta autenticação com `AccessUser` e `AccessRole`, derivados de `IdentityUser` e `IdentityRole`.
- A autenticação é obrigatória para consumir qualquer endpoint da API.
- O sistema utiliza `IdentityDbContext`, pronto para expandir com políticas de autorização.

### Funcionalidades

- Registro de usuários
- Login com geração de token JWT
- Logout
- Proteção com `[Authorize]` em todos os grupos de endpoints

### Como usar a autenticação

1. Registrar usuário:
   - Enviar `POST` para `/auth/register` com email e password.

2. Fazer login:
   - Enviar `POST` para `/auth/login` com email e password.
   - A resposta retorna um token JWT.

3. Autenticar chamadas:
   - Incluir o token JWT no cabeçalho das requisições:

```
Authorization: Bearer {seu_token}
```

## Considerações

- O projeto aplica boas práticas com separação em camadas, uso de injeção de dependência (DI), configuração via appsettings e endpoints organizados.
- Relações do tipo 1:N e N:N foram implementadas de forma correta e testada com Migrations e carregamento via EF Core.

## Autor

Projeto desenvolvido por Arthur Soares Amorelli Dias.
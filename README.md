# 🛡️ SafeScribe - Gestão Segura de Notas e Documentos  
### >>> PROTEJA | AUTENTIQUE | AUTORIZE <<<
 
O **SafeScribe** é uma API RESTful desenvolvida em **.NET 8**, com foco em **autenticação e autorização via JWT (JSON Web Token)**.  
A aplicação permite o **registro, login e gerenciamento de notas** de forma segura, aplicando **controle de acesso baseado em funções (roles)**: **Leitor**, **Editor** e **Admin**.
 
---
 
## 📌 Índice
- 🚀 Funcionalidades  
- 💻 Tecnologias  
- 📋 Pré-requisitos  
- 🔧 Instalação  
- 🏃 Execução  
- 📘 Documentação da API  
- 🔗 Endpoints  
- 🗂 Estrutura do Projeto  
- 🚧 Status da Aplicação  
- 👥 Autores  
 
---
 
## 🚀 Funcionalidades
 
### 🔐 Autenticação e Autorização JWT
- Registro e login de usuários com **hash seguro de senha (BCrypt)**.  
- Geração de tokens JWT contendo claims (UserId, Role, Jti).  
- Controle de acesso por função:  
  - **Leitor:** visualiza apenas suas notas.  
  - **Editor:** cria e edita suas próprias notas.  
  - **Admin:** controla todas as notas e usuários.
 
### 📝 Gerenciamento de Notas
- Criação, leitura, atualização e exclusão de notas.  
- Segurança aplicada a cada operação conforme o **perfil do usuário**.
 
### 🚫 Logout com Blacklist de Tokens
- Implementação de **logout seguro** via **blacklist de tokens JWT**, impedindo reutilização após o logout.
 
---
 
## 💻 Tecnologias
- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core (InMemory)**
- **JWT (Microsoft.AspNetCore.Authentication.JwtBearer)**
- **Swagger (OpenAPI)**
- **BCrypt.Net-Next**
- IDE: Visual Studio / Visual Studio Code
 
---
 
## 📋 Pré-requisitos
- .NET 8 SDK instalado  
- Editor de código (VS Code, Visual Studio, etc.)
 
---
 
## 🔧 Instalação
 
Clone o repositório:
 
```bash
git clone https://github.com/lerri05/SafeScribeAPIcp5.git
cd SafeScribeAPI

Configure o arquivo appsettings.json com sua chave JWT:
"Jwt": {

  "Key": "sua_chave_super_secreta",

  "Issuer": "SafeScribe.Api",

  "Audience": "SafeScribe.Clients",

  "ExpireMinutes": 60

}

 
🏃 Execução
 
 
dotnet run

 
Acesse o Swagger para testar os endpoints:

👉 https://localhost:5001/swagger
 
📘 Documentação da API
A API possui interface interativa via Swagger para testar endpoints e verificar o fluxo de autenticação JWT.
 
🔗 Endpoints Principais
👤 Autenticação - /api/v1/auth
Método	Endpoint	Descrição
POST	/registrar	Registra um novo usuário
POST	/login	Realiza login e retorna um token JWT
POST	/logout	Invalida o token JWT ativo (blacklist)
📝 Notas - /api/v1/notas
 
Método	Endpoint	Descrição	Permissão
POST	/api/v1/notas	Cria uma nova nota	Editor, Admin
GET	/api/v1/notas/{id}	Retorna uma nota específica	Leitor, Editor (própria) / Admin (qualquer)
PUT	/api/v1/notas/{id}	Atualiza uma nota	Editor (própria) / Admin
DELETE	/api/v1/notas/{id}	Remove uma nota	Admin
 
 
🗂 Estrutura do Projeto
SafeScribeAPI
├── Controllers
│   ├── AuthController.cs
│   └── NotesController.cs
├── Data
│   ├── ApplicationDbContext.cs
│   └── DbInitializer.cs
├── DTOs
│   ├── UserRegisterDto.cs
│   ├── LoginRequestDto.cs
│   ├── NoteCreateDto.cs
│   └── NoteDto.cs
├── Middleware
│   └── JwtBlacklistMiddleware.cs
├── Models
│   ├── User.cs
│   ├── Note.cs
│   └── UserRoles.cs
├── Services
│   ├── ITokenService.cs
│   ├── TokenService.cs
│   ├── ITokenBlacklistService.cs
│   └── InMemoryTokenBlacklistService.cs
├── Helpers
│   └── PasswordHasher.cs
├── appsettings.json
└── Program.cs
 
 
🚧 Status da Aplicação
✅ Aplicação desenvolvida
 
👥 Autores

Nome	RM	GitHub
Lucas Lerri de Almeida	554635	@lerri05
Fernanda Budniak de Seda	558274	@Febudniak
Karen Marques dos Santos	554556	@KarenMarquesS
 
lerri05 - Overview
lerri05 has 4 repositories available. Follow their code on GitHub.
 

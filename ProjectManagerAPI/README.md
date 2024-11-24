# C# Minimal API ASP.NET

## Description

A minimal API built with C# and ASP.NET for [brief description of the API's purpose].

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/downloads)

## Installation

1. **Clone the repository**

   ```bash
   git clone git@github.com:Acauhi99/full-stack-project-maneger.git
   cd ProjectManagerAPI
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

## Running the Project

To run the project with hot reload:

```bash
dotnet watch run
```

## Project Manager API Documentation

### Projects

GET /projects
Authorization: Required (Any Role)

Response 200 OK:
[
{
"id": "guid",
"nome": "string",
"descricao": "string"
}
]

Response 400 Bad Request:
{
"message": "Erro ao acessar o banco de dados."
}

Response 401 Unauthorized:
{
"message": "Não autorizado"
}

GET /projects/{id}
Authorization: Required (Any Role)

Parameters:

- id: GUID (Required)

Response 200 OK:
{
"id": "guid",
"nome": "string",
"descricao": "string"
}

Response 404 Not Found:
{
"message": "Projeto não encontrado."
}

POST /projects
Authorization: Required (Admin)

Request Body:
{
"nome": "string (Required, max 100 chars)",
"descricao": "string (Required, max 500 chars)"
}

Response 201 Created:
{
"id": "guid",
"nome": "string",
"descricao": "string"
}

Response 400 Bad Request:
{
"message": "Dados do projeto são inválidos."
}

PUT /projects/{id}
Authorization: Required (Admin)

Parameters:

- id: GUID (Required)

Request Body:
{
"nome": "string (Optional, max 100 chars)",
"descricao": "string (Optional, max 500 chars)"
}

Response 200 OK:
{
"nome": "string",
"descricao": "string"
}

Response 404 Not Found:
{
"message": "Projeto não encontrado."
}

DELETE /projects/{id}
Authorization: Required (Admin)

Parameters:

- id: GUID (Required)

Response 204 No Content:
(empty response on success)

Response 404 Not Found:
{
"message": "Projeto não encontrado."
}

Response 400 Bad Request:
{
"message": "Não é possível excluir um projeto com tarefas ativas."
}

GET /projects/reports
Authorization: Required (Admin)

Response 200 OK:
[
{
"projectId": "guid",
"projectName": "string",
"taskCount": integer
}
]

Response 400 Bad Request:
{
"message": "Erro ao acessar o banco de dados."
}

### Tasks

GET /tasks
Authorization: Required (Admin)

Response 200 OK:
[
{
"id": "guid",
"titulo": "string",
"descricao": "string",
"concluida": boolean,
"projetoId": "guid",
"usuarioId": "guid"
}
]

Response 400 Bad Request:
{
"message": "Erro ao acessar o banco de dados."
}

GET /tasks/{id}
Authorization: Required (Admin)

Parameters:

- id: GUID (Required)

Response 200 OK:
{
"id": "guid",
"titulo": "string",
"descricao": "string",
"concluida": boolean,
"projetoId": "guid",
"usuarioId": "guid"
}

Response 404 Not Found:
{
"message": "Tarefa não encontrada."
}

POST /tasks
Authorization: Required (Admin)

Request Body:
{
"titulo": "string (Required, max 100 chars)",
"descricao": "string (Required, max 500 chars)",
"projetoId": "guid (Required)",
"usuarioId": "guid (Required)"
}

Response 201 Created:
{
"id": "guid",
"titulo": "string",
"descricao": "string",
"concluida": false,
"projetoId": "guid",
"usuarioId": "guid"
}

Response 400 Bad Request:
{
"message": "Dados da tarefa são inválidos."
}
OR
{
"message": "Projeto não encontrado."
}
OR
{
"message": "Usuário não encontrado."
}

PUT /tasks/{id}
Authorization: Required (Admin)

Parameters:

- id: GUID (Required)

Request Body:
{
"titulo": "string (Optional, max 100 chars)",
"descricao": "string (Optional, max 500 chars)",
"concluida": boolean (Optional),
"projetoId": "guid (Optional)",
"usuarioId": "guid (Optional)"
}

Response 200 OK:
{
"id": "guid",
"titulo": "string",
"descricao": "string",
"concluida": boolean,
"projetoId": "guid",
"usuarioId": "guid"
}

Response 404 Not Found:
{
"message": "Tarefa não encontrada."
}

Response 400 Bad Request:
{
"message": "Projeto não encontrado."
}
OR
{
"message": "Usuário não encontrado."
}

DELETE /tasks/{id}
Authorization: Required (Admin)

Parameters:

- id: GUID (Required)

Response 204 No Content:
(empty response on success)

Response 404 Not Found:
{
"message": "Tarefa não encontrada."
}

GET /tasks/user
Authorization: Required (Any Role)

Response 200 OK:
[
{
"id": "guid",
"titulo": "string",
"descricao": "string",
"concluida": boolean,
"projetoId": "guid",
"usuarioId": "guid"
}
]

Response 401 Unauthorized:
{
"message": "Não autorizado"
}

PUT /tasks/user/complete
Authorization: Required (Regular)

Request Body:
{
"taskId": "guid (Required)"
}

Response 204 No Content:
(empty response on success)

Response 400 Bad Request:
{
"message": "Não foi possível marcar a tarefa como concluída. Verifique se a tarefa existe e pertence a você."
}

### Authentication

POST /users/register
Authorization: Not Required

Request Body:
{
"nome": "string (Required, max 100 chars)",
"email": "string (Required, valid email, max 100 chars)",
"senha": "string (Required, min 6 chars)",
"tipoUsuario": "integer (0 = Admin, 1 = Regular)"
}

Response 201 Created:
{
"message": "Usuário registrado com sucesso",
"user": {
"id": "guid",
"nome": "string",
"email": "string",
"tipoUsuario": integer
}
}

Response 400 Bad Request:
{
"message": "Email já cadastrado no sistema"
}
OR
{
"message": "Email e senha são obrigatórios"
}

POST /users/login
Authorization: Not Required

Request Body:
{
"email": "string (Required, valid email)",
"senha": "string (Required, min 6 chars)"
}

Response 200 OK:
{
"message": "Login realizado com sucesso",
"token": "string (JWT Token)"
}

Response 422 Unprocessable Entity:
{
"message": "Email ou senha incorretos"
}
OR
{
"message": "Email e senha são obrigatórios"
}

Response 400 Bad Request:
{
"message": "Erro ao acessar banco de dados"
}

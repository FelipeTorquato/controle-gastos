# API de Controle de Gastos Familiares

Minimal API em .NET 8 para gerenciamento de finanças familiares, com controle de usuários e transações financeiras.

## Funcionalidades

### Gestão de Usuários

- `GET /user` - Lista todos os usuários (com paginação)
- `POST /user` - Cadastra um novo usuário
- `GET /user/{id}` - Obtem um usuário específico
- `DELETE /user/{id}` - Deleta um usuário específico
- `GET /user/summary` - Relatório financeiro familiar

### Gestão de Transações

- `GET /transaction` - Lista todas as transações (com paginação)
- `POST /transaction` - Cadastra uma nova transação
- `GET /transaction/{id}` - Obtem uma transação específica

## Tecnologias Utilizadas

- .NET 8
- Entity Framework Core
- SQLite

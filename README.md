
# Projeto Gestão de Oficinas (Repositório: OficinaIntegracao2)

Esta é a API backend para o sistema de **Controle de Oficinas e Escolas Participantes** (Projeto GestaoOficinas). O sistema visa gerenciar o cadastro de escolas, professores, alunos, oficinas e a emissão de documentos (certificados e convites).

O projeto utiliza **.NET 8** e segue os princípios da **Arquitetura Limpa (Clean Architecture)** para garantir separação de responsabilidades, testabilidade e manutenibilidade.

## Arquitetura do Projeto

A solução (`GestaoOficinas.sln`) está organizada em quatro projetos principais, seguindo a Arquitetura Limpa:

1.  **`GestaoOficinas.Domain`**: O núcleo do sistema. Contém as entidades de negócio puras (ex: `Escola`, `Aluno`, `Oficina`) e as interfaces dos repositórios (ex: `IEscolaRepository`).
2.  **`GestaoOficinas.Application`**: Contém a lógica de negócio (serviços), DTOs (Data Transfer Objects), validações e as interfaces dos serviços.
3.  **`GestaoOficinas.Infrastructure`**: Implementa o acesso a dados. Contém o `ApplicationDbContext` (Entity Framework Core), os repositórios e a pasta `Migrations`.
4.  **`GestaoOficinas.API`**: A camada de apresentação. Expõe os *endpoints* RESTful, contém os `Controllers` e lida com autenticação (JWT) e configuração (injeção de dependência).
5.  **`GestaoOficinas.API.Tests`**: Projeto de Testes de Integração, usando xUnit e um banco de dados em memória.

---

## Guia de Instalação e Execução

Siga estes passos para configurar e executar o projeto localmente.

### Passo 1: Instalar o Ambiente de Desenvolvimento

Certifique-se de ter as seguintes ferramentas instaladas:

* **[.NET 8 SDK](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)**
* **Visual Studio 2022** (ou sua IDE .NET de preferência)
* **PostgreSQL** (ou o SGBD de sua escolha, como SQL Server)
    * *O projeto está configurado para usar PostgreSQL.*

### Passo 2: Configurar o Banco de Dados

1.  Crie um banco de dados vazio no seu servidor PostgreSQL (ex: `gestao_oficinas_db`).
2.  Abra o arquivo `appsettings.json` no projeto **`GestaoOficinas.API`**.
3.  Ajuste a `ConnectionStrings` para apontar para o seu banco de dados local. **(Nota: Use o Gerenciador de Segredos do Usuário para sua senha!)**

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Port=5432;Database=gestao_oficinas_db;User Id=postgres;Password="
      },
      // ...
    }
    ```

### Passo 3: Configurar o Projeto e as Dependências

1.  Abra a solução **`GestaoOficinas.sln`** no Visual Studio 2022.
2.  Clique com o botão direito na Solução (no Gerenciador de Soluções) e selecione **"Restaurar Pacotes NuGet"** (Restore NuGet Packages).

### Passo 4: Executar as Migrações do Banco de Dados

1.  No Visual Studio, vá em **Exibir > Outras Janelas > Console do Gerenciador de Pacotes**.
2.  No console, certifique-se de que o **"Projeto Padrão"** (Default project) esteja selecionado como **`GestaoOficinas.Infrastructure`**.
3.  Execute o primeiro comando para criar o arquivo de migração (se ainda não existir):
    ```powershell
    Add-Migration InitialCreate -StartupProject GestaoOficinas.API
    ```
4.  Execute o segundo comando para aplicar a migração ao banco de dados:
    ```powershell
    Update-Database -StartupProject GestaoOficinas.API
    ``` 

### Passo 5: Executar a API

1.  No Gerenciador de Soluções, clique com o botão direito no projeto GestaoOficinas.API.
2.  Selecione "Definir como Projeto de Inicialização" (Set as Startup Project).
3.  Pressione F5 ou clique no botão de "Play" (com o perfil `https:`) para iniciar a aplicação.
4.  O navegador será aberto com a interface do Swagger (/swagger), onde você pode testar todos os *endpoints* da API.

## Requisitos do Projeto

O sistema foi solicitado para gerenciar dois módulos principais:

### 1. Controle de Oficinas
* Cadastro e gerenciamento de professores, tutores, alunos, temas das oficinas e certificados.
* Geração de certificados como comprovante de conclusão da oficina.
* CRUD completo (Criar, Ler, Atualizar, Excluir) para todas as entidades.

### 2. Controle de Escolas Participantes
* Cadastro de escolas, com informações sobre o representante responsável e as turmas envolvidas.
* Emissão de carta convite ou convênio para participação.
* Controle dos alunos atendidos por cada escola.

## Modelo de Dados (Entidades)

O banco de dados foi modelado com base nas seguintes entidades:

* Escola: Armazena dados das escolas participantes.
* Professor: Gerencia professores e representantes.
* Aluno: Gerencia os alunos inscritos.
* Oficina: Entidade central que define a oficina.
* Turma: Agrupamento de alunos para uma oficina específica.
* Inscricao: Tabela de relacionamento (M:N) entre Aluno e Turma, com status.
* Chamada: Registro de datas de aula para controle de presença.
* Presenca: Tabela de relacionamento (M:N) entre Aluno e Chamada.
* Documento: Armazena certificados e cartas de convite/convênio.

## Objetivos da Sprint 1

A Sprint 1 foca na fundação técnica e no CRUD básico do projeto.

- [x] Implementação da arquitetura e execução do projeto na IDE.
- [x] Download dos pacotes (NuGet) a serem implementados.
- [x] Criação e relacionamento de banco de dados (EF Core Migrations).
- [x] Criação das classes e domínios na IDE.
- [x] Criação da API a ser utilizada, com nomeação das chamadas (Swagger).
- [x] Configuração da autorização (JWT).
- [x] Criação das Interfaces (Repositórios e Serviços) para todas as entidades.
- [x] Implementação dos Repositórios e Serviços para todas as entidades.
- [x] Registro de todas as dependências (Injeção de Dependência) no Program.cs
- [x] Criação das *controllers* e funções (CRUD) para *todas* as entidades (Professor, Aluno, Oficina, Turma, Inscricao, Chamada, Documento, Presenca).
- [x] Review de backlog e correção de problemas.
- [x] Definição da view de Dashboard/página inicial (criação do endpoint GET /api/dashboard`).
- [x] **Criar Testes de Integração** (Projeto `.Tests`) com exemplos para todas as entidades.
- [ ] Entrega da primeira sprint.
- [ ] Review da sprint.
- [ ] Atualização de cronograma.

---
### Próximos Passos (Planejamento Sprint 2)

O que falta implementar no projeto:

- [ ] **Lógica de Negócio Avançada:**
    - [ ] Geração do documento de `Certificado` (requer lógica de presença/conclusão).
    - [ ] Geração do documento de `Convite` (para escolas).
- [ ] **Validação de DTOs** (Implementar FluentValidation).
- [ ] **Tratamento de Erros Global** (Middleware para exceções).
- [ ] **Melhorar Testes de Integração** (Cobrir casos de falha, `PUT` e `DELETE`).
- [ ] **Definição das Views** (Prototipação do Frontend).
- [ ] Definição das views de cadastros (planejamento da Sprint 2).

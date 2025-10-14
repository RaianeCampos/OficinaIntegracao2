Quadro Kanban Trello:
https://trello.com/invite/b/68c0be63bea36e3279739379/ATTI884446f96e983c9255ea8602bfa4a0d040AF9B36/kanban-projeto-oi2

_**Implantação**_

**Passo 1: Instalar o Ambiente de Desenvolvimento **

Você precisará das ferramentas básicas para desenvolver e executar o projeto ASP.NET Core em C#. 
Instale o .NET SDK (versão 7.0 ou superior):  
Baixe o SDK do .NET 7.0 (ou a versão que você usou para o projeto) no site oficial da Microsoft. 
Instalar o SDK permite que você use a CLI do .NET (dotnet). 

Instale o Visual Studio (Community, Professional ou Enterprise):  
Baixe e instale o Visual Studio 2022. 
Durante a instalação, certifique-se de selecionar a carga de trabalho "ASP.NET e desenvolvimento web" e a carga de trabalho "Desenvolvimento para desktop com .NET". 

Instale o PostgreSQL:  
Baixe e instale o PostgreSQL no site oficial. 
Durante a instalação, o instalador também pode instalar o pgAdmin, uma ferramenta de interface gráfica para gerenciar seu banco de dados. 

**Passo 2: Configurar o Banco de Dados **

Crie o Banco de Dados:  
Abra o pgAdmin. 
Conecte-se ao seu servidor PostgreSQL (geralmente localhost). 
Clique com o botão direito em "Databases" e selecione "Create" -> "Database". 
Dê um nome ao seu banco de dados, por exemplo, EducaPrismaDb. 
Obtenha a String de Conexão:  
Sua string de conexão será semelhante a: Host=localhost;Port=5432;Database=EducaPrismaDb;Username=seu_usuario;Password=sua_senha 
Anote essa string. 

**Passo 3: Configurar o Projeto e as Dependências **

Clone ou Baixe o Código:  
Obtenha os arquivos do projeto (você já os tem). 
Configure a String de Conexão no appsettings.json:  
Abra o arquivo EducaPrisma/appsettings.json. 
Localize a seção ConnectionStrings e adicione a string de conexão que você obteve no Passo 2.  

JSON 
{ 
  "ConnectionStrings":  
{ 
"DefaultConnection":"Host=localhost;Port=5432;Database=EducaPrismaDb;Username=seu_usuario;Password=sua_senha" 
}, 
} 
 
Verifique as Referências do Projeto (.csproj):  
Abra a solução (.sln) no Visual Studio. 
Verifique se as referências entre os projetos (EducaPrisma referenciando EducaPrisma.Domain) estão corretas. 

Restaure os Pacotes NuGet:  
No Visual Studio, no "Gerenciador de Soluções", clique com o botão direito do mouse na sua solução. 
Selecione "Restaurar Pacotes NuGet". 
Alternativamente, abra um terminal na pasta da solução e execute dotnet restore. 
Isso baixará todos os pacotes .NET (Microsoft.EntityFrameworkCore.PostgreSQL, QuestPDF, Serilog, etc.). 

Restaure as Bibliotecas do Lado do Cliente (LibMan):  
Se você não tem os arquivos em wwwroot/lib e tem um libman.json no projeto EducaPrisma, clique com o botão direito em libman.json no "Gerenciador de Soluções" e selecione "Restaurar Bibliotecas do Lado do Cliente". Isso baixará jQuery, Bootstrap, etc. para a pasta wwwroot/lib. 

**Passo 4: Executar as Migrações do Banco de Dados  **

Este é um passo crucial para criar as tabelas no seu banco de dados vazio. 
Abra o Package Manager Console (PMC) no Visual Studio ("Ferramentas" -> "Gerenciador de Pacotes NuGet" -> "Console do Gerenciador de Pacotes"). 
Selecione o projeto EducaPrisma no dropdown "Projeto padrão". 
Remova as Migrações Antigas (Limpeza Completa):  
No seu terminal/PMC, navegue até a pasta do projeto principal (EducaPrisma). 
Exclua fisicamente a pasta Migrations/ e o arquivo ApplicationDbContextModelSnapshot.cs dentro dela. 
Execute dotnet ef database drop --project .\EducaPrisma\EducaPrisma.csproj --force (se precisar dropar o banco novamente). 

Crie a Migração Inicial:  

Execute o comando para criar uma nova migração do zero. Esta migração deve conter a criação de todas as tabelas.  
dotnet ef migrations add InitialMigration --project .\EducaPrisma\EducaPrisma.csproj
Verifique o arquivo .cs da migração gerada na pasta Migrations/ para garantir que ele contém CreateTable para todas as suas entidades (Usuarios, Turmas, Disciplinas, etc.) na ordem correta.

Aplique a Migração ao Banco de Dados:  
Execute o comando para aplicar a miggação ao banco de dados vazio.  

	dotnet ef database update --project .\EducaPrisma\EducaPrisma.csproj 

Verifique o terminal para garantir que não há erros. Se houver, a causa é um problema de mapeamento no ApplicationDbContext.cs que precisa ser resolvido. 

**Passo 5: Configurar o Google OAuth (se aplicável) **

Obtenha ClientId e ClientSecret do Google:  
Vá para o Google Cloud Console. 
Crie um projeto ou selecione um existente. 
Vá para APIs & Services -> Credentials. 
Crie um OAuth 2.0 Client ID do tipo "Web application". 
Em "Authorized redirect URIs", adicione https://localhost:7275/signin-google (use a URL exata do seu projeto, incluindo a porta e o protocolo HTTPS). 
Anote o Client ID e o Client Secret gerados. 

Adicione as Credenciais ao appsettings.json:  
Adicione as chaves no seu arquivo appsettings.json (ou appsettings.Development.json):  

JSON 
"Authentication": { 
  "Google": { 
    "ClientId": "SEU_CLIENT_ID_AQUI", 
    "ClientSecret": "SEU_CLIENT_SECRET_AQUI" 
  } 
} 
 

**Passo 6: Executar o Projeto **

No Visual Studio, pressione F5 ou clique no botão "Executar" para iniciar a aplicação no modo de depuração. 
A aplicação será iniciada e o navegador abrirá na página de login. 

Teste o login com o Google e o login manual com e-mail/senha. 

Crie os primeiros usuários (Coordenador, Professor, Aluno) via o banco de dados ou um formulário de cadastro, se houver. 

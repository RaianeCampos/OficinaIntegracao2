# Projeto de Testes de Integração (GestaoOficinas.API.Tests)

Este projeto é responsável por executar testes de integração automatizados contra a GestaoOficinas.API.

O objetivo dos testes de integração não é testar *unidades* isoladas (como um único método), mas sim testar o *fluxo completo* da aplicação, desde o Controller até o banco de dados, como se um usuário real estivesse fazendo uma requisição HTTP.

## 🚀 Ferramentas Utilizadas

* xUnit: O framework de testes (usado para [Fact], IClassFixture, etc.).
* Microsoft.AspNetCore.Mvc.Testing: O pacote principal que nos permite "hospedar" a API em memória (WebApplicationFactory).
* FluentAssertions: Usado para validações (Asserts) mais legíveis (ex: resultado.Should().Be(5)).
* Microsoft.EntityFrameworkCore.InMemory: Usado para substituir o banco de dados PostgreSQL por um banco de dados em memória, garantindo que os testes sejam rápidos e não afetem o banco de dados real.

## 💡 Como Funciona?

O núcleo deste projeto são dois arquivos de setup:

1.  * CustomWebApplicationFactory.cs:
    * Esta classe inicia sua API GestaoOficinas.API inteira na memória.
    * Substituição do Banco de Dados: Ela intercepta a configuração de serviços e remove o DbContext do PostgreSQL (UseNpgsql) e o substitui por um DbContext em memória (UseInMemoryDatabase). Isso garante que cada execução de teste comece com um banco de dados limpo e rápido.
    * Substituição da Autenticação: Ela também substitui o sistema de autenticação JWT real por um "handler falso" (FakeAuthHandler).

2.  * FakeAuthHandler.cs:
    * Este é um "dublê" (test double) para a autenticação.
    * Ele intercepta qualquer requisição que precise de [Authorize] e automaticamente a aprova, "fingindo" que o usuário está logado com a role "Admin".
    * É por isso que nos testes não precisamos de um token JWT.

## 🏃 Como Rodar os Testes

1.  Abra o Gerenciador de Testes (Test Explorer) no Visual Studio.
2.  Vá em Exibir > Gerenciador de Testes (ou Ctrl + E, T).
3.  O Visual Studio irá descobrir todos os testes (métodos marcados com [Fact]) automaticamente.
4.  Você pode clicar em "Executar Todos os Testes" (Run All Tests) para rodar a suíte inteira, ou clicar com o botão direito em um teste específico para executá-lo individualmente.

## ✍️ Como Escrever um Novo Teste

Para criar um novo teste (ex: GetById para `Aluno`):

1.  Abra (ou crie) a classe de teste correspondente (ex: AlunosControllerTests.cs).
2.  Garanta que a classe implemente IClassFixture<CustomWebApplicationFactory<Program>> e tenha o construtor padrão para injetar o _client e o _factory.
3.  Crie um novo método público e marque-o com [Fact]:
    
    [Fact]
    public async Task GetById_RetornaAluno_QuandoAlunoExiste()
    {
        // ...
    }
    
4.  * Arrange (Preparar): Prepare o "cenário". Use o _factory para acessar o DbContext em memória e inserir os dados que você espera que existam.
    * Importante: Ao popular dados com chaves estrangeiras, lembre-se de popular também as entidades "pai" (ex: para testar um Aluno, você precisa criar uma Turma, Oficina, Professor e Escola primeiro).
    * Dica: Lembre-se de atribuir tanto o *ID da chave estrangeira* (IdTurma = 1) quanto a propriedade de navegação (Turma = minhaTurma) para que o AutoMapper funcione corretamente com o InMemoryDatabase.

    // Arrange
    await using var scope = _factory.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // ... criar entidades pai (turma, etc.) ...
    var turma = new Turma { IdTurma = 1, ... };
    var aluno = new Aluno { IdAluno = 1, NomeAluno = "Aluno de Teste", IdTurma = 1, Turma = turma };
    
    await context.Turmas.AddAsync(turma);
    await context.Alunos.AddAsync(aluno);
    await context.SaveChangesAsync();
   

5.  * Act (Agir): Use o _client para fazer a requisição HTTP real ao seu endpoint.
    
    // Act
    var response = await _client.GetAsync("/api/alunos/1");
    

6.  * Assert (Verificar): Use FluentAssertions para verificar se o resultado foi o esperado.
    
    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var viewModel = await response.Content.ReadFromJsonAsync<AlunoViewModel>();
    viewModel.NomeAluno.Should().Be("Aluno de Teste");
    
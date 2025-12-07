using Microsoft.AspNetCore.Mvc;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Web.Models; 
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Rendering; 
using System.Text.Json;

namespace GestaoOficinas.Web.Controllers
{
    public class AlunosController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public AlunosController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
       
        private HttpClient CreateClientWithToken()
        {
            var client = _clientFactory.CreateClient("OficinasAPI");
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        // Método auxiliar para carregar Turmas no ViewBag (para os Dropdowns)
        private async Task CarregarTurmasViewBag()
        {
            // 1. Inicializa com lista vazia por padrão (Evita o NullReferenceException)
            ViewBag.ListaTurmas = new SelectList(new List<TurmaViewModel>(), "IdTurma", "NomeTurma");

            var client = CreateClientWithToken();
            try
            {
                var response = await client.GetAsync("api/turmas");

                if (response.IsSuccessStatusCode)
                {
                    // 2. Tenta obter as turmas da API
                    // Importante: Use TurmaViewModel em vez de dynamic para garantir os nomes das propriedades
                    var turmas = await response.Content.ReadFromJsonAsync<List<TurmaViewModel>>();

                    // 3. Atualiza a ViewBag se houver dados
                    if (turmas != null && turmas.Any())
                    {
                        ViewBag.ListaTurmas = new SelectList(turmas, "IdTurma", "NomeTurma");
                    }
                }
                else
                {
                    // Opcional: Logar que a API não retornou sucesso (ex: 404 Not Found)
                    // A lista continua vazia conforme inicializado na linha 1
                }
            }
            catch (Exception)
            {
                // Em caso de erro de conexão, a lista permanece vazia (evitando o crash da tela)
            }
        }

        public async Task<IActionResult> Index(string busca, int pagina = 1)
        {
            var client = CreateClientWithToken();
            try
            {
                var response = await client.GetAsync("api/alunos");
                if (response.IsSuccessStatusCode)
                {
                    var todosAlunos = await response.Content.ReadFromJsonAsync<List<AlunoViewModel>>();

                    if (!string.IsNullOrEmpty(busca))
                    {
                        todosAlunos = todosAlunos.Where(a =>
                            (a.NomeAluno ?? "").Contains(busca, StringComparison.OrdinalIgnoreCase) ||
                            (a.RaAluno ?? "").Contains(busca, StringComparison.OrdinalIgnoreCase)
                        ).ToList();
                    }

                    int tamanhoPagina = 10;
                    var count = todosAlunos.Count;
                    var items = todosAlunos.Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToList();

                    var model = new AlunoListViewModel
                    {
                        Alunos = items,
                        TermoBusca = busca,
                        PaginaAtual = pagina,
                        TotalPaginas = (int)Math.Ceiling(count / (double)tamanhoPagina)
                    };
                    return View(model);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login", "Auth");
                }
            }
            catch { }

            return View(new AlunoListViewModel());
        }

        public async Task<IActionResult> Create()
        {
            await CarregarTurmasViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAlunoDto aluno)
        {
            if (!ModelState.IsValid)
            {
                await CarregarTurmasViewBag();
                return View(aluno);
            }

            var client = CreateClientWithToken();
            var response = await client.PostAsJsonAsync("api/alunos", aluno);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            await CarregarTurmasViewBag();
            ModelState.AddModelError("", "Erro ao cadastrar aluno.");
            return View(aluno);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync($"api/alunos/{id}");

            if (response.IsSuccessStatusCode)
            {
                var viewModel = await response.Content.ReadFromJsonAsync<AlunoViewModel>();

                var updateDto = new UpdateAlunoDto
                {
                    NomeAluno = viewModel.NomeAluno,
                    EmailAluno = viewModel.EmailAluno,
                    RaAluno = viewModel.RaAluno,
                    IdTurma = viewModel.IdTurma,

                    TelefoneAluno = viewModel.TelefoneAluno,
                    NascimentoAluno = viewModel.NascimentoAluno
                };

                await CarregarTurmasViewBag();
                ViewBag.IdAluno = id;

                return View(updateDto);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateAlunoDto aluno)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ERRO DE VALIDAÇÃO: {error.ErrorMessage}");
                }

                await CarregarTurmasViewBag();
                ViewBag.IdAluno = id;
                return View(aluno);
            }

            var client = CreateClientWithToken();
            var response = await client.PutAsJsonAsync($"api/alunos/{id}", aluno);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            await CarregarTurmasViewBag();
            ViewBag.IdAluno = id;
            ModelState.AddModelError("", "Erro ao atualizar aluno na API.");
            return View(aluno);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = CreateClientWithToken();
            await client.DeleteAsync($"api/alunos/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
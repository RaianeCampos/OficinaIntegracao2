using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestaoOficinas.Application.DTOs;
using System.Net.Http.Headers;
using GestaoOficinas.Web.Models;
using System.Text.Json;

namespace GestaoOficinas.Web.Controllers
{
    public class ProfessoresController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProfessoresController(IHttpClientFactory clientFactory)
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

        private async Task CarregarEscolasViewBag()
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync("api/escolas");

            if (response.IsSuccessStatusCode)
            {
                var escolas = await response.Content.ReadFromJsonAsync<List<EscolaViewModel>>();
                ViewBag.ListaEscolas = new SelectList(escolas, "IdEscola", "NomeEscola");
            }
            else
            {
                ViewBag.ListaEscolas = new SelectList(new List<EscolaViewModel>(), "IdEscola", "NomeEscola");
            }
        }

        public async Task<IActionResult> Index(string busca, int pagina = 1)
        {
            var client = CreateClientWithToken();
            try
            {
                var response = await client.GetAsync("api/professores");
                if (response.IsSuccessStatusCode)
                {
                    var todosProfessores = await response.Content.ReadFromJsonAsync<List<ProfessorViewModel>>();

                    if (!string.IsNullOrEmpty(busca))
                    {
                        todosProfessores = todosProfessores.Where(p =>
                            (p.NomeProfessor ?? "").Contains(busca, StringComparison.OrdinalIgnoreCase) ||
                            (p.EmailProfessor ?? "").Contains(busca, StringComparison.OrdinalIgnoreCase)
                        ).ToList();
                    }

                    int tamanhoPagina = 10;
                    var count = todosProfessores.Count;
                    var items = todosProfessores.Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToList();

                    var model = new ProfessorListViewModel
                    {
                        Professores = items,
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
            catch
            {

            }

            return View(new ProfessorListViewModel());
        }

        public async Task<IActionResult> Create()
        {
            await CarregarEscolasViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProfessorDto professor)
        {
            if (!ModelState.IsValid)
            {
                await CarregarEscolasViewBag();

                var erros = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var erro in erros)
                {
                    Console.WriteLine($"ERRO DE VALIDAÇÃO: {erro.ErrorMessage} - Exception: {erro.Exception}");
                }

                return View(professor);
            }

            var client = CreateClientWithToken();
            var response = await client.PostAsJsonAsync("api/professores", professor);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            await CarregarEscolasViewBag();
            ModelState.AddModelError("", "Erro ao cadastrar professor. Verifique os dados.");
            return View(professor);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync($"api/professores/{id}");

            if (response.IsSuccessStatusCode)
            {
                var viewModel = await response.Content.ReadFromJsonAsync<ProfessorViewModel>();

                var updateDto = new UpdateProfessorDto
                {
                    IdProfessor = viewModel.IdProfessor,
                    NomeProfessor = viewModel.NomeProfessor,
                    EmailProfessor = viewModel.EmailProfessor,
                    TelefoneProfessor = viewModel.TelefoneProfessor,
                    CargoProfessor = viewModel.CargoProfessor,
                    ConhecimentoProfessor = viewModel.ConhecimentoProfessor,
                    IdEscola = viewModel.IdEscola,
                    Representante = viewModel.Representante,
                    Administrador = viewModel.Administrador
                };

                await CarregarEscolasViewBag();

                return View(updateDto);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateProfessorDto professor)
        {
            if (!ModelState.IsValid)
            {
                await CarregarEscolasViewBag();
                return View(professor);
            }

            var client = CreateClientWithToken();
            var response = await client.PutAsJsonAsync($"api/professores/{id}", professor);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            await CarregarEscolasViewBag();
            ModelState.AddModelError("", "Erro ao atualizar professor.");
            return View(professor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = CreateClientWithToken();
            await client.DeleteAsync($"api/professores/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
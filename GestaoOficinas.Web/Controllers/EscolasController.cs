using Microsoft.AspNetCore.Mvc;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Web.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GestaoOficinas.Web.Controllers
{
    public class EscolasController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public EscolasController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        private HttpClient CreateClientWithToken()
        {
            var client = _clientFactory.CreateClient("OficinasAPI");
            var token = HttpContext.Session.GetString("JWToken");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        public async Task<IActionResult> Index(string busca, int pagina = 1)
        {
            var client = CreateClientWithToken();
            try
            {
                var response = await client.GetAsync("api/escolas");

                if (response.IsSuccessStatusCode)
                {
                    var todasEscolas = await response.Content.ReadFromJsonAsync<List<EscolaViewModel>>();

                    if (!string.IsNullOrEmpty(busca))
                    {
                        todasEscolas = todasEscolas.Where(e =>
                            (e.NomeEscola ?? "").Contains(busca, StringComparison.OrdinalIgnoreCase) ||
                            (e.CnpjEscola ?? "").Contains(busca)).ToList();
                    }

                    int tamanhoPagina = 10;
                    var count = todasEscolas.Count;
                    var items = todasEscolas.Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToList();

                    var model = new EscolaListViewModel
                    {
                        Escolas = items,
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

            return View(new EscolaListViewModel());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEscolaDto escola)
        {
            if (!ModelState.IsValid) return View(escola);

            var client = CreateClientWithToken();
            var response = await client.PostAsJsonAsync("api/escolas", escola);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Erro ao criar escola. Verifique os dados.");
            return View(escola);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync($"api/escolas/{id}");

            if (response.IsSuccessStatusCode)
            {
                var escolaViewModel = await response.Content.ReadFromJsonAsync<EscolaViewModel>();

                var updateDto = new UpdateEscolaDto
                {
                    IdEscola = escolaViewModel.IdEscola,
                    NomeEscola = escolaViewModel.NomeEscola,
                    CnpjEscola = escolaViewModel.CnpjEscola,
                    EmailEscola = escolaViewModel.EmailEscola,
                    TelefoneEscola = escolaViewModel.TelefoneEscola,
                    CepEscola = escolaViewModel.CepEscola,
                    RuaEscola = escolaViewModel.RuaEscola,
                    ComplementoEscola = escolaViewModel.ComplementoEscola
                };

                return View(updateDto);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateEscolaDto escola)
        {
            if (!ModelState.IsValid) return View(escola);

            var client = CreateClientWithToken();
            var response = await client.PutAsJsonAsync($"api/escolas/{id}", escola);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Erro ao atualizar escola.");
            return View(escola);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = CreateClientWithToken();
            await client.DeleteAsync($"api/escolas/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
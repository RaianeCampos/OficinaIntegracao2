using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;

namespace GestaoOficinas.Web.Controllers
{
    public class TurmasController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public TurmasController(IHttpClientFactory clientFactory)
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

        private async Task CarregarOficinasViewBag()
        {
            var client = CreateClientWithToken();
            try
            {
                var response = await client.GetAsync("api/oficinas");
                if (response.IsSuccessStatusCode)
                {
                    var oficinas = await response.Content.ReadFromJsonAsync<List<OficinaViewModel>>();
                    ViewBag.ListaOficinas = new SelectList(oficinas, "IdOficina", "NomeOficina"); 
                }
                else
                {
                    ViewBag.ListaOficinas = new SelectList(new List<OficinaViewModel>(), "IdOficina", "NomeOficina");
                }
            }
            catch
            {
                ViewBag.ListaOficinas = new SelectList(new List<OficinaViewModel>(), "IdOficina", "NomeOficina");
            }
        }


        public async Task<IActionResult> Index(string busca, int pagina = 1)
        {
            var client = CreateClientWithToken();
            try
            {
                var response = await client.GetAsync("api/turmas");
                if (response.IsSuccessStatusCode)
                {
                    var todasTurmas = await response.Content.ReadFromJsonAsync<List<TurmaViewModel>>();

                    if (!string.IsNullOrEmpty(busca))
                    {
                        todasTurmas = todasTurmas.Where(t =>
                            (t.NomeTurma ?? "").Contains(busca, StringComparison.OrdinalIgnoreCase) ||
                            (t.NomeOficina ?? "").Contains(busca, StringComparison.OrdinalIgnoreCase)
                        ).ToList();
                    }

                    int tamanhoPagina = 10;
                    var count = todasTurmas.Count;
                    var items = todasTurmas.Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToList();

                    var model = new TurmaListViewModel
                    {
                        Turmas = items,
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

            return View(new TurmaListViewModel());
        }

        public async Task<IActionResult> Create()
        {
            await CarregarOficinasViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTurmaDto turma)
        {
            if (!ModelState.IsValid)
            {
                await CarregarOficinasViewBag();
                return View(turma);
            }

            var client = CreateClientWithToken();
            var response = await client.PostAsJsonAsync("api/turmas", turma);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var erroMsg = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Erro ao cadastrar: {erroMsg}");

            await CarregarOficinasViewBag();
            return View(turma);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync($"api/turmas/{id}");

            if (response.IsSuccessStatusCode)
            {
                var viewModel = await response.Content.ReadFromJsonAsync<TurmaViewModel>();

                var updateDto = new UpdateTurmaDto
                {
                    NomeTurma = viewModel.NomeTurma,
                    PeriodoTurma = viewModel.PeriodoTurma,
                    SemestreTurma = viewModel.SemestreTurma
                };

                await CarregarOficinasViewBag();

                ViewBag.IdTurma = id;
                return View(updateDto);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateTurmaDto turma)
        {
            if (!ModelState.IsValid)
            {
                await CarregarOficinasViewBag();
                ViewBag.IdTurma = id;
                return View(turma);
            }

            var client = CreateClientWithToken();
            var response = await client.PutAsJsonAsync($"api/turmas/{id}", turma);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var erroMsg = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Erro ao atualizar: {erroMsg}");

            await CarregarOficinasViewBag();
            ViewBag.IdTurma = id;
            return View(turma);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = CreateClientWithToken();
            await client.DeleteAsync($"api/turmas/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
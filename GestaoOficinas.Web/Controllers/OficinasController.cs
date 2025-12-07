using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Web.Models; 
using System.Net.Http.Headers;
using System.Text.Json;

namespace GestaoOficinas.Web.Controllers
{
    public class OficinasController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public OficinasController(IHttpClientFactory clientFactory)
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

        private async Task CarregarProfessoresViewBag()
        {
            var client = CreateClientWithToken();
            try
            {
                var response = await client.GetAsync("api/professores");
                if (response.IsSuccessStatusCode)
                {
                    var professores = await response.Content.ReadFromJsonAsync<List<ProfessorViewModel>>();
                    ViewBag.ListaProfessores = new SelectList(professores, "IdProfessor", "NomeProfessor");
                }
                else
                {
                    ViewBag.ListaProfessores = new SelectList(new List<ProfessorViewModel>(), "IdProfessor", "NomeProfessor");
                }
            }
            catch
            {
                ViewBag.ListaProfessores = new SelectList(new List<ProfessorViewModel>(), "IdProfessor", "NomeProfessor");
            }
        }

        // --- INDEX COM BUSCA E PAGINAÇÃO ---
        public async Task<IActionResult> Index(string busca, int pagina = 1)
        {
            var client = CreateClientWithToken();
            try
            {
                var response = await client.GetAsync("api/oficinas");
                if (response.IsSuccessStatusCode)
                {
                    var todasOficinas = await response.Content.ReadFromJsonAsync<List<OficinaViewModel>>();

                    // Filtro
                    if (!string.IsNullOrEmpty(busca))
                    {
                        todasOficinas = todasOficinas.Where(o =>
                            (o.NomeOficina ?? "").Contains(busca, StringComparison.OrdinalIgnoreCase) ||
                            (o.TemaOficina ?? "").Contains(busca, StringComparison.OrdinalIgnoreCase) ||
                            (o.NomeProfessorResponsavel ?? "").Contains(busca, StringComparison.OrdinalIgnoreCase)
                        ).ToList();
                    }

                    // Paginação
                    int tamanhoPagina = 10;
                    var count = todasOficinas.Count;
                    var items = todasOficinas.Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToList();

                    var model = new OficinaListViewModel
                    {
                        Oficinas = items,
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

            return View(new OficinaListViewModel());
        }

        public async Task<IActionResult> Create()
        {
            await CarregarProfessoresViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOficinaDto oficina)
        {
            if (!ModelState.IsValid)
            {
                await CarregarProfessoresViewBag();
                return View(oficina);
            }

            var client = CreateClientWithToken();
            var response = await client.PostAsJsonAsync("api/oficinas", oficina);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var erroApi = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Erro ao cadastrar: {erroApi}");

            await CarregarProfessoresViewBag();
            return View(oficina);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync($"api/oficinas/{id}");

            if (response.IsSuccessStatusCode)
            {
                var viewModel = await response.Content.ReadFromJsonAsync<OficinaViewModel>();

                var updateDto = new UpdateOficinaDto
                {
                    NomeOficina = viewModel.NomeOficina,
                    TemaOficina = viewModel.TemaOficina,
                    DescricaoOficina = viewModel.DescricaoOficina,
                    CargaHorariaOficinia = viewModel.CargaHorariaOficinia,
                    DataOficina = viewModel.DataOficina,
                    StatusOficina = viewModel.StatusOficina,
                    IdProfessor = viewModel.IdProfessor
                };

                await CarregarProfessoresViewBag();
                ViewBag.IdOficina = id; 

                return View(updateDto);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateOficinaDto oficina)
        {
            if (!ModelState.IsValid)
            {
                await CarregarProfessoresViewBag();
                ViewBag.IdOficina = id;
                return View(oficina);
            }

            var client = CreateClientWithToken();
            var response = await client.PutAsJsonAsync($"api/oficinas/{id}", oficina);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var erroApi = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Erro ao atualizar: {erroApi}");

            await CarregarProfessoresViewBag();
            ViewBag.IdOficina = id;
            return View(oficina);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = CreateClientWithToken();
            await client.DeleteAsync($"api/oficinas/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
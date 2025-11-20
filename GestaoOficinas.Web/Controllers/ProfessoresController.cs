using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestaoOficinas.Application.DTOs;
using System.Net.Http.Headers;
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

        public async Task<IActionResult> Index()
        {
            var client = CreateClientWithToken();
            try
            {
                var response = await client.GetAsync("api/professores");
                if (response.IsSuccessStatusCode)
                {
                    var lista = await response.Content.ReadFromJsonAsync<List<ProfessorViewModel>>();
                    return View(lista);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login", "Auth");
                }
            }
            catch { 
            
            }

            return View(new List<ProfessorViewModel>());
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
                var professorViewModel = await response.Content.ReadFromJsonAsync<ProfessorViewModel>();
                await CarregarEscolasViewBag();
                return View(professorViewModel);
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestaoOficinas.Application.DTOs;
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

        public async Task<IActionResult> Index()
        {
            var client = CreateClientWithToken();
            try
            {
                var response = await client.GetAsync("api/oficinas");
                if (response.IsSuccessStatusCode)
                {
                    var lista = await response.Content.ReadFromJsonAsync<List<OficinaViewModel>>();
                    return View(lista);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login", "Auth");
                }
            }
            catch { }

            return View(new List<OficinaViewModel>());
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

            await CarregarProfessoresViewBag();
            ModelState.AddModelError("", "Erro ao cadastrar oficina.");
            return View(oficina);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync($"api/oficinas/{id}");

            if (response.IsSuccessStatusCode)
            {
                var oficinaViewModel = await response.Content.ReadFromJsonAsync<OficinaViewModel>();
                await CarregarProfessoresViewBag();
                return View(oficinaViewModel);
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
                return View(oficina);
            }

            var client = CreateClientWithToken();
            var response = await client.PutAsJsonAsync($"api/oficinas/{id}", oficina);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            await CarregarProfessoresViewBag();
            ModelState.AddModelError("", "Erro ao atualizar oficina.");
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestaoOficinas.Application.DTOs;
using System.Net.Http.Headers;

namespace GestaoOficinas.Web.Controllers
{
    public class ChamadasController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public ChamadasController(IHttpClientFactory clientFactory)
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

        private async Task CarregarTurmasViewBag()
        {
            var client = CreateClientWithToken();
            var resp = await client.GetAsync("api/turmas");
            if (resp.IsSuccessStatusCode)
            {
                var turmas = await resp.Content.ReadFromJsonAsync<List<TurmaViewModel>>();
                ViewBag.ListaTurmas = new SelectList(turmas, "IdTurma", "NomeTurma");
            }
            else
            {
                ViewBag.ListaTurmas = new SelectList(new List<TurmaViewModel>(), "IdTurma", "NomeTurma");
            }
        }

        private async Task CarregarAlunosViewBag()
        {
            var client = CreateClientWithToken();
            var resp = await client.GetAsync("api/alunos");
            if (resp.IsSuccessStatusCode)
            {
                var alunos = await resp.Content.ReadFromJsonAsync<List<AlunoViewModel>>();
                ViewBag.ListaAlunos = new SelectList(alunos, "IdAluno", "NomeAluno");
            }
            else
            {
                ViewBag.ListaAlunos = new SelectList(new List<AlunoViewModel>(), "IdAluno", "NomeAluno");
            }
        }

        public async Task<IActionResult> Index()
        {
            var client = CreateClientWithToken();
            try
            {
                var response = await client.GetAsync("api/chamadas");
                if (response.IsSuccessStatusCode)
                {
                    var lista = await response.Content.ReadFromJsonAsync<List<ChamadaViewModel>>();
                    return View(lista);
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login", "Auth");
                }
            }
            catch { }

            return View(new List<ChamadaViewModel>());
        }

        public async Task<IActionResult> Create()
        {
            await CarregarTurmasViewBag();
            return View(new CreateChamadaDto { DataChamada = DateTime.UtcNow.Date });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateChamadaDto chamada)
        {
            if (!ModelState.IsValid)
            {
                await CarregarTurmasViewBag();
                return View(chamada);
            }

            var client = CreateClientWithToken();
            var response = await client.PostAsJsonAsync("api/chamadas", chamada);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            await CarregarTurmasViewBag();
            ModelState.AddModelError("", "Erro ao cadastrar chamada. Verifique os dados.");
            return View(chamada);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync($"api/chamadas/{id}");

            if (response.IsSuccessStatusCode)
            {
                var chamadaViewModel = await response.Content.ReadFromJsonAsync<ChamadaViewModel>();
                await CarregarTurmasViewBag();
                await CarregarAlunosViewBag();
                return View(chamadaViewModel);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateChamadaDto chamada)
        {
            if (!ModelState.IsValid)
            {
                await CarregarTurmasViewBag();
                return View(chamada);
            }

            var client = CreateClientWithToken();
            var response = await client.PutAsJsonAsync($"api/chamadas/{id}", chamada);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            await CarregarTurmasViewBag();
            ModelState.AddModelError("", "Erro ao atualizar chamada.");
            return View(chamada);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = CreateClientWithToken();
            await client.DeleteAsync($"api/chamadas/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
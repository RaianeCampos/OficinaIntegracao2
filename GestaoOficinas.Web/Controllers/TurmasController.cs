using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestaoOficinas.Application.DTOs;
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

        private async Task CarregarAlunosViewBag()
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync("api/alunos");

            if (response.IsSuccessStatusCode)
            {
                var alunos = await response.Content.ReadFromJsonAsync<List<AlunoViewModel>>();
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
                var response = await client.GetAsync("api/turmas"); 
                if (response.IsSuccessStatusCode)
                {
                    var lista = await response.Content.ReadFromJsonAsync<List<TurmaViewModel>>();
                    return View(lista);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login", "Auth");
                }
            }
            catch {

            }

            return View(new List<TurmaViewModel>());
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

            await CarregarOficinasViewBag();
            ModelState.AddModelError("", "Erro ao cadastrar turma. Verifique os dados.");
            return View(turma);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync($"api/turmas/{id}");

            if (response.IsSuccessStatusCode)
            {
                var turmaViewModel = await response.Content.ReadFromJsonAsync<TurmaViewModel>();
                await CarregarOficinasViewBag();
                await CarregarAlunosViewBag();
                return View(turmaViewModel);
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
                await CarregarAlunosViewBag();
                return View(turma);
            }

            var client = CreateClientWithToken();
            var response = await client.PutAsJsonAsync($"api/turmas/{id}", turma);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            await CarregarOficinasViewBag();
            await CarregarAlunosViewBag();
            ModelState.AddModelError("", "Erro ao atualizar turma.");
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestaoOficinas.Application.DTOs;
using System.Net.Http.Headers;

namespace GestaoOficinas.Web.Controllers
{
    public class PresencasController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public PresencasController(IHttpClientFactory clientFactory)
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

        private async Task CarregarChamadasViewBag()
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync("api/chamadas");

            if (response.IsSuccessStatusCode)
            {
                var chamadas = await response.Content.ReadFromJsonAsync<List<ChamadaViewModel>>();
                ViewBag.ListaChamadas = new SelectList(chamadas, "IdChamada", "NomeTurma");
            }
            else
            {
                ViewBag.ListaChamadas = new SelectList(new List<ChamadaViewModel>(), "IdChamada", "NomeTurma");
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
                var response = await client.GetAsync("api/presencas");
                if (response.IsSuccessStatusCode)
                {
                    var lista = await response.Content.ReadFromJsonAsync<List<PresencaViewModel>>();
                    return View(lista);
                }

                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login", "Auth");
                }
            }
            catch { 
            
            }            

            return View(new PresencaViewModel());
        }

        public async Task<IActionResult> Create()
        {
            await CarregarChamadasViewBag();
            await CarregarAlunosViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePresencaDto presenca)
        {
            if (presenca == null || presenca.Presencas == null)
            {
                ModelState.AddModelError("", "Dados inválidos.");
                await CarregarChamadasViewBag();
                await CarregarAlunosViewBag();
                return View(presenca);
            }

            var client = CreateClientWithToken();
            var response = await client.PostAsJsonAsync("api/presencas/registrar", presenca);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Auth");
            }

            await CarregarChamadasViewBag();
            await CarregarAlunosViewBag();
            ModelState.AddModelError("", "Erro ao registrar presenças.");
            return View(presenca);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync($"api/presencas/{id}"); //verificar

            if (response.IsSuccessStatusCode)
            {
                var chamadaViewModel = await response.Content.ReadFromJsonAsync<PresencaViewModel>();
                await CarregarChamadasViewBag();
                await CarregarAlunosViewBag();
                return View(chamadaViewModel);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdatePresencaDto professor)
        {
            if (!ModelState.IsValid)
            {
                await CarregarChamadasViewBag();
                await CarregarAlunosViewBag();
                return View(professor);
            }

            var client = CreateClientWithToken();
            var response = await client.PutAsJsonAsync($"api/professores/{id}", professor);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            await CarregarChamadasViewBag();
            await CarregarAlunosViewBag();
            ModelState.AddModelError("", "Erro ao atualizar professor.");
            return View(professor);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = CreateClientWithToken();
            await client.DeleteAsync($"api/presencas/chamada/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
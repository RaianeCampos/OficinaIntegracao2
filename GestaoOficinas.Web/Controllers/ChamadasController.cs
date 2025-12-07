using GestaoOficinas.Application.DTOs;
using GestaoOficinas.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            await CarregarAlunosViewBag(); 
            return View(new CreateChamadaDto { DataChamada = DateTime.UtcNow.Date });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateChamadaDto chamada)
        {
            if (!ModelState.IsValid)
            {
                await CarregarTurmasViewBag();
                await CarregarAlunosViewBag();
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
        public async Task<IActionResult> Grade(int? idTurma, DateTime? data)
        {
            await CarregarTurmasViewBag(); 

            var model = new RegistroChamadaViewModel
            {
                DataChamada = data ?? DateTime.UtcNow.Date,
                IdTurma = idTurma ?? 0
            };

            if (idTurma != null && idTurma > 0)
            {
                var client = CreateClientWithToken();

                var respTurma = await client.GetAsync($"api/turmas/{idTurma}");
                if (respTurma.IsSuccessStatusCode)
                {
                    var turma = await respTurma.Content.ReadFromJsonAsync<TurmaViewModel>();
                    model.NomeTurma = turma.NomeTurma;
                }

                var respAlunos = await client.GetAsync("api/alunos");
                if (respAlunos.IsSuccessStatusCode)
                {
                    var todosAlunos = await respAlunos.Content.ReadFromJsonAsync<List<AlunoViewModel>>();

                    var alunosDaTurma = todosAlunos
                                        .Where(a => a.TurmaIds != null && a.TurmaIds.Contains(idTurma.Value))
                                        .ToList();

                    model.Alunos = alunosDaTurma.Select(a => new AlunoPresencaItem
                    {
                        IdAluno = a.IdAluno,
                        NomeAluno = a.NomeAluno,
                        RaAluno = a.RaAluno,
                        Presente = true
                    }).ToList();
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalvarGrade(RegistroChamadaViewModel model)
        {
            var client = CreateClientWithToken();

            foreach (var item in model.Alunos)
            {
                var chamadaDto = new CreateChamadaDto
                {
                    IdTurma = model.IdTurma,
                    IdAluno = item.IdAluno,
                    DataChamada = model.DataChamada, 
                    Presente = item.Presente
                };

                await client.PostAsJsonAsync("api/chamadas", chamadaDto);
            }

            return RedirectToAction(nameof(Index));
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
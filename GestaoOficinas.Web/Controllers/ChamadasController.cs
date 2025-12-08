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
                // 1. Busca TUDO da API (lista plana)
                var response = await client.GetAsync("api/chamadas");

                if (response.IsSuccessStatusCode)
                {
                    var listaPlana = await response.Content.ReadFromJsonAsync<List<ChamadaViewModel>>();

                    // 2. AGRUPA por Data e Turma
                    var listaAgrupada = listaPlana
                        .GroupBy(c => new { c.DataChamada.Date, c.IdTurma, c.NomeTurma })
                        .Select(g => new ChamadaResumoViewModel
                        {
                            DataChamada = g.Key.Date,
                            IdTurma = g.Key.IdTurma,
                            NomeTurma = g.Key.NomeTurma ?? "Turma " + g.Key.IdTurma,
                            TotalAlunos = g.Count(),
                            TotalPresentes = g.Count(x => x.Presente)
                        })
                        .OrderByDescending(x => x.DataChamada)
                        .ToList();

                    return View(listaAgrupada);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login", "Auth");
                }
            }
            catch { }

            return View(new List<ChamadaResumoViewModel>());
        }

        public async Task<IActionResult> Create(int? idTurma, DateTime? data)
        {
            await CarregarTurmasViewBag();

            var model = new RegistroChamadaViewModel
            {
                DataChamada = data ?? DateTime.Now,
                IdTurma = idTurma ?? 0
            };

            if (idTurma != null && idTurma > 0)
            {
                var client = CreateClientWithToken();

                var response = await client.GetAsync("api/alunos");

                if (response.IsSuccessStatusCode)
                {
                    var todosAlunos = await response.Content.ReadFromJsonAsync<List<AlunoViewModel>>();

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
        public async Task<IActionResult> Create(RegistroChamadaViewModel model)
        {
            if (model.IdTurma == 0 || model.Alunos == null || !model.Alunos.Any())
            {
                ModelState.AddModelError("", "Selecione uma turma com alunos.");
                await CarregarTurmasViewBag();
                return View(model);
            }

            var client = CreateClientWithToken();
            bool houveErro = false;

            foreach (var item in model.Alunos)
            {
                var chamadaDto = new CreateChamadaDto
                {
                    IdTurma = model.IdTurma,
                    IdAluno = item.IdAluno,
                    
                    DataChamada = DateTime.SpecifyKind(model.DataChamada, DateTimeKind.Utc),
                    Presente = item.Presente
                };

                var response = await client.PostAsJsonAsync("api/chamadas", chamadaDto);
                if (!response.IsSuccessStatusCode) houveErro = true;
            }

            if (!houveErro)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Houve um erro ao salvar algumas presenças.");
                await CarregarTurmasViewBag();
                return View(model);
            }
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

        [HttpGet]
        public async Task<IActionResult> EditGrade(int idTurma, DateTime data)
        {
            var client = CreateClientWithToken();
            var model = new RegistroChamadaViewModel
            {
                IdTurma = idTurma,
                DataChamada = data
            };

            var respTurma = await client.GetAsync($"api/turmas/{idTurma}");
            if (respTurma.IsSuccessStatusCode)
            {
                var turma = await respTurma.Content.ReadFromJsonAsync<TurmaViewModel>();
                model.NomeTurma = turma.NomeTurma;
            }

            var respAlunos = await client.GetAsync("api/alunos");
            var respChamadas = await client.GetAsync("api/chamadas");

            if (respAlunos.IsSuccessStatusCode && respChamadas.IsSuccessStatusCode)
            {
                var todosAlunos = await respAlunos.Content.ReadFromJsonAsync<List<AlunoViewModel>>();
                var todasChamadas = await respChamadas.Content.ReadFromJsonAsync<List<ChamadaViewModel>>();

                var alunosDaTurma = todosAlunos
                    .Where(a => a.TurmaIds != null && a.TurmaIds.Contains(idTurma))
                    .ToList();

                var chamadasDoDia = todasChamadas
                    .Where(c => c.IdTurma == idTurma && c.DataChamada.Date == data.Date)
                    .ToList();

                model.Alunos = alunosDaTurma.Select(aluno => {
                    var chamadaExistente = chamadasDoDia.FirstOrDefault(c => c.IdAluno == aluno.IdAluno);

                    return new AlunoPresencaItem
                    {
                        IdAluno = aluno.IdAluno,
                        NomeAluno = aluno.NomeAluno,
                        RaAluno = aluno.RaAluno,
                        Presente = chamadaExistente != null ? chamadaExistente.Presente : false
                    };
                }).ToList();
            }

            return View("Grade", model); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalvarGrade(RegistroChamadaViewModel model)
        {
            var client = CreateClientWithToken();
            var respChamadas = await client.GetAsync("api/chamadas");
            var todasChamadas = await respChamadas.Content.ReadFromJsonAsync<List<ChamadaViewModel>>();
            var chamadasExistentes = todasChamadas
                .Where(c => c.IdTurma == model.IdTurma && c.DataChamada.Date == model.DataChamada.Date)
                .ToList();

            foreach (var item in model.Alunos)
            {
                var registroExistente = chamadasExistentes.FirstOrDefault(c => c.IdAluno == item.IdAluno);

                if (registroExistente != null)
                {
                    var updateDto = new CreateChamadaDto 
                    {
                        IdTurma = model.IdTurma,
                        IdAluno = item.IdAluno,
                        DataChamada = DateTime.SpecifyKind(model.DataChamada, DateTimeKind.Utc),
                        Presente = item.Presente
                    };
                    await client.PutAsJsonAsync($"api/chamadas/{registroExistente.IdChamada}", updateDto);
                }
                else
                {
                   
                    var createDto = new CreateChamadaDto
                    {
                        IdTurma = model.IdTurma,
                        IdAluno = item.IdAluno,
                        DataChamada = DateTime.SpecifyKind(model.DataChamada, DateTimeKind.Utc),
                        Presente = item.Presente
                    };
                    await client.PostAsJsonAsync("api/chamadas", createDto);
                }
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
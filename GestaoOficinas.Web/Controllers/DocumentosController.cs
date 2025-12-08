using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestaoOficinas.Application.DTOs;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GestaoOficinas.Web.Controllers
{
    public class DocumentosController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public DocumentosController(IHttpClientFactory clientFactory)
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

        private async Task CarregarViewBags()
        {
            var client = CreateClientWithToken();
            try
            {
                var resp = await client.GetAsync("api/oficinas");
                if (resp.IsSuccessStatusCode)
                {
                    var itens = await resp.Content.ReadFromJsonAsync<List<OficinaViewModel>>();
                    ViewBag.ListaOficinas = new SelectList(itens, "IdOficina", "NomeOficina");
                }
            }
            catch { ViewBag.ListaOficinas = new SelectList(new List<object>(), "IdOficina", "NomeOficina"); }

            try
            {
                var resp = await client.GetAsync("api/professores");
                if (resp.IsSuccessStatusCode)
                {
                    var itens = await resp.Content.ReadFromJsonAsync<List<ProfessorViewModel>>();
                    ViewBag.ListaProfessores = new SelectList(itens, "IdProfessor", "NomeProfessor");
                }
            }
            catch { ViewBag.ListaProfessores = new SelectList(new List<object>(), "IdProfessor", "NomeProfessor"); }

            try
            {
                var resp = await client.GetAsync("api/escolas");
                if (resp.IsSuccessStatusCode)
                {
                    var itens = await resp.Content.ReadFromJsonAsync<List<EscolaViewModel>>();
                    ViewBag.ListaEscolas = new SelectList(itens, "IdEscola", "NomeEscola");
                }
            }
            catch { ViewBag.ListaEscolas = new SelectList(new List<object>(), "IdEscola", "NomeEscola"); }

            try
            {
                var resp = await client.GetAsync("api/alunos");
                if (resp.IsSuccessStatusCode)
                {
                    var itens = await resp.Content.ReadFromJsonAsync<List<AlunoViewModel>>();
                    ViewBag.ListaAlunos = new SelectList(itens, "IdAluno", "NomeAluno");
                }
            }
            catch { ViewBag.ListaAlunos = new SelectList(new List<object>(), "IdAluno", "NomeAluno"); }
        }

        public async Task<IActionResult> Index()
        {
            var client = CreateClientWithToken();
            try
            {
                var response = await client.GetAsync("api/documentos");
                if (response.IsSuccessStatusCode)
                {
                    var lista = await response.Content.ReadFromJsonAsync<List<DocumentoViewModel>>();
                    return View(lista);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login", "Auth");
                }
            }
            catch { }

            return View(new List<DocumentoViewModel>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync($"api/documentos/{id}");

            if (response.IsSuccessStatusCode)
            {
                var documento = await response.Content.ReadFromJsonAsync<DocumentoViewModel>();
                return View(documento);
            }
            return NotFound();
        }

        public async Task<IActionResult> Create()
        {
            await CarregarViewBags();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDocumentoDto documento)
        {
            if (documento.TipoDocumento == "Certificado" && documento.IdAluno == null)
                ModelState.AddModelError("IdAluno", "Selecione um aluno para o certificado.");

            if (documento.TipoDocumento == "Convenio" && documento.IdEscola == null)
                ModelState.AddModelError("IdEscola", "Selecione uma escola para o convênio.");

            ModelState.Remove("NomeOficina");
            ModelState.Remove("TemaOficina");

            if (!ModelState.IsValid)
            {
                await CarregarViewBags();
                return View(documento);
            }

            var client = CreateClientWithToken();

            if (documento.IdOficina > 0)
            {
                var responseOficina = await client.GetAsync($"api/oficinas/{documento.IdOficina}");
                if (responseOficina.IsSuccessStatusCode)
                {
                    var oficina = await responseOficina.Content.ReadFromJsonAsync<OficinaViewModel>();
                    if (oficina != null)
                    {
                        documento.NomeOficina = oficina.NomeOficina;
                        
                        documento.TemaOficina = "Tema da Oficina " + oficina.NomeOficina; 
                    }
                }
            }

            var response = await client.PostAsJsonAsync("api/documentos", documento);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var erro = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Erro ao gerar documento: {erro}");

            await CarregarViewBags();
            return View(documento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = CreateClientWithToken();
            await client.DeleteAsync($"api/documentos/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
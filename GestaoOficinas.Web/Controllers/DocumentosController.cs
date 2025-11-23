using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestaoOficinas.Application.DTOs;
using System.Net.Http.Headers;

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

        private async Task CarregarListasViewBag()
        {
            var client = CreateClientWithToken();

            // Oficinas
            var respOf = await client.GetAsync("api/oficinas");
            if (respOf.IsSuccessStatusCode)
            {
                var oficinas = await respOf.Content.ReadFromJsonAsync<List<OficinaViewModel>>();
                ViewBag.ListaOficinas = new SelectList(oficinas, "IdOficina", "NomeOficina");
            }
            else
            {
                ViewBag.ListaOficinas = new SelectList(new List<OficinaViewModel>(), "IdOficina", "NomeOficina");
            }

            // Escolas
            var respEs = await client.GetAsync("api/escolas");
            if (respEs.IsSuccessStatusCode)
            {
                var escolas = await respEs.Content.ReadFromJsonAsync<List<EscolaViewModel>>();
                ViewBag.ListaEscolas = new SelectList(escolas, "IdEscola", "NomeEscola");
            }
            else
            {
                ViewBag.ListaEscolas = new SelectList(new List<EscolaViewModel>(), "IdEscola", "NomeEscola");
            }

            // Alunos
            var respAl = await client.GetAsync("api/alunos");
            if (respAl.IsSuccessStatusCode)
            {
                var alunos = await respAl.Content.ReadFromJsonAsync<List<AlunoViewModel>>();
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

        public async Task<IActionResult> Create()
        {
            await CarregarListasViewBag();
            return View(new CreateDocumentoDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDocumentoDto documento)
        {
            if (!ModelState.IsValid)
            {
                await CarregarListasViewBag();
                return View(documento);
            }

            var client = CreateClientWithToken();
            var response = await client.PostAsJsonAsync("api/documentos", documento);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            await CarregarListasViewBag();
            ModelState.AddModelError("", "Erro ao cadastrar documento. Verifique os dados.");
            return View(documento);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync($"api/documentos/{id}");

            if (response.IsSuccessStatusCode)
            {
                var documentoViewModel = await response.Content.ReadFromJsonAsync<DocumentoViewModel>();
                await CarregarListasViewBag();
                return View(documentoViewModel);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateDocumentoDto documento)
        {
            if (!ModelState.IsValid)
            {
                await CarregarListasViewBag();
                return View(documento);
            }

            var client = CreateClientWithToken();
            var response = await client.PutAsJsonAsync($"api/documentos/{id}", documento);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            await CarregarListasViewBag();
            ModelState.AddModelError("", "Erro ao atualizar documento.");
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
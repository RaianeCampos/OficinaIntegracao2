using Microsoft.AspNetCore.Mvc;
using GestaoOficinas.Application.DTOs;
using System.Net.Http.Headers;

namespace GestaoOficinas.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DashboardController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("OficinasAPI");

            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await client.GetAsync("api/dashboard");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<DashboardDto>();
                    return View(data);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Login", "Auth");
                }
            }
            catch (Exception)
            {
                // Em caso de erro de conexão, mantém a view vazia para não quebrar a aplicação
            }

            return View(new DashboardDto());
        }
    }
}
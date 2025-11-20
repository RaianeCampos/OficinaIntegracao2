using Microsoft.AspNetCore.Mvc;
using GestaoOficinas.Application.DTOs; // Crie um LoginDto simples se não tiver
using System.Text.Json;
using System.Text;

namespace GestaoOficinas.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public AuthController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Layout null para não mostrar sidebar na tela de login
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var client = _clientFactory.CreateClient("OficinasAPI");

            var loginData = new { Username = username, Password = password };
            var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("api/auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                    if (result.TryGetProperty("token", out var tokenElement))
                    {
                        var token = tokenElement.GetString();
                        // Salva o token na sessão
                        HttpContext.Session.SetString("JWToken", token);
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
            }
            catch
            {
                // Erro de conexão
            }

            ViewBag.Error = "Usuário ou senha inválidos";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
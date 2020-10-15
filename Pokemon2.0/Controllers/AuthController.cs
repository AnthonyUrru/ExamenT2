using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pokemon2._0.Models.Repositories;
using Pokemon2._0.Models.Services;

namespace Pokemon2._0.Controllers
{
    public class AuthController : Controller
    {

        public string MiPropiedad { get; set; }

        private readonly IConfiguration configuration;
        private readonly ICookieAuthService cookieAuthService;
        private readonly IUserRepository repository;

        public AuthController(IUserRepository repository, ICookieAuthService cookieAuthService, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.repository = repository;
            this.cookieAuthService = cookieAuthService;
        }


        [HttpGet]
        public string Index(string input)
        {
            return CreateHash(input);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            /* valid5ar si el usuario existe en la base de datos y 
             * verificar que el password sea correcto*/

            var user = repository.FindUser(username, CreateHash(password));

            if (user != null)
            {
                // Autenticaremos
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, username)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // cree la cookie y permita la autenticación
                cookieAuthService.SetHttpContext(HttpContext);
                cookieAuthService.Login(claimsPrincipal);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("Login", "Usuario o contraseña incorrectos.");
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        private string CreateHash(string input)
        {
            var sha = SHA256.Create();
            input += configuration.GetValue<string>("Token");
            var hash = sha.ComputeHash(Encoding.Default.GetBytes(input));

            return Convert.ToBase64String(hash);
        }
    }

}

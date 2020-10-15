using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Pokemon2._0.Models;

namespace Pokemon2._0.Controllers
{
    public class PokemonController : BaseController
    {
        private PokemonContext _context;
        public IHostEnvironment _hostEnv;
        public PokemonController(PokemonContext context, IHostEnvironment hostEnv) : base(context)
        {
            _context = context;
            _hostEnv = hostEnv;
        }
        [HttpGet]
        // GET: PokemonController
        public ActionResult Index()
        {


            var Pokemons = _context.Pokemons.ToList();
            ViewBag.Types = _context.PokeTypes.ToList();
            return View(Pokemons);


        }
        [HttpGet]
        public ActionResult IndexLogin()
        {

            var Pokemons = _context.Pokemons.ToList();
            ViewBag.Types = _context.PokeTypes.ToList();
            ViewBag.IdUser = LoggerUser().Id;
            return View(Pokemons);


        }
        [HttpGet]
        public ActionResult Mispokemones(string name)
        {
            var Pokemons = _context.Pokemons.Where(o=>o.UserId==LoggerUser().Id).ToList();
            ViewBag.Types = _context.PokeTypes.ToList();
            return View(Pokemons);


        }
        [HttpGet]
        public ActionResult Registrar()
        {
            ViewBag.Types = _context.PokeTypes.ToList();
            return View("Registrar", new Pokemon());
        }
        [HttpPost]
        public ActionResult Registrar(Pokemon pokemon, IFormFile image)
        {

            if (ModelState.IsValid)
            {
                pokemon.ImagePath = SaveImage(image);
                _context.Pokemons.Add(pokemon);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Types = _context.PokeTypes.ToList();
            return View("Registrar", pokemon);
        }
        public ActionResult Buscar(Pokemon pokemon, string name)
        {
            ViewBag.Types = _context.PokeTypes;
            var mostrarp = _context.Pokemons.Where(o => o.Name == name)
                .FirstOrDefault();
            if (mostrarp != null)
            {
                return View(mostrarp);
            }
            return View();
        }
        public ActionResult Capturar(int idpokemon, int iduser)
        {
            var pokemons = _context.Pokemons.Where(o=>o.Id==idpokemon);
            var user = _context.Users.Where(o=>o.Id==iduser);
            return View(pokemons);
        }
        private User LoggerUser()
        {
            var claim = HttpContext.User.Claims.FirstOrDefault();
            var user = _context.Users.Where(o => o.Username == claim.Value).FirstOrDefault();
            return user;
        }


        private string SaveImage(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                var basePath = _hostEnv.ContentRootPath + @"\wwwroot";
                var ruta = @"\files\" + image.FileName;

                using (var strem = new FileStream(basePath + ruta, FileMode.Create))
                {
                    image.CopyTo(strem);
                    return ruta;
                }
            }
            return null;
        }
    }
}

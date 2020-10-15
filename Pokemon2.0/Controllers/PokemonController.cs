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
           
            return View(Pokemons);


        }
        [HttpGet]
        public ActionResult Mispokemones()
        {
            var captura = _context.UsuarioPokemons.Where(o => o.UserId == LoggedUser().Id).ToList();
            var captura2 = _context.UsuarioPokemons.Where(o => o.UserId == LoggedUser().Id).FirstOrDefault();

            var pokemons = _context.Pokemons.Where(o=>o.UserId==LoggerUser().Id).ToList();
            //pokemons.Id = captura.PokemonId;
            //pokemons.UserId = LoggedUser().Id;
           
            ViewBag.Fecha = captura2.fecha;
            ViewBag.Types = _context.PokeTypes.ToList();
            return View(pokemons);


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
        public ActionResult Capturar(int idpokemon)
        {

            var captura = new UsuarioPokemon();
          
            captura.PokemonId =idpokemon;
            captura.UserId = LoggedUser().Id;
            DateTime fecha = DateTime.Now;
            captura.fecha = fecha;
            var pokemon = _context.Pokemons.Where(o=>o.Id==idpokemon).FirstOrDefault();
            pokemon.UserId = LoggedUser().Id;
            
            _context.SaveChanges();
            _context.UsuarioPokemons.Add(captura);
            _context.SaveChanges();
           // var Pokemons = _context.Pokemons.Where(o => o.Id==idpokemon).ToList();
            //ViewBag.Types = _context.PokeTypes.ToList();
            return RedirectToAction("MisPokemones");
        }

         public  ActionResult Liberar(int idpokemon)
        {
            var captura = _context.UsuarioPokemons.Where(o=>o.PokemonId==idpokemon).FirstOrDefault();
            var pokemon = _context.Pokemons.Where(o => o.Id == idpokemon).FirstOrDefault();
            _context.Pokemons.Remove(pokemon);
            _context.UsuarioPokemons.Remove(captura);
            _context.SaveChanges();
            return RedirectToAction("MisPokemones");
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

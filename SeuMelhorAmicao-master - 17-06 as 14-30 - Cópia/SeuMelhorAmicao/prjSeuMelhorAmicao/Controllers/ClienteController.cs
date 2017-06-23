using prjSeuMelhorAmicao.Models.DAL;
using prjSeuMelhorAmicao.Models.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjSeuMelhorAmicao.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class ClienteController : BaseController
    {
        private readonly ClienteDAO _clienteDAO;
        private readonly AnimalDAO _animalDAO;
        public ClienteController()
        {
            _clienteDAO = new ClienteDAO();
            _animalDAO = new AnimalDAO();
        }


        public ActionResult Editar()
        {
            return View();
        }

        //Buscar favoritos do cliente
        public ActionResult MeusFavoritos()
        {
            IEnumerable<Animal> animais = _animalDAO.ListarFavoritoCliente(UsuarioInfo.Id);

            return View(animais);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Cliente model)
        {
            if (ModelState.IsValid)
            {
               
                return View();
            }
            else
            {
                ModelState.AddModelError("", "");
                return View(model);
            }
        }

        public ActionResult FavoritarAnimal(int id = 0)
        {

            Cliente cliente = _clienteDAO.Buscar(UsuarioInfo.Id);

            cliente.Animal = new List<Animal>()
            {
                new Animal() {Id = id }
            };

            
            _clienteDAO.FavoritarAnimal(cliente);

            return RedirectToAction("MeusFavoritos");
        }

        public ActionResult DesFavoritarAnimal(int id = 0)
        {
            Cliente cliente = _clienteDAO.Buscar(UsuarioInfo.Id);

            cliente.Animal = new List<Animal>()
            {
                new Animal() {Id = id }
            };


            _clienteDAO.DesfavoritarAnimal(cliente);

            return RedirectToAction("MeusFavoritos");
        }


    }
}
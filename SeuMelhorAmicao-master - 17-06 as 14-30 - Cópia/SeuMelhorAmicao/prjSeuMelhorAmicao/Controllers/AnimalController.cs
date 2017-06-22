using prjSeuMelhorAmicao.Models.DAL;
using prjSeuMelhorAmicao.Models.Entidade;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjSeuMelhorAmicao.Controllers
{
    [Authorize(Roles = "Ong")]
    public class AnimalController : BaseController
    {
        private readonly AnimalDAO _animalDAO;
        public AnimalController()
        {
            _animalDAO = new AnimalDAO();
        }

        [AllowAnonymous]
        public ActionResult Index(int idOng, string pesquisa = "")
        {
            IEnumerable<Animal> animais = _animalDAO.ListarAnimalOng(idOng);

            return View(animais);
        }


        public ActionResult Cadastrar()
        {
            return View(new Animal());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Animal model, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                    using (var reader = new BinaryReader(upload.InputStream))
                    {
                        model.Foto = reader.ReadBytes(upload.ContentLength);
                    }

                //Pega o id da ong
                model.ONGId = UsuarioInfo.Id;

                _animalDAO.Salvar(model);

                return RedirectToAction("Index", "Ong");
            }
            else
            {
                ModelState.AddModelError("", "");
                return View(model);
            }
        }

        public ActionResult Detalhes(int id = 0)
        {
            if (id == 0)
                return RedirectToAction("Index", "Ong");

            Animal animal = _animalDAO.Buscar(id);

            return View(animal);
        }

        public ActionResult Editar(int id = 0)
        {
            if (id == 0)
                return RedirectToAction("Index", "Ong");

            Animal animal = _animalDAO.Buscar(id);


            return View(animal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Animal model)
        {
            if (ModelState.IsValid)
            {
                _animalDAO.Salvar(model);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "");
                return View(model);
            }
        }

        public ActionResult VisualizarAnimal(int id)
        {

            Animal animal = _animalDAO.Buscar(id);

            return File(animal.Foto, "image/jpg");
        }

     
    }






}
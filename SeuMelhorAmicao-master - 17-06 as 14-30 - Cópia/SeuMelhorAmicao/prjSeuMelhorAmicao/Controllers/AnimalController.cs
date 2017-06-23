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
        private readonly OngDAO _ongDAO;
        public AnimalController()
        {
            _ongDAO = new OngDAO();
            _animalDAO = new AnimalDAO();
        }


        //Visualizar os animais da ong
        [AllowAnonymous]
        public ActionResult Index(int idOng = 0, string pesquisa = "")
        {
            IEnumerable<Animal> animais = _animalDAO.ListarAnimalOng(idOng);
            var ong = _ongDAO.Buscar(idOng);

            ViewBag.NomeOng = ong.Nome;
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

                return RedirectToAction("MeusAnimais", "Ong");
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
        public ActionResult Editar(Animal model, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                    using (var reader = new BinaryReader(upload.InputStream))
                    {
                        model.Foto = reader.ReadBytes(upload.ContentLength);
                    }

                _animalDAO.Salvar(model);
                return RedirectToAction("MeusAnimais", "Ong");
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

        public ActionResult Delete(int id = 0)
        {
            if (id == 0)
                return RedirectToAction("Index", UsuarioInfo.Id);

            Animal animal = _animalDAO.Buscar(id);

            if (animal != null)
            {
                _animalDAO.Delete(animal);
                return RedirectToAction("MeusAnimais", "Ong");
            }
            else
            {
                ModelState.AddModelError("", "Animal não encontrado");
                return RedirectToAction("Index");
            }


        }



    }






}
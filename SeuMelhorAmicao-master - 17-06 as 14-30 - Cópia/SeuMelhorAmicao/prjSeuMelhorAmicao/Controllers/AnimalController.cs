using prjSeuMelhorAmicao.Models.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjSeuMelhorAmicao.Controllers
{

    public class AnimalController : Controller
    {
        
        public ActionResult Index(string pesquisa)
        {
            return View(new List<Animal>());
        }

        public ActionResult Cadastrar()
        {
            return View(new Animal());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Animal model)
        {
            if(ModelState.IsValid)
            {
                return View();
            }
            else
            {
                ModelState.AddModelError("", "");
                return View(model);
            }
        }

        public ActionResult Editar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Animal model)
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

        public ActionResult VisualizarAnimal(int id)
        {

            return File("", "image/jpg");
        }
    }
}
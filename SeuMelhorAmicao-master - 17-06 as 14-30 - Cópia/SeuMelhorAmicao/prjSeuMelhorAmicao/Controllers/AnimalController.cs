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
        public ActionResult Cadastrar(Animal model, HttpPostedFileBase upload)
        {
            if(ModelState.IsValid)
            {
                //using (var reader = new BinaryReader(upload.InputSream))
                //{ 
                //    Animal.Foto = reader.ReadBytes(upload.ContentLength);
                //}

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
        public ActionResult Salvar(Animal obj)
        {
            try
            {

                new AnimalDAO().Salvar(obj);
                TempData["SuccessMsg"] = "Animal salvo com sucesso!";
                if (!ModelState.IsValid)
                    return View(obj.Id == 0 ? "Cadastro" : "Edit", obj);

            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = string.Format("Falha ao salvar animal. {0}", ex.Message);

            }
            return View("Edit", obj);
        }
    }



    


}
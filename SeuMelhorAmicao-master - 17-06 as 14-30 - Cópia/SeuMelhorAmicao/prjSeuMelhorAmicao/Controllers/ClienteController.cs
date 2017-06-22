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
        public ClienteController()
        {

        }


        public ActionResult Editar()
        {
            return View();
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


        public ActionResult VisualizarOngs()
        {
            return View();
        }

        //E aqui ele veria as informações mais detalhadas da ong, e os animais dela
        public ActionResult DetalhesOng()
        {
            return View();
        }

        //Aqui pode ser tudo na mesma action ai vc faria o controle
        public ActionResult FavoritarAnimal(int id = 0)
        {
            if (id == 0)
                return View();

            

            return View();
        }

        public ActionResult DesFavoritarAnimal(int id = 0)
        {
            if (id == 0)
                return View();

            

            return View();
        }


    }
}
using prjSeuMelhorAmicao.Models.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjSeuMelhorAmicao.Controllers
{
 
    public class OngController : Controller
    {
        public ActionResult Index(string pesquisa)
        {
            return View(new List<Ong>());
        }

        public ActionResult Create()
        {
            return View(new Ong());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ong model)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Ong model)
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
    }
}

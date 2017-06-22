using prjSeuMelhorAmicao.Models.DAL;
using prjSeuMelhorAmicao.Models.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjSeuMelhorAmicao.Controllers
{
    [Authorize(Roles ="Ong")]
    public class OngController : BaseController
    {
        private readonly OngDAO _ongDAO;
        public OngController()
        {
            _ongDAO = new OngDAO();
        }

        [AllowAnonymous]
        public ActionResult Index(string pesquisa)
        {
            IEnumerable<Ong> Ongs = _ongDAO.Listar(pesquisa);

            return View(Ongs);
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


        [HttpGet]
        public ActionResult MostraImagem(int id = 0)
        {
            Ong ong = _ongDAO.Buscar(id);

            return File(ong.Foto, "image/jpg");
        }
    }
}

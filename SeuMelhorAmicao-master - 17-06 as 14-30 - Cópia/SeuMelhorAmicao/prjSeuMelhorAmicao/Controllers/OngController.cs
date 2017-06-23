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
        private readonly AnimalDAO _animalDAO;
        public OngController()
        {
            _ongDAO = new OngDAO();
            _animalDAO = new AnimalDAO();
        }
        //Busca todas as ongs
        [AllowAnonymous]
        public ActionResult Index(string pesquisa)
        {
            IEnumerable<Ong> Ongs = _ongDAO.Listar(pesquisa);

            return View(Ongs);
        }

        /// <summary>
        /// Animais da ong
        /// </summary>
        /// <returns></returns>
        public ActionResult MeusAnimais()
        {
            IEnumerable<Animal> animaisOng = _animalDAO.ListarAnimalOng(UsuarioInfo.Id);

            return View(animaisOng);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Ong model)
        {
            if (ModelState.IsValid)
            {
                _ongDAO.Salvar(model);
                return View();
            }
            else
            {
                ModelState.AddModelError("", "");
                return View(model);
            }
        }


        public ActionResult MostraImagem(int id = 0)
        {
            Ong ong = _ongDAO.Buscar(id);

            return File(ong.Foto, "image/jpg");
        }
    }
}

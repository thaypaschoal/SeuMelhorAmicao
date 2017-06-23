using prjSeuMelhorAmicao.Models;
using prjSeuMelhorAmicao.Models.DAL;
using prjSeuMelhorAmicao.Models.Entidade;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace prjSeuMelhorAmicao.Controllers
{
    [Authorize(Roles = "Cliente,Ong")]
    public class AccountController : BaseController
    {
        private readonly ClienteDAO _clienteDAO;
        private readonly OngDAO _ongDAO;

        public AccountController()
        {
            _clienteDAO = new ClienteDAO();
            _ongDAO = new OngDAO();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View(new LoginViewModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                Usuario user = new UsuarioDAO().Login(model);


                if (user != null && user.Email != null)
                {
                    AccountModel conta = new AccountModel();

                    var authCookie = conta.Credenciar(user);

                    HttpContext.Response.Cookies.Add(authCookie);

                    UsuarioInfo = new UsuarioDAO().BuscarInformacoes(user.Id);

                    if (UsuarioInfo.Perfil.Tipo == "Ong")
                        return RedirectToAction("MeusAnimais", "Ong");
                    else
                        return RedirectToAction("Index", "Ong");
                }
                else
                {
                    ModelState.AddModelError("", "Login ou/ Senha inválidos");
                    return RedirectToAction("Login", model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Login ou/ Senha inválidos");
                return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult Registrar()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View(new Cliente());
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Registrar(Cliente model)
        {
            if (ModelState.IsValid)
            {
                _clienteDAO.Insert(model);

                AccountModel conta = new AccountModel();

                Usuario user = new UsuarioDAO().BuscarInformacoes(model.Id);

                var authCookie = conta.Credenciar(user);

                HttpContext.Response.Cookies.Add(authCookie);

                UsuarioInfo = user;


                return RedirectToAction("Index", "Ong");
            }
            else
            {
                return View(model);
            }
        }


        [AllowAnonymous]
        public ActionResult RegistrarOng()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View(new Ong());
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RegistrarOng(Ong model, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                    using (var reader = new BinaryReader(upload.InputStream))
                    {
                        model.Foto = reader.ReadBytes(upload.ContentLength);
                    }


                _ongDAO.Insert(model);

                AccountModel conta = new AccountModel();

                Usuario user = new UsuarioDAO().BuscarInformacoes(model.Id);

                var authCookie = conta.Credenciar(user);

                HttpContext.Response.Cookies.Add(authCookie);

                UsuarioInfo = user;

                return RedirectToAction("MeusAnimais", "Ong");
            }
            else
            {
                return View(model);
            }
        }



        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }


        public ActionResult VisualizarPerfil()
        {
            return View();
        }
    }
}
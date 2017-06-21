using System.Web.ApplicationServices;

using prjSeuMelhorAmicao.Models.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using prjSeuMelhorAmicao.Models.DAL;
using prjSeuMelhorAmicao.Models;

namespace prjSeuMelhorAmicao.Controllers
{
    public class AccountController : Controller
    {
        private readonly ClienteDAO _clienteDAO;
        private readonly OngDAO _ongDAO;
        
        public AccountController()
        {
            _clienteDAO = new ClienteDAO();
            _ongDAO = new OngDAO();
        }

        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }
        

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                Usuario user = new UsuarioDAO().Login(model);


                if (user != null && user.Email != null)
                {
                    AccountModel conta = new AccountModel();

                    var authCookie = conta.Credenciar(user);
                    //Adiciona os cookie e prontinho
                    HttpContext.Response.Cookies.Add(authCookie);

                    FormsAuthentication.SetAuthCookie(user.Email, false);

                    return RedirectToAction("Index", "Ong");
                }

                ModelState.AddModelError("", "Login ou/ Senha inválidos");
                return RedirectToAction("Login", model);
            }
            else
            {
                ModelState.AddModelError("", "Login ou/ Senha inválidos");
                return View(model);
            }
        }

        
        public ActionResult Registrar()
        {
            return View(new Cliente());
        }

        [HttpPost]
        public ActionResult Registrar(Cliente model)
        {
            if (ModelState.IsValid)
            {
                _clienteDAO.Insert(model);

                return RedirectToAction("Login");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}
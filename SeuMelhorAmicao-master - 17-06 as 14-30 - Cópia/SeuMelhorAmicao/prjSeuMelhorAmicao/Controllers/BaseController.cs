using prjSeuMelhorAmicao.Models.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace prjSeuMelhorAmicao.Controllers
{
    public class BaseController : Controller
    {

        public Usuario UsuarioInfo
        {
            get
            {
                var usuario = (Usuario)Session["UsuarioInfo"];
                if (usuario == null)
                {
                    FormsAuthentication.SignOut();
                    Session.Clear();
                    Session.Abandon();
                    return new Usuario();
                }             

                return usuario;
            }
            set
            {
                if (Session["UsuarioInfo"] == null)
                    Session["UsuarioInfo"] = value;
            }

        }

    }
}
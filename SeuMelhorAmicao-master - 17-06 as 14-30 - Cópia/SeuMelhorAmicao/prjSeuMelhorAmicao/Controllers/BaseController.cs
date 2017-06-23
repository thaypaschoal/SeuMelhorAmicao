using prjSeuMelhorAmicao.Models.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjSeuMelhorAmicao.Controllers
{
    public class BaseController : Controller
    {

        public Usuario UsuarioInfo
        {
            get
            {
                //HttpContext.Response.Cookies.Get();

                return (Usuario)Session["UsuarioInfo"];
            }
            set
            {
                if (Session["UsuarioInfo"] == null)
                    Session["UsuarioInfo"] = value;
            }

        }

    }
}
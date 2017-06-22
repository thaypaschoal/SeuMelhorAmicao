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
                return (Usuario)Session["UsuarioInfo"];
            }
            set
            {
                Session["UsuarioInfo"] = value;
            }

        }

    }
}
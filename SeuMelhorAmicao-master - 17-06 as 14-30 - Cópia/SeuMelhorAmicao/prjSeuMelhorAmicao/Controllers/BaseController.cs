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
        public Usuario Usuario
        {
            get
            {
                return (Usuario)Session["Usuario"];
            }
            set
            {
                Session["Usuario"] = value;
            }

        }

        public int Ong
        {
            get
            {
                return HttpContext.Request.Cookies["Ong"] == null || string.IsNullOrEmpty(HttpContext.Request.Cookies["Ong"].Value)
                        ? 0
                        : Convert.ToInt32(HttpContext.Request.Cookies["Ong"].Value);
            }
            set
            {
                    HttpContext.Response.Cookies.Add(new HttpCookie("Ong")
                    {
                        Value = value.ToString()
                    });
            }

        }

        public int Cliente
        {
            get
            {
                return HttpContext.Request.Cookies["Cliente"] == null || string.IsNullOrEmpty(HttpContext.Request.Cookies["Cliente"].Value)
                        ? 0
                        : Convert.ToInt32(HttpContext.Request.Cookies["Cliente"].Value);
            }
            set
            {
                HttpContext.Response.Cookies.Add(new HttpCookie("Cliente")
                {
                    Value = value.ToString()
                });
            }

        }
    }
}
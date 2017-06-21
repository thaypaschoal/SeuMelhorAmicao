using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjSeuMelhorAmicao.Controllers
{
    
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    
        public ActionResult About()
        {
            ViewBag.Message = "Esta página tem a finalidade de ajudar Ong's a encontarem um lar para os bichinos de estimação.";

            return View();
        }

    }
}
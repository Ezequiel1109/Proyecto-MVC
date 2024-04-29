using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiPrimeraAplicacionWebConEntityFramework.Controllers
{
    public class PantallaPrincipalController : Controller
    {
        // GET: PantallaPrincipal
        public ActionResult Index()
        {
            return View();
        }
    }
}
using MiPrimeraAplicacionWebConEntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiPrimeraAplicacionWebConEntityFramework.Filters
{
    public class Acceder:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Si session es nulo , entonces retorne al Login
            var usuario = HttpContext.Current.Session["Usuario"];

            List<MenuCLS> roles =(List<MenuCLS>) HttpContext.Current.Session["Rol"];

            string nombreControlador = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string accion = filterContext.ActionDescriptor.ActionName;
            int cantidad = roles.Where(p => p.nombreControlador == nombreControlador).Count();
            if (usuario == null || cantidad==0)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
            }


            base.OnActionExecuting(filterContext);
        }


    }
}
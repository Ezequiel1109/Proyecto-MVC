using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiPrimeraAplicacionWebConEntityFramework.Models;
using MiPrimeraAplicacionWebConEntityFramework.Filters;

namespace MiPrimeraAplicacionWebConEntityFramework.Controllers
{

    [Acceder]
    public class RolController : Controller
    {
        // GET: Rol
        public ActionResult Index()
        {
            List<RolCLS> listaRol = new List<RolCLS>();
            using(var bd=new BDPasajeEntities())
            {
                listaRol = (from rol in bd.Rol
                            where rol.BHABILITADO == 1
                            select new RolCLS
                            {
                                iidRol = rol.IIDROL,
                                nombre = rol.NOMBRE,
                                descripcion = rol.DESCRIPCION
                            }).ToList();
            }

            return View(listaRol);
        }
        public ActionResult Filtro(string nombreRol)
        {

            List<RolCLS> listaRol = new List<RolCLS>();
            using (var bd = new BDPasajeEntities())
            {
                if(nombreRol == null)
                listaRol = (from rol in bd.Rol
                            where rol.BHABILITADO == 1
                            select new RolCLS
                            {
                                iidRol = rol.IIDROL,
                                nombre = rol.NOMBRE,
                                descripcion = rol.DESCRIPCION
                            }).ToList();
                else
                    listaRol = (from rol in bd.Rol
                                where rol.BHABILITADO == 1
                                && rol.NOMBRE.Contains(nombreRol)
                                select new RolCLS
                                {
                                    iidRol = rol.IIDROL,
                                    nombre = rol.NOMBRE,
                                    descripcion = rol.DESCRIPCION
                                }).ToList();
            }
            return PartialView("_TablaRol", listaRol);
        }

        public string Guardar(RolCLS oRolCLS, int titulo)
        {
            //Hay error
            string rpta = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    var query = (from state in ModelState.Values
                                 from error in state.Errors
                                 select error.ErrorMessage).ToList();
                    rpta += "<ul class='list-group'>";
                    foreach (var item in query)
                    {
                        rpta += "<li class='list-group-item'>" + item + "</li>";
                    }
                    rpta += "</ul>";
                }
                else
                {
                    using (var bd = new BDPasajeEntities())
                    {
                        int cantidad = 0;
                        if (titulo.Equals(-1))
                        {
                            cantidad = bd.Rol.Where(p => p.NOMBRE == oRolCLS.nombre).Count();
                            if (cantidad >= 1)
                            {
                                rpta = "-1";
                            }
                            else {
                                Rol oRol = new Rol();
                                oRol.NOMBRE = oRolCLS.nombre;
                                oRol.DESCRIPCION = oRolCLS.descripcion;
                                oRol.BHABILITADO = 1;
                                bd.Rol.Add(oRol);
                                rpta = bd.SaveChanges().ToString();
                                if (rpta == "0") rpta = "";
                            }
                            //-1 ya existe en la BD
                        }
                        else
                        {
                            cantidad = bd.Rol.Where(p => p.NOMBRE == oRolCLS.nombre &&
                           p.IIDROL != titulo).Count();

                            if (cantidad >= 1)
                            {
                                rpta = "-1";
                            }
                            else
                            {
                               

                                Rol oRol = bd.Rol.Where(p => p.IIDROL == titulo).First();
                                oRol.NOMBRE = oRolCLS.nombre;
                                oRol.DESCRIPCION = oRolCLS.descripcion;
                                rpta = bd.SaveChanges().ToString();
                            }
                        }

                    }
                }
            }catch(Exception ex)
            {
                rpta = "";
            }


            return rpta;
        }

        public string eliminar(RolCLS oRolCls)
        {
            string rpta = "";
            try
            {
                //Error
               
                int idrol = oRolCls.iidRol;
                using (var bd = new BDPasajeEntities())
                {
                    Rol oRol = bd.Rol.Where(p => p.IIDROL == idrol).First();
                    oRol.BHABILITADO = 0;
                    rpta=   bd.SaveChanges().ToString();

                }
            }catch(Exception ex)
            {
                rpta = "";

            }
            return rpta;
        }


        public JsonResult recuperarDatos(int titulo)
        {
            RolCLS oRolCLS = new RolCLS();
            using(var bd=new BDPasajeEntities())
            {
                Rol oRol = bd.Rol.Where(p => p.IIDROL == titulo).First();
                oRolCLS.nombre = oRol.NOMBRE;
                oRolCLS.descripcion = oRol.DESCRIPCION;

            }
            return Json(oRolCLS,JsonRequestBehavior.AllowGet);
        }

    }
}
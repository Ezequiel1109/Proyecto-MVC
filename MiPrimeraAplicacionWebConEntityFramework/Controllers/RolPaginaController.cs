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
    public class RolPaginaController : Controller
    {


        public void listarComboRol()
        {
            //agregar
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from item in bd.Rol
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDROL.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaRol = lista;
            }
        }

        public void listarComboPagina()
        {
            //agregar
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from item in bd.Pagina
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.MENSAJE,
                             Value = item.IIDPAGINA.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaPagina = lista;
            }
        }

        // GET: RolPagina
        public ActionResult Index()
        {
            listarComboRol();
            listarComboPagina();
            List<RolPaginaCLS> listaRol = new List<RolPaginaCLS>();
            using(var bd=new BDPasajeEntities())
            {
                listaRol = (from rolpagina in bd.RolPagina
                            join rol in bd.Rol
                            on rolpagina.IIDROL equals
                            rol.IIDROL
                            join pagina in bd.Pagina
                            on rolpagina.IIDPAGINA equals
                            pagina.IIDPAGINA
                            where rolpagina.BHABILITADO==1
                            select new RolPaginaCLS
                            {
                                iidrolpagina = rolpagina.IIDROLPAGINA,
                                nombreRol = rol.NOMBRE,
                                nombreMensaje = pagina.MENSAJE
                            }).ToList();
            }
            return View(listaRol);
        }

        public string Guardar(RolPaginaCLS oRolPaginaCLS, int titulo)
        {
            //Error
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
                        //agregar
                        if (titulo == -1)
                        {
                            cantidad = bd.RolPagina.Where(p => p.IIDROL == oRolPaginaCLS.iidrol
                              && p.IIDPAGINA == oRolPaginaCLS.iidpagina).Count();

                            if (cantidad >= 1)
                            {
                                rpta = "-1";
                            }
                            else
                            {
                                RolPagina oRolPagina = new RolPagina();
                                oRolPagina.IIDROL = oRolPaginaCLS.iidrol;
                                oRolPagina.IIDPAGINA = oRolPaginaCLS.iidpagina;
                                oRolPagina.BHABILITADO = 1;
                                bd.RolPagina.Add(oRolPagina);
                                rpta = bd.SaveChanges().ToString();
                                if (rpta == "0") rpta = "";
                            }
                        }
                        else
                        {
                            cantidad = bd.RolPagina.Where(p => p.IIDROL == oRolPaginaCLS.iidrol
                            && p.IIDPAGINA == oRolPaginaCLS.iidpagina
                            && p.IIDROLPAGINA!= titulo).Count();
                            if (cantidad >= 1)
                            {
                                rpta = "-1";
                            }
                            else
                            {
                                RolPagina oRolPagina = bd.RolPagina.Where(p => p.IIDROLPAGINA == titulo).First();
                                oRolPagina.IIDROL = oRolPaginaCLS.iidrol;
                                oRolPagina.IIDPAGINA = oRolPaginaCLS.iidpagina;
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

        public JsonResult recuperarInformacion(int idRolPagina)
        {
            RolPaginaCLS oRolPaginaCLS = new RolPaginaCLS();
            using (var bd=new BDPasajeEntities())
            {
                RolPagina oRolPagina = bd.RolPagina.Where(p => p.IIDROLPAGINA == idRolPagina).First();
                oRolPaginaCLS.iidrol = (int)oRolPagina.IIDROL;
                oRolPaginaCLS.iidpagina = (int)oRolPagina.IIDROLPAGINA;

            }
            return Json(oRolPaginaCLS, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Filtrar(int? iidrolFiltro)
        {
           
            List<RolPaginaCLS> listaRol = new List<RolPaginaCLS>();
            using (var bd = new BDPasajeEntities())
            {
                if (iidrolFiltro == null)
                {
                    listaRol = (from rolpagina in bd.RolPagina
                                join rol in bd.Rol
                                on rolpagina.IIDROL equals
                                rol.IIDROL
                                join pagina in bd.Pagina
                                on rolpagina.IIDPAGINA equals
                                pagina.IIDPAGINA
                                where rolpagina.BHABILITADO == 1
                                select new RolPaginaCLS
                                {
                                    iidrolpagina = rolpagina.IIDROLPAGINA,
                                    nombreRol = rol.NOMBRE,
                                    nombreMensaje = pagina.MENSAJE
                                }).ToList();
                }else
                {
                    listaRol = (from rolpagina in bd.RolPagina
                                join rol in bd.Rol
                                on rolpagina.IIDROL equals
                                rol.IIDROL
                                join pagina in bd.Pagina
                                on rolpagina.IIDPAGINA equals
                                pagina.IIDPAGINA
                                where rolpagina.BHABILITADO == 1
                                && rolpagina.IIDROL==iidrolFiltro
                                select new RolPaginaCLS
                                {
                                    iidrolpagina = rolpagina.IIDROLPAGINA,
                                    nombreRol = rol.NOMBRE,
                                    nombreMensaje = pagina.MENSAJE
                                }).ToList();
                }

            }
            return PartialView("_TableRolPagina",listaRol);
        }

        public int EliminarRolPagina(int iidrolpagina)
        {
            int rpta = 0;
            try
            {
                using (var bd = new BDPasajeEntities())
                {
                    RolPagina oRolPagina = bd.RolPagina.Where(p => p.IIDROLPAGINA == iidrolpagina).First();
                    oRolPagina.BHABILITADO = 0;
                    rpta = bd.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                rpta = 0;
            }


            return rpta;
        }

    }
}
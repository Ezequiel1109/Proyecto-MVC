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
    public class PaginaController : Controller
    {
        // GET: Pagina
        public ActionResult Index()
        {
            List<PaginaCLS> listaPagina = new List<PaginaCLS>();
            using (var bd = new BDPasajeEntities())
            {
                listaPagina = (from pagina in bd.Pagina
                               where pagina.BHABILITADO == 1
                               select new PaginaCLS
                               {
                                   iidpagina = pagina.IIDPAGINA,
                                   mensaje = pagina.MENSAJE,
                                   controlador = pagina.CONTROLADOR,
                                   accion = pagina.ACCION
                               }).ToList();
            }


            return View(listaPagina);
        }

        public ActionResult Filtrar(PaginaCLS oPaginaCLS)
        {
            string mensaje = oPaginaCLS.mensajeFiltro;
            List<PaginaCLS> listaPagina = new List<PaginaCLS>();
            using (var bd = new BDPasajeEntities())
            {
                if (mensaje == null)
                {
                    listaPagina = (from pagina in bd.Pagina
                                   where pagina.BHABILITADO == 1
                                   select new PaginaCLS
                                   {
                                       iidpagina = pagina.IIDPAGINA,
                                       mensaje = pagina.MENSAJE,
                                       controlador = pagina.CONTROLADOR,
                                       accion = pagina.ACCION
                                   }).ToList();
                }
                else
                {
                    listaPagina = (from pagina in bd.Pagina
                                   where pagina.BHABILITADO == 1
                                   && pagina.MENSAJE.Contains(mensaje)
                                   select new PaginaCLS
                                   {
                                       iidpagina = pagina.IIDPAGINA,
                                       mensaje = pagina.MENSAJE,
                                       controlador = pagina.CONTROLADOR,
                                       accion = pagina.ACCION
                                   }).ToList();
                }


            }
            return PartialView("_TablaPagina", listaPagina);
        }

        public int EliminarPagina(int iidpagina)
        {
            int rpta = 0;
            try
            {
                using(var bd=new BDPasajeEntities())
                {
                    Pagina oPagina = bd.Pagina.Where(p => p.IIDPAGINA == iidpagina).First();
                    oPagina.BHABILITADO = 0;
                    rpta= bd.SaveChanges();
                }
               
            }catch(Exception ex)
            {
                rpta = 0;
            }


            return rpta;
        }


        public string Guardar(PaginaCLS oPaginaCLS, int titulo)
        {
            //error
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
                            cantidad = bd.Pagina.Where(p => p.MENSAJE == oPaginaCLS.mensaje).Count();
                            if (cantidad >= 1)
                            {
                                rpta = "-1";
                            }
                            else
                            {
                                Pagina oPagina = new Pagina();
                                oPagina.MENSAJE = oPaginaCLS.mensaje;
                                oPagina.ACCION = oPaginaCLS.accion;
                                oPagina.CONTROLADOR = oPaginaCLS.controlador;
                                oPagina.BHABILITADO = 1;
                                bd.Pagina.Add(oPagina);
                                rpta = bd.SaveChanges().ToString();
                                if (rpta == "0") rpta = "";
                            }
                            //editar
                        }
                        else
                        {
                            cantidad = bd.Pagina.Where(p => p.MENSAJE == oPaginaCLS.mensaje
                            && p.IIDPAGINA!= titulo).Count();
                            if (cantidad >= 1)
                            {
                                rpta = "-1";
                            }
                            else
                            {
                                Pagina oPagina = bd.Pagina.Where(p => p.IIDPAGINA == titulo).First();
                                oPagina.MENSAJE = oPaginaCLS.mensaje;
                                oPaginaCLS.controlador = oPaginaCLS.controlador;
                                oPaginaCLS.accion = oPaginaCLS.accion;
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

        public JsonResult recuperarInformacion(int idPagina)
        {
            PaginaCLS oPaginaCLS = new PaginaCLS();
            using(var bd=new BDPasajeEntities())
            {

                Pagina oPagina = bd.Pagina.Where(p => p.IIDPAGINA == idPagina).First();
                oPaginaCLS.mensaje = oPagina.MENSAJE;
                oPaginaCLS.accion = oPagina.ACCION;
                oPaginaCLS.controlador = oPagina.CONTROLADOR;

            }
            return Json(oPaginaCLS, JsonRequestBehavior.AllowGet);

        }

    }
}
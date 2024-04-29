using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiPrimeraAplicacionWebConEntityFramework.Models;
namespace MiPrimeraAplicacionWebConEntityFramework.Controllers
{
    public class SucursalController : Controller
    {
        // GET: Sucursal
        public ActionResult Index(SucursalCLS oSucursalCls)
        {
            List<SucursalCLS> listaSucursal = null;
            string nombreSucursal = oSucursalCls.nombre;
                using (var bd = new BDPasajeEntities())
                {
                if(oSucursalCls.nombre==null)
                    listaSucursal = (from sucursal in bd.Sucursal
                                     where sucursal.BHABILITADO == 1
                                     select new SucursalCLS
                                     {
                                         iidsucursal = sucursal.IIDSUCURSAL,
                                         nombre = sucursal.NOMBRE,
                                         telefono = sucursal.TELEFONO,
                                         email = sucursal.EMAIL
                                     }).ToList();
                else
                    listaSucursal = (from sucursal in bd.Sucursal
                                     where sucursal.BHABILITADO == 1
                                     && sucursal.NOMBRE.Contains(nombreSucursal)
                                     select new SucursalCLS
                                     {
                                         iidsucursal = sucursal.IIDSUCURSAL,
                                         nombre = sucursal.NOMBRE,
                                         telefono = sucursal.TELEFONO,
                                         email = sucursal.EMAIL
                                     }).ToList();
            }
            
            return View(listaSucursal);
        }

        public ActionResult Editar(int id)
        {
            SucursalCLS oSucursalCLS = new SucursalCLS();
            using (var bd = new BDPasajeEntities()) {
                Sucursal oSucursal = bd.Sucursal.Where(p => p.IIDSUCURSAL.Equals(id)).First();
                oSucursalCLS.iidsucursal = oSucursal.IIDSUCURSAL;
                oSucursalCLS.nombre = oSucursal.NOMBRE;
                oSucursalCLS.direccion = oSucursal.DIRECCION;
                oSucursalCLS.telefono = oSucursal.TELEFONO;
                oSucursalCLS.email = oSucursal.EMAIL;
                oSucursalCLS.fechaApertura =(DateTime) oSucursal.FECHAAPERTURA;
                    }

            return View(oSucursalCLS);
        }
        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(SucursalCLS oSucursalCLS)
        {
            int nregistrosEncontrados = 0;
            string nombreSucursal = oSucursalCLS.nombre;
            using(var bd=new BDPasajeEntities())
            {
                nregistrosEncontrados = bd.Sucursal.Where(p => p.NOMBRE.Equals(nombreSucursal)).Count();
            }


            if (!ModelState.IsValid || nregistrosEncontrados>=1)
            {
                if (nregistrosEncontrados >= 1) oSucursalCLS.mensajeError = "Ya existe la sucursal a ingresar";
                return View(oSucursalCLS);
            }
            using(var bd=new BDPasajeEntities())
            {
                Sucursal oSucursal = new Sucursal();
                oSucursal.NOMBRE = oSucursalCLS.nombre;
                oSucursal.DIRECCION = oSucursalCLS.direccion;
                oSucursal.TELEFONO = oSucursalCLS.telefono;
                oSucursal.EMAIL = oSucursalCLS.email;
                oSucursal.FECHAAPERTURA = oSucursalCLS.fechaApertura;
                oSucursal.BHABILITADO = 1;
                bd.Sucursal.Add(oSucursal);
                bd.SaveChanges();
            }



            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Editar(SucursalCLS oSucursalCLS)
        {
            int nregistrosAfectados = 0;
            int idSucursal = oSucursalCLS.iidsucursal;
            string nombreSucursal = oSucursalCLS.nombre;
            using(var bd=new BDPasajeEntities())
            {
                nregistrosAfectados = bd.Sucursal.Where(p => p.NOMBRE.Equals(nombreSucursal) && !p.IIDSUCURSAL.Equals(idSucursal)).Count();
            }
            if (!ModelState.IsValid || nregistrosAfectados>=1 )
            {
                if (nregistrosAfectados >= 1) oSucursalCLS.mensajeError = "Ya existe la sucursal";
                    return View(oSucursalCLS);
            }

            using(var bd=new BDPasajeEntities())
            {
                Sucursal oSucursal = bd.Sucursal.Where(p => p.IIDSUCURSAL.Equals(idSucursal)).First();

                oSucursal.NOMBRE = oSucursalCLS.nombre;

                oSucursal.DIRECCION = oSucursalCLS.direccion;
                oSucursal.TELEFONO = oSucursalCLS.telefono;
                oSucursal.EMAIL = oSucursalCLS.email;
                oSucursal.FECHAAPERTURA = oSucursalCLS.fechaApertura;

                bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        public ActionResult Eliminar(int id)
        {
            using(var bd=new BDPasajeEntities())
            {
                Sucursal oSucursal = bd.Sucursal.Where(p => p.IIDSUCURSAL.Equals(id)).First();
                oSucursal.BHABILITADO = 0;
                bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}
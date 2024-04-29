using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiPrimeraAplicacionWebConEntityFramework.Models;
namespace MiPrimeraAplicacionWebConEntityFramework.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index(ClienteCLS oClienteCLS)
        {
            List<ClienteCLS> listaCliente = null;
            int iidsexo = oClienteCLS.iidsexo;
            llenarSexo();
            ViewBag.lista = listaSexo;

            using (var bd=new BDPasajeEntities())
            {
                if (oClienteCLS.iidsexo == 0)
                {
                    listaCliente = (from cliente in bd.Cliente
                                    where cliente.BHABILITADO == 1
                                    select new ClienteCLS
                                    {
                                        iidcliente = cliente.IIDCLIENTE,
                                        nombre = cliente.NOMBRE,
                                        apPaterno = cliente.APPATERNO,
                                        apMaterno = cliente.APMATERNO,
                                        telefonoFijo = cliente.TELEFONOFIJO
                                    }).ToList();
                }else
                {
                    listaCliente = (from cliente in bd.Cliente
                                    where cliente.BHABILITADO == 1
                                    && cliente.IIDSEXO== iidsexo
                                    select new ClienteCLS
                                    {
                                        iidcliente = cliente.IIDCLIENTE,
                                        nombre = cliente.NOMBRE,
                                        apPaterno = cliente.APPATERNO,
                                        apMaterno = cliente.APMATERNO,
                                        telefonoFijo = cliente.TELEFONOFIJO
                                    }).ToList();
                }

            }


            return View(listaCliente);
        }

        public ActionResult Editar(int id)
        {
            ClienteCLS oClienteCLS = new ClienteCLS();

            using(var bd=new BDPasajeEntities())
            {
                llenarSexo();
                ViewBag.lista = listaSexo;
                Cliente oCLiente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(id)).First();
                oClienteCLS.iidcliente = oCLiente.IIDCLIENTE;

                oClienteCLS.nombre = oCLiente.NOMBRE;
                oClienteCLS.apPaterno = oCLiente.APPATERNO;
                oClienteCLS.apMaterno = oCLiente.APMATERNO;
                oClienteCLS.direccion = oCLiente.DIRECCION;
                oClienteCLS.email = oCLiente.EMAIL;

                oClienteCLS.iidsexo = (int)oCLiente.IIDSEXO;
                oClienteCLS.telefonoCelular = oCLiente.TELEFONOCELULAR;
                oClienteCLS.telefonoFijo = oCLiente.TELEFONOFIJO;



            }

            return View(oClienteCLS);
        }

        [HttpPost]
        public ActionResult Editar(ClienteCLS oClienteCLS)
        {
            int nregistradosEncontrados = 0;
            int idcliente = oClienteCLS.iidcliente;
            string nombre = oClienteCLS.nombre;
            string apPaterno = oClienteCLS.apPaterno;
            string apMaterno = oClienteCLS.apMaterno;
            using(var bd=new BDPasajeEntities())
            {
             nregistradosEncontrados=   bd.Cliente.Where(p => p.NOMBRE.Equals(nombre) && p.APPATERNO.Equals(apPaterno)
                && p.APMATERNO.Equals(apMaterno) && !p.IIDCLIENTE.Equals(idcliente)).Count();
            }


            if (!ModelState.IsValid || nregistradosEncontrados>=1)
            {
                if (nregistradosEncontrados >= 1) oClienteCLS.mensajeError = "Ya existe el cliente";
                llenarSexo();
                return View(oClienteCLS);
            }

            using(var bd=new BDPasajeEntities())
            {
                Cliente oCliente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(idcliente)).First();
                oCliente.NOMBRE = oClienteCLS.nombre;
                oCliente.APPATERNO = oClienteCLS.apPaterno;

                oCliente.APMATERNO = oClienteCLS.apMaterno;
                oCliente.EMAIL = oClienteCLS.email;
                oCliente.DIRECCION = oClienteCLS.direccion;
                oCliente.IIDSEXO = oClienteCLS.iidsexo;
                oCliente.TELEFONOCELULAR = oClienteCLS.telefonoCelular;
                oCliente.TELEFONOFIJO = oClienteCLS.telefonoFijo;
                bd.SaveChanges();

            }

            return RedirectToAction("Index");
        }



        List<SelectListItem> listaSexo;

        private void llenarSexo()
        {
            using(var bd=new BDPasajeEntities())
            {
                listaSexo = (from sexo in bd.Sexo
                             where sexo.BHABILITADO == 1
                             select new SelectListItem
                             {
                                 Text = sexo.NOMBRE,
                                 Value = sexo.IIDSEXO.ToString()
                             }).ToList();
                listaSexo.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });

            }
        }



        public ActionResult Agregar()
        {
            llenarSexo();
            ViewBag.lista = listaSexo;

            return View();
        }
        [HttpPost]
        public ActionResult Agregar(ClienteCLS oClienteCLS)
        {
            int nregistrosEncontrados = 0;
            string nombre = oClienteCLS.nombre;
            string apPaterno = oClienteCLS.apPaterno;
            string apMaterno = oClienteCLS.apMaterno;
   
            using(var bd=new BDPasajeEntities())
            {
                nregistrosEncontrados = bd.Cliente.Where(p => p.NOMBRE.Equals(nombre) && p.APPATERNO.Equals(apPaterno)
                  && p.APMATERNO.Equals(apMaterno)).Count();

            }

                if (!ModelState.IsValid || nregistrosEncontrados >=1)
                {
                if (nregistrosEncontrados >= 1) oClienteCLS.mensajeError = "Ya existe cliente registrado";
                    llenarSexo();
                    ViewBag.lista = listaSexo;
                    return View(oClienteCLS);
                }

            using(var bd=new BDPasajeEntities())
            {
                Cliente oCliente = new Cliente();
                oCliente.NOMBRE = oClienteCLS.nombre;
                oCliente.APPATERNO = oClienteCLS.apPaterno;
                oCliente.APMATERNO = oClienteCLS.apMaterno;
                oCliente.EMAIL = oClienteCLS.email;
                oCliente.DIRECCION = oClienteCLS.direccion;
                oCliente.IIDSEXO = oClienteCLS.iidsexo;
                oCliente.TELEFONOCELULAR = oClienteCLS.telefonoCelular;
                oCliente.TELEFONOFIJO = oClienteCLS.telefonoFijo;
                oCliente.BHABILITADO = 1;
                bd.Cliente.Add(oCliente);
                bd.SaveChanges();
            }


            return RedirectToAction("Index");
        }


        public ActionResult Eliminar(int iidcliente)
        {
            using(var bd=new BDPasajeEntities())
            {
                Cliente oCliente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(iidcliente)).First();
                oCliente.BHABILITADO = 0;
                bd.SaveChanges();

                return RedirectToAction("Index");

            }


        }

    }
}
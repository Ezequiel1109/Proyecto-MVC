using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiPrimeraAplicacionWebConEntityFramework.Models;
using System.Transactions;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using MiPrimeraAplicacionWebConEntityFramework.Filters;
using MiPrimeraAplicacionWebConEntityFramework.ClasesAuxiliares;
namespace MiPrimeraAplicacionWebConEntityFramework.Controllers
{
    [Acceder]
    public class UsuarioController : Controller
    {

       public void listaPersonas()
        {
            List<SelectListItem> listaPersonas = new List<SelectListItem>();
            using(var bd=new BDPasajeEntities())
            {
                List<SelectListItem> listaClientes = (from item in bd.Cliente
                                                     where item.BHABILITADO == 1 && item.bTieneUsuario != 1
                                                     select new SelectListItem
                                                     {
                                                         Text = item.NOMBRE + " " + item.APPATERNO + " " + item.APMATERNO+" (C)",
                                                         Value = item.IIDCLIENTE.ToString()
                                                     }).ToList();

                List<SelectListItem> listaEmpleados= (from item in bd.Empleado
                                                      where item.BHABILITADO == 1 && item.bTieneUsuario != 1
                                                      select new SelectListItem
                                                      {
                                                          Text = item.NOMBRE + " " + item.APPATERNO + " " + item.APMATERNO+ " (E)",
                                                          Value = item.IIDEMPLEADO.ToString()
                                                      }).ToList();
                listaPersonas.AddRange(listaClientes);
                listaPersonas.AddRange(listaEmpleados);
                listaPersonas = listaPersonas.OrderBy(p => p.Text).ToList();
                listaPersonas.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaPersona = listaPersonas;
            }
        }

        public void listarRol()
        {
            List<SelectListItem> listaRol;
            using (var bd = new BDPasajeEntities()) {
              listaRol   = (from item in bd.Rol
                                                      where item.BHABILITADO == 1
                                                      select new SelectListItem
                                                      {
                                                          Text = item.NOMBRE ,
                                                          Value = item.IIDROL.ToString()
                                                      }).ToList();
            }
            listaRol.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });

            ViewBag.listaRol = listaRol;
        }


        // GET: Usuario
        public ActionResult Index()
        {
            listaPersonas();
            listarRol();
            List<UsuarioCLS> listaUsuario = new List<UsuarioCLS>();
            using(var bd=new BDPasajeEntities())
            {

                List<UsuarioCLS> listaUsuarioCliente = (from usuario in bd.Usuario
                                                        join cliente in bd.Cliente
                                                        on usuario.IID equals
                                                        cliente.IIDCLIENTE
                                                        join rol in bd.Rol
                                                        on usuario.IIDROL equals rol.IIDROL
                                                        where usuario.bhabilitado == 1
                                                        && usuario.TIPOUSUARIO == "C"
                                                        select new UsuarioCLS
                                                        {
                                                            iidusuario = usuario.IIDUSUARIO,
                                                            nombrePersona = cliente.NOMBRE + " " + cliente.APPATERNO + " " + cliente.APMATERNO,
                                                            nombreusuario = usuario.NOMBREUSUARIO,
                                                            nombreRol = rol.NOMBRE,
                                                            nombreTipoEmpleado = "Cliente"
                                                        }).ToList();


                List<UsuarioCLS> listaUsuarioEmpleado = (from usuario in bd.Usuario
                                                        join empleado in bd.Empleado
                                                        on usuario.IID equals
                                                        empleado.IIDEMPLEADO
                                                        join rol in bd.Rol
                                                        on usuario.IIDROL equals rol.IIDROL
                                                        where usuario.bhabilitado == 1
                                                        && usuario.TIPOUSUARIO == "E"
                                                        select new UsuarioCLS
                                                        {
                                                            iidusuario = usuario.IIDUSUARIO,
                                                            nombrePersona = empleado.NOMBRE + " " + empleado.APPATERNO + " " + empleado.APMATERNO,
                                                            nombreusuario = usuario.NOMBREUSUARIO,
                                                            nombreRol = rol.NOMBRE,
                                                            nombreTipoEmpleado = "Empleado"
                                                        }).ToList();
                listaUsuario.AddRange(listaUsuarioCliente);
                listaUsuario.AddRange(listaUsuarioEmpleado);
                listaUsuario = listaUsuario.OrderBy(p => p.iidusuario).ToList();

            }


            return View(listaUsuario);
        }

        public ActionResult Filtrar(UsuarioCLS oUsuarioCLS)
        {
            string nombrePersona = oUsuarioCLS.nombrePersona;
            listaPersonas();
            listarRol();
            List<UsuarioCLS> listaUsuario = new List<UsuarioCLS>();
            using (var bd = new BDPasajeEntities())
            {
                if (oUsuarioCLS.nombrePersona == null)
                {
                    List<UsuarioCLS> listaUsuarioCliente = (from usuario in bd.Usuario
                                                            join cliente in bd.Cliente
                                                            on usuario.IID equals
                                                            cliente.IIDCLIENTE
                                                            join rol in bd.Rol
                                                            on usuario.IIDROL equals rol.IIDROL
                                                            where usuario.bhabilitado == 1
                                                            && usuario.TIPOUSUARIO == "C"
                                                            select new UsuarioCLS
                                                            {
                                                                iidusuario = usuario.IIDUSUARIO,
                                                                nombrePersona = cliente.NOMBRE + " " + cliente.APPATERNO + " " + cliente.APMATERNO,
                                                                nombreusuario = usuario.NOMBREUSUARIO,
                                                                nombreRol = rol.NOMBRE,
                                                                nombreTipoEmpleado = "Cliente"
                                                            }).ToList();


                    List<UsuarioCLS> listaUsuarioEmpleado = (from usuario in bd.Usuario
                                                             join empleado in bd.Empleado
                                                             on usuario.IID equals
                                                             empleado.IIDEMPLEADO
                                                             join rol in bd.Rol
                                                             on usuario.IIDROL equals rol.IIDROL
                                                             where usuario.bhabilitado == 1
                                                             && usuario.TIPOUSUARIO == "E"
                                                             select new UsuarioCLS
                                                             {
                                                                 iidusuario = usuario.IIDUSUARIO,
                                                                 nombrePersona = empleado.NOMBRE + " " + empleado.APPATERNO + " " + empleado.APMATERNO,
                                                                 nombreusuario = usuario.NOMBREUSUARIO,
                                                                 nombreRol = rol.NOMBRE,
                                                                 nombreTipoEmpleado = "Empleado"
                                                             }).ToList();
                    listaUsuario.AddRange(listaUsuarioCliente);
                    listaUsuario.AddRange(listaUsuarioEmpleado);
                    listaUsuario = listaUsuario.OrderBy(p => p.iidusuario).ToList();
                }else
                {
                    List<UsuarioCLS> listaUsuarioCliente = (from usuario in bd.Usuario
                                                            join cliente in bd.Cliente
                                                            on usuario.IID equals
                                                            cliente.IIDCLIENTE
                                                            join rol in bd.Rol
                                                            on usuario.IIDROL equals rol.IIDROL
                                                            where usuario.bhabilitado == 1
                                                            && (
                                                            cliente.NOMBRE.Contains(nombrePersona)
                                                             || cliente.APPATERNO.Contains(nombrePersona)
                                                              || cliente.APMATERNO.Contains(nombrePersona)
                                                            )
                                                            && usuario.TIPOUSUARIO == "C"
                                                            select new UsuarioCLS
                                                            {
                                                                iidusuario = usuario.IIDUSUARIO,
                                                                nombrePersona = cliente.NOMBRE + " " + cliente.APPATERNO + " " + cliente.APMATERNO,
                                                                nombreusuario = usuario.NOMBREUSUARIO,
                                                                nombreRol = rol.NOMBRE,
                                                                nombreTipoEmpleado = "Cliente"
                                                            }).ToList();


                    List<UsuarioCLS> listaUsuarioEmpleado = (from usuario in bd.Usuario
                                                             join empleado in bd.Empleado
                                                             on usuario.IID equals
                                                             empleado.IIDEMPLEADO
                                                             join rol in bd.Rol
                                                             on usuario.IIDROL equals rol.IIDROL
                                                             where usuario.bhabilitado == 1
                                                             && usuario.TIPOUSUARIO == "E" &&
                                                              (
                                                            empleado.NOMBRE.Contains(nombrePersona)
                                                             || empleado.APPATERNO.Contains(nombrePersona)
                                                              || empleado.APMATERNO.Contains(nombrePersona)
                                                            )
                                                             select new UsuarioCLS
                                                             {
                                                                 iidusuario = usuario.IIDUSUARIO,
                                                                 nombrePersona = empleado.NOMBRE + " " + empleado.APPATERNO + " " + empleado.APMATERNO,
                                                                 nombreusuario = usuario.NOMBREUSUARIO,
                                                                 nombreRol = rol.NOMBRE,
                                                                 nombreTipoEmpleado = "Empleado"
                                                             }).ToList();
                    listaUsuario.AddRange(listaUsuarioCliente);
                    listaUsuario.AddRange(listaUsuarioEmpleado);
                    listaUsuario = listaUsuario.OrderBy(p => p.iidusuario).ToList();
                }

            }


            return PartialView("_TablaUsuario",listaUsuario);
        }




        public string Guardar(UsuarioCLS oUsuarioCLS , int titulo)
        {
            //Vacio es error
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
                else {

                    using (var bd = new BDPasajeEntities())
                    {
                        int cantidad = 0;
                        using (var transaccion = new TransactionScope())
                        {
                            if (titulo.Equals(-1))
                            {
                                cantidad = bd.Usuario.Where(p => p.NOMBREUSUARIO == oUsuarioCLS.nombreusuario).Count();
                                if (cantidad >= 1)
                                {
                                    rpta = "-1";
                                }
                                else
                                {
                                    Usuario oUsuario = new Usuario();
                                    oUsuario.NOMBREUSUARIO = oUsuarioCLS.nombreusuario;
                                    SHA256Managed sha = new SHA256Managed();
                                    byte[] byteContra = Encoding.Default.GetBytes(oUsuarioCLS.contra);
                                    byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                                    string cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
                                    oUsuario.CONTRA = cadenaContraCifrada;
                                    oUsuario.TIPOUSUARIO = oUsuarioCLS.nombrePersonaEnviar.Substring(oUsuarioCLS.nombrePersonaEnviar.Length - 2, 1);
                                    oUsuario.IID = oUsuarioCLS.iid;
                                    oUsuario.IIDROL = oUsuarioCLS.iidrol;
                                    oUsuario.bhabilitado = 1;
                                    bd.Usuario.Add(oUsuario);
                                    Cliente oCliente=null;
                                    if (oUsuario.TIPOUSUARIO.Equals("C"))
                                    {
                                        oCliente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(oUsuarioCLS.iid)).First();
                                        oCliente.bTieneUsuario = 1;
                                    }
                                    else
                                    {
                                        Empleado oEmpleado = bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(oUsuarioCLS.iid)).First();
                                        oEmpleado.bTieneUsuario = 1;
                                    }
                                    rpta = bd.SaveChanges().ToString();
                                    if (rpta == "0") rpta = "";


                                    transaccion.Complete();
                                    if(rpta=="2" && oUsuario.TIPOUSUARIO == "C")
                                    {
                                      string nombreCompleto= (string) Session["nombreCompleto"];
                                        //Buscar el nombre del correo del cliente registrado
                                        string ruta = Server.MapPath("~/Archivos/PersonasCorreo.txt");
                                        int n=  CORREO.enviarCorreo(oCliente.EMAIL,"Bienvenido al sistema",
                                            "<h1>BIENVENIDO AL SISTEMA "+ oCliente.NOMBRE+" " + oCliente.APPATERNO+ " "+oCliente.APMATERNO+ ": </h1> Muchas gracias por formar parte de este curso", ruta);
                                    }
                                }
                            }else
                            {
                                cantidad = bd.Usuario.Where(p => p.NOMBREUSUARIO == oUsuarioCLS.nombreusuario
                                && p.IIDUSUARIO!= titulo).Count();

                                if (cantidad >= 1)
                                {
                                    rpta = "-1";
                                }
                                else
                                {
                                    //Editar
                                    Usuario ousuario = bd.Usuario.Where(p => p.IIDUSUARIO == titulo).First();
                                    ousuario.IIDROL = oUsuarioCLS.iidrol;
                                    ousuario.NOMBREUSUARIO = oUsuarioCLS.nombreusuario;
                                    rpta = bd.SaveChanges().ToString();
                                    transaccion.Complete();
                                }

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


        public int Eliminar(int idUsuario)
        {
            int rpta = 0;
            try
            {
                using (BDPasajeEntities bd = new BDPasajeEntities())
                {

                    Usuario oUsuario = bd.Usuario.Where(p => p.IIDUSUARIO == idUsuario).First();
                    oUsuario.bhabilitado = 0;
                    rpta = bd.SaveChanges();


                }
            }catch(Exception ex)
            {
                rpta = 0;
            }

            return rpta;
        }

        public JsonResult recuperarInformacion(int iidusuario)
        {
            UsuarioCLS oUsuarioCLS = new UsuarioCLS();
            using (var bd=new BDPasajeEntities())
            {
                Usuario oUsuario = bd.Usuario.Where(p => p.IIDUSUARIO == iidusuario).First();
                oUsuarioCLS.nombreusuario = oUsuario.NOMBREUSUARIO;
                oUsuarioCLS.iidrol =(int) oUsuario.IIDROL;             
            }
            return Json(oUsuarioCLS, JsonRequestBehavior.AllowGet);


        }


    }
}
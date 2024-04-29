using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiPrimeraAplicacionWebConEntityFramework.Models;
using System.Security.Cryptography;
using System.Text;
using MiPrimeraAplicacionWebConEntityFramework.ClasesAuxiliares;

namespace MiPrimeraAplicacionWebConEntityFramework.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CerrarSession()
        {
            Session["Usuario"] = null;
            Session["Rol"] = null;
            return RedirectToAction("Index");
        }

        public string Login(UsuarioCLS oUsuario)
        {
            string mensaje = "";
           
            //Error
            if (!ModelState.IsValid)
            {
                var query = (from state in ModelState.Values
                             from error in state.Errors
                             select error.ErrorMessage).ToList();
                mensaje += "<ul class='list-group'>";
                foreach (var item in query)
                {
                    mensaje += "<li class='list-group-item'>" + item + "</li>";
                }
                mensaje += "</ul>";
            }
            //Bien
            else
            {
                string nombreUsuario = oUsuario.nombreusuario;
                string password = oUsuario.contra;
                //Cifrar
                SHA256Managed sha = new SHA256Managed();
                byte[] byteContra = Encoding.Default.GetBytes(password);
                byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                //Eso es lo que cuenta
                string cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
                using (var bd = new BDPasajeEntities())
                {

                    int numeroVeces = bd.Usuario.Where(p => p.NOMBREUSUARIO == nombreUsuario
                     && p.CONTRA == cadenaContraCifrada).Count();
                     mensaje = numeroVeces.ToString();
                    if (mensaje == "0") mensaje = "Usuario o contraseña incorrecta";
                    else
                    {
                        Usuario ousuario= bd.Usuario.Where(p => p.NOMBREUSUARIO == nombreUsuario
                     && p.CONTRA == cadenaContraCifrada).First();
                        //Todo el objeto Usuario
                        Session["Usuario"] = ousuario;
                        if (ousuario.TIPOUSUARIO == "C")
                        {
                            Cliente oCliente = bd.Cliente.Where(p => p.IIDCLIENTE == ousuario.IID).First();
                            Session["nombreCompleto"] = oCliente.NOMBRE + " " + oCliente.APPATERNO + " " + oCliente.APMATERNO;

                        }
                        else
                        {
                            Empleado oEmpleado = bd.Empleado.Where(p => p.IIDEMPLEADO == ousuario.IID).First();
                            Session["nombreCompleto"] = oEmpleado.NOMBRE + " " + oEmpleado.APPATERNO + " " + oEmpleado.APMATERNO;

                        }

                        List<MenuCLS> listaMenu = (from usuario in bd.Usuario
                                                   join rol in bd.Rol
                                                   on usuario.IIDROL equals rol.IIDROL
                                                   join rolpagina in bd.RolPagina
                                                   on rol.IIDROL equals rolpagina.IIDROL
                                                   join pagina in bd.Pagina
                                                   on rolpagina.IIDPAGINA equals pagina.IIDPAGINA
                                                   where rol.IIDROL == ousuario.IIDROL && rolpagina.IIDROL == usuario.IIDROL
                                                   && usuario.IIDUSUARIO == ousuario.IIDUSUARIO
                                                   select new MenuCLS
                                                   {
                                                       nombreAccion = pagina.ACCION,
                                                       nombreControlador = pagina.CONTROLADOR,
                                                       mensaje = pagina.MENSAJE
                                                   }).ToList();

                                         Session["Rol"] = listaMenu;
                    }
                }
            }

            return mensaje;
        }

        public string RecuperarContra(string IIDTIPO,string correo, string telefonoCelular)
        {
            string mensaje = "";
            using(var bd=new BDPasajeEntities())
            {
                int cantidad = 0;
                int iidcliente;
                if (IIDTIPO == "C")
                {
                    //Existe un cliente con esa informacion
                    cantidad= bd.Cliente.Where(p => p.EMAIL == correo && p.TELEFONOCELULAR == telefonoCelular).Count();
                }
                if (cantidad == 0) mensaje = "No existe una persona registrada con esa informacion";
                else
                {
                 iidcliente = bd.Cliente.Where(p => p.EMAIL == correo && p.TELEFONOCELULAR == telefonoCelular).First().IIDCLIENTE;
                    //Verificar si existe el usuario
                 int nveces=   bd.Usuario.Where(p => p.IID == iidcliente && p.TIPOUSUARIO == "C").Count();
                    if (nveces == 0)
                    {
                        mensaje = "No tiene usuario";
                    }
                    else {
                        //Ontener su Id
                        Usuario ousuario = bd.Usuario.Where(p => p.IID == iidcliente && p.TIPOUSUARIO == "C").First();
                        //Modificar su clave
                        Random ra = new Random();
                        int n1 = ra.Next(0, 9);
                        int n2 = ra.Next(0, 9);
                        int n3 = ra.Next(0, 9);
                        int n4 = ra.Next(0, 9);
                        string nuevaContra = n1.ToString() + n2 + n3 + n4;
                        //Cifrar clave

                        SHA256Managed sha = new SHA256Managed();
                        byte[] byteContra = Encoding.Default.GetBytes(nuevaContra);
                        byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                        string cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");

                        ousuario.CONTRA = cadenaContraCifrada;
                        mensaje= bd.SaveChanges().ToString();
                        CORREO.enviarCorreo(correo, "Recuperar Clave","Se reseteo su clave , ahora su clave es :"+ nuevaContra, "");
                    }
                }
            }
            return mensaje;
        }



    }
}
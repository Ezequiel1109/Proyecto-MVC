using MiPrimeraAplicacionWebConEntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace MiPrimeraAplicacionWebConEntityFramework.Controllers
{
    public class MisReservasController : Controller
    {
        // GET: MisReservas
        public ActionResult Index()
        {
            List<ReservaCLS> listaReserva = new List<ReservaCLS>();
            var pasajesId = ControllerContext.HttpContext.Request.Cookies["pasajesId"];
            var pasajesCantidad = ControllerContext.HttpContext.Request.Cookies["pasajesCantidad"];

            if (pasajesCantidad != null)
            {
                string valorId = pasajesId.Value;
                string valorCantidad = pasajesCantidad.Value;

                string[] arrayELementos= valorCantidad.Split('{');

                //6*5*3*14/12/12
                //[6,5,3,14,12/08/2019]
                //7*8*3*12/03/29
                string[] arrayIds = valorId.Split('{');
                string[] reserva;
                if (arrayIds[0] != "")
                {

                    for(int i = 0; i < arrayELementos.Count(); i++)
                    {
                        ReservaCLS oReservaCLS = new ReservaCLS();
                        reserva = arrayELementos[i].Split('*');
                        oReservaCLS.iidViaje = int.Parse(arrayIds[i]);
                        oReservaCLS.cantidad = int.Parse(reserva[0]);
                        oReservaCLS.fechaViaje = DateTime.Parse(reserva[1]);
                        oReservaCLS.lugarOrigen = reserva[2];
                        oReservaCLS.lugarDestino = reserva[3];
                        oReservaCLS.precio =decimal.Parse( reserva[4]);

                        listaReserva.Add(oReservaCLS);
                    }


                }

            }

           

            return View(listaReserva);
        }


        ///////////////
        public ActionResult Filtrar()
        {
            List<ReservaCLS> listaReserva = new List<ReservaCLS>();
            var pasajesId = ControllerContext.HttpContext.Request.Cookies["pasajesId"];
            var pasajesCantidad = ControllerContext.HttpContext.Request.Cookies["pasajesCantidad"];

            if (pasajesCantidad != null)
            {
                string valorId = pasajesId.Value;
                string valorCantidad = pasajesCantidad.Value;

                string[] arrayELementos = valorCantidad.Split('{');

                //6*5*3*14/12/12
                //[6,5,3,14,12/08/2019]
                //7*8*3*12/03/29
                string[] arrayIds = valorId.Split('{');
                string[] reserva;
                if (arrayIds[0] != "")
                {

                    for (int i = 0; i < arrayELementos.Count(); i++)
                    {
                        ReservaCLS oReservaCLS = new ReservaCLS();
                        reserva = arrayELementos[i].Split('*');
                        oReservaCLS.iidViaje = int.Parse(arrayIds[i]);
                        oReservaCLS.cantidad = int.Parse(reserva[0]);
                        oReservaCLS.fechaViaje = DateTime.Parse(reserva[1]);
                        oReservaCLS.lugarOrigen = reserva[2];
                        oReservaCLS.lugarDestino = reserva[3];
                        oReservaCLS.precio = decimal.Parse(reserva[4]);
                        oReservaCLS.iidBus = int.Parse(reserva[5]);

                        listaReserva.Add(oReservaCLS);
                    }


                }

            }



            return PartialView("_TablaMisReservas",listaReserva);
        }


        public string GuardarDatos(string total)
        {
            string cadena = "";
            try
            {
                var pasajesId = ControllerContext.HttpContext.Request.Cookies["pasajesId"];
                var pasajesCantidad = ControllerContext.HttpContext.Request.Cookies["pasajesCantidad"];
                if (pasajesCantidad != null)
                {
                  string[] arrayElementos=  pasajesCantidad.Value.Split('{');
                  string[] reserva;
                  string[] arrayIds = pasajesId.Value.Split('{');
                    using(var transaccion=new TransactionScope())
                    {

                        using (var bd = new BDPasajeEntities())
                        {
                            VENTA oVenta = new VENTA();
                            Usuario oUsuario = (Usuario)Session["Usuario"];
                            oVenta.TOTAL = decimal.Parse(total);
                            oVenta.FECHAVENTA = DateTime.Now;
                            oVenta.BHABILITADO = 1;
                            oVenta.IIDUSUARIO = oUsuario.IIDUSUARIO;
                            bd.VENTA.Add(oVenta);
                            bd.SaveChanges();
                            int idVenta = oVenta.IIDVENTA;
                            for(int i=0;i< arrayElementos.Count(); i++)
                            {
                                reserva = arrayElementos[i].Split('*');
                                DETALLEVENTA oDetalleVenta = new DETALLEVENTA();
                                oDetalleVenta.IIDVENTA = idVenta;
                                int idViaje = int.Parse(arrayIds[i]);
                                oDetalleVenta.IIDVIAJE = idViaje;
                                oDetalleVenta.IIDBUS = int.Parse(reserva[5]);
                                int cantidad = int.Parse(reserva[0]);
                                oDetalleVenta.cantidad = cantidad;
                                oDetalleVenta.PRECIO = int.Parse(reserva[4]);
                                oDetalleVenta.BHABILITADO = 1;

                                bd.DETALLEVENTA.Add(oDetalleVenta);

                                //Diminuir el Stock

                                Viaje oVentaBusqueda = bd.Viaje.Where(p => p.IIDVIAJE == idViaje).First();
                                oVentaBusqueda.NUMEROASIENTOSDISPONIBLES = oVentaBusqueda.NUMEROASIENTOSDISPONIBLES -cantidad;
                                bd.SaveChanges();


                            }
                            bd.SaveChanges();
                            transaccion.Complete();
                            cadena = "OK";
                            HttpCookie cookieId = new HttpCookie("pasajesId", "");
                            HttpCookie cookieCantidad = new HttpCookie("pasajesCantidad", "");

                            ControllerContext.HttpContext.Response.SetCookie(cookieId);
                            ControllerContext.HttpContext.Response.SetCookie(cookieCantidad);
                        }




                    }

                }
            }
            catch(Exception ex)
            {
                cadena = "";
            }



            return cadena;
        }

    }
}
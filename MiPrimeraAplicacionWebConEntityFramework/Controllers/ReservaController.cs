using MiPrimeraAplicacionWebConEntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiPrimeraAplicacionWebConEntityFramework.Controllers
{
    public class ReservaController : Controller
    {
        // GET: Reserva (LIstado)
        public ActionResult Index()
        {
            listarLugar();

            var pasajesId = ControllerContext.HttpContext.Request.Cookies["pasajesId"];
            var pasajesCantidad = ControllerContext.HttpContext.Request.Cookies["pasajesCantidad"];

            if (pasajesId != null)
            {
                ViewBag.listaId = pasajesId.Value;
                ViewBag.listaCantidad = pasajesId.Value;

            }



            using (var bd=new BDPasajeEntities())
            {
                var reserva = (from viaje in bd.Viaje
                               join lugar in bd.Lugar
                               on viaje.IIDLUGARORIGEN equals lugar.IIDLUGAR
                               join bus in bd.Bus
                               on viaje.IIDBUS equals bus.IIDBUS
                               join lugarDes in bd.Lugar
                               on viaje.IIDLUGARDESTINO equals lugarDes.IIDLUGAR
                               where viaje.BHABILITADO == 1
                               select new ReservaCLS
                               {
                                   iidViaje=viaje.IIDVIAJE,
                                   nombreArchivo=viaje.nombrefoto,
                                   foto=viaje.FOTO,
                                   lugarOrigen=lugar.NOMBRE,
                                   lugarDestino=lugarDes.NOMBRE,
                                   precio=(decimal)viaje.PRECIO,
                                   fechaViaje=(DateTime)viaje.FECHAVIAJE,
                                   nombreBus=bus.PLACA,
                                   descripcionBus=bus.DESCRIPCION,
                                   totalAsientos= (int)bus.NUMEROCOLUMNAS * (int)bus.NUMEROFILAS,
                                   asientosDisponibles=(int)viaje.NUMEROASIENTOSDISPONIBLES,
                                   iidBus=bus.IIDBUS
                               }).ToList();
                return View(reserva);
            }

            
        }
        //Lugar

            public string Agregarcookie(string idViaje , string cantidad,
                string fechaViaje , string lugarOrigen , string lugarDestino,
                string precio , int iidBus)
        {
            string rpta = "";

            try
            {
                var pasajesId = ControllerContext.HttpContext.Request.Cookies["pasajesId"];
                var pasajesCantidad = ControllerContext.HttpContext.Request.Cookies["pasajesCantidad"];
                if(pasajesId!=null && pasajesCantidad!=null && pasajesCantidad.Value!=""
                    && pasajesId.Value != "")
                {
                    //POr segunda vez

                    //5{3 
                    string idCookie = pasajesId.Value + "{" + idViaje;
                    string cantidadCookie = pasajesCantidad.Value + "{" +
                        cantidad + "*" + fechaViaje + "*" + lugarOrigen + "*" + lugarDestino + "*"
                            + precio + "*" + iidBus;

                    HttpCookie cookieId = new HttpCookie("pasajesId", idCookie);
                    HttpCookie cookieCantidad = new HttpCookie("pasajesCantidad", cantidadCookie);

                    ControllerContext.HttpContext.Response.SetCookie(cookieId);
                    ControllerContext.HttpContext.Response.SetCookie(cookieCantidad);

                }
                else
                {
                    //pasajesCantidad (Toda la data menos el IdViaje)

                    string formatoCadena = cantidad + "*" + fechaViaje + "*" + lugarOrigen + "*" + lugarDestino + "*"
                            + precio + "*" + iidBus;

                    HttpCookie cookieId = new HttpCookie("pasajesId", idViaje);
                    HttpCookie cookieCantidad = new HttpCookie("pasajesCantidad", formatoCadena);

                    ControllerContext.HttpContext.Response.SetCookie(cookieId);
                    ControllerContext.HttpContext.Response.SetCookie(cookieCantidad);



                    //Se crea por primera vez
                }
                rpta = "OK";
            }
            catch (Exception)
            {

                rpta = "";
            }


            return rpta;
        }


        public void listarLugar()
        {
            //agregar
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from item in bd.Lugar
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDLUGAR.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaLugar = lista;
            }
        }

        public string Quitarcookie(string idViaje) {
            string rpta = "";
            try
            {
                var pasajesId = ControllerContext.HttpContext.Request.Cookies["pasajesId"];
                var pasajesCantidad = ControllerContext.HttpContext.Request.Cookies["pasajesCantidad"];
                string valorId = pasajesId.Value;
                string valorCantidad = pasajesCantidad.Value;
                string[] arrayId = valorId.Split('{');
                int indiceId = Array.IndexOf(arrayId, idViaje);

                //6{7{9{2    (Texto ya esta)

                //2
                // Caso 1: .Replace("{2","")

                //Caso 2 : .Replace("6{","")
                
                //6

                //Caso 3 :  .Replace("6","")

                string nuevoId;
                if (valorId.Contains("{" + idViaje))
                {
                    nuevoId = valorId.Replace("{" + idViaje, "");
                }else if (valorId.Contains(idViaje + "{"))
                {
                    nuevoId = valorId.Replace(idViaje + "{", "");
                }else
                {
                    nuevoId = valorId.Replace(idViaje, "");
                }
                //Texto que tiene las cantidades

                List<string> valor = valorCantidad.Split('{').ToList();
                valor.RemoveAt(indiceId);
                string[] arrayCantidad=  valor.ToArray();
                string nuevaCantidad = String.Join("{", arrayCantidad);

                HttpCookie cookieId = new HttpCookie("pasajesId", nuevoId);
                HttpCookie cookieCantidad = new HttpCookie("pasajesCantidad", nuevaCantidad);

                ControllerContext.HttpContext.Response.SetCookie(cookieId);
                ControllerContext.HttpContext.Response.SetCookie(cookieCantidad);

                rpta = "OK";

            }
            catch (Exception ex)
            {

                rpta = "";
            }

            return rpta;

            }



    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiPrimeraAplicacionWebConEntityFramework.Models;
using System.IO;

namespace MiPrimeraAplicacionWebConEntityFramework.Controllers
{
    public class ViajeController : Controller
    {
        // GET: Viaje

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

        public void listarBus()
        {
            //agregar
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from item in bd.Bus
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.PLACA,
                             Value = item.IIDBUS.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaBus = lista;
            }
        }

        public void listarCombos()
        {
            listarLugar();
            listarBus();
        }




        public ActionResult Agregar()
        {
            listarCombos();
            return View();
        }

        public ActionResult Index()
        {
            List<ViajeCLS> listaViaje = null;
            listarCombos();
            using (var bd=new BDPasajeEntities())
            {
                listaViaje = (from viaje in bd.Viaje
                              join lugarOrigen in bd.Lugar
                              on viaje.IIDLUGARORIGEN equals lugarOrigen.IIDLUGAR
                              join lugarDestino in bd.Lugar
                              on viaje.IIDLUGARDESTINO equals lugarDestino.IIDLUGAR
                              join bus in bd.Bus
                              on viaje.IIDBUS equals bus.IIDBUS
                              where viaje.BHABILITADO==1
                              select new ViajeCLS
                              {
                                  iidViaje = viaje.IIDVIAJE,
                                  nombreBus = bus.PLACA,
                                  nombreLugarOrigen = lugarOrigen.NOMBRE,
                                  nombreLugarDestino = lugarDestino.NOMBRE

                              }).ToList();

            }
            return View(listaViaje);
        }

        public ActionResult Filtrar(int? lugarDestinoParametro)
        {
            List<ViajeCLS> listaViaje = new List<ViajeCLS>();
            using(var bd=new BDPasajeEntities())
            {
                if (lugarDestinoParametro == null)
                {
                    listaViaje = (from viaje in bd.Viaje
                                  join lugarOrigen in bd.Lugar
                                  on viaje.IIDLUGARORIGEN equals lugarOrigen.IIDLUGAR
                                  join lugarDestino in bd.Lugar
                                  on viaje.IIDLUGARDESTINO equals lugarDestino.IIDLUGAR
                                  join bus in bd.Bus
                                  on viaje.IIDBUS equals bus.IIDBUS
                                  where viaje.BHABILITADO == 1
                                  select new ViajeCLS
                                  {
                                      iidViaje = viaje.IIDVIAJE,
                                      nombreBus = bus.PLACA,
                                      nombreLugarOrigen = lugarOrigen.NOMBRE,
                                      nombreLugarDestino = lugarDestino.NOMBRE

                                  }).ToList();
                }else
                {
                    listaViaje = (from viaje in bd.Viaje
                                  join lugarOrigen in bd.Lugar
                                  on viaje.IIDLUGARORIGEN equals lugarOrigen.IIDLUGAR
                                  join lugarDestino in bd.Lugar
                                  on viaje.IIDLUGARDESTINO equals lugarDestino.IIDLUGAR
                                  join bus in bd.Bus
                                  on viaje.IIDBUS equals bus.IIDBUS
                                  where viaje.BHABILITADO == 1
                                  && viaje.IIDLUGARDESTINO== lugarDestinoParametro
                                  select new ViajeCLS
                                  {
                                      iidViaje = viaje.IIDVIAJE,
                                      nombreBus = bus.PLACA,
                                      nombreLugarOrigen = lugarOrigen.NOMBRE,
                                      nombreLugarDestino = lugarDestino.NOMBRE

                                  }).ToList();
                }


            }
            return PartialView("_TablaViaje", listaViaje);
           
        }


       
        public string Guardar(ViajeCLS oViajeCls , HttpPostedFileBase foto , int titulo)
        {
            string mensaje = "";
            try
            {
                if(!ModelState.IsValid || (foto==null && titulo==-1))
                {
                    var query = (from state in ModelState.Values
                                 from error in state.Errors
                                 select error.ErrorMessage).ToList();
                    if (foto == null && titulo == -1)
                    {
                        oViajeCls.mensaje = "La foto es obligatoria";
                        mensaje += "<ul><li> Debe ingresar la foto</li></ul>";
                    }
                    mensaje += "<ul class='list-group'>";
                    foreach (var item in query)
                    {
                        mensaje += "<li class='list-group-item'>" + item + "</li>";
                    }
                    mensaje += "</ul>";

                }else
                {
                    byte[] fotoBD = null;
                    if (foto != null)
                    {
                        BinaryReader lector = new BinaryReader(foto.InputStream);
                        fotoBD = lector.ReadBytes((int)foto.ContentLength);
                    }
                    using(var bd=new BDPasajeEntities())
                    {
                        if (titulo == -1)
                        {
                            Viaje oViaje = new Viaje();
                            oViaje.IIDBUS = oViajeCls.iidBus;
                            oViaje.IIDLUGARDESTINO = oViajeCls.iidLugarDestino;
                            oViaje.IIDLUGARORIGEN = oViajeCls.iidLugarOrigen;
                            oViaje.PRECIO = oViajeCls.precio;
                            oViaje.FECHAVIAJE = oViajeCls.fechaViaje;
                            oViaje.NUMEROASIENTOSDISPONIBLES = oViajeCls.numeroAsientosDisponibles;
                            oViaje.FOTO = fotoBD;
                            oViaje.nombrefoto = oViajeCls.nombreFoto;
                            oViaje.BHABILITADO = 1;
                            bd.Viaje.Add(oViaje);
                            mensaje = bd.SaveChanges().ToString();
                            if (mensaje == "0") mensaje = "";


                        }else
                        {
                            Viaje oViaje = bd.Viaje.Where(p => p.IIDVIAJE == titulo).First();
                            oViaje.IIDLUGARDESTINO = oViajeCls.iidLugarDestino;

                            oViaje.IIDLUGARORIGEN = oViajeCls.iidLugarOrigen;
                            oViaje.PRECIO = oViajeCls.precio;
                            oViaje.FECHAVIAJE = oViajeCls.fechaViaje;
                            oViaje.IIDBUS = oViajeCls.iidBus;
                            oViaje.NUMEROASIENTOSDISPONIBLES = oViajeCls.numeroAsientosDisponibles;
                            if (foto != null) oViaje.FOTO = fotoBD;
                            mensaje = bd.SaveChanges().ToString();

                        }

                    }

                }



            }catch(Exception ex)
            {
                mensaje = "";
            }



            return mensaje;

        }

        public int EliminarViaje(int idViaje)
        {
            int nregistrosAfectados = 0;
            try
            {
                using(var bd=new BDPasajeEntities())
                {
                    Viaje oViaje = bd.Viaje.Where(p => p.IIDVIAJE == idViaje).First();
                    oViaje.BHABILITADO = 0;
                    nregistrosAfectados = bd.SaveChanges();
                }

            }catch(Exception ex)
            {

                nregistrosAfectados = 0;
            }

            return nregistrosAfectados;
        }


        public JsonResult recuperarInformacion(int idViaje)
        {
            ViajeCLS oViajeCls = new ViajeCLS();
            using(var bd=new BDPasajeEntities())
            {
                Viaje oViaje = bd.Viaje.Where(p => p.IIDVIAJE == idViaje).First();
                oViajeCls.iidViaje = oViaje.IIDVIAJE;
                oViajeCls.iidBus = (int)oViaje.IIDBUS;
                oViajeCls.iidLugarDestino = (int)oViaje.IIDLUGARDESTINO;
                oViajeCls.iidLugarOrigen = (int)oViaje.IIDLUGARORIGEN;
                oViajeCls.precio =(decimal) oViaje.PRECIO;
                //año-mes-dia  (pide)
                //bd  (dia-mes-anio)
                oViajeCls.fechaViajeCadena = oViaje.FECHAVIAJE!=null ?
                    ((DateTime)oViaje.FECHAVIAJE).ToString("yyyy-MM-ddTHH:mm") : "";
                oViajeCls.numeroAsientosDisponibles = (int)oViaje.NUMEROASIENTOSDISPONIBLES;
                oViajeCls.nombreFoto = oViaje.nombrefoto;

                oViajeCls.extension = Path.GetExtension(oViaje.nombrefoto);
                oViajeCls.fotoRecuperCadena =Convert.ToBase64String( oViaje.FOTO);

            }

            return Json(oViajeCls, JsonRequestBehavior.AllowGet);
        }





    }
}
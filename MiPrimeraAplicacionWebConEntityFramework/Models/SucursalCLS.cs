using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiPrimeraAplicacionWebConEntityFramework.Models
{
    public class SucursalCLS
    {
        [Display(Name ="Id Sucursal")]
        public int iidsucursal { get; set; }
        [Display(Name ="Nombre Sucursal")]
        [StringLength(100,ErrorMessage ="Longitud maxima 100")]
        [Required]
        public string nombre { get; set; }
        [Display(Name ="Direccion")]
        [Required]
        [StringLength(200, ErrorMessage = "Longitud maxima 200")]
        public string direccion { get; set; }
        [Display(Name ="Telefono Sucursal")]
        [Required]
        [StringLength(10, ErrorMessage = "Longitud maxima 10")]
        public string telefono { get; set; }
        [Display(Name ="Email Sucursal")]
        [Required]
        [StringLength(100, ErrorMessage = "Longitud maxima 100")]
        [EmailAddress(ErrorMessage ="Ingrese un email valido")]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        [Display(Name ="Fecha Apertura")]
        public DateTime fechaApertura { get; set; }

        public int bhabilitado { get; set; }
        //Propiedad adicional

        public string mensajeError { get; set; }
    }
}
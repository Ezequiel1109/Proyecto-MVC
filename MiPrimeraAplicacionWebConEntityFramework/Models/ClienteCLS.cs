using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiPrimeraAplicacionWebConEntityFramework.Models
{
    public class ClienteCLS
    {
        [Display(Name ="Id Cliente")]
        public int iidcliente { get; set; }
        [Display(Name ="Nombre cliente")]
        [Required]
        [StringLength(100,ErrorMessage ="Longitud maxima 100")]
        public string nombre { get; set; }
        [Display(Name ="Apellido Paterno")]
        [Required]
        [StringLength(150, ErrorMessage = "Longitud maxima 150")]
        public string apPaterno { get; set; }
        [Display(Name ="Apellido Materno")]
        [Required]
        [StringLength(150, ErrorMessage = "Longitud maxima 150")]
        public string apMaterno { get; set; }
        [Required]
        [Display(Name ="Email")]
        [StringLength(200, ErrorMessage = "Longitud maxima 200")]
        [EmailAddress(ErrorMessage ="Ingrese un email valido")]
        public string email { get; set; }
        [Required]
        [Display(Name = "Direccion")]
        [DataType(DataType.MultilineText)]
        [StringLength(200, ErrorMessage = "Longitud maxima 200")]
        public string direccion { get; set; }
        [Required]
        [Display(Name = "Sexo")]
        public int iidsexo { get; set; }
        [Display(Name ="Telefono Fijo")]
        [Required]
        [StringLength(10, ErrorMessage = "Longitud maxima 10")]
        public string telefonoFijo { get; set; }
        [Required]
        [Display(Name = "Telefono Celular")]
        [StringLength(10, ErrorMessage = "Longitud maxima 10")]
        public string telefonoCelular { get; set; }
        public int bhabilitado { get; set; }

        //Propiedad adicional

        public string mensajeError { get; set; }

    }
}
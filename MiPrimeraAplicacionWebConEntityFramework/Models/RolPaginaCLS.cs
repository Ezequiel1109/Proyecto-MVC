using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiPrimeraAplicacionWebConEntityFramework.Models
{
    public class RolPaginaCLS
    {
        [Display(Name = "Id Rol Pagina")]
        public int iidrolpagina { get; set; }
        [Required]
        public int iidrol { get; set; }
        [Required]
        public int iidpagina { get; set; }
        public int bhabilitado { get; set; }
        //Propiedades adicionales
        [Display(Name ="Nombre Rol")]
        public string nombreRol { get; set; }
        [Display(Name = "Nombre Mensaje")]
        public string nombreMensaje { get; set; }
    }
}
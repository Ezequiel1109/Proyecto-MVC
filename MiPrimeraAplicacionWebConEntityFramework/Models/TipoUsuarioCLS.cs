using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiPrimeraAplicacionWebConEntityFramework.Models
{
    public class TipoUsuarioCLS
    {
        [Display(Name ="Id tipo usuario")]
        public int iidtipousuario { get; set; }

        [Display(Name = "Nombre tipo usuario")]
        [Required]
        [StringLength(150,ErrorMessage ="Longitud maxima 150")]
        public string nombre { get; set; }

        [Display(Name = "Descripciòn tipo usuario")]
        [Required]
        [StringLength(250, ErrorMessage = "Longitud maxima 250")]

        public string descripcion { get; set; }

        public int bhabilitado { get; set; }


    }
}
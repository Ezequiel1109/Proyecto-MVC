using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiPrimeraAplicacionWebConEntityFramework.Models
{
    public class PaginaCLS
    {
        [Display(Name ="Id Pagina")]
        public int iidpagina { get; set; }
        [Required]
        [Display(Name = "Titulo del link")]
        public string mensaje { get; set; }
        [Required]
        [Display(Name = "Nombre de la accion")]
        public string accion { get; set; }
        [Required]
        [Display(Name = "Nombre del Controlador")]
        public string controlador { get; set; }
        public int bhabilitado { get; set; }

        //Propiedad adicional

        public string mensajeFiltro { get; set; }
    }
}
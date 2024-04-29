using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiPrimeraAplicacionWebConEntityFramework.Models
{
    public class UsuarioCLS
    {
        public int iidusuario { get; set; }
        [Required]
        public string nombreusuario { get; set; }
        [Required]
        public string contra { get; set; }

        public string iidtipousuario { get; set; }
        [Required]
        public int iid { get; set; }

        public int iidrol { get; set; }

        //Propiedad Adicional

        public string nombrePersona { get; set; }
        public string nombreRol { get; set; }
        public string nombreTipoEmpleado { get; set; }

        public string nombrePersonaEnviar { get; set; }


    }
}
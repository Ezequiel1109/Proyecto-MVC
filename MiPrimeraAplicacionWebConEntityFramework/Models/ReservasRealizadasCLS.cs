using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiPrimeraAplicacionWebConEntityFramework.Models
{
    public class ReservasRealizadasCLS
    {
        public int iidventa { get; set; }
        public DateTime fechaVenta { get; set; }
        public decimal total { get; set; } 
    }
}
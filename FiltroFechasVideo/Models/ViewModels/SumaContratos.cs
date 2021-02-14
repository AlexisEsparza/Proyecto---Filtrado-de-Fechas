using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FiltroFechasVideo.Models.ViewModels
{
    public class SumaContratos
    {
        public string Nombre { get; set; }
        public decimal MontoContrato { get; set; }
        public DateTime Fecha { get; set; }
    }
}
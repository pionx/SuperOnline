using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperOnline_ASP_NET_MVC.Models
{
    public class ArticuloCarrito
    {
        public string nombre { get; set; }
        public string cant { get; set; }
        public float precio { get; set; }
        public float total { get; set; }

        public virtual Carrito carrito { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperOnline_ASP_NET_MVC.Models
{
    public class Carrito
    {
        public virtual ICollection<ArticuloCarrito> ArticulosCarrito { get; set; }
    }
}
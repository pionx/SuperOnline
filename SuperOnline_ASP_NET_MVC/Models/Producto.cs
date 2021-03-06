//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SuperOnline_ASP_NET_MVC.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    
    public partial class Producto
    {
        public Producto()
        {
            this.Comentarios = new HashSet<Comentario>();
            this.Detalle_pedido = new HashSet<Detalle_pedido>();
        }
    
        public int producto_id { get; set; }
        public string nombre { get; set; }
        public string marca { get; set; }
        public string contenido { get; set; }
        public decimal precio { get; set; }
        public string tipo { get; set; }
        public string Super_mercado { get; set; }
        
        
        public virtual ICollection<Comentario> Comentarios { get; set; }
        
        public virtual ICollection<Detalle_pedido> Detalle_pedido { get; set; }
        public virtual Super_mercados Super_mercados { get; set; }
        public virtual Tipos Tipos { get; set; }

        public string Tipo { get; set; }
    }
}

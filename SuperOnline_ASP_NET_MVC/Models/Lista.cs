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
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Lista
    {
        public Lista()
        {
            this.Contenido_Listas = new HashSet<Contenido_Listas>();
        }
        [Key]
        public int lista_id { get; set; }
        //[Required]
        //[StringLength(20)]
        //[DisplayName("Nombre de la lista")]
        public string nombre { get; set; }
        //[Required]
        public int usuario_id { get; set; }
        //[Required]
        public System.DateTime fecha { get; set; }
    
        public virtual ICollection<Contenido_Listas> Contenido_Listas { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}

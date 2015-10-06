using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperOnline_ASP_NET_MVC.Models
{
    public class InicioSesion
    {
        [Required(ErrorMessage = "Escribe tu correo Electrónico")]
        [DisplayName("Correo Electrónico")]
        public string correo { get; set; }

        [Required(ErrorMessage = "Escribe tu contraseña")]
        [DisplayName("Contraseña")]
        public string clave { get; set; }
    }
}
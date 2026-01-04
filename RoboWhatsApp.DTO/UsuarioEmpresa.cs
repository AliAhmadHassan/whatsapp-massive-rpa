using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWhatsApp.DTO
{
    public class UsuarioEmpresa
    {
        public int id { get; set; }
        public Usuario usuario { get; set; }
        public Empresa empresa { get; set; }
    }
}

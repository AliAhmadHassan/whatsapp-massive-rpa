using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWhatsApp.DTO
{
    public class Lote
    {
        public int id { get; set; }
        public DateTime dtCadastro { get; set; }
        public Empresa empresa { get; set; }
        public DateTime dtPrevisaoEnvio { get; set; }
        public bool finalizado { get; set; }
    }
}

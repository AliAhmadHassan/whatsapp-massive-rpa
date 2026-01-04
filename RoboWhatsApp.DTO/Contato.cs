using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWhatsApp.DTO
{
    public class Contato
    {

        public int id { get; set; }
        public Lote lote { get; set; }
        public short ddi { get; set; }
        public short ddd { get; set; }
        public long numeroTelefone { get; set; }
        public ContatoStatus contatoStatus { get; set; }
        public DateTime dtEnvio { get; set; }
        public DateTime dtSingleCheck { get; set; }
        public DateTime dtDoubleCheck { get; set; }
    }
}

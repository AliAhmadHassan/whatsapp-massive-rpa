using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWhatsApp.DTO
{
    public class Mensagem
    {
        public int id { get; set; }
        public Lote lote { get; set; }
        public string texto { get; set; }
        public string caminhoArquivo { get; set; }
        public TipoArquivo tipoArquivo { get; set; }
    }
}

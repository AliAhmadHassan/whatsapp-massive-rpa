using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWhatsApp.DTO
{
    public class Pages<T>
    {
        public int totalPages { get; set; }
        public int totalElements { get; set; }
        public bool last { get; set; }
        public int size { get; set; }
        public int number { get; set; }
        public string sort { get; set; }
        public int numberOfElements { get; set; }
        public bool first { get; set; }
        public T content { get; set; }
    }
}

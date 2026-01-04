using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWhatsApp.DTO
{
    public class Response<T>
    {
        public T data { get; set; }
        public List<string> errors { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RoboWhatsApp.DAL
{
    public class ApiClient : HttpClient
    {
        public ApiClient()
        {
            // Update port # in the following line.
            this.BaseAddress = new Uri(EASY_API.path);
            this.DefaultRequestHeaders.Accept.Clear();
            //if (EASY_API.token != null && !this.DefaultRequestHeaders.Contains("Authorization"))
            //    this.DefaultRequestHeaders.Add("Authorization", EASY_API.token.token);
            this.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}

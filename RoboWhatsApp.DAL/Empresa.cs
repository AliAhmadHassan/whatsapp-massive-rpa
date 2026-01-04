using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RoboWhatsApp.DAL
{
    public class Empresa
    {
        HttpClient client = new ApiClient();
        string restApiPath = "api/empresa";

        public async Task<DTO.Response<DTO.Empresa>> createOrUpdate(DTO.Empresa entity)
        {
            HttpResponseMessage response;

            if (entity.id == 0)
                response = await client.PostAsJsonAsync(restApiPath, entity);
            else
                response = await client.PutAsJsonAsync(restApiPath, entity);

            var entityRetorned = await response.Content.ReadAsAsync<DTO.Response<DTO.Empresa>>();

            return entityRetorned;
        }

        public async Task<DTO.Response<DTO.Empresa>> findById(int id)
        {
            DTO.Response<DTO.Empresa> entity = null;
            HttpResponseMessage response = await client.GetAsync($"{restApiPath}/{id}");
            if (response.IsSuccessStatusCode)
            {
                entity = await response.Content.ReadAsAsync<DTO.Response<DTO.Empresa>>();
            }
            return entity;
        }

        public async Task<HttpStatusCode> DeleteAsync(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                 $"{restApiPath}/{id}");

            return response.StatusCode;
        }

        public async Task<DTO.Response<DTO.Pages<DTO.Empresa[]>>> findAll(int page, int count)
        {
            DTO.Response<DTO.Pages<DTO.Empresa[]>> registers = null;
            HttpResponseMessage response = await client.GetAsync($"{restApiPath}/{page}/{count}");
            if (response.IsSuccessStatusCode)
            {
                registers = await response.Content.ReadAsAsync<DTO.Response<DTO.Pages<DTO.Empresa[]>>>();
            }
            return registers;
        }
    }
}

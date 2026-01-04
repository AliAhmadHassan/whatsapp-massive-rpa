using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RoboWhatsApp.DAL
{
    public class Contato
    {
        HttpClient client = new ApiClient();
        string restApiPath = "api/contato";

        public async Task<DTO.Response<DTO.Contato>> createOrUpdate(DTO.Contato entity)
        {
            HttpResponseMessage response;

            if (entity.id == 0)
                response = await client.PostAsJsonAsync(restApiPath, entity);
            else
                response = await client.PutAsJsonAsync(restApiPath, entity);

            var entityRetorned = await response.Content.ReadAsAsync<DTO.Response<DTO.Contato>>();

            return entityRetorned;
        }

        public async Task<DTO.Response<DTO.Contato>> findById(int id)
        {
            DTO.Response<DTO.Contato> entity = null;
            HttpResponseMessage response = await client.GetAsync($"{restApiPath}/{id}");
            if (response.IsSuccessStatusCode)
            {
                entity = await response.Content.ReadAsAsync<DTO.Response<DTO.Contato>>();
            }
            return entity;
        }

        public async Task<DTO.Response<List<DTO.Contato>>> getContato(int loteId)
        {
            DTO.Response<List< DTO.Contato>> entity = null;
            HttpResponseMessage response = await client.GetAsync($"{restApiPath}/{loteId}/loteId");

            entity = await response.Content.ReadAsAsync<DTO.Response<List<DTO.Contato>>>();

            return entity;
        }

        public async Task<DTO.Response<List<DTO.Contato>>> loteParaEnvio(int loteId)
        {
            DTO.Response<List<DTO.Contato>> entity = null;
            HttpResponseMessage response = await client.GetAsync($"{restApiPath}/{loteId}/loteParaEnvio");

            entity = await response.Content.ReadAsAsync<DTO.Response<List<DTO.Contato>>>();

            return entity;
        }

        public async Task<HttpStatusCode> DeleteAsync(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                 $"{restApiPath}/{id}");

            return response.StatusCode;
        }

        public async Task<DTO.Response<DTO.Pages<DTO.Contato[]>>> findAll(int page, int count)
        {
            DTO.Response<DTO.Pages<DTO.Contato[]>> registers = null;
            HttpResponseMessage response = await client.GetAsync($"{restApiPath}/{page}/{count}");
            if (response.IsSuccessStatusCode)
            {
                registers = await response.Content.ReadAsAsync<DTO.Response<DTO.Pages<DTO.Contato[]>>>();
            }
            return registers;
        }

        public async Task<DTO.Response<string>> setEnvio(int loteId, int id)
        {
            DTO.Response<string> registers = null;
            HttpResponseMessage response = await client.PutAsync($"{restApiPath}/{loteId}/{id}/setEnvio", null);
            if (response.IsSuccessStatusCode)
            {
                registers = await response.Content.ReadAsAsync<DTO.Response<string>>();
            }
            return registers;
        }

        public async Task<DTO.Response<string>> setSingleCheck(int loteId, int id)
        {
            DTO.Response<string> registers = null;
            HttpResponseMessage response = await client.PutAsync($"{restApiPath}/{loteId}/{id}/setSingleCheck", null);
            if (response.IsSuccessStatusCode)
            {
                registers = await response.Content.ReadAsAsync<DTO.Response<string>>();
            }
            return registers;
        }

        public async Task<DTO.Response<string>> setDoubleCheck(int loteId, int id)
        {
            DTO.Response<string> registers = null;
            HttpResponseMessage response = await client.PutAsync($"{restApiPath}/{loteId}/{id}/setDoubleCheck", null);
            if (response.IsSuccessStatusCode)
            {
                registers = await response.Content.ReadAsAsync<DTO.Response<string>>();
            }
            return registers;
        }
    }
}

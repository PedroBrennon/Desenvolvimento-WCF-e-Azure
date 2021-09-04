using AtAzure.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace AtAzure.Mvc.Helper
{
    public class ApiHelper
    {
        private readonly HttpClient _clientAmigo;
        private readonly HttpClient _clientPais;

        public ApiHelper()
        {
            _clientAmigo = new HttpClient
            {
                BaseAddress = new Uri("http://atazureamigospedropaiva.azurewebsites.net/")
            };
            _clientPais = new HttpClient
            {
                BaseAddress = new Uri("http://atazureapipaispedropaiva.azurewebsites.net/")
            };

            _clientAmigo.DefaultRequestHeaders.Accept.Clear();
            _clientPais.DefaultRequestHeaders.Accept.Clear();

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            _clientAmigo.DefaultRequestHeaders.Accept.Add(mediaType);
            _clientPais.DefaultRequestHeaders.Accept.Add(mediaType);
        }

        #region Amigo
        public async Task<HttpResponseMessage> GetAmigo()
        {
            return await _clientAmigo.GetAsync("api/Amigo");
        }

        public async Task<HttpResponseMessage> GetAmigoById(int id)
        {
            return await _clientAmigo.GetAsync($"api/Amigo/{id}");
        }

        public async Task<HttpResponseMessage> UpdateAmigo(AmigoCreateEditViewModel model)
        {
            return await _clientAmigo.PutAsJsonAsync($"api/amigo/{model.Id}", model);
        }

        public async Task<HttpResponseMessage> InsertAmigo(AmigoCreateEditViewModel model)
        {
            return await _clientAmigo.PostAsJsonAsync("api/Amigo", model);
        }

        public async Task<HttpResponseMessage> DeleteAmigo(int id)
        {
            return await _clientAmigo.DeleteAsync($"api/Amigo/{id}");
        }
        #endregion

        #region Relacionamento
        public async Task<HttpResponseMessage> GetRelacionamentoById(int id)
        {
            return await _clientAmigo.GetAsync($"api/Relacionamento/{id}");
        }

        public async Task<HttpResponseMessage> InsertRelacionamento(RelacionamentoViewModel model)
        {
            return await _clientAmigo.PostAsJsonAsync("api/Relacionamento", model);
        }

        public async Task<HttpResponseMessage> DeleteRelacionamento(int id)
        {
            return await _clientAmigo.DeleteAsync($"api/Relacionamento/{id}");
        }
        #endregion

        #region Pais
        public async Task<HttpResponseMessage> GetPais()
        {
            return await _clientPais.GetAsync("api/Pais");
        }

        public async Task<HttpResponseMessage> GetPaisById(int id)
        {
            return await _clientPais.GetAsync($"api/Pais/{id}");
        }

        public async Task<HttpResponseMessage> UpdatePais(PaisViewModel model)
        {
            return await _clientPais.PutAsJsonAsync($"api/Pais/{model.Id}", model);
        }

        public async Task<HttpResponseMessage> InsertPais(PaisViewModel model)
        {
            return await _clientPais.PostAsJsonAsync("api/Pais", model);
        }

        public async Task<HttpResponseMessage> DeletePais(int id)
        {
            return await _clientPais.DeleteAsync($"api/Pais/{id}");
        }
        #endregion

        #region Estado
        public async Task<HttpResponseMessage> GetEstado()
        {
            return await _clientPais.GetAsync("api/Estado");
        }

        public async Task<HttpResponseMessage> GetEstadoById(int id)
        {
            return await _clientPais.GetAsync($"api/Estado/{id}");
        }

        public async Task<HttpResponseMessage> UpdateEstado(EstadoCreateEditViewModel model)
        {
            return await _clientPais.PutAsJsonAsync($"api/Estado/{model.Id}", model);
        }

        public async Task<HttpResponseMessage> InsertEstado(EstadoCreateEditViewModel model)
        {
            return await _clientPais.PostAsJsonAsync("api/Estado", model);
        }

        public async Task<HttpResponseMessage> DeleteEstado(int id)
        {
            return await _clientPais.DeleteAsync($"api/Estado/{id}");
        }
        #endregion
    }
}
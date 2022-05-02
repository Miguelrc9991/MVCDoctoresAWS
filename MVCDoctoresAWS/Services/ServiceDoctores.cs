using Microsoft.Extensions.Configuration;
using MVCDoctoresAWS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MVCDoctoresAWS.Services
{
    public class ServiceDoctores
    {
        private string UrlApi;
        private MediaTypeWithQualityHeaderValue Header;

        public ServiceDoctores(IConfiguration configuration)
        {
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
            this.UrlApi = configuration.GetValue<string>("UrlApis:ApiDoctores");
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response = await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<List<Doctor>> GetDoctoresAsync()
        {
            string request = "api/Doctores";
            List<Doctor> doctores = await this.CallApiAsync<List<Doctor>>(request);
            return doctores;
        }

        public async Task<Doctor> GetDoctoreAsync(string id)
        {
            string request = "api/Doctores/" + id;
            Doctor doctor = await this.CallApiAsync<Doctor>(request);
            return doctor;
        }

        public async Task InsertDoctorAsync(Doctor doctor)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/Doctores";

                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                string json = JsonConvert.SerializeObject(doctor);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(request, content);
            }
        }

        public async Task UpdateDoctoresAsync(Doctor doctor)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/Doctores";

                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                string json = JsonConvert.SerializeObject(doctor);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(request, content);
            }
        }

        public async Task DeleteDoctor(string idDoctor)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/Doctores/" + idDoctor;

                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response = await client.DeleteAsync(request);
            }
        }
    }
}

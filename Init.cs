using RoboVotacao.Model;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading;

namespace RoboVotacao
{
    public static class Init
    {
        static HttpClient client = new HttpClient();
        static string URL = ConfigurationManager.AppSettings.Get("ApiPath");
        static int IDRobo = Convert.ToInt32(ConfigurationManager.AppSettings.Get("RoboID"));
        public static bool Iniciou()
        {
            Thread.Sleep(5000);
            HttpResponseMessage response = client.GetAsync(URL + "api/start/GetIniciado").Result;
            return Convert.ToBoolean(response.Content.ReadAsStringAsync().Result);
        }


        public static ConfiguracaoInicialRobo BuscarConfiguracaoInicial()
        {
            HttpResponseMessage response = client.GetAsync(URL + $"api/start/GetConfiguracao?roboId={IDRobo}").Result;
            string json = response.Content.ReadAsStringAsync().Result;
            try
            {
                return JsonConvert.DeserializeObject<ConfiguracaoInicialRobo>(json);
            }
            catch
            {
                return null;
            }
        }

        public static void AtualizarStatusVotante(int votante, StatusVotante status)
        {
            try
            {
                HttpResponseMessage response = client.PostAsync(URL + $"api/start/PostStatusVotante?votanteId={votante}&status={(int)status}", null).Result;
            }
            catch { }
        }

        public static void AddVotanteOnline()
        {
            try
            {
                HttpResponseMessage response = client.PostAsync(URL + $"api/start/PostAddVotanteOnline", null).Result;
            }
            catch { }
        }

        public static void RemoveVotanteOnline()
        {
            try
            {
                HttpResponseMessage response = client.PostAsync(URL + $"api/start/PostRemoveVotanteOnline", null).Result;
            }
            catch { }
        }
    }
}

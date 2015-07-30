using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace ConsolaJumbo
{
    class Program
    {
        static void Main(string[] args)
        {
            int num = 443431;
            SaveArticleData(num);

            Console.WriteLine("Finished");
            Console.ReadLine();
        }

        private static void SaveArticleData(int num)
        {
            // HTTP POST
            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var myObject = new A();
                myObject.textoBusqueda = "a";
                myObject.IdMenu = "";
                myObject.marca = "";
                myObject.pager = "";
                myObject.precioHasta = "";
                myObject.precioDesde = "";
                myObject.producto = "";
                myObject.ordenamiento = 0;

                
                StringContent data = new StringContent(JsonConvert.SerializeObject(myObject).ToString(), Encoding.UTF8, "application/json");
                var response = client.PostAsJsonAsync("https://www.jumbo.com.ar/Comprar/HomeService.aspx/ObtenerArticulosPorDescripcionMarcaFamiliaLevex", data).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content;
                    result = null;
                }
                else
                {
                    // show the response status code 
                    String failureMsg = "HTTP Status: " + response.StatusCode.ToString() + " - Reason: " + response.ReasonPhrase;
                    failureMsg += "\n " + response.Content.ReadAsStringAsync().Result;
                    Console.Write(failureMsg);
                }
            }
        }
    }

    class A
    {
        public string IdMenu { get; set; }
        public string textoBusqueda { get; set; }
        public string producto { get; set; }
        public string marca { get; set; }
        public string pager { get; set; }
        public int ordenamiento { get; set; }
        public string precioDesde { get; set; }
        public string precioHasta { get; set; }
    }
}

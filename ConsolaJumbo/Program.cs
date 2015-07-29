using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

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
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://www.jumbo.com.ar/Comprar/HomeService.aspx/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var data = new A { code = num.ToString() };
            HttpResponseMessage response = client.PostAsJsonAsync("ObtenerDetalleDelArticuloLevex", data).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content;
                result = null;
            }
        }
    }

    class A
    {
        public string code { get; set; }
    }
}

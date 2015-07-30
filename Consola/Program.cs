using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Consola
{
    class Program
    {
        static void Main(string[] args)
        {
            int num = 4749;//GetMinimumId();

            for (int i = num; i > 0; i--)
            {
                try
                {
                    ReadUrl(i);
                }
                catch (Exception ex)
                {
                    SaveToDB(new ProductoAsia { IDProductoAsia = i, Descripcion = "UNKNOWN", Codigo = "UNK", Precio = "0" });
                }
            }
            Console.WriteLine("Finished!");
            Console.ReadLine();
        }

        private static int GetMinimumId()
        {
            int? entity;
            using (var db = new ProductosEntities())
            {
                if (db.ProductoAsia.Count() == 0) entity = 4750; else entity = db.ProductoAsia.Min(p => p.IDProductoAsia);
            }
            return entity.Value - 1;
        }

        private static void ReadUrl(int num)
        {
            string Url = "http://www.supermercadoasia.com/goods.php?id=&German&#.Vbex5_lVhBf";
            Url = Url.Replace("&German&", num.ToString());
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();
            ShowResults(result, num);
        }

        private static void ShowResults(string result, int num)
        {

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(result);
            HtmlNode docNode = doc.DocumentNode;
            HtmlNode name = docNode.SelectNodes("//div[@class ='goods_name']").First();
            Console.WriteLine(name.InnerText);
            HtmlNode sn = docNode.SelectNodes("//div[@class ='sn']").First();
            Console.WriteLine(sn.InnerText);
            HtmlNode price = docNode.SelectNodes("//font[@id ='ECS_SHOPPRICE']").First();
            Console.WriteLine(price.InnerText);
            /*
            SaveToDB(new ProductoAsia
            {
                IDProductoAsia = num,
                Descripcion = name.InnerText.Trim().ToUpper(),
                Codigo = sn.InnerText.Replace("Codigo：		", "").Trim(),
                Precio = price.InnerText
            });
            */
            
            HtmlNode img = docNode.SelectNodes("//div[@class ='imgInfo']").First();
            img = img.SelectNodes("//a[@class ='jqzoom']").First();
            string url = "http://www.supermercadoasia.com/" + img.Attributes["href"].Value;
            SaveImageToDrive(url, num.ToString());
            

        }

        private static void SaveToDB(ProductoAsia productoAsia)
        {
            using (var db = new ProductosEntities())
            {
                db.ProductoAsia.Add(productoAsia);
                db.SaveChanges();
            }
        }

        private static void SaveImageToDrive(string src, string name)
        {
            string localFilename = @"c:\Super\" + name + ".jpg";
            using (var client = new WebClient())
            {
                client.DownloadFile(src, localFilename);
            }
        }
    }
}

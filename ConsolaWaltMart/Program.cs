using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ConsolaWaltMart
{
    class Program
    {
        static void Main(string[] args)
        {
            const string path = "C:\\WalMart\\";
            var allFilenames = Directory.EnumerateFiles(path).Select(p => Path.GetFileName(p)).Where(f => !f.Contains("_done"));
            int i = 0;
            foreach (var file in allFilenames)
            {
                using (var reader = new StreamReader(path + file))
                {
                    string line = reader.ReadToEnd();
                    dynamic dynObj2 = new JavaScriptSerializer().Deserialize<List<object>>(line);
                    foreach (var item in dynObj2)
                    {
                        SaveObject(item);
                        i++;
                    }
                }
                System.IO.File.Move(path + file, path + "_done_" + file);

            }

            if (allFilenames.Count() == 0)
            {
                allFilenames = Directory.EnumerateFiles(path).Select(p => Path.GetFileName(p));
                foreach (var file in allFilenames)
                {
                    System.IO.File.Move(path + file, path + file.Replace("_done_", ""));
                }
            }

            Console.WriteLine("Finished" + i);
            Console.ReadLine();
        }

        private static void SaveObject(dynamic item)
        {
            ProductoWallmart entity = new ProductoWallmart
            {
                Descripcion = item["Description"],
                Precio = item["Precio"],
                UPC = item["upc"]
            };
            entity.Descripcion = entity.Descripcion.Trim();
            entity.Precio = entity.Precio.Trim();
            entity.UPC = entity.UPC.Trim();

            Console.WriteLine(entity.Descripcion);
            Console.WriteLine(entity.Precio);
            Console.WriteLine(entity.UPC);

            SaveP(entity);
        }

        private static void SaveP(ProductoWallmart entity)
        {
            using (var db = new ProductosEntities())
            {
                db.ProductoWallmart.Add(entity);
                db.SaveChanges();
            }
        }
    }

}

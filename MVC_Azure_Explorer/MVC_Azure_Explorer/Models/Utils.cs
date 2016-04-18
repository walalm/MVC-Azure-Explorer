using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace MVC_Azure_Explorer.Models
{
    public static class Utils
    {

        public static string GetGUID()
        {
            Guid g = Guid.NewGuid();
            return g.ToString();
        }
        public static bool ExisteImagen(string pRuta)
        {
            string _rutaImagen = HttpContext.Current.Server.MapPath(pRuta);
            return File.Exists(_rutaImagen);
        }
        public static string AcortarNombre(string nombre, int derecha, int izquierda)
        {
            try
            {
                if (nombre.Length > (derecha + izquierda))
                {
                    string _tmpD;
                    string _tmpI;
                    _tmpD = nombre.Substring(0, derecha);
                    _tmpI = nombre.Substring(nombre.Length - izquierda, izquierda);
                    return _tmpD + "..." + _tmpI;
                }
                else
                {
                    return nombre;

                }


            }
            catch (Exception ex)
            {


                return nombre;
            }

        }
        public static string GetExtension(string fileName)
        {
            try
            {
                return Path.GetExtension(fileName).ToLower();
            }
            catch (Exception)
            {

                return ".bin";
            }


        }

        public static string UrlCDNImagenes
        {
            get
            {
                return string.Format("{0}/{1}/", ConfigurationManager.AppSettings["UrlCDN"], ContenedorImagenes);
            }
        }

        public static string UrlCDNSimple
        {
            get
            {
                return ConfigurationManager.AppSettings["UrlCDN"];
            }
        }

        public static string UrlBlob
        {
            get
            {
                return ConfigurationManager.AppSettings["UrlBlobStorage"];
            }
        }
        public static string ContenedorImagenes
        {
            get
            {
                return ConfigurationManager.AppSettings["ContenedorImagenes"];
            }
        }
    }
}
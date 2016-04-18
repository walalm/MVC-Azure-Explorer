using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MVC_Azure_Explorer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Azure_Explorer.Controllers
{
    public class FilesController : Controller
    {
        // GET: Files


        #region Properties
        public static CloudStorageAccount StorageAccount
        {
            get { return CloudStorageAccount.Parse(AzureConnString); }
        }

        public static string RutaRaiz
        {
            get
            {
                CloudBlobClient blobClient = StorageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(ContenedorRaiz);
                container.CreateIfNotExists();
                return container.Uri.ToString();
            }
        }
        public static string ContenedorRaiz
        {
            get
            {
                return ConfigurationManager.AppSettings["ContenedorBrowser"];
            }
        }
        public static string AzureConnString
        {
            get
            {
                return ConfigurationManager.AppSettings["azurestorage"];
            }
        }
        #endregion

        #region Methods

        [HttpPost]
        public JsonResult Upload(string qqfile, string ruta)
        {
            //var path = Server.MapPath(ruta);
            BlobHandler bh = new BlobHandler(ContenedorRaiz);

            ruta = ruta.Replace(string.Format("{0}/", ContenedorRaiz), string.Empty);
            string strId = Utils.GetGUID();


            string nombreArchivo = string.Empty;
            MensajeUpload msg = new MensajeUpload();
            try
            {
                var stream = Request.InputStream;
                if (String.IsNullOrEmpty(Request["qqfile"]))
                {
                    //IE
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string archivo = strId + Utils.GetExtension(Request.Files[0].FileName);
                    string ServerFileName = new Uri(ruta).LocalPath.Replace(ContenedorRaiz, string.Empty) + archivo;
 

                    bh.UploadToBlob(Request.Files[0], ServerFileName);


                    nombreArchivo = archivo;

                }
                else
                {
                    //Webkit, Mozilla


                    string archivo = strId + Utils.GetExtension(qqfile);
                    string ServerFileName = new Uri(ruta).LocalPath.Replace(ContenedorRaiz, string.Empty) + archivo;



                    bh.UploadToBlobFromStream(stream, ServerFileName);

                    nombreArchivo = archivo;
                }


            }
            catch (Exception ex)
            {
                msg.success = false;
                msg.message = ex.Message;
                return Json(msg, "text/html");
            }
            //Request.Headers["Acept"] = "application/json, text/javascript, */*; q=0.01";
            msg.success = true;
            msg.message = string.Format("File {0} uploaded!", nombreArchivo);
            return Json(msg, "text/html");


        }
        public ActionResult Index()
        {

            ViewBag.TieneSuperior = "0";
            ViewBag.RutaSuperior = "";
            string _RutaInicialRelativa = ContenedorRaiz;

            //Blob initialization operations
            CloudBlobClient blobClient = StorageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(ContenedorRaiz);
            container.CreateIfNotExists();
            // End blob initialization


            string _RutaInicialFull = container.Uri.ToString();

            ViewBag.RutaActual = _RutaInicialFull;


            //Primero los directorios
            List<ManejadorArchivos> listado = new List<ManejadorArchivos>();
            foreach (IListBlobItem item in container.ListBlobs(null, false))
            {
                //string _dirRutaRel = RelativePath(dirinfo.FullName, Request);

                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    ManejadorArchivos manarch = new ManejadorArchivos();
                    manarch.Tamano = Math.Round(ConvertBytesToKilobytes(blob.Properties.Length), 2).ToString("#,###,###,##0.00 Kb");
                    manarch.Nombre = blob.Name;
                    manarch.RutaCompleta = blob.Uri.ToString().Replace(Utils.UrlBlob, Utils.UrlCDNSimple);
                    manarch.RutaRelativa = blob.Uri.ToString().Replace(Utils.UrlBlob, Utils.UrlCDNSimple);
                    manarch.Tipo = TipoArchivo.File;
                    manarch.Fecha = blob.Properties.LastModified.GetValueOrDefault().UtcDateTime;
                    listado.Add(manarch);
                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory)item;
                    ManejadorArchivos manarch = new ManejadorArchivos();
                    manarch.Tamano = "";
                    manarch.Nombre = directory.Uri.Segments[directory.Uri.Segments.Length - 1].Replace("/", string.Empty);
                    manarch.RutaCompleta = directory.Uri.ToString().Replace(Utils.UrlBlob, Utils.UrlCDNSimple);
                    manarch.RutaRelativa = directory.Uri.ToString().Replace(Utils.UrlBlob, Utils.UrlCDNSimple);
                    manarch.Tipo = TipoArchivo.Folder;
                    manarch.Fecha = DateTime.Now;
                    listado.Add(manarch);
                }

            }

            return View(listado);
        }


        public ActionResult FileBrowse(FormCollection formulario)
        {



            //Blob initialization operations
            CloudBlobClient blobClient = StorageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(ContenedorRaiz);
            container.CreateIfNotExists();
            // End blob initialization

            Uri _RutaActualRelativa = new Uri(Convert.ToString(formulario["RutaSolicitada"]));


            //string _archivoEliminar = formulario["ArchivoParaEliminar"];
            //if (!string.IsNullOrEmpty(_archivoEliminar))
            //{
            //    EliminarArchivo(_archivoEliminar);
            //}

            if (_RutaActualRelativa.ToString() == RutaRaiz)
            {

                ViewBag.RutaActual = _RutaActualRelativa;
                ViewBag.TieneSuperior = "0";
                ViewBag.RutaSuperior = "";
                return RedirectToAction("Index", "Files");
            }
            else
            {
                CloudBlobDirectory directorioActual = container.GetDirectoryReference(_RutaActualRelativa.LocalPath.Replace(string.Format("/{0}/", ContenedorRaiz), string.Empty));
                CloudBlobDirectory directorioSuperior = directorioActual.Parent;
                ViewBag.RutaSuperior = directorioSuperior.Uri.ToString();
                ViewBag.RutaActual = _RutaActualRelativa;
                ViewBag.TieneSuperior = "1";

            }

            CloudBlobDirectory directorioInicial = container.GetDirectoryReference(_RutaActualRelativa.LocalPath.Replace(string.Format("/{0}/", ContenedorRaiz), string.Empty));

            List<ManejadorArchivos> listado = new List<ManejadorArchivos>();
            foreach (IListBlobItem item in directorioInicial.ListBlobs())
            {
                //string _dirRutaRel = RelativePath(dirinfo.FullName, Request);

                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    ManejadorArchivos manarch = new ManejadorArchivos();
                    manarch.Tamano = Math.Round(ConvertBytesToKilobytes(blob.Properties.Length), 2).ToString("#,###,###,##0.00 Kb");
                    manarch.Nombre = Path.GetFileName(blob.Uri.LocalPath);
                    manarch.RutaCompleta = blob.Uri.ToString().Replace(Utils.UrlBlob, Utils.UrlCDNSimple);
                    manarch.RutaRelativa = blob.Uri.LocalPath;
                    manarch.Tipo = TipoArchivo.File;
                    manarch.Fecha = blob.Properties.LastModified.GetValueOrDefault().UtcDateTime;
                    listado.Add(manarch);
                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory)item;
                    ManejadorArchivos manarch = new ManejadorArchivos();
                    manarch.Tamano = "";
                    manarch.Nombre = directory.Uri.Segments[directory.Uri.Segments.Length - 1].Replace("/", string.Empty);
                    manarch.RutaCompleta = directory.Uri.ToString().Replace(Utils.UrlBlob, Utils.UrlCDNSimple);
                    manarch.RutaRelativa = directory.Uri.LocalPath;
                    manarch.Tipo = TipoArchivo.Folder;
                    manarch.Fecha = DateTime.Now;
                    listado.Add(manarch);
                }

            }

            return View("Index", listado);
        }

        public static string MappedApplicationPath
        {
            get
            {
                string APP_PATH = System.Web.HttpContext.Current.Request.ApplicationPath.ToLower();
                if (APP_PATH == "/")      //a site
                    APP_PATH = "/";
                else if (!APP_PATH.EndsWith(@"/")) //a virtual
                    APP_PATH += @"/";

                string it = System.Web.HttpContext.Current.Server.MapPath(APP_PATH);
                if (it.EndsWith(@"\"))
                {
                    it = it.Remove(it.Length - 1, 1);
                }
                return it;
            }
        }

        public static string RelativePath(string path, HttpRequestBase context)
        {
            string _retorno = string.Empty;
            string approot = MappedApplicationPath;
            DirectoryInfo _directorioRaiz = new DirectoryInfo(approot);
            DirectoryInfo _directorioSolicitado = new DirectoryInfo(path);
            if (_directorioRaiz.FullName == _directorioSolicitado.FullName)
            {

                _retorno = "~/";

            }
            else
            {
                string apppath = context.ServerVariables["APPL_PHYSICAL_PATH"];
                _retorno = path.Replace(apppath, "~/").Replace(@"\", "/");
            }
            return _retorno;
        }

        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        static double ConvertKilobytesToMegabytes(long kilobytes)
        {
            return kilobytes / 1024f;
        }

        public string[] RutasExcluidas()
        {
            string[] _strRutas = { "~/app_code", "~/App_Data", "~/bin", "~/Views", "~/Controllers", "~/Web References", "~/Service References" };

            return _strRutas;
        }

        static double ConvertBytesToKilobytes(long bytes)
        {
            return bytes / 1024f;
        }


        #endregion
    }
}
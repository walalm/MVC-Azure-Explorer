using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace MVC_Azure_Explorer.Models
{
    public class ManejadorArchivos
    {
        public string RutaRelativa { get; set; }
        public string Nombre { get; set; }
        public string Tamano { get; set; }
        public string RutaCompleta { get; set; }
        public DateTime Fecha { get; set; }
        public TipoArchivo Tipo { get; set; }
        public bool Protegido
        {
            get
            {

                if (Tipo == TipoArchivo.File)
                {
                    string extension = Convert.ToString(Path.GetExtension(new Uri(RutaCompleta).LocalPath)).ToLower();
                    return (!ArchivosPermitidos().Contains(extension));
                }
                else
                {

                    return false;
                }

            }
        }
        public string Icono
        {
            get
            {
                if (Tipo == TipoArchivo.File)
                {

                    string extension = Convert.ToString(Path.GetExtension(new Uri(RutaCompleta).LocalPath)).ToLower();
                    return String.Format("~/Images/extensiones/16/{0}.png", extension.Replace(".", ""));
                }
                else
                {

                    return "~/Images/extensiones/16/folder.png";
                }
            }


        }
        public ManejadorArchivos()
        {
        }

        public string[] ArchivosPermitidos()
        {
            string[] _permitidos = new string[] { ".png", ".jpg", ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".zip", ".rar", ".jpeg", ".gif", ".pps", ".ppsx", ".txt", ".html", ".htm" };

            return _permitidos;

        }

    }
    public enum TipoArchivo
    {
        File = 1, Folder = 2
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Azure_Explorer.Models
{
    public class MensajeView
    {
        public MensajeView()
        {
        }
        public bool GrowlStyle { get; set; }
        public string Mensaje { get; set; }
        public bool IsError { get; set; }
        public string ClaseCSS { get; set; }
    }

    public class ActionMensaje
    {
        public ActionMensaje()
        {
        }
        public string Msg { get; set; }
        public object DataObject { get; set; }
        public bool Success { get; set; }

    }
    public class MensajeUpload
    {
        public MensajeUpload()
        {
        }
        public string message { get; set; }
        public object DataObject { get; set; }
        public bool success { get; set; }

    }

}
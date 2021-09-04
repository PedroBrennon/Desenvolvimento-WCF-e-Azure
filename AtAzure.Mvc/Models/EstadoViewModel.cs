using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AtAzure.Mvc.Models
{
    public class EstadoViewModel
    {
        public int Id { get; set; }
        public string Foto { get; set; }
        public string Nome { get; set; }
        public int paisId { get; set; }
        public string paisNome { get; set; }
    }

    public class EstadoCreateEditViewModel
    {
        public int Id { get; set; }
        public string Foto { get; set; }
        public string Nome { get; set; }
        public int paisId { get; set; }
        public List<SelectListItem> paisNome { get; set; }
    }
}
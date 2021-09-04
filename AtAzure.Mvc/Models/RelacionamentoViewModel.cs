using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AtAzure.Mvc.Models
{
    public class RelacionamentoViewModel
    {
        public int Id { get; set; }
        public int IdAmigo1 { get; set; }
        public int IdAmigo2 { get; set; }
        public List<SelectListItem> Amigo1 { get; set; }
        public List<SelectListItem> Amigo2 { get; set; }
        public string Foto { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
    }
}
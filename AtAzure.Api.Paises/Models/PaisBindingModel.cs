using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtAzure.Api.Paises.Models
{
    public class PaisBindingModel
    {
        public int Id { get; set; }
        public string Foto { get; set; }
        public string Nome { get; set; }
    }
}
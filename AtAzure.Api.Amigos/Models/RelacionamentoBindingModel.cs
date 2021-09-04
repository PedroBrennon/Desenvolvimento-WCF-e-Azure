using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtAzure.Api.Amigos.Models
{
    public class RelacionamentoBindingModel
    {
        public int Id { get; set; }
        public int IdAmigo1 { get; set; }
        public int IdAmigo2 { get; set; }
        public string Foto { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
    }
}
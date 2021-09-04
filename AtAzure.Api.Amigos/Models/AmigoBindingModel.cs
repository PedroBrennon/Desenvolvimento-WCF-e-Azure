using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtAzure.Api.Amigos.Models
{
    public class AmigoBindingModel
    {
        public int Id { get; set; }
        public string Foto { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTime Aniversario { get; set; }
        public string Pais { get; set; }
        public string Estado { get; set; }
        public string Amigo { get; set; }
    }
}
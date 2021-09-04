using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AtAzure.Mvc.Models
{
    public class AmigoViewModel
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

    public class AmigoCreateEditViewModel
    {
        public int Id { get; set; }
        public string Foto { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTime Aniversario { get; set; }
        public int PaisId { get; set; }
        public string Pais { get; set; }
        public List<SelectListItem> PaisNome { get; set; }
        public int EstadoId { get; set; }
        public string Estado { get; set; }
        public List<SelectListItem> EstadoNome { get; set; }
    }
}
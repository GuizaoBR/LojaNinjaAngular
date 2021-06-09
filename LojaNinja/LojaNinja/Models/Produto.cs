using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LojaNinja.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string Imagem { get; set; }
    }
}
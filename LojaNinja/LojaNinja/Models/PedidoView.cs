using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LojaNinja.Models
{
    public class PedidoView
    {
        public int Numero { get; set; }
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public decimal Desconto { get; set; }
        public decimal ValorTotal { get; set; }
        public string Produtos { get; set; }
        public string Cliente { get; set; }


    }
}
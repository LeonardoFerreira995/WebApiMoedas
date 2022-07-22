using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class MoedaModel
    {
        public string moeda { get; set; }
        public DateTime data_inicio { get; set; }
        public DateTime data_fim { get; set; }
    }
}

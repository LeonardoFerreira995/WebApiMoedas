using System;

namespace ConsumoApi.Models
{
    public class DadosResultadoModel
    {
        public string moeda { get; set; }
        public DateTime dat_cotacao { get; set; }
        public double vlr_cotacao { get; set; }
    }
}

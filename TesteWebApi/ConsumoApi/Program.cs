using ConsumoApi.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ConsumoApi
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var cliente = new HttpClient { BaseAddress = new Uri("https://localhost:44301") };
            var excel = new ExcelService();

            while (true)
            {
                var response = await cliente.GetAsync("Moedas");
                var content = await response.Content.ReadAsStringAsync();

                if (content is null)
                {
                    Console.WriteLine("Não foi gerado respostas.");
                    return;
                }
                else
                {
                    if (content == "Não existem moedas armazenadas.")
                    {
                        Console.WriteLine(content);
                    }
                    else
                    {
                        var tempoProcessamento = DateTime.Now;
                        var moeda = JsonConvert.DeserializeObject<MoedaModel>(content);

                        Console.WriteLine("Moeda: " + moeda.moeda);
                        Console.WriteLine("Data Inicio: " + moeda.data_inicio);
                        Console.WriteLine("Data Fim: " + moeda.data_fim);

                        excel.LeituraCSV(moeda.moeda, moeda.data_inicio.ToString(), moeda.data_fim.ToString());
                        Console.WriteLine("Tempo de processamento: " + (DateTime.Now - tempoProcessamento));
                    }
                }

                Thread.Sleep(2 * 60 * 1000);
            }
        }
    }
}


using ConsumoApi.Enum;
using ConsumoApi.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsumoApi
{
    public class ExcelService
    {
        private static ConcurrentStack<DadosMoedaModel> _pilhaDadosMoedaModel;
        private static ConcurrentStack<DadosCotacaoModel> _pilhaDadosCotacaoModel;
        private static StreamReader _moeda;
        private static StreamReader _cotacao;
        private static DadosMoedaModel _dadosMoedaModel;
        private static DadosCotacaoModel _dadosCotacaoModel;
        private static List<DadosResultadoModel> _dadosResultadoModel;
        private static StringBuilder _arquivo;

        public void LeituraCSV(string id_moeda, string data_inicio, string data_fim)
        {
            string path = PegarCaminho();

            string pathDadosMoeda = path + @"Planilhas\DadosMoeda.csv";
            string pathDadosCotacao = path + @"Planilhas\DadosCotacao.csv";

            string linhaMoeda = string.Empty;
            string linhaCotacao = string.Empty;
            string[] campo;

            if(_pilhaDadosMoedaModel != null && _pilhaDadosCotacaoModel != null)
            {
                _pilhaDadosMoedaModel.Clear();
                _pilhaDadosCotacaoModel.Clear();
            }

            _moeda = new StreamReader(pathDadosMoeda);
            _cotacao = new StreamReader(pathDadosCotacao);

            Console.WriteLine("Executando leitura da planilha DadosMoeda.csv...");

            while ((linhaMoeda = _moeda.ReadLine()) != null)
            {
                _dadosMoedaModel = new DadosMoedaModel();

                campo = linhaMoeda.Split(';');

                if (campo[0] == id_moeda && ConverterData(campo[1]) >= ConverterData(data_inicio) &&
                                            ConverterData(campo[1]) <= ConverterData(data_fim))
                {
                    _dadosMoedaModel.ID_COTACAO = EnumMoedas.PegarEnumPorDescricao(campo[0], typeof(MoedasEnum.tabelaEnum));
                    _dadosMoedaModel.ID_MOEDA = campo[0];
                    _dadosMoedaModel.DATA_REF = Convert.ToDateTime(campo[1]);

                    if (_dadosMoedaModel != null)
                        InsereExcelDadosMoeda(_dadosMoedaModel);
                }

            }

            if (!_pilhaDadosMoedaModel.IsEmpty && _pilhaDadosMoedaModel != null)
            {
                Console.WriteLine("Executando leitura da planilha DadosCotacao.csv...");

                while ((linhaCotacao = _cotacao.ReadLine()) != null)
                {
                    var id_cotacao = _pilhaDadosMoedaModel.First();

                    _dadosCotacaoModel = new DadosCotacaoModel();

                    campo = linhaCotacao.Split(';');

                    if (campo[2] != "dat_cotacao")
                    {

                        if (Convert.ToDateTime(campo[2]) >= Convert.ToDateTime(data_inicio) &&
                            Convert.ToDateTime(campo[2]) <= Convert.ToDateTime(data_fim) &&
                            id_cotacao.ID_COTACAO == int.Parse(campo[1]))
                        {
                            _dadosCotacaoModel.vlr_cotacao = double.Parse(campo[0]);
                            _dadosCotacaoModel.cod_cotacao = int.Parse(campo[1]);
                            _dadosCotacaoModel.dat_cotacao = Convert.ToDateTime(campo[2]);

                            if (_dadosCotacaoModel != null)
                                InsereExcelDadosCotacao(_dadosCotacaoModel);
                        }
                    }
                }
            }

            if (!_pilhaDadosMoedaModel.IsEmpty && _pilhaDadosMoedaModel != null || 
                !_pilhaDadosCotacaoModel.IsEmpty && _pilhaDadosCotacaoModel != null)
            {
                Console.WriteLine("Processando informações...");

                var query = (from pilhaDadosMoeda in _pilhaDadosMoedaModel
                             join pilhaDadosCotacao in _pilhaDadosCotacaoModel on
                             pilhaDadosMoeda.ID_COTACAO equals pilhaDadosCotacao.cod_cotacao

                             select new
                             {
                                 moeda = pilhaDadosMoeda.ID_MOEDA,
                                 data_cotacao = pilhaDadosCotacao.dat_cotacao,
                                 vlr_cotacao = pilhaDadosCotacao.vlr_cotacao
                             }).Distinct();

                _dadosResultadoModel = new List<DadosResultadoModel>();

                foreach (var item in query)
                {
                    _dadosResultadoModel.Add(
                        new DadosResultadoModel()
                        {
                            moeda = item.moeda,
                            dat_cotacao = item.data_cotacao,
                            vlr_cotacao = item.vlr_cotacao
                        });
                }
                CriarExcel(_dadosResultadoModel);
            }
            else
            {
                Console.WriteLine("Não foi encontrado dados nas planilhas.");
            }
        }

        private static void InsereExcelDadosMoeda(DadosMoedaModel DadosMoeda)
        {
            if (_pilhaDadosMoedaModel is null)
                _pilhaDadosMoedaModel = new ConcurrentStack<DadosMoedaModel>();

            _pilhaDadosMoedaModel.Push(DadosMoeda);
        }

        private static void InsereExcelDadosCotacao(DadosCotacaoModel DadosCotacao)
        {
            if (_pilhaDadosCotacaoModel is null)
                _pilhaDadosCotacaoModel = new ConcurrentStack<DadosCotacaoModel>();

            _pilhaDadosCotacaoModel.Push(DadosCotacao);
        }

        private static void CriarExcel(List<DadosResultadoModel> informacoes)
        {
            Console.WriteLine("Criando planilha...");

            var registros = informacoes;
            var path = PegarCaminho();

            _arquivo = new StringBuilder();

            _arquivo.AppendLine("moeda;dat_cotacao;vlr_cotacao");

            foreach (var item in registros)
            {
                _arquivo.AppendLine(item.moeda + ";" + item.dat_cotacao.ToString("dd/MM/yyyy") + ";" + item.vlr_cotacao);
            }

            string horario = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            File.WriteAllText(path + @"Planilhas\Resultado_" +
                                      horario +
                                     ".csv",
                                     _arquivo.ToString());

            Console.WriteLine("Planilha criado com sucesso!");
            Console.WriteLine("Local: " + path + @"Planilhas\Resultado_" + horario + ".csv");
        }

        private static string PegarCaminho()
        {
            string path = Directory.GetCurrentDirectory();

            string path2 = path.Remove(path.LastIndexOf("\\"));
            path = path2.Remove(path2.LastIndexOf("\\") + 1);
            path2 = path.Remove(path.LastIndexOf("\\"));
            path = path2.Remove(path2.LastIndexOf("\\") + 1);

            return path;
        }

        private static DateTime ConverterData(string data)
        {
            var Data = Convert.ToDateTime(data);

            return Data;
        }
    }
}

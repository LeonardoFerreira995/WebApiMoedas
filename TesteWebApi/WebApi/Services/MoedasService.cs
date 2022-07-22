using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models;

namespace WebApi.Services
{
    public class MoedasService
    {
        private static ConcurrentStack<MoedaModel> _filaMoedas;

        public MoedasService()
        {
            if(_filaMoedas is null)
                _filaMoedas = new ConcurrentStack<MoedaModel>();
        }

        public void InsereMoeda(List<MoedaModel> ListaMoedas)
        {
            foreach(var item in ListaMoedas)
            {
                _filaMoedas.Push(item);
            }
        }

        public MoedaModel RetornaMoeda()
        {
            if (_filaMoedas.Any())
            {
                var ultimaMoeda = _filaMoedas.Last();
                _filaMoedas.TryPop(out ultimaMoeda);

                return ultimaMoeda;
            }

            return null;
        }
    }
}

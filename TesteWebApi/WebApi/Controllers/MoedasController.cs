using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoedasController : ControllerBase
    {
        private MoedasService _moedasService;

        [HttpPost]
        public IActionResult AddItemFila(List<MoedaModel> listaMoedas)
        {
            if (!listaMoedas.Any())
                return BadRequest();

            _moedasService = new MoedasService();

            _moedasService.InsereMoeda(listaMoedas);

            return Ok("Moedas adicionadas com sucesso!");
        }

        [HttpGet]
        public IActionResult GetItemFila()
        {
            _moedasService = new MoedasService();

            var moeda = _moedasService.RetornaMoeda();

            if (moeda is null)
                return BadRequest("Não existem moedas armazenadas.");

            return Ok(moeda);
        }
    }
}

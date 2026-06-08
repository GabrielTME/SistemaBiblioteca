using Microsoft.AspNetCore.Mvc;
using MsLeitores.DTO;
using System;

namespace MsLeitores
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeitoresController : Controller
    {
        private IServLeitores _servLeitores;

        public LeitoresController()
        {
            _servLeitores = new ServLeitores();
        }

        // Rota consumida pelo MsEmprestimos para o recibo
        [Route("{id}")]
        [HttpGet]
        public IActionResult ObterLeitor(int id)
        {
            try { return Ok(_servLeitores.ObterLeitor(id)); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        // Rota auxiliar para testes
        [HttpPost]
        public IActionResult CadastrarLeitor([FromBody] Leitor leitor)
        {
            try { return Ok(_servLeitores.CadastrarLeitor(leitor)); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        // Rota consumida pelo MsEmprestimos na hora da devolução
        [Route("{id}/zerar-multa")]
        [HttpPut]
        public IActionResult ZerarMulta(int id)
        {
            try
            {
                _servLeitores.ZerarMulta(id);
                return Ok(new { mensagem = "Multa do leitor zerada com sucesso." });
            }
            catch (Exception e) { return BadRequest(e.Message); }
        }
    }
}

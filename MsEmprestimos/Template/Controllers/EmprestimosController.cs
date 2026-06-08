using Microsoft.AspNetCore.Mvc;
using MsEmprestimos.DTO;
using System;
using System.Threading.Tasks;

namespace MsEmprestimos
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmprestimosController : Controller
    {
        private IServEmprestimos _servEmprestimos;

        public EmprestimosController()
        {
            _servEmprestimos = new ServEmprestimos();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarEmprestimo([FromBody] Emprestimo emprestimo)
        {
            try { return Ok(await _servEmprestimos.RegistrarEmprestimo(emprestimo)); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [Route("{id}/recibo")]
        [HttpGet]
        public async Task<IActionResult> ObterRecibo(int id)
        {
            try { return Ok(await _servEmprestimos.ObterRecibo(id)); }
            catch (Exception e) { return BadRequest(e.Message); }
        }

        [Route("{id}/devolver")]
        [HttpPut]
        public async Task<IActionResult> DevolverLivro(int id)
        {
            try
            {
                await _servEmprestimos.DevolverLivro(id);
                return Ok(new { mensagem = "Devolução efetuada e multa pendente (se houvesse) zerada no sistema." });
            }
            catch (Exception e) { return BadRequest(e.Message); }
        }
    }
}

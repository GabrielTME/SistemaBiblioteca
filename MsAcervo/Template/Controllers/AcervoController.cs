using Microsoft.AspNetCore.Mvc;
using MsAcervo.DTO;
using System;

namespace MsAcervo
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcervoController : Controller
    {
        private IServAcervo _servAcervo;

        public AcervoController()
        {
            _servAcervo = new ServAcervo();
        }

        // Rota: GET /api/acervo/{id}
        // Essa rota será consumida pelo MsEmprestimos para validar a disponibilidade
        [Route("{id}")]
        [HttpGet]
        public IActionResult ObterLivro(int id)
        {
            try
            {
                var livro = _servAcervo.ObterLivro(id);
                return Ok(livro);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Rota: POST /api/acervo
        // Rota auxiliar para você cadastrar livros no seu SQLite e conseguir testar a aplicação
        [HttpPost]
        public IActionResult CadastrarLivro([FromBody] Livro livro)
        {
            try
            {
                var novoLivro = _servAcervo.CadastrarLivro(livro);
                return Ok(novoLivro);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

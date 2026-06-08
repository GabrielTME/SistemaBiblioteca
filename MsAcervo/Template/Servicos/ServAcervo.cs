using MsAcervo.DTO;
using System;
using System.Linq;
using Template.Infra;

namespace MsAcervo
{
    public interface IServAcervo
    {
        Livro ObterLivro(int id);
        Livro CadastrarLivro(Livro livro);
    }

    public class ServAcervo : IServAcervo
    {
        private DataContext _dataContext;

        public ServAcervo()
        {
            _dataContext = GeradorDeServicos.CarregarContexto();
        }

        public Livro ObterLivro(int id)
        {
            // Busca o livro no banco de dados SQLite
            var livro = _dataContext.Livros.FirstOrDefault(l => l.Id == id);
            
            if (livro == null)
            {
                throw new Exception("Livro não encontrado no acervo.");
            }

            return livro;
        }

        public Livro CadastrarLivro(Livro livro)
        {
            // Adiciona um livro ao banco para permitir testes futuros
            _dataContext.Livros.Add(livro);
            _dataContext.SaveChanges();
            return livro;
        }
    }
}

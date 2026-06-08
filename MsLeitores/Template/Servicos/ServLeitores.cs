using MsLeitores.DTO;
using System;
using System.Linq;
using Template.Infra;

namespace MsLeitores
{
    public interface IServLeitores
    {
        Leitor ObterLeitor(int id);
        Leitor CadastrarLeitor(Leitor leitor);
        void ZerarMulta(int id);
    }

    public class ServLeitores : IServLeitores
    {
        private DataContext _dataContext;

        public ServLeitores()
        {
            _dataContext = GeradorDeServicos.CarregarContexto();
        }

        public Leitor ObterLeitor(int id)
        {
            var leitor = _dataContext.Leitores.FirstOrDefault(l => l.Id == id);
            if (leitor == null)
            {
                throw new Exception("Leitor não encontrado no sistema.");
            }
            return leitor;
        }

        public Leitor CadastrarLeitor(Leitor leitor)
        {
            _dataContext.Leitores.Add(leitor);
            _dataContext.SaveChanges();
            return leitor;
        }

        public void ZerarMulta(int id)
        {
            var leitor = ObterLeitor(id);
            leitor.MultaPendente = 0; // Altera o dado (Regra do PDF)
            _dataContext.SaveChanges();
        }
    }
}

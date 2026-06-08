using MsEmprestimos.DTO;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Template.Infra;

namespace MsEmprestimos
{
    public interface IServEmprestimos
    {
        Task<Emprestimo> RegistrarEmprestimo(Emprestimo emprestimo);
        Task<object> ObterRecibo(int id);
        Task DevolverLivro(int id);
    }

    public class ServEmprestimos : IServEmprestimos
    {
        private DataContext _dataContext;
        private readonly HttpClient _httpClient;

        public ServEmprestimos()
        {
            _dataContext = GeradorDeServicos.CarregarContexto();
            _httpClient = new HttpClient();
        }

        // INTEGRAÇÃO 2 (Busca Simples): Buscar status do Livro no MsAcervo
        public async Task<Emprestimo> RegistrarEmprestimo(Emprestimo emprestimo)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5001/api/acervo/{emprestimo.IdLivro}");
            if (!response.IsSuccessStatusCode) throw new Exception("Erro: Livro não encontrado no Acervo.");

            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            var statusLivro = jsonDoc.RootElement.GetProperty("statusDisponibilidade").GetString();

            if (statusLivro != "Disponível")
                throw new Exception($"Bloqueado. O status atual do livro é: {statusLivro}");

            emprestimo.DataEmprestimo = DateTime.Now;
            emprestimo.Status = "Ativo";

            _dataContext.Emprestimos.Add(emprestimo);
            _dataContext.SaveChanges();
            return emprestimo;
        }

        // INTEGRAÇÃO 1 (Busca Simples): Buscar dados do Leitor no MsLeitores para o Recibo
        public async Task<object> ObterRecibo(int id)
        {
            var emprestimo = _dataContext.Emprestimos.FirstOrDefault(e => e.Id == id);
            if (emprestimo == null) throw new Exception("Empréstimo não encontrado.");

            var response = await _httpClient.GetAsync($"http://localhost:5002/api/leitores/{emprestimo.IdLeitor}");
            if (!response.IsSuccessStatusCode) throw new Exception("Erro de integração ao buscar Leitor.");

            var content = await response.Content.ReadAsStringAsync();
            var leitor = JsonSerializer.Deserialize<object>(content);

            // Retorna um recibo com os dados combinados
            return new
            {
                ReciboId = emprestimo.Id,
                Data = emprestimo.DataEmprestimo,
                Status = emprestimo.Status,
                LivroId = emprestimo.IdLivro,
                DadosContatoLeitor = leitor
            };
        }

        // INTEGRAÇÃO 3 (Alteração): Disparar alteração para zerar multa no MsLeitores
        public async Task DevolverLivro(int id)
        {
            var emprestimo = _dataContext.Emprestimos.FirstOrDefault(e => e.Id == id);
            if (emprestimo == null) throw new Exception("Empréstimo não encontrado.");
            if (emprestimo.Status == "Devolvido") throw new Exception("Livro já devolvido.");

            emprestimo.Status = "Devolvido";
            emprestimo.DataDevolucao = DateTime.Now;
            _dataContext.SaveChanges();

            // Dispara a requisição PUT que altera dados no outro MS
            var response = await _httpClient.PutAsync($"http://localhost:5002/api/leitores/{emprestimo.IdLeitor}/zerar-multa", null);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Livro devolvido, mas houve falha ao avisar o sistema de leitores para zerar a multa.");
        }
    }
}

using System;

namespace MsEmprestimos.DTO
{
    public class Emprestimo
    {
        public int Id { get; set; }
        public int IdLivro { get; set; }
        public int IdLeitor { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public string Status { get; set; } // Valores: "Ativo" ou "Devolvido"
    }
}

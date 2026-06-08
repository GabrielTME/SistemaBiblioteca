namespace MsAcervo.DTO
{
    public class Livro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string StatusDisponibilidade { get; set; } // Valores esperados: "Disponível", "Emprestado", "Perdido", "Em manutenção"
    }
}

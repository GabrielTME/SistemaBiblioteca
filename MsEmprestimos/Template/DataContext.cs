using Microsoft.EntityFrameworkCore;
using MsEmprestimos.DTO;

namespace MsEmprestimos
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Emprestimo> Emprestimos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Emprestimo>().HasKey(p => p.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}

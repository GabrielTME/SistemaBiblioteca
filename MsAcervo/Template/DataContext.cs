using Microsoft.EntityFrameworkCore;
using MsAcervo.DTO;

namespace MsAcervo
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        
        public DbSet<Livro> Livros { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Livro>().HasKey(p => p.Id);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}

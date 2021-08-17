using ApiWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiWeb
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        { }

        public MyContext() { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=CNL000661TI\SQLEXPRESS;Initial Catalog=ControleDeUsuarios;Integrated Security=True");
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace ApiCart.Models
{
    class APIContextDB : DbContext
    {
        public APIContextDB(DbContextOptions<APIContextDB> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "CarrinhoData");
        }
        public DbSet<Carrinho> Carrinhos { get; set; }
        public DbSet<Produto> produtos { get; set; }
    }


}


namespace ApiCart.Models{
    public class Carrinho
    {
        public Carrinho() { itens = new List<Produto>(); }
        public int Id { get; set; }
        public virtual List<Produto> itens { get; set; }

    }
}

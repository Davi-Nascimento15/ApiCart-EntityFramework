using System.ComponentModel.DataAnnotations;

namespace ApiCart.Models
{
    public class Produto
    {
        [Key]
        public int codigo { get; set; }
        public string? nome { get; set; }
        public int quantidade { get; set; }
        public double valor { get; set; }
    }
}

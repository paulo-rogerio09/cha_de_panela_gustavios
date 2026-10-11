namespace cha_de_panela.Models;

public class Convidado
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int QuantidadeAcompanhantes { get; set; } = 0;
}
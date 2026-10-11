namespace cha_de_panela.Models;

public class Presente
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string ImagemUrl { get; set; } = string.Empty;
    public bool Comprado { get; set; } = false;
    public string? NomeConvidado { get; set; }
}
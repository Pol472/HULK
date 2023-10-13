using Hulk.Biblioteca.Semantic;

public sealed class VariableSymbol
{
    public VariableSymbol(string nombre , Type tipo, TipoHulk tipoHulk)
    {
        Nombre = nombre;
        Tipo = tipo;
        TipoHulk = tipoHulk;
    }

    public string Nombre { get; }
    public Type Tipo { get; }
    public TipoHulk TipoHulk { get; }
}

using Hulk.Biblioteca.Semantic;
using Hulk.Biblioteca.Tree;

public  class VariableSymbol
{
    public VariableSymbol(string nombre, Type tipo, TipoHulk tipoHulk)
    {
        Nombre = nombre;
        Tipo = tipo;
        TipoHulk = tipoHulk;
    }

    public string Nombre { get; }
    public Type Tipo { get; }
    public TipoHulk TipoHulk { get; }
}

public class FuncionSymbol
{

    public FuncionSymbol(string name, int parametros)
    {
        Name = name;
        Parametros = parametros;
    }

    public string Name { get; }
    public int Parametros { get; }

}


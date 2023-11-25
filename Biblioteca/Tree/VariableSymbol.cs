using Hulk.Biblioteca.Semantic;
using Hulk.Biblioteca.Tree;

public  class VariableSymbol
{
    //Esta objeto tiene como utilidad principal poder asignar un tipo a una variable
    // cosa que con una simple asociacion string-object no era posible, con el objetivo
    //de saber con que valor se podra sobrescribir
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

//De igual manera, la utilidad de este simbolo era la sobrecarga de funciones ,
// no implementada finalmente por la no exigencia 
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


using Hulk.Biblioteca.Errores;
using Hulk.Biblioteca.Parser;
using Hulk.Biblioteca.Tree;

//Esta clase se encarga de manejar el objeto que parsea cada linea de entrada
public sealed class SyntaxTree
{
    //Errores producidos en el proceso de parsing
    public IEnumerable<Error> Errores { get; }
    //Expresion mas externa obtenida
    public Expresion Declaracion { get; }

    public Token FinLinea { get; }
    
    public SyntaxTree(IEnumerable<Error> errores, Expresion expresion, Token finLinea)
    {
        Errores = errores;
        Declaracion= expresion;
        FinLinea = finLinea;
        
    }

    public static SyntaxTree Parse(string text)
    {
        //Creacion del objeto parser
        var parser = new Parser(text);
        //Llamada a parsear
        return parser.Analiza();
    }
}

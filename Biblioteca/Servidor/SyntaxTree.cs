using Hulk.Biblioteca.Errores;
using Hulk.Biblioteca.Parser;
using Hulk.Biblioteca.Tree;


public sealed class SyntaxTree
{
    public IEnumerable<Error> Errores { get; }
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
        var parser = new Parser(text);
        return parser.Analiza();
    }
}

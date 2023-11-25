using Hulk.Biblioteca.Semantic;

namespace Hulk.Biblioteca.Tree
{
    //Clase de las expresiones let-in
    public sealed class VariableDeclaracionExpresion : DeclaracionNodo
    {
        public VariableDeclaracionExpresion(Token letKeyWord, List<AsignacionVariableExpresion> declaraciones, Token inKeyWord, Expresion contexto)
        {
            LetKeyWord = letKeyWord;
            Declaraciones = declaraciones;
            InKeyWord = inKeyWord;
            Contexto = contexto;
        }

        public override TokenType Type => TokenType.VariableDeclaracionExpresion;

        public Token LetKeyWord { get; }
        public List<AsignacionVariableExpresion> Declaraciones { get; }
        public Token? InKeyWord { get; }
        public Expresion? Contexto { get; }

        public override TipoHulk TipoDato => TipoHulk.Void;
    }
    

}
using Hulk.Biblioteca.Semantic;

namespace Hulk.Biblioteca.Tree
{
    //Esta clase maneja los objeros de tipo identificador 
    public sealed class IDExpresion : Expresion
    {
        public IDExpresion(Token identificador)
        {
            Identificador = identificador;
        }

        public Token Identificador { get; }

        public override TokenType Type => TokenType.IDExpresion;

        public override TipoHulk TipoDato => TipoHulk.Identificador;
    }
}
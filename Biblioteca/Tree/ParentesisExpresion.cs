using Hulk.Biblioteca.Semantic;

namespace Hulk.Biblioteca.Tree
{
    // Este objeto maneja a las expresiones entre parentesis
 
    public sealed class ParentesisExpresion : Expresion
    {
        public Token OpenParenToken { get; }
        public Expresion Expresion { get; }
        public Token CloseParenToken { get; }

        public ParentesisExpresion(Token openParenToken, Expresion expresion, Token closeParenToken)
        {
            OpenParenToken = openParenToken;
            Expresion = expresion;
            CloseParenToken = closeParenToken;
        }

        public override TokenType Type => TokenType.ParentesisExpresion;
        public override TipoHulk TipoDato => Expresion.TipoDato;


    }
}
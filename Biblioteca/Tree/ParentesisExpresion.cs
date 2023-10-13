namespace Hulk.Biblioteca.Tree
{

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
        public override TiposPrimitivosHulk TipoDato => Expresion.TipoDato;


    }
}
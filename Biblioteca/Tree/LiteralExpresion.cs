using Hulk.Biblioteca.Semantic;
namespace Hulk.Biblioteca.Tree
{

    public sealed class LiteralExpresion : Expresion
    {
        public Token Literal { get; }
        public object Value { get; }
        public override TipoHulk TipoDato {get;}
        public override TokenType Type => TokenType.LiteralExpresion;
        public LiteralExpresion(Token literal) : this(literal, literal.Value)
        {
        }

        public LiteralExpresion(Token literal, object value)
        {
            Literal = literal;
            Value = value;
            switch(Literal.Type)
            {
                case TokenType.IdentificadorToken:
                TipoDato = TipoHulk.Identificador;
                break;
                case TokenType.StringToken:
                TipoDato = TipoHulk.String;
                break;
                case TokenType.NumberToken:
                TipoDato = TipoHulk.Number;
                break;
                case TokenType.FalseKeyWord:
                TipoDato = TipoHulk.Boolean;
                break;
            }
        }

       
    }
}
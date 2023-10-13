namespace Hulk.Biblioteca.Tree
{

    public sealed class LiteralExpresion : Expresion
    {
        public Token Literal { get; }
        public object Value { get; }
        public override TiposPrimitivosHulk TipoDato {get;}
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
                TipoDato = TiposPrimitivosHulk.Identificador;
                break;
                case TokenType.StringToken:
                TipoDato = TiposPrimitivosHulk.String;
                break;
                case TokenType.NumberToken:
                TipoDato = TiposPrimitivosHulk.Number;
                break;
                case TokenType.FalseKeyWord:
                TipoDato = TiposPrimitivosHulk.Identificador;
                break;
            }
        }

       
    }
}
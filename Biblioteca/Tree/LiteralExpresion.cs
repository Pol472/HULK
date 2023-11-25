using Hulk.Biblioteca.Semantic;
namespace Hulk.Biblioteca.Tree
{
    //Aqui son manejados todos los literales
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
            //Con este swicht clasificamos cada literal en el tipo de dato Hulk correspondiente de acuerdo a su tipo System
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
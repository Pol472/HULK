using Hulk.Biblioteca.Tree;
namespace Hulk.Biblioteca.Semantic
{
    internal  class Semantic_UnaryExpresion : Semantic_Expresion
    {
        public Semantic_UnaryExpresion(Semantic_UnaryOperador operador, Semantic_Expresion operando)
        {
            Operador = operador;
            Operando = operando;
            
        }

        public Semantic_UnaryOperador Operador { get; }
        public Semantic_Expresion Operando { get; }

        public override SemanticType Kind => SemanticType.UnaryExpresion;
        

        public override TipoHulk TipoHulk => Operando.TipoHulk;

        
    }
    internal sealed class Semantic_UnaryOperador
    {
        public Semantic_UnaryOperador(TokenType tokentype,
        Semantic_UnaryOperadorType tipoOperador, TipoHulk operando) : this(tokentype, tipoOperador, operando, operando)
        {
        }
        public Semantic_UnaryOperador(TokenType tokentype, Semantic_UnaryOperadorType tipoOperador, TipoHulk operando, TipoHulk returnType)
        {
            Tokentype = tokentype;
            TipoOperador = tipoOperador;
            Operando = operando;
            ReturnType = returnType;
        }

        public TokenType Tokentype { get; }
        public Semantic_UnaryOperadorType TipoOperador { get; }
        public TipoHulk Operando { get; }
        public TipoHulk ReturnType { get; }
        private static Semantic_UnaryOperador[] operadores =
        {
            new Semantic_UnaryOperador(TokenType.Negation,Semantic_UnaryOperadorType.NegacionLogica,TipoHulk.Boolean),
            new Semantic_UnaryOperador(TokenType.PlusToken,Semantic_UnaryOperadorType.Identidad,TipoHulk.Number),
            new Semantic_UnaryOperador(TokenType.MinusToken,Semantic_UnaryOperadorType.Negacion,TipoHulk.Number),
            new Semantic_UnaryOperador(TokenType.Sen,Semantic_UnaryOperadorType.Seno,TipoHulk.Number),
            new Semantic_UnaryOperador(TokenType.Cos,Semantic_UnaryOperadorType.Coseno,TipoHulk.Number),
            new Semantic_UnaryOperador(TokenType.Sqrt,Semantic_UnaryOperadorType.RaizCuadrada,TipoHulk.Number)
        };
        public static Semantic_UnaryOperador Semantic_Parse_UO(TokenType tokenType, TipoHulk operandoTipo)
        {
            foreach (var a in operadores)
            {
                if (a.Tokentype == tokenType && a.Operando == operandoTipo)
                    return a;

            }
            return null;
        }

    }
      internal enum Semantic_UnaryOperadorType
    {
        Negacion,
        Identidad,
        NegacionLogica,
        Seno,
        Coseno,
        RaizCuadrada
    }

}
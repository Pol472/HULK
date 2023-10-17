using Hulk.Biblioteca.Semantic;

namespace Hulk.Biblioteca.Tree
{
    public sealed class UnaryExpresion : Expresion
    {
        public Token Operador { get; }
        public Expresion Operando { get; }

        public UnaryExpresion(Token operador, Expresion operando)
        {
            Operador = operador;
            Operando = operando;
        }
        public override TokenType Type => TokenType.UnaryExpresion;

        public override TipoHulk TipoDato => Operando.TipoDato;
    }
}
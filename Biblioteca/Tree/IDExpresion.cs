namespace Hulk.Biblioteca.Tree
{
    public sealed class IDExpresion : Expresion
    {
        public IDExpresion(Token identificador)
        {
            Identificador = identificador;
        }

        public Token Identificador { get; }

        public override TokenType Type => TokenType.IDExpresion;

        public override TiposPrimitivosHulk TipoDato => TiposPrimitivosHulk.Identificador;
    }
}
namespace Hulk.Biblioteca.Tree
{


    public sealed class BinaryExpresion : Expresion
    {
        public Expresion Left { get; }
        public Token Operador { get; }
        public Expresion Rigth { get; }
        

        public BinaryExpresion(Expresion left, Token operador, Expresion rigth)
        {
            Left = left;
            Rigth = rigth;
            Operador = operador;
        }
        public override TokenType Type => TokenType.BinaryExpresion;

        public override TiposPrimitivosHulk TipoDato => Left.TipoDato;
    }
}
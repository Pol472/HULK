using Hulk.Biblioteca.Semantic;

namespace Hulk.Biblioteca.Tree
{
    //Objeto que caracteriza a las expresiones print
    public class PrintExpresion : Expresion
    {
        public PrintExpresion(Token printKeyword, ParentesisExpresion expresion)
        {
            PrintKeyword = printKeyword;
            Expresion = expresion;
        }

        public override TipoHulk TipoDato =>  Expresion.TipoDato;

        public override TokenType Type => TokenType.PrintExpresion;

        public Token PrintKeyword { get; }
        public ParentesisExpresion Expresion { get; }
    }



}

using Hulk.Biblioteca.Semantic;

namespace Hulk.Biblioteca.Tree
{
    public abstract class DeclaracionNodo : Expresion
    {

    }
   

    public sealed class DeclaracionExpresion : DeclaracionNodo
    {
        public DeclaracionExpresion(Expresion expresion)
        {
            Expresion = expresion;
        }

        public override TokenType Type => TokenType.DeclaracionExpresion;

        public Expresion Expresion { get; }

        public override TipoHulk TipoDato => Expresion.TipoDato;
    }


    public sealed class IFDeclaracionExpresion : DeclaracionNodo
    {
        public IFDeclaracionExpresion(Token ifKeyWord, Expresion condicion, Expresion cuerpoIf,
        ElseDeclaracion cuerpoElse)
        {
            IfKeyWord = ifKeyWord;
            Condicion = condicion;
            CuerpoIf = cuerpoIf;
            CuerpoElse = cuerpoElse;
        }

        public override TokenType Type => TokenType.IFDeclaracionExpresion;

        public Token IfKeyWord { get; }
        public Expresion Condicion { get; }
        public Expresion CuerpoIf { get; }
        public ElseDeclaracion CuerpoElse { get; }

        public override TipoHulk TipoDato => TipoHulk.Void;
    }


    public sealed class ElseDeclaracion : DeclaracionNodo
    {
        public ElseDeclaracion(Token elseKeyWord, Expresion cuerpoElse)
        {
            ElseKeyWord = elseKeyWord;
            CuerpoElse = cuerpoElse;
        }

        public override TokenType Type => TokenType.ElseDeclaracion;

        public Token ElseKeyWord { get; }
        public Expresion CuerpoElse { get; }

        public override TipoHulk TipoDato => TipoHulk.Void;
    }
}
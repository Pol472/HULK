using Hulk.Biblioteca.Semantic;

namespace Hulk.Biblioteca.Tree
{
    public sealed class AsignacionVariableExpresion : Expresion
    {
        public AsignacionVariableExpresion( Token identificador, Token asignador,
        Expresion expresion)
        {
           
            Identificador = identificador;
            Asignador = asignador;
            Expresion = expresion;
            
        }

       
        public Token Identificador { get; }
        public Token Asignador { get; }
        public Expresion Expresion { get; }
       

        public override TokenType Type => TokenType.AsignacionVariableExpresion;

        public override TipoHulk TipoDato => TipoHulk.Void;
    }
}
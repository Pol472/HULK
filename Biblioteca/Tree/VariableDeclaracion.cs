namespace Hulk.Biblioteca.Tree
{
    
    public sealed class VariableDeclaracionExpresion : DeclaracionNodo
    {
        public VariableDeclaracionExpresion(Token letKeyWord, List<AsignacionVariableExpresion> declaraciones, Token inKeyWord, Expresion contexto)
        {
            LetKeyWord = letKeyWord;
            Declaraciones = declaraciones;
            InKeyWord = inKeyWord;
            Contexto = contexto;
        }

        public override TokenType Type => TokenType.VariableDeclaracionExpresion;

        public Token LetKeyWord { get; }
        public List<AsignacionVariableExpresion> Declaraciones { get; }
        public Token InKeyWord { get; }
        public Expresion Contexto { get; }

        public override TiposPrimitivosHulk TipoDato => TiposPrimitivosHulk.MultipleAsignacion;
    }
    public sealed class FuncionDeclaracion : Expresion
    {
        public Token FuncionKeyWord { get; }
        public Token Identificador { get; }
        public ParentesisExpresion Parametros { get; }
        public Token Retornador { get; }
        public Expresion Cuerpo { get; }

        public FuncionDeclaracion(Token funcionKeyWord , Token identificador , ParentesisExpresion parametros , Token retornador, Expresion cuerpo)
        {
            FuncionKeyWord = funcionKeyWord;
            Identificador = identificador;
            Parametros = parametros;
            Retornador = retornador;
            Cuerpo = cuerpo;
            
        }

        public override TiposPrimitivosHulk TipoDato => Cuerpo.TipoDato;

        public override TokenType Type => TokenType.FuncionDeclaracion;
    }

}
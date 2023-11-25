using Hulk.Biblioteca.Semantic;

namespace Hulk.Biblioteca.Tree
{
    //clase que maneja los objetos de tipo llamada a funcion 
    public sealed class FuncionCallExpresion : Expresion
    {
        public Token Identificador { get; }
        // Dado que una llamada a funcion puede contener como argumentos cualquier tipo de expresion
        // la lista tiene al tipo "generico" Expresion 
        public List<Expresion> Argumentos { get; }

        public FuncionCallExpresion(Token identificador, List<Expresion> argumentos)
        {
            Identificador = identificador;
            Argumentos = argumentos;
        }

        public override TipoHulk TipoDato => TipoHulk.Void;

        public override TokenType Type => TokenType.CallFuncion;
    }
    // Esta clase maneja las declaraciones de funciones
    public sealed class FuncionDeclaracion : Expresion
    {
        public Token FuncionKeyWord { get; }
        public Token Identificador { get; }
        public List<Token> Parametros { get; }
        public Token Retornador { get; }
        public Expresion Cuerpo { get; }

        public FuncionDeclaracion(Token funcionKeyWord, Token identificador, List<Token> parametros, Token retornador, Expresion cuerpo)
        {
            FuncionKeyWord = funcionKeyWord;
            Identificador = identificador;
            Parametros = parametros;
            Retornador = retornador;
            Cuerpo = cuerpo;

        }

        public override TipoHulk TipoDato => Cuerpo.TipoDato;

        public override TokenType Type => TokenType.FuncionDeclaracion;
    }




}


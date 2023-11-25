using Hulk;
using Hulk.Biblioteca.Errores;
using Hulk.Biblioteca.Tree;

namespace Hulk.Biblioteca.Parser
{
    public sealed class Parser
    {
        //Array de Token proveniente del lexer
        private readonly Token[] tokens;

        //Posicion actual del array 
        private int position;

        //Lista de Errores
        private List<Error> errores = new List<Error>();

        //Diccionario para guardar las funciones declaradas inline
        public static Dictionary<string, FuncionDeclaracion> Funciones = new Dictionary<string, FuncionDeclaracion>();
        public Parser(string text)
        {
            var tokensList = new List<Token>();
            var Lexer = new Lexer(text);

            //Crear la lista de tokens
            Token token;
            do
            {
                token = Lexer.NextToken();
                if (token.Type != TokenType.WhiteSpaceToken && token.Type != TokenType.UnknownToken)
                    tokensList.Add(token);
            } while (token.Type != TokenType.EndOfLine);

            //Inicializar el array de token
            tokens = tokensList.ToArray();

            errores.AddRange(Lexer.Errores);

        }
        //Metodo central
        public SyntaxTree Analiza()
        {
            if (tokens.Length == 1)
                return null;

            var declaracion = AnalizaExpresion();
            var finLinea = Coincidencia(TokenType.EndOfLine);
            return new SyntaxTree(errores, declaracion, finLinea);
        }
        //Metodo para identificar el posible tipo de expresion actual
        private Expresion AnalizaExpresionBase()
        {
            switch (Actual.Type)
            {
                //Constante E
                case TokenType.Euler:
                    return ParseEuler();
                //Constante PI
                case TokenType.PI:
                    return ParsePI();
                //Declaracion de funciones
                case TokenType.FuncionKeyWord:
                    return AnalizaFuncionDeclaracion();
                //Expresion Let-in
                case TokenType.LetKeyWord:
                    return AnalizaExpresionLet();
                //Expresion if-else
                case TokenType.IfKeyWord:
                    return AnalizaExpresionIf();
                //Llamada a funcion print
                case TokenType.PrintKeyWord:
                    return AnalizaPrintExpresion();
                //Expresiones entre parentesis
                case TokenType.OpenParenToken:
                    return ParseParentesisExpresion();


                //Literales
                //Booleanos
                case TokenType.TrueKeyWord:
                case TokenType.FalseKeyWord:
                    return ParseBooleanExpresion();
                //Numeros
                case TokenType.NumberToken:
                    return ParseNumberExpresion();
                //String
                case TokenType.StringToken:
                    return ParseStringExpresion();

                //Funciones matematicas predefinidas
                case TokenType.SenKeyWord:
                case TokenType.CosKeyWord:
                case TokenType.LogKeyWord:
                case TokenType.SqrtKeyWord:
                //Identificador
                case TokenType.IdentificadorToken:
                    return ParseIDorFuncionCallExpresion();
                default:
                    return ParseIDExpresion();
            }

        }


        //Parsing de expresiones binarias
        private Expresion AnalizaPrioridad(int padrePrioridad = 0)
        {
            Expresion left;
            var prioridadOperadorUnario = Actual.Type.PrioridadOperadorUnario();

            if (prioridadOperadorUnario != 0 && prioridadOperadorUnario >= padrePrioridad)
            {
                var operador = NextToken();
                var operando = AnalizaPrioridad(prioridadOperadorUnario);
                left = new UnaryExpresion(operador, operando);
            }
            else
            {
                left = AnalizaExpresionBase();
            }

            while (true)
            {
                var prioridad = Actual.Type.PrioridadOperadorBinario();
                if (prioridad == 0 || prioridad <= padrePrioridad)
                    break;

                var operador = NextToken();
                var rigth = AnalizaPrioridad(prioridad);
                left = new BinaryExpresion(left, operador, rigth);
            }
            return left;
        }
        public Expresion AnalizaExpresionSecundaria()
        {
            return AnalizaAsignacion();
        }

        private Expresion AnalizaExpresion()
        {
            switch (Actual.Type)
            {
                case TokenType.FuncionKeyWord:
                    return AnalizaFuncionDeclaracion();
                case TokenType.PrintKeyWord:
                    return AnalizaPrintExpresion();
                case TokenType.LetKeyWord:
                    return AnalizaExpresionLet();
                case TokenType.IfKeyWord:
                    return AnalizaExpresionIf();
                default:
                    return AnalizaDeclaracionExpresion();
            }
        }

        //Metodo para parsear declaracion de funcion

        private Expresion AnalizaFuncionDeclaracion()
        {
            Token funcionKeyWord = Coincidencia(TokenType.FuncionKeyWord);
            Token nombre = Coincidencia(TokenType.IdentificadorToken);
            var parametros = ParseParametros();
            Token retornador = Coincidencia(TokenType.Flecha);
            Expresion cuerpo = AnalizaExpresion();

            var funcionsymbol = new FuncionSymbol(nombre.Text, parametros.Count);
            if (Funciones.ContainsKey(funcionsymbol.Name))
                errores.Add(new Error(TipoError.FuncionError, $"Function '{nombre}' with the same number of parameters and the same type of result already exist."));
            else
                Funciones.Add(funcionsymbol.Name, new FuncionDeclaracion(funcionKeyWord, nombre, parametros, retornador, cuerpo));

            return null;
        }
        //Metodo auxiliar para parsear los parametros de una funcion

        private List<Token> ParseParametros()
        {
            Coincidencia(TokenType.OpenParenToken);
            List<Token> parametros = new List<Token>();
            if (Actual.Type != TokenType.CloseParenToken)
            {
                while (Actual.Type != TokenType.CloseParenToken)
                {

                    var ident = Coincidencia(TokenType.IdentificadorToken);
                    if (parametros.Contains(ident))
                    {
                        errores.Add(new Error(TipoError.SintacticError, $"Function declaration already has a parameter with the name '{ident.Text}'."));
                    }
                    else
                        parametros.Add(ident);
                    if (Actual.Type == TokenType.Coma)
                        Coincidencia(TokenType.Coma);
                }
            }
            Coincidencia(TokenType.CloseParenToken);
            return parametros;
        }

        //Metodos para parsear expresion if-else

        private DeclaracionNodo AnalizaExpresionIf()
        {
            var ifKeyWord = Coincidencia(TokenType.IfKeyWord);
            var condicion = AnalizaExpresion();
            var cuerpo = AnalizaExpresion();
            var cuerpoElse = AnalizaExpresionElse();
            return new IFDeclaracionExpresion(ifKeyWord, condicion, cuerpo, cuerpoElse);
        }

        private ElseDeclaracion AnalizaExpresionElse()
        {
            if (Actual.Type != TokenType.ElseKeyWord)
                return null;

            var elseKeyWord = Coincidencia(TokenType.ElseKeyWord);
            var cuerpo = AnalizaExpresion();
            return new ElseDeclaracion(elseKeyWord, cuerpo);
        }


        //Metodo para parsear expresion let-in
        private VariableDeclaracionExpresion AnalizaExpresionLet()
        {
            var Keyword = Coincidencia(TokenType.LetKeyWord);

            List<AsignacionVariableExpresion> lista = new List<AsignacionVariableExpresion>();
            //Este bucle es para garantizar que se manejan las expresiones let con mas de una variable
            while (Actual.Type != TokenType.InKeyWord)
            {
                var identificador = Coincidencia(TokenType.IdentificadorToken);
                var asignador = Coincidencia(TokenType.IgualAsignador);
                var expresion = AnalizaExpresion();
                lista.Add(new AsignacionVariableExpresion(identificador, asignador, expresion));
                if (Actual.Type == TokenType.Coma)
                    NextToken();
            }

            var InKeyWord = Coincidencia(TokenType.InKeyWord);
            var Contexto = AnalizaExpresion();
            return new VariableDeclaracionExpresion(Keyword, lista, InKeyWord, Contexto);
        }

        //Metodo auxiliar para expresiones let-in, asignaciones
        public Expresion AnalizaAsignacion()
        {

            if (Take(0).Type == TokenType.IdentificadorToken && Take(1).Type == TokenType.IgualAsignador)
            {
                var identificador = NextToken();
                Token operador = NextToken();
                Expresion rigth = AnalizaAsignacion();
                return new AsignacionVariableExpresion(identificador, operador, rigth);
            }
            return AnalizaPrioridad();
        }


        private Expresion AnalizaDeclaracionExpresion()
        {
            var expresion = AnalizaExpresionSecundaria();
            return new DeclaracionExpresion(expresion);
        }




        private Expresion ParseIDorFuncionCallExpresion()
        {

            if ((Actual.Type == TokenType.IdentificadorToken
            || Actual.Type == TokenType.SenKeyWord || Actual.Type == TokenType.CosKeyWord ||
            Actual.Type == TokenType.LogKeyWord || Actual.Type == TokenType.SqrtKeyWord)
            && Take(1).Type == TokenType.OpenParenToken)
                return ParseFuncionCallExpresion();

            return ParseIDExpresion();
        }

        //Metodo para parsear llamadas a funciones

        private Expresion ParseFuncionCallExpresion()
        {

            var ident = Actual;
            NextToken();
            List<Expresion> parametros = new List<Expresion>();
            Coincidencia(TokenType.OpenParenToken);
            while (Actual.Type != TokenType.CloseParenToken)
            {
                var argumento = AnalizaExpresion();
                parametros.Add(argumento);
                if (Actual.Type == TokenType.Coma)
                    NextToken();
            }
            Coincidencia(TokenType.CloseParenToken);
            return new FuncionCallExpresion(ident, parametros);
        }

        //Metodos para parsear las constantes

        private Expresion ParsePI()
        {
            var pi = Coincidencia(TokenType.PI);
            return new LiteralExpresion(pi, Math.PI);
        }

        private Expresion ParseEuler()
        {
            var euler = Coincidencia(TokenType.Euler);
            return new LiteralExpresion(euler, Math.E);
        }

        //Metodo para parsear la llamada a print
        private Expresion AnalizaPrintExpresion()
        {
            var printKeyword = Coincidencia(TokenType.PrintKeyWord);
            var expresion = ParseParentesisExpresion();
            return new PrintExpresion(printKeyword, expresion);
        }

        //Metodos para parsear literales
        //String

        private Expresion ParseStringExpresion()
        {
            var a = Coincidencia(TokenType.StringToken);
            return new LiteralExpresion(a);
        }
        //Number
        private Expresion ParseNumberExpresion()
        {
            var num = Coincidencia(TokenType.NumberToken);
            return new LiteralExpresion(num);
        }
        //Boolean
           private Expresion ParseBooleanExpresion()
        {
            var keyword = NextToken();
            var value = keyword.Type == TokenType.TrueKeyWord;
            return new LiteralExpresion(keyword, value);
        }

        //Metodo para parsear expresiones entre parentesis
        private ParentesisExpresion ParseParentesisExpresion()
        {
            var left = Coincidencia(TokenType.OpenParenToken);
            var expresion = AnalizaExpresion();
            var rigth = Coincidencia(TokenType.CloseParenToken);

            return new ParentesisExpresion(left, expresion, rigth);
        }

     
        //Metodo para devolver un identificador(ya parseado)
        private Expresion ParseIDExpresion()
        {
            Token identificador = Coincidencia(TokenType.IdentificadorToken);
            return new IDExpresion(identificador);
        }

        


        //Metodos para moverse sobre el array de tokens
        private Token Actual => Take(0);
        private Token Take(int count)
        {
            var index = position + count;
            if (index >= tokens.Length)
                return tokens[tokens.Length - 1];

            return tokens[index];
        }
        private Token NextToken()
        {
            var actual = Actual;
            position++;
            return actual;
        }

        //Metodo para contrastar si el token actual era el esperado(matching)
        private Token Coincidencia(TokenType tipo)
        {
            if (Actual.Type == tipo)
                return NextToken();

            string expected = (Prioridad.GetText(tipo) == null) ? tipo.ToString() : Prioridad.GetText(tipo);
            errores.Add(new Error(TipoError.LexicalError, $"Unexpected token '{Actual.Text}'."));
            return new Token(tipo, Actual.Position, null, null);
        }




    }
}
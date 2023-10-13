using Hulk;
using Hulk.Biblioteca.Errores;
using Hulk.Biblioteca.Tree;

namespace Hulk.Biblioteca.Parser
{
    internal sealed class Parser
    {

        private readonly Token[] tokens;
        private int position;
        private List<Error> errores = new List<Error>();
        public Parser(string text)
        {
            var tokensList = new List<Token>();
            var Lexer = new Lexer(text);

            //Crear la lista de tokens
            Token token;
            do
            {
                token = Lexer.NextToken();
                if (token.Type != TokenType.WhiteSpaceToken && token.Type != TokenType.UnknownToken )
                    tokensList.Add(token);
            } while (token.Type != TokenType.EndOfLine);

            //Inicializar el array de token
            tokens = tokensList.ToArray();

            errores.AddRange(Lexer.Errores);
        }
        public IEnumerable<Error> Errores => errores;


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
        public SyntaxTree Analiza()
        {
            if(tokens.Length == 1)
            return null; 
           
            var declaracion = AnalizaExpresion();
            var finLinea = Coincidencia(TokenType.EndOfLine);
            return new SyntaxTree(errores,declaracion,finLinea);
        }
        private Expresion AnalizaExpresion()
        {
            switch (Actual.Type)
            {
                case TokenType.LetKeyWord:
                    return AnalizaExpresionLet();
                case TokenType.IfKeyWord:
                    return AnalizaExpresionIf();
                default:
                    return AnalizaDeclaracionExpresion();
            }
        }

       

        private DeclaracionNodo AnalizaExpresionIf()
        {
            var ifKeyWord = Coincidencia(TokenType.IfKeyWord);
            var condicion = AnalizaExpresion();
            var cuerpo = AnalizaExpresion();
            var cuerpoElse = AnalizaExpresionElse();
            return new IFDeclaracionExpresion(ifKeyWord,condicion,cuerpo,cuerpoElse);
        }

        private ElseDeclaracion AnalizaExpresionElse()
        {
            if(Actual.Type != TokenType.ElseKeyWord)
            return null;

            var elseKeyWord = Coincidencia(TokenType.ElseKeyWord);
            var cuerpo = AnalizaExpresion();
            return new ElseDeclaracion(elseKeyWord,cuerpo);
        }

        private VariableDeclaracionExpresion AnalizaExpresionLet()
        {
            var Keyword = Coincidencia(TokenType.LetKeyWord);

            List<AsignacionVariableExpresion> lista = new List<AsignacionVariableExpresion>();
            while(Actual.Type != TokenType.InKeyWord && Actual.Type != TokenType.Coma)
            {
                var identificador = Coincidencia(TokenType.IdentificadorToken);
                var asignador = Coincidencia(TokenType.IgualAsignador);
                var expresion = AnalizaExpresion();
                lista.Add(new AsignacionVariableExpresion(identificador,asignador,expresion));
        

            }

     
            var InKeyWord = Coincidencia(TokenType.InKeyWord);
            var Contexto = AnalizaExpresion();
            return new VariableDeclaracionExpresion(Keyword,lista, InKeyWord,Contexto);
        }

        private Expresion AnalizaDeclaracionExpresion()
        {
            var expresion = AnalizaExpresionSecundaria();
            return new DeclaracionExpresion(expresion);
        }

        

        private Expresion AnalizaExpresionBase()
        {
            switch (Actual.Type)
            {
                case TokenType.OpenParenToken:
                    return ParseParentesisExpresion();
                case TokenType.TrueKeyWord:
                case TokenType.FalseKeyWord:
                    return ParseBooleanExpresion();
                case TokenType.NumberToken:
                    return ParseNumberExpresion();
                case TokenType.StringToken:
                    return ParseStringExpresion();
                case TokenType.IdentificadorToken:
                    return ParseIDExpresion();
                default:
                    return AnalizaExpresion();
            }

        }

        private Expresion ParseStringExpresion()
        {
             var a = Coincidencia(TokenType.StringToken);
            return new LiteralExpresion(a);
        }

        private Expresion ParseNumberExpresion()
        {
            var num = Coincidencia(TokenType.NumberToken);
            return new LiteralExpresion(num);
        }

        private ParentesisExpresion ParseParentesisExpresion()
        {
            var left = Coincidencia(TokenType.OpenParenToken);
            var expresion = AnalizaExpresion();
            var rigth = Coincidencia(TokenType.CloseParenToken);

            return new ParentesisExpresion(left, expresion, rigth);
        }

        private Expresion ParseBooleanExpresion()
        {
            var keyword = NextToken();
            var value = keyword.Type == TokenType.TrueKeyWord;
            return new LiteralExpresion(keyword, value);
        }

        private Expresion ParseIDExpresion()
        {
            Token identificador = Coincidencia(TokenType.IdentificadorToken);
            return new IDExpresion(identificador);
        }

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
       

      
        private Token Actual => Take(0);
        private Token Take(int count)
        {
            var index = position + count;
            if (index >= tokens.Length)
                return tokens[tokens.Length - 1];

            return tokens[index];
        }
        private Token Coincidencia(TokenType tipo)
        {
            if (Actual.Type == tipo)
                return NextToken();
            
            errores.Add(new Error(TipoError.LexicalError,$"Unexpected token '{Actual.Text}' , expected '{Prioridad.GetText(tipo)}'"));
            return new Token(tipo, Actual.Position, null, null);
        }

    

        private Token NextToken()
        {
            var actual = Actual;
            position++;
            return actual;
        }

    }
}
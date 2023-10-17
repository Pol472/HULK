using Hulk;
using Hulk.Biblioteca.Parser;
using Hulk.Biblioteca.Tree;
using Hulk.Biblioteca.Errores;
using System.Text;

namespace Hulk.Biblioteca
{
    internal sealed class Lexer
    {
        private readonly List<Error> errores = new List<Error>();
        private readonly string text;
        private int position;
        private int inicio;
        private TokenType Tipo;
        private object Valor;

        public Lexer(string texto)
        {
            text = texto;
        }
        public IEnumerable<Error> Errores => errores;


        private char Actual => Take(0);
        private char Busca => Take(1);
        private char Take(int count)
        {
            int index = position + count;
            if (index >= text.Length)
                return '\0';


            return text[index];
        }


        public Token NextToken()
        {
            Tipo = TokenType.UnknownToken;
            Valor = null;
            inicio = position;

            switch (Actual)
            {
                case ';':
                case '\0':
                    if (Actual != ';')
                        errores.Add(new Error(TipoError.SintacticError, "Missing ';' after the declaration."));
                    Tipo = TokenType.EndOfLine;
                    break;
                case ',':
                    Tipo = TokenType.Coma;
                    position++;
                    break;
                //Operadores aritmeticos
                case '+':
                    Tipo = TokenType.PlusToken;
                    position++;
                    break;
                case '-':
                    Tipo = TokenType.MinusToken;
                    position++;
                    break;
                case '*':
                    Tipo = TokenType.MultToken;
                    position++;
                    break;
                case '/':
                    Tipo = TokenType.DivToken;
                    position++;
                    break;
                case '%':
                    Tipo = TokenType.ModuloResto;
                    position++;
                    break;
                case '^':
                    Tipo = TokenType.Potencia;
                    position++;
                    break;
                //Operadores logicos
                case '&':
                    Tipo = TokenType.AndLogic;
                    position++;
                    break;
                case '|':
                    Tipo = TokenType.OrLogic;
                    position++;
                    break;
                //Comparadores
                case '=':
                    position++;
                    if (Actual == '>')
                    {
                        Tipo = TokenType.Flecha;
                        position++;
                    }
                    else if (Actual == '=')
                    {
                        Tipo = TokenType.IgualComparador;
                        position++;
                    }
                    else Tipo = TokenType.IgualAsignador;
                    break;
                case '!':
                    position++;
                    if (Actual != '=')
                        Tipo = TokenType.Negation;
                    else
                    {
                        Tipo = TokenType.DesigualComparador;
                        position++;
                    }
                    break;
                case '<':
                    position++;
                    if (Actual != '=')
                        Tipo = TokenType.MenorQue;
                    else
                    {
                        Tipo = TokenType.MenorOIgual;
                        position++;
                    }
                    break;
                case '>':
                    position++;
                    if (Actual != '=')
                        Tipo = TokenType.MayorQue;
                    else
                    {
                        Tipo = TokenType.MayorOIgual;
                        position++;
                    }
                    break;
                //Reconocimiento de parentesis
                case '(':
                    Tipo = TokenType.OpenParenToken;
                    position++;
                    break;
                case ')':
                    Tipo = TokenType.CloseParenToken;
                    position++;
                    break;

                case '"':
                    IsString();
                    break;

                case '@':
                    Tipo = TokenType.Concatenador;
                    position++;
                    break;

                default:
                    //Reconocimiento de Digitos
                    if (char.IsDigit(Actual))
                        IsNumberToken();
                    else if (char.IsLetter(Actual))
                        IsIdentificadorOrKeyword();
                    //Reconocimiento de Espacios en blanco
                    else if (char.IsWhiteSpace(Actual))
                        IsWhiteSpace();
                    else
                    {
                        //Reconocimiento de operadores
                        Tipo = TokenType.UnknownToken;
                        errores.Add(new Error(TipoError.LexicalError, $"'{Actual}' is not valid token"));
                        position++;

                    }
                    break;
            }


            int length = position - inicio;
            var NewText = Prioridad.GetText(Tipo);
            if (NewText == null)
                NewText = text.Substring(inicio, length);
            return new Token(Tipo, inicio, NewText, Valor);
        }

        private void IsString()
        {
            position++;
            var stringb = new StringBuilder();
            var done = false;
            while (!done)
            {
                switch (Actual)
                {
                    case '\r':
                    case '\n':
                    case '\0':

                        errores.Add(new Error(TipoError.SintacticError, $"String interrupted: '{stringb.ToString()}'."));
                        done = true;

                        break;
                    case '"':
                        if (Busca == '"')
                        {
                            stringb.Append(Actual);
                            position += 2;
                        }
                        else
                        {
                            position++;
                            done = true;
                        }
                        break;
                    default:
                        stringb.Append(Actual);
                        position++;
                        break;

                }
            }
            Tipo = TokenType.StringToken;
            Valor = stringb.ToString();
        }

        private void IsNumberToken()
        {
            while (char.IsDigit(Actual))
            {
                position++;

                if (Actual == '.')
                {
                    position++;
                }
            }

            var NewLength = position - inicio;
            var NewText = text.Substring(inicio, NewLength);

            if (!double.TryParse(NewText, out var value))
                errores.Add(new Error(TipoError.SemanticError, $"El número '{text}' no puede ser convertido a double"));

            Valor = value;
            Tipo = TokenType.NumberToken;

        }
        private void IsIdentificadorOrKeyword()
        {
            while (char.IsLetter(Actual))
                position++;

            var length = position - inicio;
            var NewText = text.Substring(inicio, length);
            Tipo = Prioridad.KeywordType(NewText);
        }
        private void IsWhiteSpace()
        {
            while (char.IsWhiteSpace(Actual))
                position++;

            Tipo = TokenType.WhiteSpaceToken;
        }
    }
}
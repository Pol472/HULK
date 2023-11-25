using Hulk;
using Hulk.Biblioteca.Tree;
namespace Hulk.Biblioteca.Parser
{
    internal static class Prioridad
    {
        // Metodo para devolver los textos de los tokens predefinidos
        public static string GetText(TokenType tipo)
        {
            switch (tipo)
            {
                case TokenType.Coma:
                    return ",";
                case TokenType.EndOfLine:
                    return ";";
                case TokenType.PlusToken:
                    return "+";
                case TokenType.MinusToken:
                    return "-";
                case TokenType.MultToken:
                    return "*";
                case TokenType.DivToken:
                    return "/";
                case TokenType.Potencia:
                    return "^";
                case TokenType.ModuloResto:
                    return "%";
                case TokenType.TrueKeyWord:
                    return "true";
                case TokenType.FalseKeyWord:
                    return "false";
                case TokenType.LetKeyWord:
                    return "let";
                case TokenType.InKeyWord:
                    return "in";
                case TokenType.IfKeyWord:
                    return "if";
                case TokenType.ElseKeyWord:
                    return "else";
                case TokenType.PrintKeyWord:
                    return "print";
                case TokenType.SenKeyWord:
                    return "sen";
                case TokenType.CosKeyWord:
                    return "cos";
                case TokenType.LogKeyWord:
                    return "log";
                case TokenType.SqrtKeyWord:
                    return "sqrt";
                case TokenType.PI:
                    return "PI";
                case TokenType.Euler:
                    return "E";
                case TokenType.MayorQue:
                    return ">";
                case TokenType.MayorOIgual:
                    return ">=";
                case TokenType.MenorQue:
                    return "<";
                case TokenType.MenorOIgual:
                    return "<=";
                case TokenType.IgualComparador:
                    return "==";
                case TokenType.IgualAsignador:
                    return "=";
                case TokenType.Flecha:
                    return "=>";
                case TokenType.AndLogic:
                    return "&";
                case TokenType.OrLogic:
                    return "|";
                case TokenType.OpenParenToken:
                    return "(";
                case TokenType.CloseParenToken:
                    return ")";
                case TokenType.Negation:
                    return "!";
                case TokenType.DesigualComparador:
                    return "!=";
                case TokenType.Concatenador:
                    return "@";
                default:
                    return null;
            }
        }

        //Metodo que devuelve la prioridad de los operadores binarios
        //Llamado desde el parser para evaluar las expresiones binarias
        public static int PrioridadOperadorBinario(this TokenType tipo)
        {
            switch (tipo)
            {
                case TokenType.Potencia:
                    return 6;
                case TokenType.MultToken:
                case TokenType.DivToken:
                case TokenType.ModuloResto:
                    return 5;
                case TokenType.PlusToken:
                case TokenType.MinusToken:
                case TokenType.Concatenador:
                    return 4;
                case TokenType.IgualComparador:
                case TokenType.DesigualComparador:
                case TokenType.MayorQue:
                case TokenType.MenorQue:
                case TokenType.MayorOIgual:
                case TokenType.MenorOIgual:
                    return 3;
                case TokenType.AndLogic:
                    return 2;
                case TokenType.OrLogic:
                    return 1;

                default:
                    return 0;
            }
        }
        //Metodo que devuelve la prioridad de los operadores unarios
        //Llamado desde el parser para evaluar las expresiones unarias
        public static int PrioridadOperadorUnario(this TokenType tipo)
        {
            switch (tipo)
            {
                case TokenType.PlusToken:
                case TokenType.MinusToken:
                case TokenType.Negation:
                    return 7;
                default:
                    return 0;
            }
        }


        //Metodo que identifica el tipo de palabra clave 
        internal static TokenType KeywordType(string text)
        {
            switch (text)
            {
                case "PI":
                    return TokenType.PI;
                case "E":
                    return TokenType.Euler;
                case "true":
                    return TokenType.TrueKeyWord;
                case "false":
                    return TokenType.FalseKeyWord;
                case "sen":
                    return TokenType.SenKeyWord;
                case "cos":
                    return TokenType.CosKeyWord;
                case "log":
                    return TokenType.LogKeyWord;
                case "sqrt":
                    return TokenType.SqrtKeyWord;
                case "let":
                    return TokenType.LetKeyWord;
                case "in":
                    return TokenType.InKeyWord;
                case "if":
                    return TokenType.IfKeyWord;
                case "else":
                    return TokenType.ElseKeyWord;
                case "print":
                    return TokenType.PrintKeyWord;
                case "function":
                    return TokenType.FuncionKeyWord;
                default:
                    return TokenType.IdentificadorToken;

            }
        }
    }
}

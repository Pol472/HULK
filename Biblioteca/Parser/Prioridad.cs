using Hulk;
using Hulk.Biblioteca.Tree;
namespace Hulk.Biblioteca.Parser
{
    internal static class Prioridad
    {
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

        internal static TokenType KeywordType(string text)
        {
            switch (text)
            {
                case "true":
                    return TokenType.TrueKeyWord;
                case "false":
                    return TokenType.FalseKeyWord;
                case "let":
                    return TokenType.LetKeyWord;
                case "in":
                    return TokenType.InKeyWord;
                case "if":
                    return TokenType.IfKeyWord;
                case "else":
                    return TokenType.ElseKeyWord;
                case "function":
                    return TokenType.FuncionKeyWord;
                default:
                    return TokenType.IdentificadorToken;

            }
        }
    }
}

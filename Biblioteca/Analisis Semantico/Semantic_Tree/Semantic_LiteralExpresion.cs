using Hulk.Biblioteca.Tree;

namespace Hulk.Biblioteca.Semantic
{
    internal sealed class Semantic_LiteralExpresion : Semantic_Expresion
    {
        public Semantic_LiteralExpresion(object value)
        {
            Value = value;

        }

        public object Value { get; private set; }


        public override SemanticType Kind => SemanticType.LiteralExpresion;

        public override TipoHulk TipoHulk => GetTipoHulk();
        private TipoHulk GetTipoHulk()
        {
            switch(Value)
            {
                case System.Boolean:
                return TipoHulk.Boolean;
                case System.Double:
                return TipoHulk.Number;
                case System.String:
                return TipoHulk.String;
                default:
                return TipoHulk.Identificador;
            }
        }
    }



}
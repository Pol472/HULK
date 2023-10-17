namespace Hulk.Biblioteca.Semantic
{
    internal class Semantic_SenExpresion : Semantic_Expresion
    {
        public Semantic_SenExpresion(Semantic_Expresion argumento)
        {
            Argumento = argumento;
        }

        public override TipoHulk TipoHulk => TipoHulk.Number;

        public override SemanticType Kind => SemanticType.SenExpresion;

        public Semantic_Expresion Argumento { get; }
    }
    internal class Semantic_CosExpresion : Semantic_Expresion
    {
        public Semantic_CosExpresion(Semantic_Expresion argumento)
        {
            Argumento = argumento;
        }

        public override TipoHulk TipoHulk => TipoHulk.Number;

        public override SemanticType Kind => SemanticType.CosExpresion;

        public Semantic_Expresion Argumento { get; }
    }
    internal class Semantic_LogExpresion : Semantic_Expresion
    {
        public Semantic_LogExpresion(Semantic_Expresion baseLog , Semantic_Expresion argumento)
        {
            BaseLog = baseLog;
            Argumento = argumento;
        }

        public override TipoHulk TipoHulk => TipoHulk.Number;

        public override SemanticType Kind => SemanticType.LogExpresion;

        public Semantic_Expresion BaseLog { get; }
        public Semantic_Expresion Argumento { get; }
    }
    internal class Semantic_SqrtExpresion : Semantic_Expresion
    {
        public Semantic_SqrtExpresion(Semantic_Expresion argumento)
        {
            Argumento = argumento;
        }

        public override TipoHulk TipoHulk => TipoHulk.Number;

        public override SemanticType Kind => SemanticType.SqrtExpresion;

        public Semantic_Expresion Argumento { get; }
    }



}
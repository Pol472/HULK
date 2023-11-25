namespace Hulk.Biblioteca.Semantic
{
    //Objetos semanticos de las funciones aritmeticas integradas en HULK



    //Seno 
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

    //Coseno
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

    //Logaritmo 
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

    //Raiz Cuadrada
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
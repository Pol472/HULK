namespace Hulk.Biblioteca.Semantic
{
    internal class Semantic_FuncionCall : Semantic_Declaracion
    {
        public Semantic_FuncionCall(FuncionSymbol simbolo,Semantic_Expresion expresion)
        {
            Simbolo = simbolo;
            Expresion = expresion;
        }

        public override TipoHulk TipoHulk => TipoHulk.FuncionCall;
        public override SemanticType Kind => SemanticType.FuncionCall;

        public FuncionSymbol Simbolo { get; }
        public Semantic_Expresion Expresion { get; }
  
    }



}
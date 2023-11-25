namespace Hulk.Biblioteca.Semantic
{
    //Objeto semantico llamada a funcion
    internal class Semantic_FuncionCall : Semantic_Declaracion
    {
        public Semantic_FuncionCall(FuncionSymbol simbolo,List<Semantic_Expresion> argumentos)
        {
            Simbolo = simbolo;
            Argumentos = argumentos;
        }

        public override TipoHulk TipoHulk => Argumentos[0].TipoHulk;
        public override SemanticType Kind => SemanticType.FuncionCall;

        public FuncionSymbol Simbolo { get; }
        public List<Semantic_Expresion> Argumentos { get; }
    }



}
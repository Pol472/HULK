namespace Hulk.Biblioteca.Semantic
{
    internal abstract class Semantic_Declaracion : Semantic_Expresion
    {
       public override SemanticType Kind => SemanticType.Declaracion;

    }
    


    internal sealed class Semantic_DeclaracionExpresion : Semantic_Declaracion
    {
        public Semantic_DeclaracionExpresion(Semantic_Expresion expresion)
        {
            Expresion = expresion;
        }

        public override SemanticType Kind => SemanticType.DeclaracionExpresion;

        public Semantic_Expresion Expresion { get; }

        public override TipoHulk TipoHulk => Expresion.TipoHulk;

        
    }
    internal sealed class Semantic_VariableDeclaracion : Semantic_Declaracion
    {
        public Semantic_VariableDeclaracion(List<Semantic_AsignacionVariable> lista, Semantic_Expresion contexto)
        {
            Lista = lista;
            Contexto = contexto;
        }

        public override SemanticType Kind => SemanticType.VariableDeclaracion;

        public List<Semantic_AsignacionVariable> Lista { get; }

        public Semantic_Expresion Contexto { get; }

        public override TipoHulk TipoHulk => Contexto.TipoHulk;

        
    }
    internal sealed class Semantic_IF_Declaracion : Semantic_Declaracion
    {
        public Semantic_IF_Declaracion(Semantic_Expresion condicion, Semantic_Expresion cuerpoIf, Semantic_Expresion? cuerpoElse)
        {
            Condicion = condicion;
            CuerpoIf = cuerpoIf;
            CuerpoElse = cuerpoElse;
        }

        public override SemanticType Kind => SemanticType.IfDeclaracion;

        public Semantic_Expresion Condicion { get; }
        public Semantic_Expresion CuerpoIf { get; }
        public Semantic_Expresion? CuerpoElse { get; }

        public override TipoHulk TipoHulk => CuerpoIf.TipoHulk;

        
    }
    


}
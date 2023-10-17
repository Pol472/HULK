namespace Hulk.Biblioteca.Semantic
{
   internal abstract class Semantic_Nodo
    {
        
        public abstract SemanticType Kind { get; }
    }
     internal abstract class Semantic_Expresion : Semantic_Nodo
    {
        public abstract TipoHulk TipoHulk {get;}
        
    }

}
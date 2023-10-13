namespace Hulk.Biblioteca.Semantic
{
    internal  class Semantic_VariableExpresion : Semantic_Expresion
    {
        public Semantic_VariableExpresion(VariableSymbol variable)
        {
            Variable = variable;
        }
        public override SemanticType Kind => SemanticType.VariableExpresion;
       
        public VariableSymbol Variable { get; }

        public override TipoHulk TipoHulk => Variable.TipoHulk ;
    }
    
    internal sealed class Semantic_AsignacionVariable : Semantic_Expresion
    {
        public Semantic_AsignacionVariable(VariableSymbol variable, Semantic_Expresion valor)
        {
            Variable = variable;

            Valor = valor;
        }

        public VariableSymbol Variable { get; }

        public Semantic_Expresion Valor { get; }
        

        public override SemanticType Kind => SemanticType.AsignacionVariable;

        public override TipoHulk TipoHulk => Valor.TipoHulk;
    }

}
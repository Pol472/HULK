namespace Hulk.Biblioteca.Semantic
{

    //Objeto variable expresion, que contiene al simbolo de la variable para ser llamada en el evaluador
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

    //Objeto asignacion de variable 
    
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
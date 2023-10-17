namespace Hulk.Biblioteca.Semantic
{
    internal sealed class Semantic_Ambito
    {
        private Dictionary<string, VariableSymbol> Variables = new Dictionary<string, VariableSymbol>();

        public Semantic_Ambito Padre { get; }

        public Semantic_Ambito(Semantic_Ambito padre)
        {
            Padre = padre;
        }
        public bool Try_AsignarVariable(string name, out VariableSymbol variable)
        {

            
            if (Variables.TryGetValue(name, out variable))
                return true;

            
            if (Padre == null)
                return false;
            return Padre.Try_AsignarVariable(name, out variable);
            
        }

        public bool Try_DeclararVariable(VariableSymbol variable)
        {
            

            if (Variables.ContainsKey(variable.Nombre))
                return false;

            Variables.Add(variable.Nombre, variable);
            return true;

        }
        public List<VariableSymbol> GetDeclaredVariables()
        {
          return Variables.Values.ToList();
        }
        public void AddVariableFuncion(VariableSymbol variable)
        {
            Variables.Add(variable.Nombre,variable);
        }
        
    }

}
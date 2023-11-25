namespace Hulk.Biblioteca.Semantic
{
    //Clase para manejar el contexto de existencia de las variables
    internal sealed class Semantic_Ambito
    {
        private Dictionary<string, VariableSymbol> Variables = new Dictionary<string, VariableSymbol>();

        public Semantic_Ambito Padre { get; }

        public Semantic_Ambito(Semantic_Ambito padre)
        {
            Padre = padre;
        }
        //Chequea la existencia de la variable para ver si puede ser reasignada(de acuerdo a tipos compatibles)
        public bool Try_AsignarVariable(string name, out VariableSymbol variable)
        {

            
            if (Variables.TryGetValue(name, out variable))
                return true;

            
            if (Padre == null)
                return false;
            return Padre.Try_AsignarVariable(name, out variable);
            
        }

        //Chequea si existe una variable con el mismo simbolo y en caso de no existir declararla

        public bool Try_DeclararVariable(VariableSymbol variable)
        {
            

            if (Variables.ContainsKey(variable.Nombre))
                return false;

            Variables.Add(variable.Nombre, variable);
            return true;

        }

        //Devuelve las variables declaradas
        public List<VariableSymbol> GetDeclaredVariables()
        {
          return Variables.Values.ToList();
        }
       
    }

}
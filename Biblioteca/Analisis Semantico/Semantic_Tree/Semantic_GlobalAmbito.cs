using Hulk.Biblioteca.Errores;
using Hulk.Biblioteca.Tree;

namespace Hulk.Biblioteca.Semantic
{
    //Objeto final devuelto con los errores del parsing semantico, la expresion con chequeo de tipos
    //y el contexto de las variables
    internal sealed class Semantic_GlobalAmbito
    {
        public Semantic_GlobalAmbito(Semantic_GlobalAmbito padre, List<Error> errores,
        List<VariableSymbol> variables, Semantic_Expresion declaracion)
        {
            Padre = padre;
            Errores = errores;
            Variables = variables;
            Declaracion = declaracion;
            
        }

        public Semantic_GlobalAmbito Padre { get; }
        public List<Error> Errores { get; }
        public List<VariableSymbol> Variables { get; }
        public Semantic_Expresion Declaracion{ get; }
        
    }

}
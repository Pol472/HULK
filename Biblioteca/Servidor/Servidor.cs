using System.Linq;
using Hulk.Biblioteca.Semantic;
using Hulk.Biblioteca.Errores;
using Hulk.Biblioteca.Tree;

namespace Hulk.Biblioteca.Servidor
{
    public sealed class Servidor
    {
        private Semantic_GlobalAmbito _ambitoGlobal;

        public Servidor Padre { get; }
        public SyntaxTree Arbol { get; }
        

        public Servidor(Servidor padre, SyntaxTree arbol)
        {
            Padre = padre;
            Arbol = arbol;
            
        }

       

        internal Semantic_GlobalAmbito AmbitoGlobal
        {
            get
            {
                if (_ambitoGlobal == null)
                {
                    var ambitoGlobal = Semantic_Parser.ConectaAmbitoGlobal(Padre?.AmbitoGlobal,Arbol);
                    Interlocked.CompareExchange(ref _ambitoGlobal, ambitoGlobal, null);
                }
                return _ambitoGlobal;
            }
        }

        

        public Retorno Sirve(Dictionary<VariableSymbol, object> variables)
        {


            IEnumerable<Error> errores = Arbol.Errores.Concat(AmbitoGlobal.Errores).ToArray();
            if (errores.Any())
            {
                return new Retorno(errores, null);
            }
            var semantic_Funcion = Semantic_Parser.Conecta_CuerpoFuncion(Hulk.Program.funciones);

            Evaluator resultado = new Evaluator(AmbitoGlobal.Declaracion, variables,semantic_Funcion);
            object valorResultado = resultado.Evalua();
            return new Retorno(Array.Empty<Error>(), valorResultado);

        }
        



    }

}
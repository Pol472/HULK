using System.Linq;
using Hulk.Biblioteca.Semantic;
using Hulk.Biblioteca.Errores;

namespace Hulk.Biblioteca.Servidor
{
    public sealed class Servidor
    {
        private Semantic_GlobalAmbito _ambitoGlobal;

        public Servidor Padre { get; }
        public SyntaxTree Arbol { get; }
        public Servidor(SyntaxTree arbol) : this(null, arbol)
        {
        }
        private Servidor(Servidor padre, SyntaxTree arbol)
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
            Evaluator resultado = new Evaluator(AmbitoGlobal.Declaracion, variables);
            object valorResultado = resultado.Evalua();
            return new Retorno(Array.Empty<Error>(), valorResultado);

        }
        public Servidor ContinuaCon(SyntaxTree arbol)
        {
          return new Servidor(this,arbol);
        }



    }

}
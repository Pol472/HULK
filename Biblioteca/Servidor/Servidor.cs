using System.Linq;
using Hulk.Biblioteca.Semantic;
using Hulk.Biblioteca.Errores;
using Hulk.Biblioteca.Tree;

namespace Hulk.Biblioteca.Servidor
{
    //Esta clase maneja a los objetos que se encargan de dirigir el proceso de parsing semantico
    //y llamar a la evaluacion de la expresion ya chequeada
    public sealed class Servidor
    {
        //Propiedades para garantizar el contexto de las variables
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

        
        //Este metodo adiciona los errores que pudieron existir en el  proceso de parsing semantico 
        // a los obtenidos en el proceso anterior pues no termina la ejecucion del programa
        public Retorno Sirve(Dictionary<VariableSymbol, object> variables)
        {


            IEnumerable<Error> errores = Arbol.Errores.Concat(AmbitoGlobal.Errores).ToArray();
            if (errores.Any())
            {
                return new Retorno(errores, null);
            }
            // Parsing semantico realizado a los cuerpos de funciones existentes
            var semantic_Funcion = Semantic_Parser.Conecta_CuerpoFuncion(Hulk.Program.funciones);

            //Construccion del objeto evluador en caso de no existir errores 

            Evaluator resultado = new Evaluator(AmbitoGlobal.Declaracion, variables,semantic_Funcion);
            //Llamada a evaluar 
            object valorResultado = resultado.Evalua();
            //Se devuelve a Main un objeto de tipo de retorno que tiene como propiedad el object del resultado
            return new Retorno(Array.Empty<Error>(), valorResultado);

        }
        



    }

}
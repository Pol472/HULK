using System.Collections.Generic;
using System.Reflection;

namespace Hulk.Biblioteca.Tree
{

    // Esta es la clase abstracta del espacio de nombres Tree , cuyo unico metodo abstracto es el tipo de expresion
    // que implementan todos sus hijos
    public abstract class Nodo
    {

        public abstract TokenType Type { get; }
    }



}

using Hulk.Biblioteca.Errores;

namespace Hulk.Biblioteca.Servidor
{
    //Objeto de tipo retorno
    public sealed class Retorno
    {
        public Retorno(IEnumerable<Error> errores, object value)
        {
            Errores = errores;
            Value = value;
        }

        public IEnumerable<Error> Errores { get; }
        //Valor que se imprime en consola
        public object Value { get; }
    }

}
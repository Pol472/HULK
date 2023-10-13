using Hulk.Biblioteca.Errores;

namespace Hulk.Biblioteca.Servidor
{
    public sealed class Retorno
    {
        public Retorno(IEnumerable<Error> errores, object value)
        {
            Errores = errores;
            Value = value;
        }

        public IEnumerable<Error> Errores { get; }
        public object Value { get; }
    }

}
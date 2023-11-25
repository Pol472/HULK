namespace Hulk.Biblioteca.Errores
{
    //Clase Error , para crear los objetos de tipo error en Hulk
    public sealed class Error
    {
        public TipoError Tipo { get; }
        public string Mensaje { get; }

        public Error(TipoError tipo, string mensaje)
        {
            Tipo = tipo;
              switch(Tipo)
            {
                case TipoError.LexicalError:
                 Mensaje = "! LEXICAL ERROR: "+ mensaje;
                 break;
                 case TipoError.SemanticError:
                 Mensaje = "! SEMANTIC ERROR: "+ mensaje;
                 break;
                 case TipoError.SintacticError:
                 Mensaje = "! SYNTAX ERROR: "+ mensaje;
                 break;
                 case TipoError.FuncionError:
                 Mensaje = "! FUNCTION ERROR: "+ mensaje;
                 break;
                 default:
                 Mensaje = mensaje;
                 break;
            }
            
        }
       
    }
    //Tipos de errores
    public enum TipoError
    {
        SintacticError,
        LexicalError,
        SemanticError,
        FuncionError
    }
    

}
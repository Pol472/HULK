using Hulk.Biblioteca.Semantic;
using Hulk.Biblioteca.Tree;
namespace Hulk.Biblioteca
{

    internal sealed class Evaluator
    {
        //Regulador de las llamadas recursivas
        private int Pila;
        //Limite de las llamadas recursivas
        private readonly int StackOverflow = 10000;
        //Variable que devuelve un valor para las expresiones "void"
        private object LastValue;
        //Expresion a evaluar
        private readonly Semantic_Expresion Declaracion;
        //Diccionario que guarda las variables
        public Dictionary<VariableSymbol, object> Variables { get; }
        //Pila con los diccionarios para tomar las variables de las llamadas recursivas
        private readonly Stack<Dictionary<string, object>> Locales = new Stack<Dictionary<string, object>>();
        //Diccionario con los cuerpos semanticos de las funciones
        public Dictionary<string, Semantic_Expresion> Cuerpos_Funcion { get; }

        public Evaluator(Semantic_Expresion declaracion, Dictionary<VariableSymbol, object> variables, Dictionary<string, Semantic_Expresion> cuerpos_Funcion)
        {
            Declaracion = declaracion;
            Variables = variables;
            Cuerpos_Funcion = cuerpos_Funcion;
        }
        
        
        //Este es el metodo principal que es llamado desde la clase servidor
        public object Evalua()
        {
            return EvaluaExpresion(Declaracion);
        }
        //Este metodo evalua las expresiones compuestas
        private void EvaluaDeclaracion(Semantic_Expresion declaracion)
        {
            
            switch (declaracion.Kind)
            {
                case SemanticType.FuncionDeclaracion:
                    EvaluaFuncionDeclaracion((Semantic_FuncionDeclaracion)declaracion);
                    break;
                case SemanticType.DeclaracionExpresion:
                    EvaluaDeclaracionExpresion((Semantic_DeclaracionExpresion)declaracion);
                    break;
                case SemanticType.VariableDeclaracion:
                    EvaluaVariableDeclaracion((Semantic_VariableDeclaracion)declaracion);
                    break;
                case SemanticType.IfDeclaracion:
                    EvaluaIfDeclaracion((Semantic_IF_Declaracion)declaracion);
                    break;
                case SemanticType.PrintExpresion:
                    EvaluaPrintExpresion((Semantic_PrintExpresion)declaracion);
                    break;
                default:
                    throw new Exception($"Expresion,<{declaracion.Kind}> inesperada");
            }
        }
    

        private void EvaluaFuncionDeclaracion(Semantic_FuncionDeclaracion declaracion)
        {
            LastValue = $"Function '{declaracion.Nombre.Name}' has been adeed.";
        }
        //Metodo para evaluar la llamada a la funcion print

        private void EvaluaPrintExpresion(Semantic_PrintExpresion declaracion)
        {
            if (declaracion.Expresion != null)
                EvaluaExpresion(declaracion.Expresion);
        }
        //Metodo para evaluar la expresion if-else

        private void EvaluaIfDeclaracion(Semantic_IF_Declaracion declaracion)
        {
            bool condicion = (bool)EvaluaExpresion(declaracion.Condicion);
            if (condicion)
                EvaluaExpresion(declaracion.CuerpoIf);
            else
            {
                if (declaracion.CuerpoElse != null)
                    EvaluaExpresion(declaracion.CuerpoElse);
            }
        }
        //Metodo para evaluar la declaracion de variables

        private void EvaluaVariableDeclaracion(Semantic_VariableDeclaracion declaracion)
        {
            foreach (var a in declaracion.Lista)
            {
                EvaluaExpresion(a.Valor);

                Variables[a.Variable] = LastValue;
            }
            EvaluaExpresion(declaracion.Contexto);
        }


        //Metodo para evaluar las expresiones de tipo declaracion
        //Forma general
        private void EvaluaDeclaracionExpresion(Semantic_DeclaracionExpresion declaracion)
        {
            LastValue = EvaluaExpresion(declaracion.Expresion);
        }
        //Metodo que evalua las expresiones primarias(las que como tal tienen un retorno)

        private object EvaluaExpresion(Semantic_Expresion raiz)
        {
            //Aqui se actualiza el numero de llamadas recursivas
            Pila++;
            if(Pila > StackOverflow)
            throw new Exception($"! FUNCTION ERROR: Stack overflow.");


            switch (raiz)
            {
                case Semantic_FuncionCall l:
                    return EvaluaFuncionCall(l);
                case Semantic_SqrtExpresion f:
                    return EvaluaRaiz(f);
                case Semantic_SenExpresion w:
                    return EvaluaSen(w);
                case Semantic_CosExpresion k:
                    return EvaluaCos(k);
                case Semantic_LogExpresion j:
                     return EvaluaLog(j);
                case Semantic_LiteralExpresion a:
                    return a.Value;
                case Semantic_UnaryExpresion b:
                    return EvaluaExpresionUnaria(b);
                case Semantic_VariableExpresion w:
                    {
                        object valor = null;
                        try{
                        valor = Variables[w.Variable];
                        }
                        catch{
                         var locals = Locales.Peek();
                        valor = locals[w.Variable.Nombre];
                        }
                        return valor;
                    }
                case Semantic_AsignacionVariable m:
                    {
                        var valor = EvaluaExpresion(m.Valor);
                        Variables[m.Variable] = valor;
                        return valor;
                    }
                case Semantic_BinaryExpresion c:
                    return EvaluaExpresionBinaria(c);
                case Semantic_Declaracion j:
                    {
                        EvaluaDeclaracion(j);
                        return LastValue;
                    }
                default:
                    throw new Exception($"Expresion,<{raiz.Kind}> inesperada");
            }
        }
        //Metodo que valua a la funcion raiz cuadrada

        private object EvaluaRaiz(Semantic_SqrtExpresion f)
        {
            var argumento = EvaluaExpresion(f.Argumento);
            return Math.Sqrt((double)argumento);
        }
        //Metodo que evalua las llamadas a funciones no propias del lenguaje

        private object EvaluaFuncionCall(Semantic_FuncionCall l)
        {
            //Aqui se crea un diccionario con el objetivo de agregar las variables propias de las llamadas a funciones
            var locals = new Dictionary<string, object>();
            for ( int i = 0 ;i<l.Argumentos.Count;i++)
            {
                var valor = EvaluaExpresion(l.Argumentos[i]);
                var nombre = Hulk.Program.funciones[l.Simbolo.Name].Parametros[i].Text;
                //Se agrega al diccionario anterior la variable de turno con su valor
                locals.Add(nombre,valor);
            }
            //Se agrega a la pila el diccionario 
            Locales.Push(locals);
            //Se evalua la expresion resultante del cuerpo de la funcion y las variables ya agregadas
            var result = EvaluaExpresion(Cuerpos_Funcion[l.Simbolo.Name]);
            //Se elimina de la pila el diccionario en el tope pues ya se termino la llamada actual
            Locales.Pop();
            //Se devuelve el resultado
            return result;
            
        }

        
        //Metodo para evaluar funcion logaritmo
        private object EvaluaLog(Semantic_LogExpresion j)
        {

            var baseLog = (double) EvaluaExpresion(j.BaseLog);
            var Argumento = (double) EvaluaExpresion(j.Argumento);
            
             return Math.Log(Argumento,baseLog);

        }

        //Metodo para evaluar funcion seno

        private object EvaluaSen(Semantic_SenExpresion w)
        {
            var argumento = EvaluaExpresion(w.Argumento);
            return Math.Sin((double)argumento);
        }

        //Metodo para evaluar funcion coseno
        private object EvaluaCos(Semantic_CosExpresion w)
        {
            var argumento = EvaluaExpresion(w.Argumento);
            return Math.Cos((double)argumento);
        }


         //Metodo para evaluar expresiones modificadas por un operador unario
        private object EvaluaExpresionUnaria(Semantic_UnaryExpresion b)
        {
            var operando = EvaluaExpresion(b.Operando);
            switch (b.Operador.TipoOperador)
            {
                //Opuesto
                case Semantic_UnaryOperadorType.Negacion:
                    return -(double)operando;
                //Identidad
                case Semantic_UnaryOperadorType.Identidad:
                    return (double)operando;
                //Negacion Logica
                case Semantic_UnaryOperadorType.NegacionLogica:
                    return !(bool)operando;
                default:
                    throw new Exception($"Operador Unario: <{b.Operador.TipoOperador}>, no esperado");
            }
        }


        //Metodo para evaluar expresiones modificadas por un operador binario
        private object EvaluaExpresionBinaria(Semantic_BinaryExpresion c)
        {
            var left = EvaluaExpresion(c.Left);
            var rigth = EvaluaExpresion(c.Rigth);

            switch (c.Operador.TipoOperador)
            {
                //Concatenador string
                case Semantic_BinaryOperadorType.Concatenacion:
                    return left.ToString() + rigth.ToString();
                //Operadores aritmeticos

                //Suma
                case Semantic_BinaryOperadorType.Addition:
                    return (double)left + (double)rigth;
                //Resta
                case Semantic_BinaryOperadorType.Diference:
                    return (double)left - (double)rigth;
                //Producto
                case Semantic_BinaryOperadorType.Multiplicacion:
                    return (double)left * (double)rigth;
                //Division
                case Semantic_BinaryOperadorType.Division:
                    return (double)left / (double)rigth;
                //Potencia
                case Semantic_BinaryOperadorType.Pow:
                    return Math.Pow((double)left, (double)rigth);
                //Resto de la division (modulo)
                case Semantic_BinaryOperadorType.Modulo:
                    return (double)left % (double)rigth;

                //Operadores logicos
                //Conjuncion
                case Semantic_BinaryOperadorType.AndLogica:
                    return (bool)left && (bool)rigth;
                //Disyuncion
                case Semantic_BinaryOperadorType.OrLogica:
                    return (bool)left || (bool)rigth;


                //Comparadores
                //Igualdad ==
                case Semantic_BinaryOperadorType.EsIgual:
                    return Equals(left, rigth);
                //Desigual !=
                case Semantic_BinaryOperadorType.Desigual:
                    return !Equals(left, rigth);
                //Mayor que > 
                case Semantic_BinaryOperadorType.MayorComparar:
                    return (double)left > (double)rigth;
                //Menor que <
                case Semantic_BinaryOperadorType.MenorComparar:
                    return (double)left < (double)rigth;
                //Mayor o igual >=
                case Semantic_BinaryOperadorType.MayorOIgualComparar:
                    return (double)left >= (double)rigth;
                //Menor o igual <=
                case Semantic_BinaryOperadorType.MenorOIgualComparar:
                    return (double)left <= (double)rigth;
                default:
                    throw new Exception($"Operador binario: <{c.TipoOperador}>, no esperado");
            }
        }
    }
}
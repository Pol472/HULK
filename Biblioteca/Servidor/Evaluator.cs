using Hulk.Biblioteca.Semantic;
using Hulk.Biblioteca.Tree;
namespace Hulk.Biblioteca
{

    internal sealed class Evaluator
    {
        private object LastValue;
        private readonly Semantic_Expresion Declaracion;
        public Evaluator(Semantic_Expresion declaracion, Dictionary<VariableSymbol, object> variables)
        {
            Declaracion = declaracion;
            Variables = variables;
          
        }
        public Dictionary<VariableSymbol, object> Variables { get; }
        

        public object Evalua()
        {
            return EvaluaExpresion(Declaracion);
        }
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

        private void EvaluaPrintExpresion(Semantic_PrintExpresion declaracion)
        {
            if (declaracion.Expresion != null)
                EvaluaExpresion(declaracion.Expresion);
        }

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

        private void EvaluaVariableDeclaracion(Semantic_VariableDeclaracion declaracion)
        {
            foreach (var a in declaracion.Lista)
            {
                EvaluaExpresion(a.Valor);

                Variables[a.Variable] = LastValue;
            }
            EvaluaExpresion(declaracion.Contexto);
        }



        private void EvaluaDeclaracionExpresion(Semantic_DeclaracionExpresion declaracion)
        {
            LastValue = EvaluaExpresion(declaracion.Expresion);
        }

        private object EvaluaExpresion(Semantic_Expresion raiz)
        {
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
                        var valor = Variables[w.Variable];
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

        private object EvaluaRaiz(Semantic_SqrtExpresion f)
        {
            var argumento = EvaluaExpresion(f.Argumento);
            return Math.Sqrt((double)argumento);
        }

        private object EvaluaFuncionCall(Semantic_FuncionCall l)
        {
           return EvaluaExpresion(l.Expresion); 
        }

        private object EvaluaLog(Semantic_LogExpresion j)
        {
            var baseLog = (double) EvaluaExpresion(j.BaseLog);
            var Argumento = (double) EvaluaExpresion(j.Argumento);
            
             return Math.Log(Argumento,baseLog);

        }

        private object EvaluaSen(Semantic_SenExpresion w)
        {
            var argumento = EvaluaExpresion(w.Argumento);
            return Math.Sin((double)argumento);
        }
        private object EvaluaCos(Semantic_CosExpresion w)
        {
            var argumento = EvaluaExpresion(w.Argumento);
            return Math.Cos((double)argumento);
        }

        private object EvaluaExpresionUnaria(Semantic_UnaryExpresion b)
        {
            var operando = EvaluaExpresion(b.Operando);
            switch (b.Operador.TipoOperador)
            {
                case Semantic_UnaryOperadorType.Negacion:
                    return -(double)operando;
                case Semantic_UnaryOperadorType.Identidad:
                    return (double)operando;
                case Semantic_UnaryOperadorType.NegacionLogica:
                    return !(bool)operando;
                default:
                    throw new Exception($"Operador Unario: <{b.Operador.TipoOperador}>, no esperado");
            }
        }

        private object EvaluaExpresionBinaria(Semantic_BinaryExpresion c)
        {
            var left = EvaluaExpresion(c.Left);
            var rigth = EvaluaExpresion(c.Rigth);

            switch (c.Operador.TipoOperador)
            {
                case Semantic_BinaryOperadorType.Concatenacion:
                    return left.ToString() + rigth.ToString();
                //Operadores aritmeticos
                case Semantic_BinaryOperadorType.Addition:
                    return (double)left + (double)rigth;
                case Semantic_BinaryOperadorType.Diference:
                    return (double)left - (double)rigth;
                case Semantic_BinaryOperadorType.Multiplicacion:
                    return (double)left * (double)rigth;
                case Semantic_BinaryOperadorType.Division:
                    return (double)left / (double)rigth;
                case Semantic_BinaryOperadorType.Pow:
                    return Math.Pow((double)left, (double)rigth);
                case Semantic_BinaryOperadorType.Modulo:
                    return (double)left % (double)rigth;
                //Operadores logicos
                case Semantic_BinaryOperadorType.AndLogica:
                    return (bool)left && (bool)rigth;
                case Semantic_BinaryOperadorType.OrLogica:
                    return (bool)left || (bool)rigth;
                //Comparadores
                case Semantic_BinaryOperadorType.EsIgual:
                    return Equals(left, rigth);
                case Semantic_BinaryOperadorType.Desigual:
                    return !Equals(left, rigth);
                case Semantic_BinaryOperadorType.MayorComparar:
                    return (double)left > (double)rigth;
                case Semantic_BinaryOperadorType.MenorComparar:
                    return (double)left < (double)rigth;
                case Semantic_BinaryOperadorType.MayorOIgualComparar:
                    return (double)left >= (double)rigth;
                case Semantic_BinaryOperadorType.MenorOIgualComparar:
                    return (double)left <= (double)rigth;
                default:
                    throw new Exception($"Operador binario: <{c.TipoOperador}>, no esperado");
            }
        }
    }
}
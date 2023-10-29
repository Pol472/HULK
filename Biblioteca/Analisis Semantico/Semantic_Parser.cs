using Hulk.Biblioteca.Errores;
using Hulk.Biblioteca.Tree;

namespace Hulk.Biblioteca.Semantic
{

    internal class Semantic_Parser
    {
        private readonly List<Error> errores = new List<Error>();
        
        private Semantic_Ambito Ambito;
        public Semantic_Parser(Semantic_Ambito padre)
        {
            Ambito = new Semantic_Ambito(padre);
            
        }

        public IEnumerable<Error> Errores => errores;


        public static Semantic_GlobalAmbito ConectaAmbitoGlobal(Semantic_GlobalAmbito previous, SyntaxTree arbol)
        {

            var parentAmbito = CreaAmbitoPadre(previous);
            Semantic_Parser semantic_object = new Semantic_Parser(parentAmbito);
            var expresion = semantic_object.ConectaExpresion(arbol.Declaracion);
            List<VariableSymbol> variables = semantic_object.Ambito.GetDeclaredVariables();
            List<Error> errores = semantic_object.Errores.ToList();
            if (previous != null && semantic_object == null)
            {
                errores = previous.Errores;
            }

            return new Semantic_GlobalAmbito(previous, errores, variables, expresion);
        }
        private static Semantic_Ambito CreaAmbitoPadre(Semantic_GlobalAmbito previous)
        {
            var stack = new Stack<Semantic_GlobalAmbito>();
            while (previous != null)
            {
                stack.Push(previous);
                previous = previous.Padre;
            }
            Semantic_Ambito parent = null;
            while (stack.Count > 0)
            {
                previous = stack.Pop();
                var ambito = new Semantic_Ambito(parent);
                foreach (var a in previous.Variables)
                    ambito.Try_DeclararVariable(a);

                parent = ambito;
            }
            return parent;
        }



        private Semantic_Expresion ConectaIfDeclaracion(IFDeclaracionExpresion declaracion)
        {
            var condicion = ConectaExpresionCondicion(declaracion.Condicion, TipoHulk.Boolean);
            var cuerpoIf = ConectaExpresion(declaracion.CuerpoIf);
            var cuerpoElse = (declaracion.CuerpoElse == null) ? null : ConectaExpresion(declaracion.CuerpoElse.CuerpoElse);

            if (cuerpoElse == null)
                errores.Add(new Error(TipoError.SemanticError, $"Expected else in if-else expresion"));

            return new Semantic_IF_Declaracion(condicion, cuerpoIf, cuerpoElse);
        }

        private Semantic_Expresion ConectaExpresionCondicion(Expresion expresion, TipoHulk type)
        {
            var result = ConectaExpresion(expresion);
            if (result.TipoHulk != type)
                errores.Add(new Error(TipoError.SemanticError, $"The conditional expresion in the if statement is not boolean."));
            return result;
        }

        private Semantic_Expresion ConectaVariableDeclaracion(VariableDeclaracionExpresion declaracion)
        {
            List<Semantic_AsignacionVariable> lista = new List<Semantic_AsignacionVariable>();
            foreach (var alfa in declaracion.Declaraciones)
            {
                string name = alfa.Identificador.Text;
                var expresion = ConectaExpresion(alfa.Expresion);
                var variable = new VariableSymbol(name, expresion.GetType(), expresion.TipoHulk);
                lista.Add(new Semantic_AsignacionVariable(variable, expresion));
                Ambito = new Semantic_Ambito(Ambito);
                if (!Ambito.Try_DeclararVariable(variable))
                {
                    errores.Add(new Error(TipoError.SemanticError, $"Variable '{name}' has already been declared"));
                }

            }
            var contexto = ConectaExpresion(declaracion.Contexto);
            if (contexto == null)
                errores.Add(new Error(TipoError.SemanticError, $"Expected a context for variable let-in expresion."));

            Ambito = Ambito.Padre;

            return new Semantic_VariableDeclaracion(lista, contexto);
        }

        private Semantic_Expresion ConectaDeclaracionExpresion(DeclaracionExpresion declaracion)
        {
            var expresion = ConectaExpresion(declaracion.Expresion);
            return new Semantic_DeclaracionExpresion(expresion);

        }


        private Semantic_Expresion ConectaParentesisExpresion(ParentesisExpresion expresion)
        {
            return ConectaExpresion(expresion.Expresion);
        }

        private Semantic_Expresion ConectaExpresion(Expresion expresion)
        {
            switch (expresion.Type)
            {
                case TokenType.ParentesisExpresion:
                    return ConectaParentesisExpresion((ParentesisExpresion)expresion);
                case TokenType.BinaryExpresion:
                    return ConectaBinaryExpresion((BinaryExpresion)expresion);
                case TokenType.UnaryExpresion:
                    return ConectaUnaryExpresion((UnaryExpresion)expresion);
                case TokenType.LiteralExpresion:
                    return ConectaLiteralExpresion((LiteralExpresion)expresion);
                case TokenType.IDExpresion:
                    return ConectaIDExpresion((IDExpresion)expresion);
                case TokenType.AsignacionVariableExpresion:
                    return ConectaAsignacionVariableExpresion((AsignacionVariableExpresion)expresion);
                case TokenType.DeclaracionExpresion:
                    return ConectaDeclaracionExpresion((DeclaracionExpresion)expresion);
                case TokenType.CallFuncion:
                    return ConectaFuncionCall((FuncionCallExpresion)expresion);
                case TokenType.VariableDeclaracionExpresion:
                    return ConectaVariableDeclaracion((VariableDeclaracionExpresion)expresion);
                case TokenType.IFDeclaracionExpresion:
                    return ConectaIfDeclaracion((IFDeclaracionExpresion)expresion);
                case TokenType.PrintExpresion:
                    return ConectaPrintExpresion((PrintExpresion)expresion);
                default:
                    return null;
            }
        }

        private Semantic_Expresion ConectaFuncionCall(FuncionCallExpresion expresion)
        {
            var nombre = expresion.Identificador.Text;
            switch (nombre)
            {
                case "sen":
                    return ConectaSenoExpresion(expresion);
                case "cos":
                    return ConectaCosenoExpresion(expresion);
                case "log":
                    return ConectaLogaritmoExpresion(expresion);
                case "sqrt":
                    return ConectaSqrtExpresion(expresion);
                default:
                    int cant = expresion.Argumentos.Count;
                    List<Semantic_Expresion> argumentos = new List<Semantic_Expresion>();
                    FuncionSymbol simbolo = new FuncionSymbol(nombre, cant);
                    if (!Hulk.Program.funciones.ContainsKey(simbolo.Name))
                    {
                        errores.Add(new Error(TipoError.FuncionError, $"Function '{nombre}' with {cant} parameters do not exist."));
                        return null;
                    }
                    if(Hulk.Program.funciones[simbolo.Name].Parametros.Count != expresion.Argumentos.Count)
                    {
                        errores.Add(new Error(TipoError.FuncionError, $"Function '{nombre}' with {cant} parameters do not exist."));
                        return null;
                    }
                    foreach(var a in expresion.Argumentos)
                    {
                        var semantic = ConectaExpresion(a);
                        argumentos.Add(semantic);
                    }
        
                  return new Semantic_FuncionCall(simbolo,argumentos);
            }
        }

        private Semantic_Expresion ConectaSqrtExpresion(FuncionCallExpresion expresion)
        {
            int argumentos = expresion.Argumentos.Count;
            if (argumentos != 1)
            {
                errores.Add(new Error(TipoError.SemanticError, $"Function sqrt cannot have {argumentos} parameters."));
                return new Semantic_LiteralExpresion(0);
            }
            var argumento = ConectaExpresion(expresion.Argumentos[0]);

            if (argumento.TipoHulk != TipoHulk.Number)
                errores.Add(new Error(TipoError.SemanticError, $"'{argumento.TipoHulk}' is not a valid argument for sqrt."));
            return new Semantic_SqrtExpresion(argumento);
        }



        private Semantic_Expresion ConectaLogaritmoExpresion(FuncionCallExpresion expresion)
        {
            int argumentos = expresion.Argumentos.Count;

            if (argumentos == 1)
            {
                errores.Add(new Error(TipoError.SemanticError, $"Function logarithm cannot have  only {argumentos} parameter."));
                return new Semantic_LiteralExpresion(0);
            }
            if (argumentos > 2 || argumentos == 0)
            {
                errores.Add(new Error(TipoError.SemanticError, $"Function logarithm cannot have {argumentos} parameters."));
                return new Semantic_LiteralExpresion(0);
            }


            var baseLog = ConectaExpresion(expresion.Argumentos[0]);
            var argumento = ConectaExpresion(expresion.Argumentos[1]);

            if (baseLog.TipoHulk != TipoHulk.Number)
                errores.Add(new Error(TipoError.SemanticError, $"'{argumento.TipoHulk}' is not a valid base for logarithm."));
            if (argumento.TipoHulk != TipoHulk.Number)
                errores.Add(new Error(TipoError.SemanticError, $"'{argumento.TipoHulk}' is not a valid argument for logarithm."));


            return new Semantic_LogExpresion(baseLog, argumento);
        }

        private Semantic_Expresion ConectaCosenoExpresion(FuncionCallExpresion expresion)
        {
            int argumentos = expresion.Argumentos.Count;
            if (argumentos != 1)
            {
                errores.Add(new Error(TipoError.SemanticError, $"Function coseno cannot have {argumentos} parameters."));
                return new Semantic_LiteralExpresion(0);
            }
            var argumento = ConectaExpresion(expresion.Argumentos[0]);

            if (argumento.TipoHulk != TipoHulk.Number)
                errores.Add(new Error(TipoError.SemanticError, $"'{argumento.TipoHulk}' is not a valid argument for coseno."));
            return new Semantic_CosExpresion(argumento);
        }

        private Semantic_Expresion ConectaSenoExpresion(FuncionCallExpresion expresion)
        {
            int argumentos = expresion.Argumentos.Count;
            if (argumentos != 1)
            {
                errores.Add(new Error(TipoError.SemanticError, $"Function seno cannot have {argumentos} parameters."));
                return new Semantic_LiteralExpresion(0);
            }
            var argumento = ConectaExpresion(expresion.Argumentos[0]);
            if (argumento.TipoHulk != TipoHulk.Number)
                errores.Add(new Error(TipoError.SemanticError, $"'{argumento.TipoHulk}' is not a valid argument for seno."));
            return new Semantic_SenExpresion(argumento);
        }

        private Semantic_Expresion ConectaPrintExpresion(PrintExpresion expresion)
        {
            var impresion = ConectaExpresion(expresion.Expresion);
            return new Semantic_PrintExpresion(impresion);
        }

        private Semantic_Expresion ConectaLiteralExpresion(LiteralExpresion expresion)
        {
            var value = expresion.Value ?? 0;
            return new Semantic_LiteralExpresion(value);
        }

        private Semantic_Expresion ConectaUnaryExpresion(UnaryExpresion expresion)
        {
            var Operando = ConectaExpresion(expresion.Operando);
            var Operador = Semantic_UnaryOperador.Semantic_Parse_UO(expresion.Operador.Type, Operando.TipoHulk);
            if (Operador == null)
            {
                errores.Add(new Error(TipoError.SemanticError, $"Operator '{expresion.Operador.Text}' cannot be used with '{Operando.TipoHulk}'."));
                return Operando;
            }
            return new Semantic_UnaryExpresion(Operador, Operando);
        }

        private Semantic_Expresion ConectaBinaryExpresion(BinaryExpresion expresion)
        {
            var left = ConectaExpresion(expresion.Left);

            var rigth = ConectaExpresion(expresion.Rigth);
            var operador = Semantic_BinaryOperador.Semantic_Parse_BO(expresion.Operador.Type, left.TipoHulk, rigth.TipoHulk);

            if (operador == null)
            {
                errores.Add(new Error(TipoError.SemanticError, $"Operator '{expresion.Operador.Text}' cannot be used between '{left.TipoHulk}' and '{rigth.TipoHulk}'."));
                return left;
            }
            return new Semantic_BinaryExpresion(left, operador, rigth);
        }

        private Semantic_Expresion ConectaAsignacionVariableExpresion(AsignacionVariableExpresion expresion)
        {
            string nombre = expresion.Identificador.Text;
            Semantic_Expresion semantic_Expresion = ConectaExpresion(expresion.Expresion);

            if (!Ambito.Try_AsignarVariable(nombre, out var variable))
            {
                errores.Add(new Error(TipoError.SemanticError, $"'{nombre}' does not exist in the current context."));
                return semantic_Expresion;
            }


            if (semantic_Expresion.TipoHulk != variable.TipoHulk)
            {
                errores.Add(new Error(TipoError.SemanticError, $"Variable '{nombre}' cannot have '{semantic_Expresion.TipoHulk}' as value, only '{variable.TipoHulk}'."));
                return semantic_Expresion;
            }

            return new Semantic_AsignacionVariable(variable, semantic_Expresion);

        }

        private Semantic_Expresion ConectaIDExpresion(IDExpresion expresion)
        {
            string nombre = expresion.Identificador.Text;

            if (!Ambito.Try_AsignarVariable(nombre, out var variable))
            {
                errores.Add(new Error(TipoError.SemanticError, $"'{nombre}' does not exist in the current context."));
                return new Semantic_LiteralExpresion(0);
            }

            return new Semantic_VariableExpresion(variable);

        }

        public static Dictionary<string, Semantic_Expresion> Conecta_CuerpoFuncion(Dictionary<string, FuncionDeclaracion> funciones)
        {
            var diccionarioDeclaracion = funciones;
            var cuerpos_Funcion = new Dictionary<string,Semantic_Expresion>();
            foreach( var  funcion in funciones )
            {
                var semantic_Object = new Semantic_Parser(null);
                foreach( var param in funcion.Value.Parametros)
                {
                    semantic_Object.Ambito.Try_DeclararVariable(new VariableSymbol(param.Text, param.GetType(),TipoHulk.Number));
                }
                var expresion = semantic_Object.ConectaExpresion(funcion.Value.Cuerpo);
                cuerpos_Funcion.Add(funcion.Value.Identificador.Text,expresion);

            }
            return cuerpos_Funcion;
        }
    }


}

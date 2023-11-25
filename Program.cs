using System;
using Hulk.Biblioteca;
using Hulk.Biblioteca.Semantic;
using Hulk.Biblioteca.Servidor;
using Hulk.Biblioteca.Tree;
using Hulk.Biblioteca.Errores;
using Hulk.Biblioteca.Parser;

namespace Hulk
{

    public static class Program
    {
        //Diccionario que guarda las funciones declaradas durante la ejecucion del programa
        public static Dictionary<string,FuncionDeclaracion> funciones = new Dictionary<string,FuncionDeclaracion>();
        public static void Main(string[] args)
        {
            while (true)
            {
                Servidor previous = null;
                //Diccionario con las variables que existiran en el contexto de la linea
                var variables = new Dictionary<VariableSymbol, object>();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("> ");
                Console.ResetColor();
                
                var line = Console.ReadLine();
                //Si el codigo entra dentro del condicional se detiene la ejecucion del programa
                if (string.IsNullOrEmpty(line))
                    break;

                //Comando adicional para limpiar la consola en runtime
                else if (line == "#clear")
                {
                    Console.Clear();
                    continue;
                }

                //LLamada a iniciar el proceso de parsing
                try
                {
                var syntaxTree = SyntaxTree.Parse(line);
                 
                 //Aqui se garantiza que no se pierdan las funciones declaradas con la ejecucion 
                 //de una nueva entrada del usuario
                if(syntaxTree.Declaracion== null)
                {
                foreach(var a in Parser.Funciones)
                funciones.TryAdd(a.Key,a.Value);
                continue;
                }
                
                if (syntaxTree != null)
                {
                    // Aqui manejamos los errores no relacionados con excepciones previos al proceso de parsing semantico
                    if (syntaxTree.Errores.Any())
                    {
                        ImprimeError(syntaxTree);
                        continue;
                    }
                    var semantic_expresion = new Servidor(previous,syntaxTree);
                    
                    var expresion = semantic_expresion.Sirve(variables);
                    
                    // Aqui manejamos los errores no relacionados con excepciones previos al proceso de evaluacion
                    if (expresion.Errores.Any())
                    {
                        ImprimeError(expresion);
                    }
                    //En caso de no haber errores, se imprimira en consola en caso de existir el retorno de la expresion
                    else
                    {

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("► ");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine(expresion.Value);
                        Console.ResetColor();
                        previous = semantic_expresion;

                    }
                }
                //Aqui se manejan las expresiones cuyo proceso de parsing retorno null, aquellas que no tienen 
                //ningun patron de expresion
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine($"Invalid expresion.");
                    Console.ResetColor();
                    continue;
                }}
                //Finalmente son manejados los errores relacionados con excepciones
                //(por el momento los que vienen como resultado de evaluar una funcion con argumentos no compatibles)
                //ya que en HULK no se declaran los tipos de los argumentos en una funcion
                catch(Exception e )
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                    continue;
                }

            }

        }
        

        //Este metodo se encarga de elegir el color de la salida de acuerdo al tipo de error
        private static void ImprimeError(SyntaxTree expresion)
        {
            ConsoleColor amarillo = ConsoleColor.DarkYellow;
            ConsoleColor rojoFuerte = ConsoleColor.DarkRed;
            ConsoleColor rojo = ConsoleColor.Red;
            ConsoleColor magenta = ConsoleColor.DarkMagenta;
            List<Error> alfa = expresion.Errores.ToList();

            switch (alfa[0].Tipo)
            {
                case TipoError.LexicalError:
                    Console.ForegroundColor = amarillo;
                    break;
                case TipoError.SemanticError:
                    Console.ForegroundColor = rojoFuerte;
                    break;
                case TipoError.SintacticError:
                    Console.ForegroundColor = rojo;
                    break;
                case TipoError.FuncionError:
                    Console.ForegroundColor = magenta;
                    break;
            }
            Console.WriteLine(alfa[0].Mensaje);
            Console.ResetColor();
        }
        private static void ImprimeError(Retorno expresion)
        {
            ConsoleColor amarillo = ConsoleColor.DarkYellow;
            ConsoleColor rojoFuerte = ConsoleColor.DarkRed;
            ConsoleColor rojo = ConsoleColor.Red;
            ConsoleColor magenta = ConsoleColor.DarkMagenta;
            List<Error> alfa = expresion.Errores.ToList();

            switch (alfa[0].Tipo)
            {
                case TipoError.LexicalError:
                    Console.ForegroundColor = amarillo;
                    break;
                case TipoError.SemanticError:
                    Console.ForegroundColor = rojoFuerte;
                    break;
                case TipoError.SintacticError:
                    Console.ForegroundColor = rojo;
                    break;
                case TipoError.FuncionError:
                    Console.ForegroundColor = magenta;
                    break;
            }
            Console.WriteLine(alfa[0].Mensaje);
            Console.ResetColor();
        }


    }
}
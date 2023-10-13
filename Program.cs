using System;
using Hulk.Biblioteca;
using Hulk.Biblioteca.Semantic;
using Hulk.Biblioteca.Servidor;
using Hulk.Biblioteca.Tree;
using Hulk.Biblioteca.Errores;

namespace Hulk
{

    class Porgram
    {
        public static void Main(string[] args)
        {



            while (true)
            {
                Servidor previous = null;

                var variables = new Dictionary<VariableSymbol, object>();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("> ");
                Console.ResetColor();
                var line = Console.ReadLine();
              
                if (string.IsNullOrEmpty(line))
                    break;
                else if (line == "#clear")
                {
                    Console.Clear();
                    continue;
                }
          
                var lexer = new Lexer(line);

                var syntaxTree = SyntaxTree.Parse(line);
                if (syntaxTree != null)
                {
                    if(syntaxTree.Errores.Any()) 
                    {
                        ImprimeError(syntaxTree);
                        continue;
                    }
                    var semantic_expresion = previous == null ? new Servidor(syntaxTree) : previous.ContinuaCon(syntaxTree);

                    var expresion = semantic_expresion.Sirve(variables);


                    if (expresion.Errores.Any())
                    {
                          ImprimeError(expresion);
                    }
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
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine($"Invalid expresion.");
                    Console.ResetColor();
                    continue;
                }
            }

        }

        private static void ImprimeError(SyntaxTree expresion)
        {
            ConsoleColor amarillo = ConsoleColor.DarkYellow;
            ConsoleColor rojoFuerte = ConsoleColor.DarkRed;
            ConsoleColor rojo = ConsoleColor.Red;
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
            }
            Console.WriteLine(alfa[0].Mensaje);
            Console.ResetColor();
        }
        private static void ImprimeError(Retorno expresion)
        {
            ConsoleColor amarillo = ConsoleColor.DarkYellow;
            ConsoleColor rojoFuerte = ConsoleColor.DarkRed;
            ConsoleColor rojo = ConsoleColor.Red;
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
            }
            Console.WriteLine(alfa[0].Mensaje);
            Console.ResetColor();
        }

        
    }
}
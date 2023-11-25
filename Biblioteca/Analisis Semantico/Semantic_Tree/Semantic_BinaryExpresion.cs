using Hulk.Biblioteca.Tree;

namespace Hulk.Biblioteca.Semantic
{
    //Objeto semantico de expresiones binarias
    internal sealed class Semantic_BinaryExpresion : Semantic_Expresion
    {
        public Semantic_Expresion Left { get; }
        public Semantic_BinaryOperador Operador { get; }
        public Semantic_Expresion Rigth { get; }
        public Semantic_BinaryExpresion(Semantic_Expresion left, Semantic_BinaryOperador operador, Semantic_Expresion rigth)
        {
            Left = left;
            Operador = operador;
            Rigth = rigth;
        }


        public Semantic_BinaryOperadorType TipoOperador { get; }


        public override SemanticType Kind => SemanticType.BinaryExpresion;
        public override TipoHulk TipoHulk => Operador.ReturnType;

       
    }

    //Operadores binarios
    internal sealed class Semantic_BinaryOperador
    {
        public Semantic_BinaryOperador(TokenType tokentype, Semantic_BinaryOperadorType tipoOperador,
        TipoHulk type) : this(tokentype, tipoOperador, type, type, type)
        {
        }
        public Semantic_BinaryOperador(TokenType tokentype, Semantic_BinaryOperadorType tipoOperador,
       TipoHulk operandosTipo, TipoHulk returnType) : this(tokentype, tipoOperador, operandosTipo, operandosTipo, returnType)
        {
        }
        public Semantic_BinaryOperador(TokenType tokentype, Semantic_BinaryOperadorType tipoOperador,
        TipoHulk leftType, TipoHulk rigthType, TipoHulk returnType)
        {
            Tokentype = tokentype;
            TipoOperador = tipoOperador;
            LeftType = leftType;
            RigthType = rigthType;
            ReturnType = returnType;
        }

        public TokenType Tokentype { get; }
        public Semantic_BinaryOperadorType TipoOperador { get; }
        public TipoHulk LeftType { get; }
        public TipoHulk RigthType { get; }
        public TipoHulk ReturnType { get; }
        
        //Con este array de operadores se cbren todas las posibles variantes de las operaciones
        //teniendo en cuenta tipo de retorno, tipo de los operandos y el operador
        private static Semantic_BinaryOperador[] operadores =
        {
            //Concatenador @
            new Semantic_BinaryOperador(TokenType.Concatenador,Semantic_BinaryOperadorType.Concatenacion,TipoHulk.String),
            new Semantic_BinaryOperador(TokenType.Concatenador,Semantic_BinaryOperadorType.Concatenacion,TipoHulk.Number,TipoHulk.String),
            new Semantic_BinaryOperador(TokenType.Concatenador,Semantic_BinaryOperadorType.Concatenacion,TipoHulk.Boolean,TipoHulk.String),
            new Semantic_BinaryOperador(TokenType.Concatenador,Semantic_BinaryOperadorType.Concatenacion,TipoHulk.Boolean,TipoHulk.Number, TipoHulk.String),
            new Semantic_BinaryOperador(TokenType.Concatenador,Semantic_BinaryOperadorType.Concatenacion,TipoHulk.Number,TipoHulk.Boolean,TipoHulk.String),
            new Semantic_BinaryOperador(TokenType.Concatenador,Semantic_BinaryOperadorType.Concatenacion,TipoHulk.Number, TipoHulk.String,TipoHulk.String),
            new Semantic_BinaryOperador(TokenType.Concatenador,Semantic_BinaryOperadorType.Concatenacion,TipoHulk.String,TipoHulk.Number,TipoHulk.String),
            new Semantic_BinaryOperador(TokenType.Concatenador,Semantic_BinaryOperadorType.Concatenacion,TipoHulk.String, TipoHulk.Boolean,TipoHulk.String),
            new Semantic_BinaryOperador(TokenType.Concatenador,Semantic_BinaryOperadorType.Concatenacion,TipoHulk.Boolean,TipoHulk.String,TipoHulk.String),
            
            //Operadores aritmeticos
            //Suma +
            new Semantic_BinaryOperador(TokenType.PlusToken,Semantic_BinaryOperadorType.Addition,TipoHulk.Number),
            //Resta -
            new Semantic_BinaryOperador(TokenType.MinusToken,Semantic_BinaryOperadorType.Diference,TipoHulk.Number),
            //Division /
            new Semantic_BinaryOperador(TokenType.DivToken,Semantic_BinaryOperadorType.Division,TipoHulk.Number),
            //Multiplicacion *
            new Semantic_BinaryOperador(TokenType.MultToken,Semantic_BinaryOperadorType.Multiplicacion,TipoHulk.Number),
            //Potencia ^
            new Semantic_BinaryOperador(TokenType.Potencia,Semantic_BinaryOperadorType.Pow,TipoHulk.Number),
            //Modulo %
            new Semantic_BinaryOperador(TokenType.ModuloResto,Semantic_BinaryOperadorType.Modulo,TipoHulk.Number),


            //Operadores Logicos
            //Disyuncion
            new Semantic_BinaryOperador(TokenType.OrLogic,Semantic_BinaryOperadorType.OrLogica,TipoHulk.Boolean),
            //Conjuncion
            new Semantic_BinaryOperador(TokenType.AndLogic,Semantic_BinaryOperadorType.AndLogica,TipoHulk.Boolean),

            //Comparadores
            //Igual a ==
            new Semantic_BinaryOperador(TokenType.IgualComparador,Semantic_BinaryOperadorType.EsIgual,TipoHulk.Number, TipoHulk.Boolean),
            new Semantic_BinaryOperador(TokenType.IgualComparador,Semantic_BinaryOperadorType.EsIgual,TipoHulk.String, TipoHulk.Boolean),
            new Semantic_BinaryOperador(TokenType.IgualComparador,Semantic_BinaryOperadorType.EsIgual, TipoHulk.Boolean),
            //Desigual a !=
            new Semantic_BinaryOperador(TokenType.DesigualComparador,Semantic_BinaryOperadorType.Desigual,TipoHulk.Number, TipoHulk.Boolean),
            new Semantic_BinaryOperador(TokenType.DesigualComparador,Semantic_BinaryOperadorType.Desigual,TipoHulk.String, TipoHulk.Boolean),
            new Semantic_BinaryOperador(TokenType.DesigualComparador,Semantic_BinaryOperadorType.Desigual, TipoHulk.Boolean),
            //Mayor que >
            new Semantic_BinaryOperador(TokenType.MayorQue,Semantic_BinaryOperadorType.MayorComparar,TipoHulk.Number, TipoHulk.Boolean),
            //Menor que <
            new Semantic_BinaryOperador(TokenType.MenorQue,Semantic_BinaryOperadorType.MenorComparar,TipoHulk.Number, TipoHulk.Boolean),
            //Mayor o igual >=
            new Semantic_BinaryOperador(TokenType.MayorOIgual,Semantic_BinaryOperadorType.MayorOIgualComparar,TipoHulk.Number, TipoHulk.Boolean),
            //Menor o igual <=
            new Semantic_BinaryOperador(TokenType.MenorOIgual,Semantic_BinaryOperadorType.MenorOIgualComparar,TipoHulk.Number, TipoHulk.Boolean)
        };
        
        
        //Este metodo recorre el array y verifica si la operacion en cuestion coincide con algun patron
        public static Semantic_BinaryOperador Semantic_Parse_BO(TokenType tokenType, TipoHulk leftType, TipoHulk rigthType)
        {

            
            foreach (var a in operadores)
            {
                if (a.Tokentype == tokenType && a.LeftType == leftType && a.RigthType == rigthType)
                    return a;
            }
            return null;
        }


    }
    //Enum de los operadores semanticos
    internal enum Semantic_BinaryOperadorType
    {
        Addition,
        Diference,
        Multiplicacion,
        Division,
        Pow,
        Modulo,
        AndLogica,
        OrLogica,
        EsIgual,
        Desigual,
        MayorComparar,
        MenorComparar,
        MayorOIgualComparar,
        MenorOIgualComparar,
        Concatenacion
    }



}
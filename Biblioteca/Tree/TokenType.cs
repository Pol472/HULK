namespace Hulk.Biblioteca.Tree
{
    // En el enum estan presentes, todos los tipos de tokens conocidos por el lenguaje, y de manera especial
    // un tipo para las expresiones , usado a la hora de relacionarlas entre si
    public enum TokenType
    {
        //Especial Tokens
        UnknownToken,
        EndOfLine,
        WhiteSpaceToken,
        OpenParenToken,
        CloseParenToken,
        Cierre,
        IgualAsignador,
        Coma,
    

        //KeyWords
        TrueKeyWord,
        FalseKeyWord,
        LetKeyWord,
        InKeyWord,
        IfKeyWord,
        ElseKeyWord,
        FuncionKeyWord,

        //Tipos de datos
        NumberToken,
        IdentificadorToken,

        //Operadores Aritmeticos
        PlusToken,
        MinusToken,
        MultToken,
        DivToken,
        Potencia,
        ModuloResto,

        //Operadores Logicos
        Negation,
        AndLogic,
        OrLogic,

        //Comparadores
        IgualComparador,
        DesigualComparador,
        MayorQue,
        MenorQue,
        MenorOIgual,
        MayorOIgual,

        //Operadores Aritmeticos Funcionales
        Sqrt,
        Sen,
        Cos,

        //Expresion Tokens
        AsignacionVariableExpresion,
        BinaryExpresion,
        ParentesisExpresion,
        LiteralExpresion,
        UnaryExpresion,
        IDExpresion,
       
       

        //Declaraciones 
        
        
        DeclaracionExpresion,
        VariableDeclaracionExpresion,
        IFDeclaracionExpresion,
        
        ElseDeclaracion,
        FuncionDeclaracion,
        Concatenador,
        StringToken,
        PrintExpresion,
        PrintKeyWord,
        FuncionOperadorExpresion,
        SenoExpresion,
        CosenoExpresion,
        LogaritmoExpresion,
        SenKeyWord,
        CosKeyWord,
        LogKeyWord,
        PI,
        Euler,
        CallFuncion,
        Flecha,
        SqrtKeyWord,
    }
   
}
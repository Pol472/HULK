namespace Hulk.Biblioteca.Tree
{
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
        OpenBrace,
        CloseBrace,

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
    }
    public enum TiposPrimitivosHulk
    {
        String,
        Number,
        Boolean,
        Identificador,
        LetInExpresion,
        ElseExpresion,
        IfExpresion,
        BloqueExpresion,
        MultipleAsignacion,
    }
}
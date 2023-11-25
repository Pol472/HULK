namespace Hulk.Biblioteca.Semantic
{
    //Enum de los tipos de expresiones semanticas que son devueltos al evaluador
    public enum SemanticType
    {
        UnaryExpresion,
        LiteralExpresion,
        BinaryExpresion,
        AsignacionVariable,
        VariableExpresion,
        DeclaracionExpresion,
       
        VariableDeclaracion,
        IfDeclaracion,
        Parentesis,
        Declaracion,
        PrintExpresion,
        SenExpresion,
        CosExpresion,
        LogExpresion,
        FuncionCall,
        FuncionDeclaracion,
        SqrtExpresion
    }

    //Tipos de Hulk usados para chequeo de tipos en el analisis semantico
    public enum TipoHulk
    {
        Number,
        Boolean,
        Void,
        Identificador,
        String,
        FuncionDeclaracion
    }
    


}
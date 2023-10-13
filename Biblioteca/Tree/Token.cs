namespace Hulk.Biblioteca.Tree
{
    public sealed class Token : Nodo
    {
        public int Position;
        public string Text;
        public object Value;
        public override TokenType Type { get; }

        public Token(TokenType type, int position, string text, object value)
        {
            Type = type;
            Position = position;
            Text = text;
            Value = value;
        }

     
    }
}
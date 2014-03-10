namespace MiniPL.AbstractSyntaxTree
{
    public class Variable
    {
        public Variable(string identifier)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }
}

namespace MiniPL.AbstractSyntaxTree
{
    public class VariableType<T> : Variable
    {
        public VariableType(string identifier, T value) : base(identifier)
        {
            Value = value;
        }
        
        protected internal VariableType(string identifier) : base(identifier) { }

        public T Value { get; set; }
    }
}

namespace MiniPL.AbstractSyntaxTree
{
    /// <summary>
    /// Base class for statements
    /// </summary>
    public class Statement
    {
        /// <summary>
        /// Executes the statement. Must be overridden.
        /// </summary>
        public virtual void Execute()
        {
            throw new System.NotImplementedException("Execute() method was not overrided");
        }
    }
}

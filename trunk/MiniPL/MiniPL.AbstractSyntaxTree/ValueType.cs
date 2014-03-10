namespace MiniPL.AbstractSyntaxTree
{
    /// @author Jani Viherväs
    /// @version 8.3.2014
    ///
    /// <summary>
    /// Class to hold a single value
    /// </summary>
    public class ValueType<T> : Value
    {
        /// <summary>
        /// Value
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Creates a new object to hold a single value
        /// </summary>
        /// <param name="value">Value</param>
        public ValueType(T value)
        {
            Value = value;
        }

    }
}

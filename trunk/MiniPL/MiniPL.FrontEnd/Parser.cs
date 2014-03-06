using System.Collections.Generic;

namespace MiniPL.FrontEnd
{
    /// @author Jani Viherväs
    /// @version 27.2.2014
    ///
    /// <summary>
    /// Class to handle the parsing of the source code. Creates the abstract syntax tree.
    /// </summary>
    public class Parser
    {
        private int _i = 0;
        private List<Token> _tokens;

        /// <summary>
        /// Parses the token list
        /// </summary>
        /// <param name="tokens">Tokens scanned</param>
        public void Parse(List<Token> tokens) // TODO: construct the abstract syntax tree
        {
            _tokens = tokens;
            Statements();
        }


        /// <summary>
        /// Gets the next token or null
        /// </summary>
        /// <returns>The next token or null if there are no more tokens</returns>
        private Token NextToken()
        {
            return _i < _tokens.Count
                       ? _tokens[_i++]
                       : null;
        }

        
        /// <summary>
        /// Handles the "stmts" production
        /// </summary>
        private void Statements()
        {
            
        }


        /// <summary>
        /// Handles the "stmts'" production
        /// </summary>
        private void Statements_()
        {
            
        }


        /// <summary>
        /// Handles the "stmt" production
        /// </summary>
        private void Statement()
        {
            
        }


        /// <summary>
        /// Handles the "stmt'" production
        /// </summary>
        private void Statement_()
        {
            
        }


        /// <summary>
        /// Handles the "expr" production
        /// </summary>
        private void Expression()
        {
            
        }


        /// <summary>
        /// Handles the "expr'" production
        /// </summary>
        private void Expression_()
        {
            
        }


        /// <summary>
        /// Handles the "opnd" production
        /// </summary>
        private void Operand()
        {
            
        }


        /// <summary>
        /// Handles the "type" production
        /// </summary>
        private void Type()
        {
            
        }


        /// <summary>
        /// Handles the "op" production
        /// </summary>
        private void Operator()
        {
            
        }


        /// <summary>
        /// Handles the "unary" production
        /// </summary>
        private void UnaryOperator()
        {
            
        }


        /// <summary>
        /// Handles the "ident" production
        /// </summary>
        private void Identifier()
        {
            
        }


        /// <summary>
        /// Handles the "ident'" production
        /// </summary>
        private void Identifier_()
        {
            
        }
    }
}

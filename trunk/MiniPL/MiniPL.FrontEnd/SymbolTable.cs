using System.Collections.Generic;

namespace MiniPL.FrontEnd
{
    /// @author Jani Viherväs
    /// @version 6.3.2014
    ///
    /// <summary>
    /// Symbol table, which holds the symbols
    /// </summary>
    public static class SymbolTable
    {
        /// <summary>
        /// Symbol table
        /// </summary>
        private static HashSet<string> _symbolTable;


        /// <summary>
        /// Adds a new symbol to the symbol table. Can't add existing ones.
        /// </summary>
        /// <param name="identifier">Symbols identifier to add</param>
        /// <returns>True if adding was successfull</returns>
        public static bool AddSymbol(string identifier)
        {
            if ( _symbolTable == null )
            {
                _symbolTable = new HashSet<string>();
            }
            return !_symbolTable.Contains(identifier) && _symbolTable.Add(identifier);
        }


        /// <summary>
        /// Finds out if the symbol is in the symbol table
        /// </summary>
        /// <param name="identifier">Symbol's identifier</param>
        /// <returns>True if symbol table contains the symbol</returns>
        public static bool FindSymbol(string identifier)
        {
            if ( _symbolTable == null )
            {
                _symbolTable = new HashSet<string>();
            }
            return _symbolTable.Contains(identifier);
        }


        /// <summary>
        /// Empties symbol table
        /// </summary>
        public static void DeleteAllSymbols()
        {
            _symbolTable = null;
        }
    }
}

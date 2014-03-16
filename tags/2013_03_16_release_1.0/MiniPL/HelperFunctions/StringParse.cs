using System.Text;

namespace HelperFunctions
{
    /// @author Jani Viherväs
    /// @version 3.3.2014
    ///
    /// <summary>
    /// Helper functions for string parsing and handling
    /// </summary>
    public static class StringParse
    {
        /// <summary>
        /// Moves the index so that it skips the string that may have escape characters
        /// </summary>
        /// <param name="input">Input to scan</param>
        /// <param name="startIndex">The starting index of scanning, has to be beginning of the string</param>
        /// <returns></returns>
        public static int SkipString(string input, int startIndex = 0)
        {
            if ( input[startIndex] != '"' )
            {
                return startIndex;
            }
            startIndex++;
            for ( var i = startIndex; i < input.Length; i++ )
            {
                if ( input[i] == '"' )
                {
                    startIndex++;
                    break;
                }
                if ( input[i] == '\\' )
                {
                    startIndex++;
                    i++;
                }
                startIndex++;
            }
            return startIndex;
        }


        /// <summary>
        /// Scans the string literal
        /// </summary>
        /// <param name="input">Input to scan</param>
        /// <param name="startIndex">The starting index of scanning, has to be beginning of the string</param>
        /// <returns>"te\"st" => te"st</returns>
        public static string ScanString(string input, int startIndex = 0)
        {
            var s = input.ToCharArray();
            if ( s.Length < 2 )
            {
                return "";
            }
            if ( s[startIndex] != '"' )
            {
                return "";
            }

            var result = new StringBuilder();

            for ( var i = startIndex + 1; i < s.Length; i++ )
            {
                if ( s[i] == '"' ) // End of the string, i.e. string s = "test";
                {
                    break;
                }

                if ( i == s.Length - 1 && s[i] != '"' ) // We are at the last character 
                {                                       // and have not stumbled upon closing quotes
                    return "";                          // f.g. string s = "test
                }

                if ( s[i] == '\\' && i < s.Length - 1 ) // Escape character, i.e. string s = "te\"st"  ==> te"st
                {
                    if ( s[i + 1] != 'n' &&
                        s[i + 1] != 'r' &&
                        s[i + 1] != 't' &&
                        s[i + 1] != '\\' )
                        i++;
                }

                result.Append(s[i]);
            }
            return result.ToString();
        }

    }
}

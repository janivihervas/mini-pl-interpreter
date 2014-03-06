using System;
using HelperFunctions;
using MiniPL.FrontEnd;

namespace MiniPL.Interpreter
{
    /// @author Jani Viherväs
    /// @version 27.2.2014
    ///
    /// <summary>
    /// An interpreter for the Mini-PL programming language
    /// </summary>
    public class Interpreter
    {
        /// <summary>
        /// File extension
        /// </summary>
        public const string FileExtension = ".mpl";

        /// <summary>
        /// Mark to add to the end of file
        /// </summary>
        public const string EndOfFileMark = "$$";

        /// <summary>
        /// Reads the Mini-PL source code and executes it.
        /// </summary>
        /// <param name="args">File name or path/fileName</param>
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("You must give a file name or a path/fileName as a parameter");
                return;
            }
            if (args.Length > 1)
            {
                Console.WriteLine("Too many parameters");
                return;
            }

            var fileReader = new FileReader(FileExtension);
            try
            {
                var lines = fileReader.ReadFile(args[0]);
                var scanner = new Scanner();
                var tokens = scanner.Tokenize(lines);
                var parser = new Parser();
                parser.Parse(tokens);

            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }

        }
    }
}

using System;
using MiniPL.FrontEnd;
using MiniPL.IO;

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
            //var fileReader = new FileReader();
            //var scanner = new Scanner();
        }
    }
}

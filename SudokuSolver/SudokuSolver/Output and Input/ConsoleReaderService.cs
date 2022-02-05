using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class ConsoleReaderService : IReader
    {
        //The class ConsoleReaderService responsible of reading the string (the string board) from the console..

        /*
        * Summary:
        * The functionc gets the string (the string board) from the console.
        *  
        * Input:
        * None.
        * 
        * OutPut:
        * The functions returns the string (the string board) from the console.
        */
        public string ReadBoard()
        {
            string contant = string.Empty;
            contant = Console.ReadLine();
            return contant;
        }

    }
}

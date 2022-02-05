using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SudokuSolver.Exceptions;
using System.IO;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetIn(new StreamReader(Console.OpenStandardInput(8192)));

            IConsoleOutputService consoleOutputService = new ConsoleOutputService();
            consoleOutputService.view();

           
        }
    }
}

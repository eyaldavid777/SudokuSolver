using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
             Board b = new Board("0140020000000310");
             b.print();

             Console.WriteLine();

             Board a = new Board("014002000000031000000000000000000008000500000000000000040000000000000000000000001");
             a.print();


        }
    }
}

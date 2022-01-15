using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    static class NumbersNotInBoard
    {
         public static Dictionary<int, List<int>> numbersInRows;
         public static Dictionary<int, List<int>> numbersInCols;

    
        static NumbersNotInBoard()
        {
            numbersInRows = new Dictionary<int, List<int>>();
            numbersInCols = new Dictionary<int, List<int>>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    interface IReader
    {
        // the interface responsible af getting the sudoku string 
        string ReadBoard();
    }
}

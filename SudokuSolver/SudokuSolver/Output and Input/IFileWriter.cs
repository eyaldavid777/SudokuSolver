using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Output_and_Input
{
    interface IFileWriter
    {
        // the interface responsible af writing the sudoku string to a new file
        void writeToFile(string boardStringResult);
    }
}

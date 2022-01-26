using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{

    interface ICell
    {
        int number { get; set; }
       int index { get; }
        List<int> optionalNumbers { get; set; }
        void solvedTheCell(int knownNumber);

        bool isSolved();
    }
}

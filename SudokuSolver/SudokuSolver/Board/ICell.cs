using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{

    public interface ICell
    {
        // the interface ICell reveals to you what you are allowes to use in cell.
        int number { get; set; }
       int index { get; }
        List<int> optionalNumbers { get; set; }
        void solvedTheCell(int knownNumber);
        void cellIntegrity( int sizeOfBoard);
        bool isSolved();
    }
}

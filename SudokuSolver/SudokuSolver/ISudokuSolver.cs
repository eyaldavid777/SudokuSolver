using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    interface ISudokuSolver
    {
        void Solve();

        List<int> emptyCellsIndexes { get; set; }

        int countHowManySolvedCells();

        void secondStepOfSolving();

        ISudokuBoard backTracking(ISudokuBoard board);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public interface ISudokuSolver
    {
        // the interface ISudokuSolver reveals to you what you are allowes to use in the Solver class.
        void Solve();

        ISudokuBoard sudokuBoard { get;  }
        List<int> emptyCellsIndexes { get; set; }

        int countHowManyUnSolvedCells();

        void secondStepOfSolving();

        ISudokuBoard backTracking(ISudokuBoard board);
    }
}

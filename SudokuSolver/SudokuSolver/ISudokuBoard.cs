using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    interface ISudokuBoard 
    {
        Dictionary<int, List<int>> placesOfNumbers { get; set; }
        int sizeOfBoard { get; }
        int SqrtOfSizeOfBoard { get; }

        KnownNumbersNotInBoard knownNumbersNotInBoard { get; set; }
        void print();
        void Solve();
        bool isPossibleIndexToNumber(int indexOfNumberInBoard, int indexOfNumberInCube, int number);

        void deleteNumberFromRowOrCol(bool col, int indexOfNumberInBoard, int indexOfNumberInCube, int number);

    }
}

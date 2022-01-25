using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    interface ISudokuCube 
    {
        void print(int rowInCube);

        List<int> fillOptionsInCube(int number, bool checkTheOptions);

        bool isNumberInRowOrColInCube(bool col, int colOrRowIndex, int number);

        bool isRowOrColFull(bool col, int colOrRowIndex);

        void putTheNumberAndDeletOptions(int indexInCube, int knownNumber);

        void deleteNumberFromRowOrColInCube(bool col, int colOrRowIndexInCube, int number);

        void rowOrColIntegrity(bool col, int RowOrColInCube, int RowOrColOfCube, List<int> CountNumberInRowAndCol);

        void cubeIntegrity(List<int> CountNumberInRowAndCol, int cubeNumber);

        int countHowManySolvedCells();
    }
}

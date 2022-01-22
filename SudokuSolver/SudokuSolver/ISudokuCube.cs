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

        void putTheNumberAndDeletOptions(int indexInCube, int knownNumber, int cubeNumber);

        void deleteNumberFromRowOrColInCube(bool col, int colOrRowIndexInCube, int number);


    }
}

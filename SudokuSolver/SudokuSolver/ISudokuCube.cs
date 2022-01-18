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

        int fillOptionsInCube(int number, bool checkTheOptions);

        bool isNumberInRowOrColInCube(bool col, int colOrRowIndex, int number);

        bool isRowOrColFull(bool col, int colOrRowIndex);
    }
}

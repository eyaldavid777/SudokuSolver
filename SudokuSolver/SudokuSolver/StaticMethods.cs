using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public static class StaticMethods
    {
        public static int[] forParameters(int colOrRowIndex, bool col,int sqrtOfSizeOfBoard)
        {
            int[] forParams = new int[3];
            if (col)
            {
                forParams[0] = sqrtOfSizeOfBoard;
                forParams[1] = colOrRowIndex;
                forParams[2] = colOrRowIndex + sqrtOfSizeOfBoard * (sqrtOfSizeOfBoard - 1) + 1;
                return forParams;
            }
            forParams[0] = 1;
            forParams[1] = colOrRowIndex * sqrtOfSizeOfBoard;
            forParams[2] = forParams[1] + sqrtOfSizeOfBoard;
            return forParams;
        }
    }
}

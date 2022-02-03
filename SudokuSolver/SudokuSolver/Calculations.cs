using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public static class Calculations
    { 
        public static int getCubeNumberByIndex(int index,int sizeOfBoard)
        {
            int SqrtOfSizeOfBoard = Sqrt(sizeOfBoard);
            return index / sizeOfBoard / SqrtOfSizeOfBoard * SqrtOfSizeOfBoard + index % sizeOfBoard / SqrtOfSizeOfBoard;
        }
        public static int getRowOrColInCubeByIndexInBoard(bool col, int index, int sizeOfBoard)
        {
            return getRowOrCol(col, index, sizeOfBoard) % Sqrt(sizeOfBoard);
        }
        public static int getIndexInCubeByIndexInBoard(int indexInBoard, int sizeOfBoard)
        {
            return getRowOrColInCubeByIndexInBoard(false, indexInBoard, sizeOfBoard) * Sqrt(sizeOfBoard) + getRowOrColInCubeByIndexInBoard(true, indexInBoard, sizeOfBoard);              
        }

        public static int getRowOrCol(bool col, int number1, int number2)
        {
            if (col)
                return number1 % number2;
            return number1 / number2;
        }
        public static int Sqrt(int number)
        {
            return (int)Math.Sqrt(number);
        }

    }
}

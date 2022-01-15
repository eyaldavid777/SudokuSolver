using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Cube : IInitializeable
    {
        public ICell[] cube;

        public Cube(String cubeNumbers, int numOfCube)
        {
            cube = new ICell[Board.sizeOfBoard];
            Initialize(cubeNumbers, numOfCube);
        }
        public int indexInBoard(int indexInCube, int numOfCube)
        {
            int sqrtOfBoardSize = Board.SqrtOfSizeOfBoard;
            return (numOfCube / sqrtOfBoardSize) * Board.sizeOfBoard * sqrtOfBoardSize
                         + indexInCube % sqrtOfBoardSize + Board.sizeOfBoard * (indexInCube / sqrtOfBoardSize)
                         + (numOfCube % sqrtOfBoardSize) * sqrtOfBoardSize;
        }
        public void Initialize(String numbersInBoard, int numOfCube)
        {
            for (int indexInCube = 0; indexInCube < cube.Length; indexInCube++)
            {
                int index = indexInBoard(indexInCube, numOfCube);

                if (numbersInBoard[index] == '0')
                    cube[indexInCube] = new UnsolvedCell(numbersInBoard[index], index);
                else
                    cube[indexInCube] = new SolvedCell(numbersInBoard[index], index);
            }
        }
        public void print(int rowInCube)
        {
            for (int colInCube = 0; colInCube < Board.SqrtOfSizeOfBoard; colInCube++)
            {
                if (cube[rowInCube * Board.SqrtOfSizeOfBoard + colInCube].GetType() == typeof(SolvedCell))
                {
                    System.Console.Write("   {0}   ", ((SolvedCell)cube[rowInCube * Board.SqrtOfSizeOfBoard + colInCube]).number);
                    if (colInCube == Board.SqrtOfSizeOfBoard - 1)
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    System.Console.Write("|");
                }
                else
                {
                    if (colInCube == Board.SqrtOfSizeOfBoard - 1)
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    System.Console.Write("       |");
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        public int fillOptionsInCube(int number,bool checkTheOptions)
        {
            int countOfOptionsInCube = 0;
            for(int indexInCube =0; indexInCube < cube.Length; indexInCube++)
            {
                if(cube[indexInCube].GetType() == typeof(UnsolvedCell))
                {
                    if (!checkTheOptions || Board.isPossibleIndexToNumber(cube[indexInCube].getIndex(), indexInCube, number))
                    {
                        ((UnsolvedCell)cube[indexInCube]).optionalNumbers.Add(number);
                        countOfOptionsInCube++;
                    }
                }
            }
            return countOfOptionsInCube;
        }
        private bool isNumberInRowOrColInCube(int addToIndexInCube,int indexInCube, int endOfColOrRow, int number)
        {
            for (; indexInCube < endOfColOrRow; indexInCube += addToIndexInCube)
            {
                if (cube[indexInCube].GetType() == typeof(SolvedCell))
                    if (((SolvedCell)cube[indexInCube]).number == number)
                        return true;
            }
            return false;
        }
        private bool isNumberInRowOrColInNumbersNotInBoard(Dictionary<int, List<int>> numbersInRowsOrCols,int colOrRowIndex, int number)
        {
            if (numbersInRowsOrCols.ContainsKey(colOrRowIndex))
                foreach (int numberInColOrRow in numbersInRowsOrCols[colOrRowIndex])
                {
                    if (numberInColOrRow == number)
                        return true;
                }
            return false;
        }
        public bool isNumberInRowOrCol(bool col, int colOrRowIndex, int number)
        {
            int addToIndexInCube = 1;
            int indexInCube = colOrRowIndex * Board.SqrtOfSizeOfBoard;
            int endOfColOrRow = indexInCube + Board.SqrtOfSizeOfBoard;
            Dictionary<int, List<int>> numbersInRowsOrCols = NumbersNotInBoard.numbersInRows;
            if (col) {
                numbersInRowsOrCols = NumbersNotInBoard.numbersInCols;
               addToIndexInCube = Board.SqrtOfSizeOfBoard;
                indexInCube = colOrRowIndex;
                endOfColOrRow = colOrRowIndex + Board.SqrtOfSizeOfBoard * (Board.SqrtOfSizeOfBoard - 1) +1;
            }
            if (isNumberInRowOrColInCube(addToIndexInCube, indexInCube, endOfColOrRow, number))
                return true;
            else
                return isNumberInRowOrColInNumbersNotInBoard(numbersInRowsOrCols, colOrRowIndex, number);
        }
        public bool isRowOrColFull(bool col, int colOrRowIndex)
        {
            int addToIndexInCube = 1;
            int indexInCube = colOrRowIndex * Board.SqrtOfSizeOfBoard;
            int endOfColOrRow = indexInCube + Board.SqrtOfSizeOfBoard;
            if (col)
            {
                addToIndexInCube = Board.SqrtOfSizeOfBoard;
                indexInCube = colOrRowIndex;
                endOfColOrRow = colOrRowIndex + Board.SqrtOfSizeOfBoard * (Board.SqrtOfSizeOfBoard - 1) + 1;
            }
            for (; indexInCube < endOfColOrRow; indexInCube += addToIndexInCube)
            {
                if (cube[indexInCube].GetType() == typeof(UnsolvedCell))
                    return false;
                    
            }
            return true;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Cube
    {
        private ICell[] cube;
        private ISudokuBoard inBoard;
        public Cube(String cubeNumbers, int numOfCube, ISudokuBoard InBoard)
        {
            inBoard = InBoard;
            cube = new ICell[inBoard.sizeOfBoard];
            Initialize(cubeNumbers, numOfCube);
        } 
        private void Initialize(String numbersInBoard, int numOfCube)
        {
            for (int indexInCube = 0; indexInCube < cube.Length; indexInCube++)
            {
                int index = indexInBoard(indexInCube, numOfCube);

                if (numbersInBoard[index] == '0')
                    cube[indexInCube] = new UnsolvedCell(index);
                else
                    cube[indexInCube] = new SolvedCell(inBoard.placesOfNumbers,numbersInBoard[index], index);
            }
        }
        private int indexInBoard(int indexInCube, int numOfCube)
        {
            int sqrtOfBoardSize = inBoard.SqrtOfSizeOfBoard;
            return (numOfCube / sqrtOfBoardSize) * inBoard.sizeOfBoard * sqrtOfBoardSize
                         + indexInCube % sqrtOfBoardSize + inBoard.sizeOfBoard * (indexInCube / sqrtOfBoardSize)
                         + (numOfCube % sqrtOfBoardSize) * sqrtOfBoardSize;
        }
        public void print(int rowInCube)
        {
            for (int colInCube = 0; colInCube < inBoard.SqrtOfSizeOfBoard; colInCube++)
            {
                if (cube[rowInCube * inBoard.SqrtOfSizeOfBoard + colInCube].GetType() == typeof(SolvedCell))
                {
                    System.Console.Write("   {0}   ", ((SolvedCell)cube[rowInCube * inBoard.SqrtOfSizeOfBoard + colInCube]).number);
                    if (colInCube == inBoard.SqrtOfSizeOfBoard - 1)
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    System.Console.Write("|");
                }
                else
                {
                    if (colInCube == inBoard.SqrtOfSizeOfBoard - 1)
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
                    if (!checkTheOptions || inBoard.isPossibleIndexToNumber(cube[indexInCube].getIndex(), indexInCube, number))
                    {
                        ((UnsolvedCell)cube[indexInCube]).optionalNumbers.Add(number);
                        countOfOptionsInCube++;
                    }
                }
            }
            return countOfOptionsInCube;
        }
        private bool isNumberInRowOrColInCubeFor(int addToIndexInCube,int indexInCube, int endOfColOrRow, int number)
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
        public bool isNumberInRowOrColInCube(bool col, int colOrRowIndex, int number)
        {
            int[] forParams = StaticMethods.forParameters(colOrRowIndex, col, inBoard.SqrtOfSizeOfBoard);
            Dictionary<int, List<int>> numbersInRowsOrCols;
            if (col)
                numbersInRowsOrCols = NumbersNotInBoard.numbersInCols;
            else
                numbersInRowsOrCols = NumbersNotInBoard.numbersInRows;
            if (isNumberInRowOrColInCubeFor(forParams[0], forParams[1], forParams[2], number))
                return true;
            else
                return isNumberInRowOrColInNumbersNotInBoard(numbersInRowsOrCols, colOrRowIndex, number);
        }
        public bool isRowOrColFull(bool col, int colOrRowIndex)
        {
            int[] forParams = StaticMethods.forParameters(colOrRowIndex, col, inBoard.SqrtOfSizeOfBoard);
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
            {
                if (cube[forParams[1]].GetType() == typeof(UnsolvedCell))
                    return false;
                    
            }
            return true;
        }
    }
}

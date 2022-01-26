using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Cube : ISudokuCube
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
                    cube[indexInCube] = new Cell(index);
                else
                    cube[indexInCube] = new Cell(inBoard.placesOfNumbers, numbersInBoard[index], index);
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
                bool withDarkBlue = colInCube == inBoard.SqrtOfSizeOfBoard - 1;
                if (cube[rowInCube * inBoard.SqrtOfSizeOfBoard + colInCube].isSolved())
                {
                    System.Console.Write("   {0}   ", cube[rowInCube * inBoard.SqrtOfSizeOfBoard + colInCube].number);
                    StaticMethods.printAColOrCol(true, withDarkBlue, false); 
                }
                else
                    StaticMethods.printAColOrCol(true, withDarkBlue, true);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        public List<int> fillOptionsInCube(int number,bool checkTheOptions)
        {
            List<int> optionsInCubeByBoardIndex = new List<int>();
            for (int indexInCube = 0; indexInCube < cube.Length; indexInCube++)
            {
                if (!cube[indexInCube].isSolved())
                    if (!checkTheOptions || inBoard.isPossibleIndexToNumber(cube[indexInCube].index, indexInCube, number))
                    {
                        cube[indexInCube].optionalNumbers.Add(number);
                        optionsInCubeByBoardIndex.Add(cube[indexInCube].index);
                    }
            }
            return optionsInCubeByBoardIndex;
        }
        private bool isNumberInRowOrColInCubeFor(int addToIndexInCube,int indexInCube, int endOfColOrRow, int number)
        {
            for (; indexInCube < endOfColOrRow; indexInCube += addToIndexInCube)
            {
                if (cube[indexInCube].isSolved())
                    if (cube[indexInCube].number == number)
                        return true;
            }
            return false;
        }
        public bool isNumberInRowOrColInCube(bool col, int colOrRowIndex, int number)
        {
            int[] forParams = StaticMethods.forParameters(colOrRowIndex, col, inBoard.SqrtOfSizeOfBoard);
            if (isNumberInRowOrColInCubeFor(forParams[0], forParams[1], forParams[2], number))
                return true;
            return false;
        }
        public bool isRowOrColFull(bool col, int colOrRowIndex)
        {
            int[] forParams = StaticMethods.forParameters(colOrRowIndex, col, inBoard.SqrtOfSizeOfBoard);
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
                if (!cube[forParams[1]].isSolved())
                    return false;
            return true;
        }
        private void isNumberHasOnePlaceInCube(int number)
        {
            List<int> countNumberInCube = new List<int>();
            for (int indexInCube = 0; indexInCube < inBoard.sizeOfBoard; indexInCube++)
                if (!cube[indexInCube].isSolved())
                {
                    if (cube[indexInCube].optionalNumbers.Contains(number))
                        countNumberInCube.Add(indexInCube);
                }
                else
                    if (cube[indexInCube].number == number)
                        return;
            if (countNumberInCube.Count == 1)
                putTheNumberAndDeletOptions(countNumberInCube.ElementAt(0), number);               
        }
        public void deleteNumberFromRowOrColInCube(bool col, int colOrRowIndexInCube, int number)
        {
            int[] forParams = StaticMethods.forParameters(colOrRowIndexInCube, col, inBoard.SqrtOfSizeOfBoard);
            bool removeNumberFromCube = false;
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
                if (!cube[forParams[1]].isSolved())
                    if (cube[forParams[1]].optionalNumbers.Remove(number))
                        removeNumberFromCube = true;
            if (removeNumberFromCube)
                isNumberHasOnePlaceInCube(number);
        }
        public void putTheNumberAndDeletOptions(int indexInCube, int knownNumber)
        {
            List<int> optionalNumbersOfPlace = cube[indexInCube].optionalNumbers;
            cube[indexInCube].solvedTheCell(knownNumber);
            optionalNumbersOfPlace.Remove(knownNumber);
            foreach (int number in optionalNumbersOfPlace)
                isNumberHasOnePlaceInCube(number);

            inBoard.deleteNumberFromRowOrCol(true,cube[indexInCube].index, indexInCube, knownNumber);
            inBoard.deleteNumberFromRowOrCol(false, cube[indexInCube].index, indexInCube, knownNumber);
        }
        public void rowOrColIntegrity(bool col,int rowOrColInCube, int rowOrColOfCube, List<int> CountNumberInRowAndCol)
        {
            int[] forParams = StaticMethods.forParameters(rowOrColInCube, col, inBoard.SqrtOfSizeOfBoard);
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
                if (cube[forParams[1]].isSolved())
                {
                    int numberInCell = cube[forParams[1]].number;
                    if (CountNumberInRowAndCol.Contains(numberInCell))
                    {
                        string rowOrCol;
                        int rowOrColInBoard = rowOrColOfCube * inBoard.SqrtOfSizeOfBoard + rowOrColInCube;
                        if (col)
                            rowOrCol = "col";
                        else
                            rowOrCol = "row";
                        throw new SameNumberInARowOrColException("The number " + numberInCell + " appears more than once in the " + rowOrCol + " "+ rowOrColInBoard);
                    }                       
                    CountNumberInRowAndCol.Add(numberInCell);                 
                }             
        }

        public void cubeIntegrity(List<int> CountNumberInRowAndCol,int cubeNumber) {
            for (int indexInCube = 0; indexInCube < inBoard.sizeOfBoard; indexInCube++)
                if (cube[indexInCube].isSolved())
                {
                    int numberInCell = cube[indexInCube].number;
                    if (CountNumberInRowAndCol.Contains(numberInCell))
                        throw new SameNumberInACubeException("The number " + numberInCell + " appears more than once in the cube "+ cubeNumber);
                    CountNumberInRowAndCol.Add(numberInCell);
                }
        }
        public int countHowManySolvedCells()
        {
            int count = 0;
            for (int indexInCube = 0; indexInCube < inBoard.sizeOfBoard; indexInCube++)
            {
                if (cube[indexInCube].isSolved())
                    count++;
            }
            return count;
        }
    }
}

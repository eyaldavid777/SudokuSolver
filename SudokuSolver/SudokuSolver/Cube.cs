using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Cube : ISudokuCube
    {
        public ICell[] cube { get; set; }

        public ISudokuBoard inBoard { get; }
        public int cubeNumber { get; }
        public Cube(String cubeNumbers, int numOfCube, ISudokuBoard InBoard)
        {
            inBoard = InBoard;
            cubeNumber = numOfCube;
            cube = new ICell[inBoard.sizeOfBoard];
            Initialize(cubeNumbers);
        } 
        private void Initialize(String numbersInBoard)
        {
            for (int indexInCube = 0; indexInCube < cube.Length; indexInCube++)
            {
                int index = indexInBoard(indexInCube);
                if (numbersInBoard[index] == '0')
                    cube[indexInCube] = new Cell(index);
                else
                    cube[indexInCube] = new Cell(inBoard.placesOfNumbers, numbersInBoard[index], index);
            }
        }
        private int indexInBoard(int indexInCube)
        {
            int sqrtOfBoardSize = inBoard.SqrtOfSizeOfBoard;
            return (cubeNumber / sqrtOfBoardSize) * inBoard.sizeOfBoard * sqrtOfBoardSize
                         + indexInCube % sqrtOfBoardSize + inBoard.sizeOfBoard * (indexInCube / sqrtOfBoardSize)
                         + (cubeNumber % sqrtOfBoardSize) * sqrtOfBoardSize;
        }
        public void print(int rowInCube)
        {
            for (int colInCube = 0; colInCube < inBoard.SqrtOfSizeOfBoard; colInCube++)
            {
                bool withDarkBlue = colInCube == inBoard.SqrtOfSizeOfBoard - 1;
                if (cube[rowInCube * inBoard.SqrtOfSizeOfBoard + colInCube].isSolved())
                {
                    char number = (char)(cube[rowInCube * inBoard.SqrtOfSizeOfBoard + colInCube].number + '0');
                    System.Console.Write("   {0}   ", number);
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
        public bool isRowOrColFull(bool col, int colOrRowIndexInCube)
        {
            int[] forParams = StaticMethods.forParameters(colOrRowIndexInCube, col, inBoard.SqrtOfSizeOfBoard);
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
                putTheNumberAndDeletOptions(countNumberInCube.ElementAt(0), number,true,true);
            if (countNumberInCube.Count == 0)
            {
                // in the cube cubeNumber you can't put the number "number" (exception)
            }
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
        public void deleteNumberFromCube(int number, Dictionary<int, List<int>> optionsOfMissingNumbersInRowOrCol)
        {
            // this function accurs only after each empty cell in the board contains all its options 
            for (int indexInCube=0; indexInCube< inBoard.sizeOfBoard; indexInCube++)
            {
                if (!cube[indexInCube].isSolved())
                {
                    if (cube[indexInCube].optionalNumbers.Remove(number))
                    {
                        if (cube[indexInCube].optionalNumbers.Count == 1)
                        {
                            int knownNumber = cube[indexInCube].optionalNumbers.ElementAt(0);
                            putTheNumberAndDeletOptions(indexInCube, knownNumber,true, true);
                            deleteNumberFromCube(knownNumber, optionsOfMissingNumbersInRowOrCol);
                            // remove from optionsOfMissingNumbersInRowOrCol the knownNumber that you found
                            optionsOfMissingNumbersInRowOrCol.Remove(knownNumber);
                        }
                        else { 
                            if (cube[indexInCube].optionalNumbers.Count == 0) 
                            {
                                // in cell indexInBoard(indexInCube) you cant put anything
                            }
                        }
                    }
                }
            }
        }
        public void putTheNumberAndDeletOptions(int indexInCube, int knownNumber,bool deletFromRow, bool deletFromCol)
        {
            List<int> optionalNumbersOfPlace = cube[indexInCube].optionalNumbers;
            cube[indexInCube].solvedTheCell(knownNumber);
            optionalNumbersOfPlace.Remove(knownNumber);
            foreach (int number in optionalNumbersOfPlace)
                isNumberHasOnePlaceInCube(number);

            if(deletFromCol)
                inBoard.deleteNumberFromRowOrCol(true,cube[indexInCube].index, indexInCube, knownNumber);
            if (deletFromRow)
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

        public void cubeIntegrity(List<int> CountNumberInRowAndCol) {
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

        public void checksOptionsOfARowOrAColInCube(bool col, int rowOrColInCube,Dictionary<int, List<int>> optionsOfMissingNumbersInRowOrCol)
        {
            int[] forParams = StaticMethods.forParameters(rowOrColInCube, col, inBoard.SqrtOfSizeOfBoard);
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
            {
                if (cube[forParams[1]].isSolved())
                {
                    // if the cell is solved remove the number in it from optionsOfMissingNumbersInRowOrCol
                    // because the row or col contains it (the number) so we dont need to look at this place optional numbers
                    optionsOfMissingNumbersInRowOrCol.Remove(cube[forParams[1]].number);
                }
                else
                {
                    int countOfOptionalNumbers = cube[forParams[1]].optionalNumbers.Count;
                    switch (countOfOptionalNumbers)
                    {
                        case 0:
                            // in the cell at index indexInBoard(forParams[1], CubeNumber) cant contain a valid number (exception)
                            break;
                        case 1:
                            int knownNumber = cube[forParams[1]].optionalNumbers[0];
                            putTheNumberAndDeletOptions(forParams[1], knownNumber,true,true);
                            deleteNumberFromCube(knownNumber, optionsOfMissingNumbersInRowOrCol);
                            // remove from optionsOfMissingNumbersInRowOrCol the knownNumber that you found
                            optionsOfMissingNumbersInRowOrCol.Remove(knownNumber);
                            break;
                        default:
                            // copy all the optional numbers to the dictionary optionsOfMissingNumbersInRowOrCol
                            // the key is an optional number and the value is the indexes list of where the optional number is located
                            for (int indexOfAnOption = 0; indexOfAnOption < countOfOptionalNumbers; indexOfAnOption++)
                                optionsOfMissingNumbersInRowOrCol[cube[forParams[1]].optionalNumbers.ElementAt(indexOfAnOption)].Add(cube[forParams[1]].index);
                            break;
                    }
                }
            }
        }


        public List<int> getOptionalNumbers(int indexInCube)
        {
            return cube[indexInCube].optionalNumbers;
        }
    }
}

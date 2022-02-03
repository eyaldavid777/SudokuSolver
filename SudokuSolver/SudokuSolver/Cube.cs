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
                    System.Console.Write(" {0} ", number);
                    StaticMethods.printAColOrCol(true, withDarkBlue, false);
                }
                else
                    StaticMethods.printAColOrCol(true, withDarkBlue, true);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void initializePlacesOfNumbersFromCube()
        {
            // The dictionary placesOfNumbers contains after this for below 
            // keys of optional numbers in the cube (cubeNumber) and for each optional 
            // number it contains an indexes list of where the optional number can be located
            for(int indexInCube =0; indexInCube < inBoard.sizeOfBoard; indexInCube++)
            {
                if (cube[indexInCube].isSolved())
                {
                    // if the cell is solved remove the number in it from inBoard.placesOfNumbers
                    // because the cube contains it (the number) so we dont need to look at this place optional numbers
                    inBoard.placesOfNumbers.Remove(cube[indexInCube].number);
                }
                else
                {
                    copyOptionsInIndex(indexInCube);
                }
            }
        }
        public List<int> fillOptionsInCube(int number, bool checkTheOptions)
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

        private bool isNumberInRowOrColInCubeFor(int addToIndexInCube, int indexInCube, int endOfColOrRow, int number)
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
                        countNumberInCube.Add(indexInBoard(indexInCube));
                }
                else
                    if (cube[indexInCube].number == number)
                        return;
            inBoard.checkNumberOfOptions(countNumberInCube, number);
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

        private bool checkCountOfOptionsInIndex(int indexInCube)
        {
            bool foundNakedSingle = false;
            int countOfOptionalNumbers = cube[indexInCube].optionalNumbers.Count;
            if (countOfOptionalNumbers == 0)
            {
                // the cell at index indexInBoard(forParams[1]) cant contain a valid number (exception)
            }
            if (countOfOptionalNumbers == 1)
            {
                foundNakedSingle = true;
                // knownNumber = the only option in the in the cell
                int knownNumber = cube[indexInCube].optionalNumbers[0];
                putTheNumberAndDeletOptions(indexInCube, knownNumber, true, true);
                deleteNumberFromCube(knownNumber,new List<int>());
            }
            return foundNakedSingle;
        }

        public bool nakedSingleOfACube()
        {
            bool theBoardHasChanged = false;
            for (int indexInCube = 0; indexInCube < inBoard.sizeOfBoard; indexInCube++)
            {
                if(!cube[indexInCube].isSolved())
                    if (checkCountOfOptionsInIndex(indexInCube))
                        theBoardHasChanged = true;
            }
            return theBoardHasChanged;
        }

        public void deleteNumberFromCube(int number, List<int> PlacesNotToDelete)
        {
            //deleteNumberFromCube(MissingNumberInDic, inBoard.placesOfNumbers,col, rowOrColInCube)
            // this function accurs only after each empty cell in the board contains all its options 
            for (int indexInCube = 0; indexInCube < inBoard.sizeOfBoard; indexInCube++)
                if (!cube[indexInCube].isSolved())
                    if (!PlacesNotToDelete.Contains(indexInCube))
                        cube[indexInCube].optionalNumbers.Remove(number);
        }

        public void putTheNumberAndDeletOptions(int indexInCube, int knownNumber, bool deletFromRow, bool deletFromCol)
        {
            List<int> optionalNumbersOfPlace = cube[indexInCube].optionalNumbers;
            cube[indexInCube].solvedTheCell(knownNumber);
            optionalNumbersOfPlace.Remove(knownNumber);

            foreach (int number in optionalNumbersOfPlace)
                isNumberHasOnePlaceInCube(number);

            if (deletFromCol)
                inBoard.deleteNumberFromRowOrCol(true, cube[indexInCube].index, indexInCube, knownNumber);
            if (deletFromRow)
                inBoard.deleteNumberFromRowOrCol(false, cube[indexInCube].index, indexInCube, knownNumber);
        }

        public void rowOrColIntegrity(bool col, int rowOrColInCube, int rowOrColOfCube, List<int> CountNumberInRowAndCol)
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
                        throw new SameNumberInARowOrColException("The number " + numberInCell + " appears more than once in the " + rowOrCol + " " + rowOrColInBoard);
                    }
                    CountNumberInRowAndCol.Add(numberInCell);
                }
        }

        public void cubeIntegrity(List<int> CountNumberInRowAndCol)
        {
            for (int indexInCube = 0; indexInCube < inBoard.sizeOfBoard; indexInCube++)
                if (cube[indexInCube].isSolved())
                {
                    int numberInCell = cube[indexInCube].number;
                    if (CountNumberInRowAndCol.Contains(numberInCell))
                        throw new SameNumberInACubeException("The number " + numberInCell + " appears more than once in the cube " + cubeNumber);
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

        private void copyOptionsInIndex(int indexInCube)
        {
            int countOfOptionalNumbers = cube[indexInCube].optionalNumbers.Count;
            // copy all the optional numbers to the dictionary inBoard.placesOfNumbers
            // the key is an optional number and the value is the indexes list of where the optional number is located
            for (int indexOfAnOption = 0; indexOfAnOption < countOfOptionalNumbers; indexOfAnOption++)
                inBoard.placesOfNumbers[cube[indexInCube].optionalNumbers.ElementAt(indexOfAnOption)].Add(cube[indexInCube].index);
        }

        public void checksOptionsOfARowOrAColInCube(bool col, int rowOrColInCube)
        {
            int[] forParams = StaticMethods.forParameters(rowOrColInCube, col, inBoard.SqrtOfSizeOfBoard);
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
            {
                if (cube[forParams[1]].isSolved())
                {
                    // if the cell is solved remove the number in it from inBoard.placesOfNumbers
                    // because the row or col contains it (the number) so we dont need to look at this place optional numbers
                    inBoard.placesOfNumbers.Remove(cube[forParams[1]].number);
                }
                else
                    copyOptionsInIndex(forParams[1]);
            }
        }

        public List<int> getOptionalNumbers(int indexInCube)
        {
            return cube[indexInCube].optionalNumbers;
        }

        public bool leaveOnlyThePairNumbers(int indexInCube, int firstPairNumbers, int secondPairNumbers)
        {
            if (cube[indexInCube].optionalNumbers.Count == 2)
                return false;
            cube[indexInCube].optionalNumbers.Clear();
            cube[indexInCube].optionalNumbers.Add(firstPairNumbers);
            cube[indexInCube].optionalNumbers.Add(secondPairNumbers);
            return true;
        }

        public bool leaveInCellOnlyTheListOptions(int indexInCube,List<int> remainingOptions)
        {
            if (remainingOptions.Count == 1)
            {
                cube[indexInCube].optionalNumbers.Clear();
                cube[indexInCube].optionalNumbers.Add(remainingOptions.ElementAt(0));
                return true;
            }
            if (cube[indexInCube].optionalNumbers.Count == 2)
            {
                //the cell already has only two options so you dont need to change it
                return false;
            }
            cube[indexInCube].optionalNumbers.Clear();
            cube[indexInCube].optionalNumbers.Add(remainingOptions.ElementAt(0));
            cube[indexInCube].optionalNumbers.Add(remainingOptions.ElementAt(1));
            return true;
        }
    }
}

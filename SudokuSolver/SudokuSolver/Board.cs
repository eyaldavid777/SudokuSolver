using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public enum Type { row, col, cube }
    class Board : ISudokuBoard
    {

        public ISudokuCube[] board { get;  }
        public Dictionary<int, List<int>> placesOfNumbers { get; set; }
        public int sizeOfBoard { get; }
        public int SqrtOfSizeOfBoard { get; }
        public KnownNumbersNotInBoard knownNumbersNotInBoard { get; set; }
        public int step { get; set; }

        public delegate bool Ptr();

        public Board(String numbersInBoard)
        {
            SizeOfBoardIntegrity(numbersInBoard);
            sizeOfBoard = (int)Math.Sqrt(numbersInBoard.Length);
            SqrtOfSizeOfBoard = (int)Math.Sqrt(sizeOfBoard);
            board = new Cube[sizeOfBoard];
            Initialize(numbersInBoard, 0);
            BoardIntegrity();
        }
        public void SizeOfBoardIntegrity(string numbersInBoard)
        {
            double doubleSizeOfBoard = Math.Sqrt(numbersInBoard.Length);
            if (!StaticMethods.isInt(Math.Sqrt(doubleSizeOfBoard)))
                throw new InvalidBoardSizeException();
        }
        private void Initialize(String numbersInBoard, int index)
        {
            step = 1;
            placesOfNumbers = new Dictionary<int, List<int>>();
            InitializePlacesOfNumbers();
            knownNumbersNotInBoard = new KnownNumbersNotInBoard();
            for (int numOfCube = 0; numOfCube < board.Length; numOfCube++)
            {
                board[numOfCube] = new Cube(numbersInBoard, numOfCube, this);
            }
        }
        public void InitializePlacesOfNumbers()
        {
            for (int numOfCube = 0; numOfCube < sizeOfBoard; numOfCube++)
            {
                if (placesOfNumbers.ContainsKey(numOfCube + 1))
                    placesOfNumbers[numOfCube + 1] = new List<int>();
                else
                    placesOfNumbers.Add(numOfCube + 1, new List<int>());
            }
        }
        public void initializePlacesOfNumbersFromRowOrCol(bool col, int rowOrColInBoard)
        {
            // The dictionary placesOfNumbers contains after this for below 
            // keys of optional numbers in the row or col (rowOrColInBoard) and for each optional 
            // number it contains an indexes list of where the optional number can be located
            int[] forParams = StaticMethods.forParameters(rowOrColInBoard / SqrtOfSizeOfBoard, col, SqrtOfSizeOfBoard);
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
                board[forParams[1]].checksOptionsOfARowOrAColInCube(col, rowOrColInBoard % SqrtOfSizeOfBoard);
        }

        private void printALine()
        {
            System.Console.Write("   ");
            for (int i = 0; i < sizeOfBoard; i++)
                StaticMethods.printAColOrCol(false, true, true);
            System.Console.WriteLine();
        }
        private void printCols(bool Line, bool DarkBlue)
        {
            for (int i = 0; i < sizeOfBoard + 1; i++)
                if (i % SqrtOfSizeOfBoard == 0)
                    if (i == 0)
                        StaticMethods.printAColOrCol(true, true, true);
                    else
                    {
                        if (Line)
                        {
                            StaticMethods.printAColOrCol(false, DarkBlue, false);
                            StaticMethods.printAColOrCol(true, true, false);
                        }
                        else
                            StaticMethods.printAColOrCol(true, true, true);
                    }
                else
                    if (Line)
                {
                    StaticMethods.printAColOrCol(false, DarkBlue, false);
                    StaticMethods.printAColOrCol(true, false, false);
                }
                else
                    StaticMethods.printAColOrCol(true, false, true);
            System.Console.WriteLine();
        }
        public void print()
        {
            printALine();
            for (int rowOfCubes = 0; rowOfCubes < SqrtOfSizeOfBoard; rowOfCubes++)
            {
                for (int rowInCube = 0; rowInCube < SqrtOfSizeOfBoard; rowInCube++)
                {
                    printCols(false, false);
                    StaticMethods.printAColOrCol(true, true, true);
                    for (int colOfCubes = 0; colOfCubes < SqrtOfSizeOfBoard; colOfCubes++)
                    {
                        board[rowOfCubes * SqrtOfSizeOfBoard + colOfCubes].print(rowInCube);
                    }
                    System.Console.WriteLine();
                    if ((rowInCube + 1) % SqrtOfSizeOfBoard == 0)
                        printCols(true, true);
                    else
                        printCols(true, false);
                }
            }
            Console.WriteLine("\n");
        }       
        public bool isTheCubeWorthChecking(int cubeNumber, int mostCommonNumber)
        {
            foreach (int indexInBoard in placesOfNumbers[mostCommonNumber])
                if (Calculations.getCubeNumberByIndex(indexInBoard,sizeOfBoard) / SqrtOfSizeOfBoard == cubeNumber / SqrtOfSizeOfBoard)
                {
                    if (!board[cubeNumber].isRowOrColFull(false, Calculations.getRowOrColInCubeByIndexInBoard(false,indexInBoard,sizeOfBoard)))
                        return true;
                }
                else
                    if (Calculations.getCubeNumberByIndex(indexInBoard, sizeOfBoard) % SqrtOfSizeOfBoard == cubeNumber % SqrtOfSizeOfBoard)
                        if (!board[cubeNumber].isRowOrColFull(true, Calculations.getRowOrColInCubeByIndexInBoard(true,indexInBoard, sizeOfBoard)))
                            return true;
            return false;
        }

        private bool isNumberInRowOrColOfCubes(int cubeNumber, bool col, int colOrRowIndexInCube, int[] forParams, int number)
        {
            int colOrRowIndexInBoard = Calculations.getRowOrCol(col, cubeNumber, SqrtOfSizeOfBoard) * SqrtOfSizeOfBoard + colOrRowIndexInCube;
            if (knownNumbersNotInBoard.isNumberInRowOrColInNumbersNotInBoard(col, colOrRowIndexInBoard, number))
                return true;
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
                if (forParams[1] != cubeNumber)
                    if (board[forParams[1]].isNumberInRowOrColInCube(col, colOrRowIndexInCube, number))
                        return true;
            return false;
        }
        public bool isPossibleIndexToNumber(int indexOfNumberInBoard, int indexOfNumberInCube, int number)
        {
            int cubeNumber = Calculations.getCubeNumberByIndex(indexOfNumberInBoard,sizeOfBoard);
            int rowOfCubeNumber = cubeNumber / SqrtOfSizeOfBoard;
            int colOfCubeNumber = cubeNumber % SqrtOfSizeOfBoard;
            int[] forParams = StaticMethods.forParameters(rowOfCubeNumber, false, SqrtOfSizeOfBoard);

            if (isNumberInRowOrColOfCubes(cubeNumber, false, indexOfNumberInCube / SqrtOfSizeOfBoard, forParams, number))
                return false;

            forParams = StaticMethods.forParameters(colOfCubeNumber, true, SqrtOfSizeOfBoard);

            if (isNumberInRowOrColOfCubes(cubeNumber, true, indexOfNumberInCube % SqrtOfSizeOfBoard, forParams, number))
                return false;
            return true;
        }
        public void deleteNumberFromRowOrCol(bool col, int indexOfNumberInBoard, int indexOfNumberInCube, int number)
        {
            int cubeNumber =Calculations.getCubeNumberByIndex(indexOfNumberInBoard,sizeOfBoard);
            int rowOrColOfCube = Calculations.getRowOrCol(col, cubeNumber, SqrtOfSizeOfBoard);
            int rowOrColInCube = Calculations.getRowOrCol(col, indexOfNumberInCube, SqrtOfSizeOfBoard);
            int[] forParams = StaticMethods.forParameters(rowOrColOfCube, col, SqrtOfSizeOfBoard);

            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
                if (forParams[1] != cubeNumber)
                    board[forParams[1]].deleteNumberFromRowOrColInCube(col, rowOrColInCube, number);
        }
        private void putKnownNumberAndDeletOptions(int indexInBoard, int mostCommonNumber, bool deletFromRow, bool deletFromCol)
        {
            int indexInCube = Calculations.getIndexInCubeByIndexInBoard(indexInBoard,sizeOfBoard);
            board[Calculations.getCubeNumberByIndex(indexInBoard, sizeOfBoard)].putTheNumberAndDeletOptions(indexInCube, mostCommonNumber, deletFromRow, deletFromCol);
            // only if we are in the "firstStepOfSolving", there is a chance that mostCommonNumber
            // will already be removed from placesOfNumbers so we need to check if placesOfNumbers contains it
            if (step == 1 && placesOfNumbers.ContainsKey(mostCommonNumber))
            {
                // add to mostCommonNumber in placesOfNumbers his new place
                placesOfNumbers[mostCommonNumber].Add(indexInBoard);
            }

            //זה פותר את הלוח ככה שזה מוחק את האופציה של מוסט קומון ואז פלאיסיס נאל
        }
   
        private bool isNumberHasOnePlaceInRowOrCol(bool col, int MissingNumberInDic, int rowOrColInCube) // hidden single
        {
            // if MissingNumberInDic has only one place in rowOrColInBoard then
            if (placesOfNumbers[MissingNumberInDic].Count == 1)
            {
                // indexInBoard = only option of MissingNumberInDic in rowOrColInBoard
                int indexInBoard = placesOfNumbers[MissingNumberInDic].ElementAt(0);
                int indexInCube = Calculations.getIndexInCubeByIndexInBoard(indexInBoard, sizeOfBoard);
                int indexOfCube = Calculations.getCubeNumberByIndex(indexInBoard, sizeOfBoard);

                if (board[indexOfCube].getOptionalNumbers(indexInCube).Count == 1)
                    return false;

                List<int> remainingOptions = new List<int>();
                remainingOptions.Add(MissingNumberInDic);
                board[indexOfCube].leaveInCellOnlyTheListOptions(indexInCube, remainingOptions);
                return true;


                //// optionalNumbersOfPlace = save the options at index 'indexInBoard'
                //List<int> optionalNumbersOfPlace = board[indexOfCube].getOptionalNumbers(indexInCube);
                //// remove from optionalNumbersOfPlace the MissingNumberInDic that you found
                //optionalNumbersOfPlace.Remove(MissingNumberInDic);
                //putKnownNumberAndDeletOptions(indexInBoard, MissingNumberInDic, !col, col);
                //// delete all the options of MissingNumberInDic from the cube we found it at
                //board[indexOfCube].deleteNumberFromCube(MissingNumberInDic, new List<int>());
                //// remove from placesOfNumbers the MissingNumberInDic that you found
                //placesOfNumbers.Remove(MissingNumberInDic);

                //InitializePlacesOfNumbers();
                //initializePlacesOfNumbersFromRowOrCol(col, Calculations.getRowOrCol(col, indexInBoard, sizeOfBoard));

                //foreach (int optionalNumber in optionalNumbersOfPlace)
                //{
                //    if (!placesOfNumbers.ContainsKey(optionalNumber))
                //        continue;
                //    placesOfNumbers[optionalNumber].Remove(indexInBoard);
                //    if (placesOfNumbers[optionalNumber].Count == 0)
                //    {
                //        // the row/col getRowOrCol(col,indexInBoard,SqrtOfSizeOfBoard) can not contains
                //        // both of the numbers optionalNumber,MissingNumberInDic because the only place
                //        // they can be in the row/col is in the same cell indexInBoard
                //    }
                //    else
                //    {
                //        isNumberHasOnePlaceInRowOrCol(col, optionalNumber, rowOrColInCube);
                //    }
                //}
                //return true;
            }
            else
            {
                if (placesOfNumbers[MissingNumberInDic].Count == 0)
                {
                    // the row or col rowOrColInBoard can not conatins the number MissingNumberInDic
                    
                }
            }
            return false;
        }
   
        public void checkNumberOfOptions(List<int> optionsInCubeByBoardIndex, int mostCommonNumber)
        {
            switch (optionsInCubeByBoardIndex.Count)
            {
                case 0:
                    // in the cube indexOfCube you can't put the number mostCommonNumber (exception)
                    break;
                case 1:
                    // put the number in it's only place and delete the options of the number
                    // in the cells with the same row and col
                    putKnownNumberAndDeletOptions(optionsInCubeByBoardIndex.ElementAt(0), mostCommonNumber, true, true);
                    break;
                default:
                    // if the options of 'mostCommonNumber' in the same row or col you can
                    //  delete mostCommonNumber's options from the rest of the row or col
                    checkIfOptionsInTheSameRowOrCol(optionsInCubeByBoardIndex, mostCommonNumber);
                    break;
            }
        }
        private void checkIfOptionsInTheSameRowOrCol(List<int> optionsInCubeByBoardIndex, int mostCommonNumber)
        {
            if (optionsInCubeByBoardIndex.Count <= SqrtOfSizeOfBoard)
                switch (isOptionsInTheSameRowOrCol(optionsInCubeByBoardIndex))
                {
                    case -1: // the options dont have the same row or col
                        break;
                    case 1:  // the options have the same row
                        theOptionsInTheSameRowOrCol(false, optionsInCubeByBoardIndex, mostCommonNumber, optionsInCubeByBoardIndex.ElementAt(0) / sizeOfBoard);
                        break;
                    case 2: // the options have the same col
                        theOptionsInTheSameRowOrCol(true, optionsInCubeByBoardIndex, mostCommonNumber, optionsInCubeByBoardIndex.ElementAt(0) % sizeOfBoard);
                        break;
                }
        }

        private int isOptionsInTheSameRowOrCol(List<int> numberOfOptionsInCube)
        {
            // comparing all the rows and cols to the row and col of the first option
            int rowOfFirst = numberOfOptionsInCube.ElementAt(0) / SqrtOfSizeOfBoard;
            int colOfFirst = numberOfOptionsInCube.ElementAt(0) % SqrtOfSizeOfBoard;
            bool sameRow = true;
            bool sameCol = true;
            for (int i = 1; i < numberOfOptionsInCube.Count; i++)
            {
                if (sameRow && numberOfOptionsInCube.ElementAt(i) / SqrtOfSizeOfBoard != rowOfFirst)
                {
                    // There is at least one option with a diffrent row
                    sameRow = false;
                }
                if (sameCol && numberOfOptionsInCube.ElementAt(i) % SqrtOfSizeOfBoard != colOfFirst)
                {
                    // There is at least one option with a diffrent col
                    sameCol = false;
                }
                if (!sameRow && !sameCol)
                {
                    // There is at least one option with a diffrent row
                    // and at least one option with a diffrent col
                    return -1;
                }
            }
            return sameRow ? 1 : 2;
        }
        private void theOptionsInTheSameRowOrCol(bool col, List<int> optionsInCubeByBoardIndex, int mostCommonNumber, int theSameRowOrCol)
        {
            Dictionary<int, List<int>> numbersInRowOrCol;
            if (col)
                numbersInRowOrCol = knownNumbersNotInBoard.numbersInCols;
            else
                numbersInRowOrCol = knownNumbersNotInBoard.numbersInRows;

            if (!numbersInRowOrCol.ContainsKey(theSameRowOrCol))
            {
                numbersInRowOrCol.Add(theSameRowOrCol, new List<int>());
            }
            numbersInRowOrCol[theSameRowOrCol].Add(mostCommonNumber);
            int firstIndexInBoard = optionsInCubeByBoardIndex.ElementAt(0);
            int indexInCube = Calculations.getIndexInCubeByIndexInBoard(firstIndexInBoard, sizeOfBoard);
            deleteNumberFromRowOrCol(col, optionsInCubeByBoardIndex.ElementAt(0), indexInCube, mostCommonNumber);
        }

        public bool checkplacesOfNumbers(bool col, int rowOrColInCube)
        {
            bool theBoardHasChanged = false;
            for (int indexOfMissingNumberInDic = 0; indexOfMissingNumberInDic < placesOfNumbers.Keys.Count; indexOfMissingNumberInDic++)
            {
                int MissingNumberInDic = placesOfNumbers.Keys.ElementAt(indexOfMissingNumberInDic);
                if (isNumberHasOnePlaceInRowOrCol(col, MissingNumberInDic, rowOrColInCube))
                {
                    theBoardHasChanged = true;
                    indexOfMissingNumberInDic = -1;
                }
            }
            return theBoardHasChanged;
        }

        public bool repetition(Board.Ptr repetitionContent)
        {
            int numbersOfLopps = -1;
            do
            {
                numbersOfLopps++;
            } while (repetitionContent());
            return numbersOfLopps > 0;
        }     
        private void BoardIntegrityOfRowOrCol(bool col, List<int> CountNumberInRowAndCol, int RowOrColOfCube, int RowOrColInCube)
        {
            int[] forParams = StaticMethods.forParameters(RowOrColOfCube, col, SqrtOfSizeOfBoard);
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
            {
                board[forParams[1]].rowOrColIntegrity(col, RowOrColInCube, RowOrColOfCube, CountNumberInRowAndCol);
            }
            CountNumberInRowAndCol.Clear();
        }
        private void BoardIntegrityOfCube(List<int> CountNumberInCube, int cubeNumber)
        {
            board[cubeNumber].cubeIntegrity(CountNumberInCube);
            CountNumberInCube.Clear();
        }
        private void ScanCubesInBoard(bool checkRowsOrCols)
        {
            // checkRowsOrCols is true when we want to check integrity of 
            // rows or cols and false when we want to check integrity of cubes.
            List<int> CountNumberInRowAndColOrCube = new List<int>();
            for (int RowOrColOfCube = 0; RowOrColOfCube < SqrtOfSizeOfBoard; RowOrColOfCube++)
            {
                for (int RowOrColInCube = 0; RowOrColInCube < SqrtOfSizeOfBoard; RowOrColInCube++)
                {
                    if (checkRowsOrCols)
                    {
                        BoardIntegrityOfRowOrCol(true, CountNumberInRowAndColOrCube, RowOrColOfCube, RowOrColInCube);
                        BoardIntegrityOfRowOrCol(false, CountNumberInRowAndColOrCube, RowOrColOfCube, RowOrColInCube);
                    }
                    else
                        BoardIntegrityOfCube(CountNumberInRowAndColOrCube, RowOrColOfCube * SqrtOfSizeOfBoard + RowOrColInCube);
                }
            }
        }
        private void BoardIntegrity()
        {
            ScanCubesInBoard(true);
            ScanCubesInBoard(false);
        }
     


    }
}

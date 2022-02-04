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

        public ISudokuSolver sudokuSolver { get; set; }

        public delegate bool Ptr();

        public Board(string numbersInBoard, bool checkBoardIntegrity = true)
        {
            SizeOfBoardIntegrity(numbersInBoard);
            sizeOfBoard = Calculations.sqrt(numbersInBoard.Length);
            SqrtOfSizeOfBoard = Calculations.sqrt(sizeOfBoard);
            sudokuSolver = new Solver(this);
            board = new Cube[sizeOfBoard];
            Initialize(numbersInBoard, 0, checkBoardIntegrity);
            if (checkBoardIntegrity)
                BoardIntegrity();
        }


        public void SizeOfBoardIntegrity(string numbersInBoard)
        {
            double doubleSizeOfBoard = Math.Sqrt(numbersInBoard.Length);
            if (!StaticMethods.isInt(Math.Sqrt(doubleSizeOfBoard)))
                throw new InvalidBoardSizeException();
        }
        private void Initialize(string numbersInBoard, int index, bool checkBoardIntegrity)
        {
            step = 1;
            placesOfNumbers = new Dictionary<int, List<int>>();
            knownNumbersNotInBoard = new KnownNumbersNotInBoard();
            InitializePlacesOfNumbers();
            InitializeBorad(numbersInBoard, checkBoardIntegrity);
        }
        public void InitializeBorad(string numbersInBoard, bool checkBoardIntegrity)
        {
            for (int numOfCube = 0; numOfCube < board.Length; numOfCube++)
            {
                board[numOfCube] = new Cube(numbersInBoard, numOfCube, this , checkBoardIntegrity);
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
        public Board copyBoardWithNumber(int indexInBoard,char option)
        {          
            string numbersInBoard = boardString();

            StringBuilder temp = new StringBuilder(numbersInBoard);
            temp[indexInBoard] = option; 
            numbersInBoard = temp.ToString();

            Board cloneBoard = new Board(numbersInBoard,false);
            return cloneBoard;
        }
        public Board copyBoard()
        {
            string numbersInBoard = boardString();
            Board cloneBoard = new Board(numbersInBoard, false);
            return cloneBoard;
        }
        private string boardString()
        {
            string boardString = string.Empty;
            for (int rowIndex =0; rowIndex < sizeOfBoard; rowIndex++)
            {
                for(int cubeCol =0; cubeCol < SqrtOfSizeOfBoard; cubeCol++)
                {
                    int cubeNumber = rowIndex / SqrtOfSizeOfBoard * SqrtOfSizeOfBoard + cubeCol;
                    boardString += board[cubeNumber].rowOrCulIncubeString(false, rowIndex % SqrtOfSizeOfBoard);
                }
            }
            return boardString;
        }
        private void BoardIntegrity()
        {
            rowColCubeIntegrity(true);
            rowColCubeIntegrity(false);
        }

        private void rowColCubeIntegrity(bool checkRowsOrCols)
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
                        BoardIntegrityOfRowOrCol(Type.col, CountNumberInRowAndColOrCube, RowOrColOfCube, RowOrColInCube);
                        BoardIntegrityOfRowOrCol(Type.row, CountNumberInRowAndColOrCube, RowOrColOfCube, RowOrColInCube);
                    }
                    else
                        BoardIntegrityOfCube(CountNumberInRowAndColOrCube, RowOrColOfCube * SqrtOfSizeOfBoard + RowOrColInCube);
                }
            }
        }
        private void BoardIntegrityOfRowOrCol(Type type , List<int> CountNumberInRowAndCol, int RowOrColOfCube, int RowOrColInCube)
        {
            int[] forParams = StaticMethods.forParameters(RowOrColOfCube, type == Type.col, SqrtOfSizeOfBoard);
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
            {
                board[forParams[1]].rowOrColIntegrity(type, RowOrColInCube, RowOrColOfCube, CountNumberInRowAndCol);
            }
            CountNumberInRowAndCol.Clear();
        }
        private void BoardIntegrityOfCube(List<int> CountNumberInCube, int cubeNumber)
        {
            board[cubeNumber].cubeIntegrity(CountNumberInCube);
            CountNumberInCube.Clear();
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
                if (Calculations.getCubeNumberByIndex(indexInBoard, sizeOfBoard) / SqrtOfSizeOfBoard == cubeNumber / SqrtOfSizeOfBoard)
                {
                   if (!board[cubeNumber].isRowOrColFull(false, Calculations.getRowOrColInCubeByIndexInBoard(false,indexInBoard,sizeOfBoard)))
                        return true;
                }
                else
                {
                    if (Calculations.getCubeNumberByIndex(indexInBoard, sizeOfBoard) % SqrtOfSizeOfBoard == cubeNumber % SqrtOfSizeOfBoard)
                        if (!board[cubeNumber].isRowOrColFull(true, Calculations.getRowOrColInCubeByIndexInBoard(true,indexInBoard, sizeOfBoard)))
                            return true;
                }
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
        public void putKnownNumberAndDeletOptions(int indexInBoard, int mostCommonNumber, bool deletFromRow, bool deletFromCol)
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
        }  
        private bool isNumberHasOnePlaceInRowOrCol(Type type, int MissingNumberInDic, int rowOrColInBoard) // hidden single
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
            }
            else
            {
                if (placesOfNumbers[MissingNumberInDic].Count == 0)
                    throw new NoPlaceForANumberInARowOrColOrCubeException("the " + type + " at index " + rowOrColInBoard + " cannot conatin the number " + MissingNumberInDic);
            }
            return false;
        }
        public void hiddenSingleOfCube(List<int> optionsInCubeByBoardIndex, int mostCommonNumber)
        {
            if (optionsInCubeByBoardIndex.Count == 1)
            {
                    // put the number in it's only place and delete the options of the number
                    // in the cells with the same row and col
                    putKnownNumberAndDeletOptions(optionsInCubeByBoardIndex.ElementAt(0), mostCommonNumber, true, true);
            }
            else
            {
                // if the options of 'mostCommonNumber' in the same row or col you can
                //  delete mostCommonNumber's options from the rest of the row or col
                checkIfOptionsInTheSameRowOrCol(optionsInCubeByBoardIndex, mostCommonNumber);
            }
        }
        private void checkIfOptionsInTheSameRowOrCol(List<int> optionsInCubeByBoardIndex, int mostCommonNumber)
        {
            if (optionsInCubeByBoardIndex.Count <= SqrtOfSizeOfBoard && optionsInCubeByBoardIndex.Count > 0)
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
        public bool hiddenSingleOfRowAndCol(Type type, int rowOrColInBoard)
        {
            bool theBoardHasChanged = false;
            for (int indexOfMissingNumberInDic = 0; indexOfMissingNumberInDic < placesOfNumbers.Keys.Count; indexOfMissingNumberInDic++)
            {
                int MissingNumberInDic = placesOfNumbers.Keys.ElementAt(indexOfMissingNumberInDic);
                if (isNumberHasOnePlaceInRowOrCol(type, MissingNumberInDic, rowOrColInBoard))
                {
                    theBoardHasChanged = true;
                    indexOfMissingNumberInDic = -1;
                }
            }
            return theBoardHasChanged;
        }
        public bool repetition(Ptr repetitionContent)
        {
            int numbersOfLopps = -1;
            do
            {
                numbersOfLopps++;
            } while (repetitionContent());
            return numbersOfLopps > 0;
        }     
    }
}

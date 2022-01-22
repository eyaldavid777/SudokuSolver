using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Board : ISudokuBoard
    {

        private ISudokuCube[] board;

        public Board(String numbersInBoard)
        {
            sizeOfBoard = (int)Math.Sqrt(numbersInBoard.Length);
            SqrtOfSizeOfBoard = (int)Math.Sqrt(sizeOfBoard);
            placesOfNumbers = new Dictionary<int, List<int>>();
            knownNumbersNotInBoard = new KnownNumbersNotInBoard();
            board = new Cube[sizeOfBoard];
            Initialize(numbersInBoard, 0);
        }
        public Dictionary<int, List<int>> placesOfNumbers { get; set; }
        public int sizeOfBoard { get; }
        public int SqrtOfSizeOfBoard { get; }
        public KnownNumbersNotInBoard knownNumbersNotInBoard { get; set; }
        private void Initialize(String numbersInBoard, int index)
        {
            for (int numOfCube = 0; numOfCube < board.Length; numOfCube++)
            {
                placesOfNumbers.Add(numOfCube + 1, new List<int>());
            }
            for (int numOfCube = 0; numOfCube < board.Length; numOfCube++)
            {
                board[numOfCube] = new Cube(numbersInBoard, numOfCube, this);
            }
        }
        private void printALine()
        {
            System.Console.Write("       ");
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
        }
        private int getCubeNumberByIndex(int index)
        {
            return ((index / sizeOfBoard) / SqrtOfSizeOfBoard) * SqrtOfSizeOfBoard +
                    (index % sizeOfBoard) / SqrtOfSizeOfBoard;
        }
        private int getRowOrColInCubeByIndex(int index,bool col)
        {
            if (col)
                return index % sizeOfBoard % SqrtOfSizeOfBoard;
            return index / sizeOfBoard % SqrtOfSizeOfBoard;
        }
        private bool isplacesOfNumberContainsACube(int cubeNumber, int number)
        {
            foreach (int indexInBoard in placesOfNumbers[number])
                if (getCubeNumberByIndex(indexInBoard) == cubeNumber)
                    return true;
            return false;
        }
        private bool isCubeWorthChecking(int cubeNumber, int mostCommonNumber)
        {
            foreach (int indexInBoard in placesOfNumbers[mostCommonNumber])
                if (getCubeNumberByIndex(indexInBoard) / SqrtOfSizeOfBoard == cubeNumber / SqrtOfSizeOfBoard)
                {
                    if (!board[cubeNumber].isRowOrColFull(false, getRowOrColInCubeByIndex(indexInBoard, false)))
                        return true;
                }
                else
                    if (getCubeNumberByIndex(indexInBoard) % SqrtOfSizeOfBoard == cubeNumber % SqrtOfSizeOfBoard)
                        if (!board[cubeNumber].isRowOrColFull(true, getRowOrColInCubeByIndex(indexInBoard, true)))
                            return true;
            return false;
        }
        private bool isNumberInRowOrColOfCubes(int cubeNumber,bool col,int colOrRowIndexInCube, int addToIndexOfCube, int indexofCube, int endOfColOrRow, int number)
        {
            int colOrRowIndexInBoard;
            if(col)
                colOrRowIndexInBoard  = cubeNumber % SqrtOfSizeOfBoard * SqrtOfSizeOfBoard + colOrRowIndexInCube;
            else
                colOrRowIndexInBoard = cubeNumber / SqrtOfSizeOfBoard * SqrtOfSizeOfBoard + colOrRowIndexInCube;
            if (knownNumbersNotInBoard.isNumberInRowOrColInNumbersNotInBoard(col, colOrRowIndexInBoard, number))
                return true;
            for (; indexofCube < endOfColOrRow; indexofCube += addToIndexOfCube)
                if (indexofCube != cubeNumber)
                    if (board[indexofCube].isNumberInRowOrColInCube(col, colOrRowIndexInCube, number))
                        return true;
            return false;
        }
        public bool isPossibleIndexToNumber(int indexOfNumberInBoard, int indexOfNumberInCube, int number)
        {
            int cubeNumber = getCubeNumberByIndex(indexOfNumberInBoard);
            int rowOfCubeNumber = cubeNumber / SqrtOfSizeOfBoard;
            int colOfCubeNumber = cubeNumber % SqrtOfSizeOfBoard;
            int[] forParams = StaticMethods.forParameters(rowOfCubeNumber, false, SqrtOfSizeOfBoard);

            if (isNumberInRowOrColOfCubes(cubeNumber, false, indexOfNumberInCube / SqrtOfSizeOfBoard, forParams[0], forParams[1], forParams[2], number))
                return false;

            forParams = StaticMethods.forParameters(colOfCubeNumber, true, SqrtOfSizeOfBoard);

            if (isNumberInRowOrColOfCubes(cubeNumber, true, indexOfNumberInCube % SqrtOfSizeOfBoard, forParams[0], forParams[1], forParams[2], number))
                return false;
            return true;
        }
        public void deleteNumberFromRowOrCol(bool col,int indexOfNumberInBoard, int indexOfNumberInCube, int number)
        {
            int cubeNumber = getCubeNumberByIndex(indexOfNumberInBoard);
            int rowOrColOfCubeNumber;
            if(col)
                rowOrColOfCubeNumber = cubeNumber % SqrtOfSizeOfBoard;
            else
                rowOrColOfCubeNumber = cubeNumber / SqrtOfSizeOfBoard;
            int[] forParams = StaticMethods.forParameters(rowOrColOfCubeNumber, col, SqrtOfSizeOfBoard);

            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
                if (forParams[1] != cubeNumber)
                    board[forParams[1]].deleteNumberFromRowOrColInCube(col, indexOfNumberInCube/ SqrtOfSizeOfBoard,number);
        }
        private int isOptionsInTheSameRowOrCol(List<int> numberOfOptionsInCube)
        {
            int rowOfFirst = numberOfOptionsInCube.ElementAt(0) / SqrtOfSizeOfBoard;
            int colOfFirst = numberOfOptionsInCube.ElementAt(0) % SqrtOfSizeOfBoard;
            bool row = true;
            bool col = true;
            for (int i =1;i< numberOfOptionsInCube.Count; i++)
            {
                if (numberOfOptionsInCube.ElementAt(i) / SqrtOfSizeOfBoard != rowOfFirst)
                    row = false;
                if (numberOfOptionsInCube.ElementAt(i) % SqrtOfSizeOfBoard != colOfFirst)
                    col = false;
                if (!row && !col)
                    return -1;
            }
            return row ? 1 : 2;
        }     
        private void putKnownNumberAndDeletOptions(List<int>  optionsInCubeByBoardIndex,int indexOfCube,int mostCommonNumber)
        {
            int firstIndexInBoard = optionsInCubeByBoardIndex.ElementAt(0);
            int indexInCube = getRowOrColInCubeByIndex(firstIndexInBoard, false) * SqrtOfSizeOfBoard + getRowOrColInCubeByIndex(firstIndexInBoard, true);
            board[indexOfCube].putTheNumberAndDeletOptions(indexInCube, mostCommonNumber);
            placesOfNumbers[mostCommonNumber].Add(firstIndexInBoard);
        }
        private void theOptionsInTheSameRowOrCol(bool col,List<int> optionsInCubeByBoardIndex, int mostCommonNumber, int theSameRowOrCol, Dictionary<int, List<int>> numbersInRowOrCol)
        {
            if (!numbersInRowOrCol.ContainsKey(theSameRowOrCol))
            {
                numbersInRowOrCol.Add(theSameRowOrCol, new List<int>());
            }
            numbersInRowOrCol[theSameRowOrCol].Add(mostCommonNumber);
            int firstIndexInBoard1 = optionsInCubeByBoardIndex.ElementAt(0);
            int indexInCube1 = getRowOrColInCubeByIndex(firstIndexInBoard1, false) * SqrtOfSizeOfBoard + getRowOrColInCubeByIndex(firstIndexInBoard1, true);
            deleteNumberFromRowOrCol(col, optionsInCubeByBoardIndex.ElementAt(0), indexInCube1, mostCommonNumber);
        }  
        private void checkNumberOfOptions(List<int> optionsInCubeByBoardIndex, int mostCommonNumber,int indexOfCube)
        {
            switch (optionsInCubeByBoardIndex.Count)
            {
                case 0:
                    // in the cube indexOfCube you can't put mostCommonNumber (exception)
                    break;
                case 1:
                    // put the number in it's only place and delete the options of the number
                    // in the cells with the same row and col
                    putKnownNumberAndDeletOptions(optionsInCubeByBoardIndex, indexOfCube, mostCommonNumber);
                    break;
                default:
                    if (optionsInCubeByBoardIndex.Count <= SqrtOfSizeOfBoard)
                        switch (isOptionsInTheSameRowOrCol(optionsInCubeByBoardIndex))
                        {
                            case -1: // the options dont have the same row or col
                                break;
                            case 1:  // the options have the same row
                                theOptionsInTheSameRowOrCol(false, optionsInCubeByBoardIndex, mostCommonNumber, optionsInCubeByBoardIndex.ElementAt(0) / sizeOfBoard, knownNumbersNotInBoard.numbersInRows);
                                break;
                            case 2: // the options have the same col
                                theOptionsInTheSameRowOrCol(true, optionsInCubeByBoardIndex, mostCommonNumber, optionsInCubeByBoardIndex.ElementAt(0) % sizeOfBoard, knownNumbersNotInBoard.numbersInCols);
                                break;
                        }
                    break;
            }
        }
        private void firstStepOfSolving()
        {
            int mostCommonNumber;
            // sorts 'placesOfNumbers' by value - by the length of the list
            placesOfNumbers = placesOfNumbers.OrderByDescending(x => x.Value.Count) .ToDictionary(x => x.Key, x => x.Value);                  
            while (placesOfNumbers.Count != 0)
            {
                // mostCommonNumber = the element with the longest length of the value in 'placesOfNumbers',
                // (the number that exists most times in the board and it's in 'placesOfNumbers')
                mostCommonNumber = placesOfNumbers.Keys.ElementAt(0);
                for (int indexOfCube = 0; indexOfCube < sizeOfBoard; indexOfCube++)
                {
                    if (!isplacesOfNumberContainsACube(indexOfCube, mostCommonNumber))
                    {
                        bool  checkTheOptions = isCubeWorthChecking(indexOfCube, mostCommonNumber);
                        List<int> optionsInCubeByBoardIndex = board[indexOfCube].fillOptionsInCube(mostCommonNumber, checkTheOptions);
                        checkNumberOfOptions(optionsInCubeByBoardIndex,mostCommonNumber,indexOfCube);
                    }
                }
                placesOfNumbers.Remove(mostCommonNumber);
            }
        }

        public int countHowManySolvedCells()
        {
            int count = 0;
            for (int indexOfCube = 0; indexOfCube < sizeOfBoard; indexOfCube++)
            {
                count += board[indexOfCube].countHowManySolvedCells();
            }
            return count;
        }
        public void Solve()
        {
            firstStepOfSolving();
            int howManySolvedCells = countHowManySolvedCells();
            if (howManySolvedCells == sizeOfBoard * sizeOfBoard)
                return;
            if (howManySolvedCells == 0)
            {

            }


        }

    }
}

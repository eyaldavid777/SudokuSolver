using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Board : ISudokuBoard
    {
        private Cube[] board;

        public Board(String numbersInBoard)
        {
            sizeOfBoard = (int)Math.Sqrt(numbersInBoard.Length);
            SqrtOfSizeOfBoard = (int)Math.Sqrt(sizeOfBoard);
            placesOfNumbers = new Dictionary<int, List<int>>();
            board = new Cube[sizeOfBoard];
            Initialize(numbersInBoard, 0);
        }
        public Dictionary<int, List<int>> placesOfNumbers { get; set; }
        public int sizeOfBoard { get; }
        public int SqrtOfSizeOfBoard { get; }
        private void Initialize(String numbersInBoard, int index)
        {
            for (int numOfCube = 0; numOfCube < board.Length; numOfCube++)
            {
                board[numOfCube] = new Cube(numbersInBoard, numOfCube, this);
            }
        }
        private void printALine()
        {
            System.Console.Write("       ");
            for (int i = 0; i < sizeOfBoard; i++)
            {
                printAColOrCol(false, true, true);
            }
            System.Console.WriteLine();
        }
        private void printAColOrCol(bool col, bool DarkBlue, bool withSpaces)
        {
            if (DarkBlue)
                Console.ForegroundColor = ConsoleColor.DarkBlue;
            if (col)
                if (withSpaces)
                    System.Console.Write("       |");
                else
                    System.Console.Write("|");
            else
                 if (withSpaces)
                System.Console.Write(" _______");
            else
                System.Console.Write("_______");
            if (DarkBlue)
                Console.ForegroundColor = ConsoleColor.White;
        }
        private void printCols(bool Line, bool DarkBlue)
        {
            for (int i = 0; i < sizeOfBoard + 1; i++)
                if (i % SqrtOfSizeOfBoard == 0)
                    if (i == 0)
                        printAColOrCol(true, true, true);
                    else
                    {
                        if (Line)
                        {
                            printAColOrCol(false, DarkBlue, false);
                            printAColOrCol(true, true, false);
                        }
                        else
                            printAColOrCol(true, true, true);
                    }
                else
                    if (Line)
                {
                    printAColOrCol(false, DarkBlue, false);
                    printAColOrCol(true, false, false);
                }
                else
                    printAColOrCol(true, false, true);
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
                    printAColOrCol(true, true, true);
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
        private int findMostCommonNumber()
        {
            int mostCommonNumber = 0;
            int amountOfTheMostCommonNumber = 0;
            foreach(int number in placesOfNumbers.Keys)
            {
                int amountOfTheNumber = placesOfNumbers[number].Count;
                if (amountOfTheNumber > amountOfTheMostCommonNumber)
                {
                    amountOfTheMostCommonNumber = amountOfTheNumber;
                    mostCommonNumber = number;
                }
            }
            return mostCommonNumber;
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
            return index / sizeOfBoard / SqrtOfSizeOfBoard;
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
        private bool isNumberInRowOrColOfCubes(int cubeNumber,bool col,int colOrRowIndex, int addToIndexOfCube, int indexofCube, int endOfColOrRow, int number)
        {
            for (; indexofCube < endOfColOrRow; indexofCube += addToIndexOfCube)
            {
                if (indexofCube != cubeNumber)
                {
                    if (board[indexofCube].isNumberInRowOrColInCube(col, colOrRowIndex, number))
                        return true;
                }
            }
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
        private bool firstStepOfSolving()
        {
            int mostCommonNumber;
            int numberOfOptionsInCube = 0;
            while (placesOfNumbers.Count != 0)
            {
                mostCommonNumber = findMostCommonNumber();
                for (int indexOfCube = 0; indexOfCube < sizeOfBoard; indexOfCube++)
                {
                    if (!isplacesOfNumberContainsACube(indexOfCube, mostCommonNumber))
                    {
                        bool checkTheOptions = isCubeWorthChecking(indexOfCube, mostCommonNumber);
                        numberOfOptionsInCube = board[indexOfCube].fillOptionsInCube(mostCommonNumber, checkTheOptions);
                        if (numberOfOptionsInCube == 0)
                        {
                            // in the cube indexOfCube you can't put mostCommonNumber
                        }
                        if (numberOfOptionsInCube == 1)
                        {
                            // put the number in it's only place and delete the options of the number
                            // from the others
                        }
                        else
                        {
                            if (numberOfOptionsInCube <= SqrtOfSizeOfBoard)
                            {
                                // call to the function that works with NumbersNotInBoard
                            }
                        }
                    }
                }
                placesOfNumbers.Remove(mostCommonNumber);
            }
            return true;
        }
        public bool Solve()
        {
            if (firstStepOfSolving())
                return true;
            else
            {
                // secondStepOfSolving(); ...
            }
            return true;
        }
    }

}

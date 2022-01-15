using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Board : IInitializeable
    {
        public Cube[] board;
        public static Dictionary<int, List<int>> placesOfNumbers;
        public static int sizeOfBoard;
        public static int SqrtOfSizeOfBoard;
        public Board(String numbersInBoard)
        {
            sizeOfBoard = (int)Math.Sqrt(numbersInBoard.Length);
            SqrtOfSizeOfBoard = (int)Math.Sqrt(sizeOfBoard);
            placesOfNumbers = new Dictionary<int, List<int>>();
            board = new Cube[sizeOfBoard];
            Initialize(numbersInBoard, 0);
        }
        public void Initialize(String numbersInBoard, int index)
        {
            for (int numOfCube = 0; numOfCube < board.Length; numOfCube++)
            {
                board[numOfCube] = new Cube(numbersInBoard, numOfCube);
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
        public int findMostCommonNumber()
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
    }

}

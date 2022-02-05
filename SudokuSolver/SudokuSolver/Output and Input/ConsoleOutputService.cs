using SudokuSolver.Exceptions;
using SudokuSolver.Output_and_Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class ConsoleOutputService : IConsoleOutputService
    {
        // The class ConsoleOutputService responses to the user requests

        /*
         * Summary:
         * The function prints the menu to the console.
         * 
         * Input:
         * None.
         * 
         * OutPut:
         * None.
         */
        private void showMenu()
        {
            Console.WriteLine("\nmenu: \nIf you want to take the sudoku from a file - write down file \n" +
                              "If you want to take the sudoku from the console - write down console \n" +
                              "To end the program - write down end ");
        }

        /*
         * Summary:
         * The function is running according
         * to the user's requests.
         * 
         * Input:
         * None.
         * 
         * OutPut:
         * None.
         */
        public void view()
        {
            Console.WriteLine("welcom to my sudoku \n*note: all the indexes in the error messages in this program start from left to right ,and from top to bottom (for each row)");
            bool wantToContinue = true;
            while (wantToContinue)
            {
                showMenu();
                string fileOrConsole = Console.ReadLine();
                if (fileOrConsole == "end")
                    break;
                inputExecution(fileOrConsole);
            }
            Console.WriteLine("the program ended");
        }

        /*
          * Summary:
          * The function respones to what the user enters:
          * file - open new FileReaderService.
          * console - open new ConsoleReaderService.
          * else - print error message.
          * 
          * Input:
          * fileOrConsole - a string that the user entered
          * 
          * 
          * OutPut:
          * None.
          */
        private void inputExecution(string fileOrConsole)
        {
            IReader reader = null;
            switch (fileOrConsole)
            {
                case "file":
                    reader = new FileReaderService();
                    solvedSudokuAndShowSolution(reader);
                    break;
                case "console":
                    Console.WriteLine("print sudoku: ");
                    reader = new ConsoleReaderService();
                    solvedSudokuAndShowSolution(reader);
                    break;
                default:
                    Console.WriteLine("you wrote an invalid input. Please write an option from the menu \n");
                    break;
            }
        }

        /*
          * Summary:
          * The function solves the board.
          * 
          * Input:
          * reader - you can take from reader the content of the console/file
          * 
          * OutPut:
          * None.
          */
        private void solvedSudokuAndShowSolution(IReader reader)
        {
            string boardStringResult = string.Empty;
            try
            {
                string content = reader.ReadBoard();
                if(content == "")
                {
                    Console.WriteLine("the file is invalid");
                    return;
                }
                ISudokuBoard c = new Board(content);
                c.print();
                int emptyCellsBefore = c.sudokuSolver.countHowManyUnSolvedCells();
                Stopwatch sw = new Stopwatch();
                sw.Start();
                c.sudokuSolver.Solve();
                c = c.sudokuSolver.sudokuBoard;
                sw.Stop();
                int emptyCellsAfter = c.sudokuSolver.countHowManyUnSolvedCells();

                c.print();
                Console.WriteLine("solved {0} out of {1} in {2} seconds", emptyCellsBefore - emptyCellsAfter, emptyCellsBefore, (double)sw.ElapsedMilliseconds / 1000);
                boardStringResult = c.boardString();
            }
            catch (SudokuExceptions s)
            {
                Console.WriteLine(s.Message);
                boardStringResult = s.Message;
            }

            IFileWriter writer = new WriteToFile();
            writer.writeToFile(boardStringResult);
        }
    }
}

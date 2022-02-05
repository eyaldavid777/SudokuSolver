using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Output_and_Input
{
    class WriteToFile : IFileWriter
    {
        // the class WriteToFile responsible of open a new file for the
        // solved sudoku and write to it the solved sudoku result string.

        /*
         * Summary:
         * The function open a new txt file solvedSudoku.txt
         * at d:/user data/Documents/solvedSudoku.txt
         * 
         * Input:
         * None.
         * 
         * OutPut:
         * None.
         */
        public void openNewFile()
        {
            string path = "C:/Users/user/Desktop/solvedSudoku.txt";

            using (StreamWriter sw = File.CreateText(path));
        }

        /*
         * Summary:
         * The function write to solvedSudoku.txt file the sudoku result.
         * 
         * Input:
         * None.
         * 
         * OutPut:
         * None.
         */
        public void writeToFile(string boardStringResult)
        {
            string text = boardStringResult;

            string docPath =Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            openNewFile();
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "C:/Users/user/Desktop/solvedSudoku.txt")))
            {
                outputFile.WriteLine(text);
            }
        }
    }
}

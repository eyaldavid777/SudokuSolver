using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class IntersectionNumbers
    {
        // the class IntersectionNumbers helps us with Intersection tactics -
        // If a the options of a number can be only in indexes in the same row or col
        // you can delete the options of the number from the rest of the row or cols

        // the key - an index of a row 
        // the values - list of numbers that you found
        // out that in their cube they are in the same row
        public Dictionary<int, List<int>> numbersInRows { get; set; }

        // the key - an index of a col 
        // the values - list of numbers that you found
        // out that in their cube they are in the same col
        public Dictionary<int, List<int>> numbersInCols { get; set; }

        /*
         * Summary:
         * The function initialized an instance of the KnownNumbersNotInBoard class.
         * 
         * Input:
         * 
         * OutPut:
         * None.
         */
        public IntersectionNumbers()
        {
            numbersInRows = new Dictionary<int, List<int>>();
            numbersInCols = new Dictionary<int, List<int>>();

        }

        /*
         * Summary:
         * The function checks if the number "number" is in the "colOrRowIndex"
         * in the instance of KnownNumbersNotInBoard (this).
         * 
         * Input:
         * col - true for col and false for row.
         * It tells the function if to search in "numbersInRows"
         * or "numbersInCols".
         * colOrRowIndex - The colOrRowIndexwe where we want to search fot "number".
         * number - A number.
         * 
         * OutPut:
         * The function returns true if the number "number" is in the "colOrRowIndex"
         * in the instance of KnownNumbersNotInBoard (this).Otherwise it returns false.
         */
        public bool isNumberInRowOrColInNumbersNotInBoard(bool col,int colOrRowIndex, int number)
        {
            Dictionary<int, List<int>> numbersInColsOrRows;
            if (col)
                numbersInColsOrRows = numbersInCols;
            else
                numbersInColsOrRows = numbersInRows;

            if (numbersInColsOrRows.ContainsKey(colOrRowIndex))
                foreach (int numberInColOrRow in numbersInColsOrRows[colOrRowIndex])
                    if (numberInColOrRow == number)
                        return true;
            return false;          
        }
    }
}

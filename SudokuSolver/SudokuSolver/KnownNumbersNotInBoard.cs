using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class KnownNumbersNotInBoard
    {
        public Dictionary<int, List<int>> numbersInRows { get; set; }
         public Dictionary<int, List<int>> numbersInCols { get; set; }


        public KnownNumbersNotInBoard()
        {
            numbersInRows = new Dictionary<int, List<int>>();
            numbersInCols = new Dictionary<int, List<int>>();

        }
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

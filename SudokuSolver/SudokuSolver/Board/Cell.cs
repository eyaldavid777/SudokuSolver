using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Cell : ICell
    {
        // the cell class helps us with changing the number in the board. 

        // the number in the cell (0 is empty)
        public int number { get; set; }

        // the index in board of the cell
        public int index { get; }
        
        //the optional numbers of the cell
        public List<int> optionalNumbers { get; set; }

        /*
         * Summary:
         * The function initialized an instance of the Cell class.
         * 
         * Input:
         * placesOfNumbers - the dictionary of placesOfNumbers from Board class
         * Number - the number we want to put in the cell
         * Index - the index we want to give the cell
         * checkCellIntegrity - telling the constructor whether
         * to check the integrity of the cell or not.
         * 
         * OutPut:
         * None.
         */
        public Cell(Dictionary<int, List<int>> placesOfNumbers, int Number, int Index,bool checkCellIntegrity,int sizeOfBoard)
        {
            index = Index; 
            number = Number - '0';
            if(checkCellIntegrity)
                cellIntegrity(sizeOfBoard);
            placesOfNumbers[number].Add(Index);
            optionalNumbers = null;
        }

        /*
         * Summary:
         * The function initialized an instance of the Cell class. (An empty cell)
         * 
         * Input:
         * Index - the index we want to give the cell
         * 
         * OutPut:
         * None.
         */
        public Cell(int Index)
        {
            index = Index;
            number = 0;
            optionalNumbers = new List<int>();
        }

        /*
         * Summary:
         * The function is checking if the cell's number is valid -
         * between 0 and sizeOfBoard +1.
         * 
         * Input:
         * sizeOfBoard - the size of the board (size of row/col/cube)
         * 
         * OutPut:
         * None.
         *  if the cell's number is invalid (not suitable for the board)
         * the function throws InvalidNumberException.
         */
        public void cellIntegrity(int sizeOfBoard)
        {
            if (number < 0 || number > sizeOfBoard)
            {
                throw new InvalidNumberException("The number " + number + " is invalid in this board");
            }
        }

        /*
         * Summary:
         * The function puts the "knownNumber" in the cell
         * and delete the optionalNumbers.
         * 
         * Input:
         * knownNumber - the number we want to put in the cell.
         * 
         * OutPut:
         * None.
         */
        public void solvedTheCell(int knownNumber)
        {
            number = knownNumber;
            optionalNumbers = null;
        }

        /*
         * Summary:
         * The function checks if the cell is solved (number != 0).
         * 
         * Input:
         * None
         * 
         * OutPut:
         * The functions returns true if the cell is solved.Otherwise it returns false.
         */
        public bool isSolved()
        {
            return number != 0;
        }
    }
}

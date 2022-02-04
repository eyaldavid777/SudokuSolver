using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Cell : ICell
    {
        public int number { get; set; }

        public int index { get; }
        public List<int> optionalNumbers { get; set; }

        public Cell(Dictionary<int, List<int>> placesOfNumbers, int Number, int Index, bool checkCellIntegrity)
        {
            index = Index; 
            number = Number - '0';
            // check if the number is valid
            if (checkCellIntegrity)
                CellIntegrity(placesOfNumbers, number);
            placesOfNumbers[number].Add(Index);
            optionalNumbers = null;
        }
        public Cell(int Index)
        {
            index = Index;
            number = 0;
            optionalNumbers = new List<int>();
        }
        public void CellIntegrity(Dictionary<int, List<int>> placesOfNumbers, int number)
        {
            if (!placesOfNumbers.ContainsKey(number))
            {
                throw new InvalidNumberException("The number " + number + " is invalid in this board");
            }
        }
        public void solvedTheCell(int knownNumber)
        {
            number = knownNumber;
            optionalNumbers = null;
        }

        public bool isSolved()
        {
            return number != 0;
        }
    }
}

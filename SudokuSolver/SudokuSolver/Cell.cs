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

        public Cell(Dictionary<int, List<int>> placesOfNumbers, int Number, int Index)
        {
            index = Index; 
            number = Number - '0';
            // check if the number is valid
            placesOfNumbers[number].Add(Index);
            optionalNumbers = null;
        }
        public Cell(int Index)
        {
            index = Index;
            number = 0;
            optionalNumbers = new List<int>();
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

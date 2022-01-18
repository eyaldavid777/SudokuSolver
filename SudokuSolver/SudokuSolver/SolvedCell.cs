using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class SolvedCell : ICell
    {
        public int number { get; set; }

        public SolvedCell(Dictionary<int, List<int>> placesOfNumbers,int Number, int Index) : base(Index)
        {
            number = Number - '0';
            // check if a number is found several times in the same row or col 
            if (placesOfNumbers.ContainsKey(number))
                placesOfNumbers[number].Add(Index);
            else { 
                placesOfNumbers.Add(number, new List<int>());
                placesOfNumbers[number].Add(Index);
            }
        }

    }
}

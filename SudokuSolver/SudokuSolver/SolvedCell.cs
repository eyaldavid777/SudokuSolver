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

        public SolvedCell(int Number, int Index) : base(Index)
        {
            number = Number - '0';
            // check if a number is found several times in the same row or col 
            if (Board.placesOfNumbers.ContainsKey(number))
                Board.placesOfNumbers[number].Add(Index);
            else { 
                Board.placesOfNumbers.Add(number, new List<int>());
                Board.placesOfNumbers[number].Add(Index);
            }
        }

    }
}

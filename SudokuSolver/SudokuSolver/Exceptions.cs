using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    abstract class SudokuExceptions : Exception
    {
        public SudokuExceptions(string message) : base(message) { }
    }

    [Serializable]
    class InvalidBoardSizeException : SudokuExceptions
    {
        public InvalidBoardSizeException() : base("Invalid board size") { }

    }

    [Serializable]
    class InvalidNumberException : SudokuExceptions
    {
        public InvalidNumberException(string message) : base(message) { }

    }

    [Serializable]
    class SameNumberInARowOrColException : SudokuExceptions
    {
        public SameNumberInARowOrColException(string message) : base(message) { }
    }

    [Serializable]
    class SameNumberInACubeException : SudokuExceptions
    {
        public SameNumberInACubeException(string message) : base(message) { }
    }

    [Serializable]
    class NoPlaceForANumberInACubeException :  SudokuExceptions
    {
        public NoPlaceForANumberInACubeException(string message) : base(message) { }
    }

    [Serializable]
    class NoPlaceForANumberInARowOrColException :  SudokuExceptions
    {
        public NoPlaceForANumberInARowOrColException(string message) : base(message) { }
    }
    [Serializable]
    class UnSolvableCellException : SudokuExceptions
    {
        public UnSolvableCellException(string message) : base(message) { }
    }
    [Serializable]
    class UnSolvableBordException :  SudokuExceptions
    {
        public UnSolvableBordException() : base("Unsolvable Bord") { }
    }

}

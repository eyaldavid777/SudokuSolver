using SudokuSolver.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{

    [Serializable]
    public class InvalidBoardSizeException : SudokuExceptions
    {
        public InvalidBoardSizeException() : base("Invalid board size") { }

    }

    [Serializable]
    public class InvalidNumberException : SudokuExceptions
    {
        public InvalidNumberException(string message) : base(message) { }

    }

    [Serializable]
    public class SameNumberInARowOrColOrCubeException : SudokuExceptions
    {
        public SameNumberInARowOrColOrCubeException(string message) : base(message) { }
    }


    [Serializable]
    public class NoPlaceForANumberInARowOrColOrCubeException :  SudokuExceptions
    {
        public NoPlaceForANumberInARowOrColOrCubeException(string message) : base(message) { }
    }
    [Serializable]
    public class UnSolvableCellException : SudokuExceptions
    {
        public UnSolvableCellException(string message) : base(message) { }
    }
    [Serializable]
    public class UnSolvableBordException :  SudokuExceptions
    {
        public UnSolvableBordException() : base("Unsolvable Bord") { }
    }

}

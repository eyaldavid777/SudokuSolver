using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Exceptions
{
    public abstract class SudokuExceptions : Exception
    {
        public SudokuExceptions(string message) : base(message) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    interface IInitializeable
    {
        void Initialize(String numbers, int index);
    }
}

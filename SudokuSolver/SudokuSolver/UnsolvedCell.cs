﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class UnsolvedCell : ICell
    {
        public List<int> optionalNumbers;
        public UnsolvedCell(char number,int Index) :base(Index)
        {
            optionalNumbers = new List<int>();
        }

    }
}
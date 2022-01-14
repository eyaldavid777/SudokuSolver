﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class SolvedCell : ICell
    {
        public int number { get; set; }

        public SolvedCell(char Number, int Index) : base(Index)
        {
            number = Number - '0';
        }

    }
}
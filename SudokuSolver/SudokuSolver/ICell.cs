using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{

    abstract class ICell
    {
        protected int index;
        
        public ICell(int Index)
        {
            index = Index;
        }

        public int getIndex()
        {
            return index;
        }
    }
}

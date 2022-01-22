using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            ISudokuBoard smallcheck = new Board("0140020000000310");
         //   smallcheck.print();

            smallcheck.Solve();

           //  smallcheck.print();

            ISudokuBoard b = new Board("0001200000300000");
            // check if solving a cell deletes it's options 
            // in the cells with the same row and col
            //  b.print();

            b.Solve();

            // b.print();

            ISudokuBoard checkIfKnownNumbersNotInBoardWorks = new Board("100000000002000000000000000543020000600000870000000001000000000001000000000000000");
           // checkIfKnownNumbersNotInBoardWorks.print();

            checkIfKnownNumbersNotInBoardWorks.Solve();

              // checkIfKnownNumbersNotInBoardWorks.print();

            ISudokuBoard checkIfKnownNumbersNotInBoardDeletesInRowOrCol = new Board("0000000010000002000000000000000203450780000161000000000000000000000001000000000004");
           // checkIfKnownNumbersNotInBoardDeletesInRowOrCol.print();

            checkIfKnownNumbersNotInBoardDeletesInRowOrCol.Solve();

          // checkIfKnownNumbersNotInBoardDeletesInRowOrCol.print();


            ISudokuBoard checkRec = new Board("400030000630001006000000003000000000000410060000000000160000034340580000000000000");

            checkRec.print();

            checkRec.Solve();

            checkRec.print();


            ISudokuBoard a = new Board("014002000000031000000000000000000008000500000000000000040000000000000000000000001");

            // a.print();

            a.Solve();
          //  a.print();

            ISudokuBoard c = new Board("009800000501072000000000613090000032010396000753000000000400308105000400070020000");
            //c.print();

            c.Solve();
          //  c.print();

            ISudokuBoard d = new Board("000805013905203600603090204001000005040100706256304890590007102102080470004910038");
           // d.print();

            d.Solve();
           // d.print();

            d.Solve();
          //  d.print();

        }
    }
}

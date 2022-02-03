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
            try
            {
                //ISudokuBoard letterscheck = new Board("1000000400200300");
                //ISudokuBoard letterscheck = new Board("1000000000000000");

                //ISudokuBoard letterscheck = new Board("12");
                //ISudokuBoard letterscheck = new Board("3040010204032010");
               // ISudokuBoard letterscheck = new Board("100025038000040000000007009000000000000000000070000000000000400009400000040000000");
                //ISudokuBoard letterscheck = new Board("100025038000000000000007000000000000000000000070000000000000400009400000040000000");
                //ISudokuBoard letterscheck = new Board("100025038000000000000004000000000000000000000040000000000000700009700000070000000");
                //ISudokuBoard letterscheck = new Board("000400060002000000003000000000007800005000000000006400001000000009000000000060040");// invalid col
                //letterscheck.print();
                //letterscheck.Solve();
                //letterscheck.print();

            }
            catch (SudokuExceptions s)
            {
                Console.WriteLine(s.Message);
            }

            // ISudokuBoard b = new Board("0001200000300000");
            // check if solving a cell deletes it's options 
            // in the cells with the same row and col
            // b.print();

            // b.Solve();

            //  b.print();

            // ISudokuBoard checkIfKnownNumbersNotInBoardWorks = new Board("100000000002000000000000000543020000600000870000000001000000000001000000000000000");
            // checkIfKnownNumbersNotInBoardWorks.print();

            //checkIfKnownNumbersNotInBoardWorks.Solve();

            // checkIfKnownNumbersNotInBoardWorks.print();

            //ISudokuBoard checkIfKnownNumbersNotInBoardDeletesInRowOrCol = new Board("0000000010000002000000000000000203450780000161000000000000000000000001000000000004");
            // checkIfKnownNumbersNotInBoardDeletesInRowOrCol.print();

            // checkIfKnownNumbersNotInBoardDeletesInRowOrCol.Solve();

            // checkIfKnownNumbersNotInBoardDeletesInRowOrCol.print();


            // ISudokuBoard checkRec = new Board("400030000630001006000000003000000000000410060000000000160000034340580000000000000");

            //  checkRec.print();

            //  checkRec.Solve();

            //  checkRec.print();


            // ISudokuBoard a = new Board("014002000000031000000000000000000008000500000000000000040000000000000000000000001");

            // a.print();

            //  a.Solve();
            //  a.print();
            try
            {
                //ISudokuBoard c = new Board("009800000501072000000000613090000032010396000753000000000400308105000400070020000");
                //ISudokuBoard c = new Board("000805013905203600603090204001000005040100706256304890590007102102080470004910038");
                //ISudokuBoard c = new Board("123456780000000000000000000000000000000000000000000000000000000000000000000000000");
                //ISudokuBoard c = new Board("123456780000000090000000000000000000000000000000000000000000000000000000000000000");
                //ISudokuBoard c = new Board("014002000000031000000000000000000008000500000000000000040000000000000000000000001");
                //ISudokuBoard c = new Board("000000000670030000859000030000000000400000000000000000002000000041000000000000000"); //////////////////////////////////////////////
                //ISudokuBoard c = new Board("000000000670030000850000030000000000490000000000000000002000000941000000000000000"); //to do
                //ISudokuBoard c = new Board("000568030190000000803100200400051060700020004000070800010005007004000003050730100"); 
           //ISudokuBoard c = new Board("000190208802005000094000000200300001000009700400060080000207000035900000000610094"); 
             //ISudokuBoard c = new Board("005600004000000000000000300050007000070000000930000040100000400009040000500000070");
                ISudokuBoard c = new Board("630000900215000070040002001000190300000207000003058000400700030080000167002000048");
                //ISudokuBoard c = new Board("500009:0800000000700005000000>80003000000000000:0000000000000000000000000000:0000000000000000000000000000000000000001000000000080000000000000000000000030000;000000000000010000000000000000000000000000:3000000000800=000000900;00000000090002000300900000>00000");
                c.print();

                Solver solver = new Solver(c);
                solver.Solve();
                c.print();
            }
            catch (SudokuExceptions s)
            {
                Console.WriteLine(s.Message);
            }
            //  ISudokuBoard d = new Board("000805013905203600603090204001000005040100706256304890590007102102080470004910038");
            // d.print();

            //  d.Solve();
            // d.print();

            // d.Solve();
            //  d.print();


        }
    }
}

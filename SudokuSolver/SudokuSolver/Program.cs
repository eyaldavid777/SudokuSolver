﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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
                //ISudokuBoard c = new Board("2");
                //ISudokuBoard c = new Board("0");
                //ISudokuBoard c = new Board("1");
                //ISudokuBoard c = new Board("000400060002000000003000000000007800005000000000006400001000000009000000000060040");
                //ISudokuBoard c = new Board("000000317500000000001900000012600074600000030000000005000006000024350760080010400");
                // ISudokuBoard c = new Board("009800000501072000000000613090000032010396000753000000000400308105000400070020000");
                // ISudokuBoard c = new Board("403600000050900000006000040700095102005001079019067038568024903340509687100080250");
                // ISudokuBoard c = new Board("000015600000060850000000003208000004040390002601200000480670000000800090006040000");
                //ISudokuBoard c = new Board("500080049000500030067300001150000000000208000000000018700004150030002000490050003");
                //ISudokuBoard c = new Board("006000007970000040520000800000700500400003170050008006000301002000805000603902000");
                // ISudokuBoard c = new Board("040000050081030000070100400009000010100004000035000000050096300000000700000503000");
                //ISudokuBoard c = new Board("002000000000000000000008000050001406000000170601000090000000000000009000003600000");
                //ISudokuBoard c = new Board("000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                // ISudokuBoard c = new Board("000804700074100000108600402000005307700000049009007006007000000200030000000402005");
                //ISudokuBoard c = new Board("000805013905203600603090204001000005040100706256304890590007102102080470004910038");
                // ISudokuBoard c = new Board("123456780000000000000000000000000000000000000000000000000000000000000000000000000");
                //ISudokuBoard c = new Board("123456780000000090000000000000000000000000000000000000000000000000000000000000000");
                //ISudokuBoard c = new Board("014002000000031000000000000000000008000500000000000000040000000000000000000000001");
                //ISudokuBoard c = new Board("000000000670030000859000030000000000400000000000000000002000000041000000000000000"); 
                //ISudokuBoard c = new Board("000000000670030000850000030000000000490000000000000000002000000941000000000000000"); 
                //ISudokuBoard c = new Board("000568030190000000803100200400051060700020004000070800010005007004000003050730100"); 
                //ISudokuBoard c = new Board("000190208802005000094000000200300001000009700400060080000207000035900000000610094"); 
                //ISudokuBoard c = new Board("005600004000000000000000300050007000070000000930000040100000400009040000500000070");
                // ISudokuBoard c = new Board("630000900215000070040002001000190300000207000003058000400700030080000167002000048");
                //ISudokuBoard c = new Board("030>0:060092040?5@00<00;006300070?09000000040;@007000010@;00000000010>0000000=00600;0000092=4001090008;00000207000040<0?0008050000>=160<0:700983000200001000;<?0<000;00:00@0=000@090>3070200:006000041?020050009>000070000;06030000060000>0:1@50?20000300000000:");
                //  ISudokuBoard c = new Board("0000700000000400003000000000000000040000001=000000000000000070000000500000000000000;080000000000000000700000000000000000000000000000000000000000000000000000000000030000000000000500000900000200000000000000000000<0000000000000000000010000600<08000000;0000000");
                // ISudokuBoard c = new Board("040080@0;010060>30090?04:70=00<006002;0080003000?000000=00>002040008030070005000000000>0000:0@0=009250000800;000<010000@00=00030000<?0000600=047@1=0:00300700900600000;04009002:03000@000>800;005;3:000800<400010>00;9000=?04050=040600000020<090<0@0=00010060?0");
                // ISudokuBoard c = new Board("00<60070B05H0:1004000000020C0=000:00000000B50000010000000F000030200><0@8I000@0002G00=<F000E?C30000>0G0H00000I840@CE0070003<0904020000000:0H<A@00050000009006I0008053000000D00BC?0:;000B<C0000000G00700400000000>0F00B@D06;=0000000F0I001A54700>083E00;00060D=?00000090020I0180050E0010000@:07004D00900>0H0A0I2500000=00000F00000C1800007E00<02A000B6@00?00=0:08:B<@900050000C00A0E0?00F0800?03060E0070B>50100000000C010@;000:FI?000000E000310E004000020=0HI00>00600000000?0<>06AH0000000=0005G@40=H720900?30F000000800ID20C000010000E300<0000@<050;E0G00?0000C900000I:00009DF74030>IB0;0000010?H060F80I>0:;090D1@070G00=>=E100500060F0G:000200B7;");
                  ISudokuBoard c = new Board("BI00D20<G00004@F100>060CAA00;C0IH:000<007004@01E00>010=000;000H:D050G?0394@@03040F10=A00;CBI00005<G??000G@7304>0000080;CB0H:D0B0H:?0000000900F0E=A00;CCA00;DB0H:?20<G00390>F00=0>00EC080;000H:?05<000390000090000ECA86000IH:?0500G?25<00039=>000C08000B0H::0B00G020<00039=00000A00000A00:DB00002004@039=0F0000>000CA000DB0H0000<4073000@030=>F000A00:0000G?050<0000040700=>00;CA000D0IHH:D0000?2500000E=0000CA800;0A8000BI<000004000E00F110=>00;C08H0DBI<0020940003000710=0F0;C08000B0<00255<G?2004@01E0>F00CA000D0II0:D000G?2090@00000060CA8860C0IH0005<002090000000F00E00800CAIH0D000G02000@00300000E=>06;000H00B5<0?005<0000040000=0860C00H:0B");
                // ISudokuBoard c = new Board("000000;H0004090000:I000F001000A060000H@030G00><0000005:200000B0?D0;H00000900000900<5:000CF0060D00H@0000H@80009I0<0:2170FA00000000000;0@030G900050200CF0210C0A00?00;00030000><0::000002100D000?=000@804G00030000>00000000AB0?000H@00E0H0000G:0>05F0000DAB600DAB60=0009804000>050010000000?0AB0@0000900000I000500><0020000AB60000008340G900050000C02100D000@0E00000E;0003400I00C0007?DAB66000B000E;098000:0000F007000010?000H@=E0G080400I00<00I0000216000BH000;000044G000<000000020000AB0@=00000004000005:I>7CF000?00BB0?000H0=E0G900<0:I00C0001000206?DA000=04098300;0>0000000C00B0?00;H0000000004098005:007000000D00H@000;H0000000000:I00CF0B0?0A");
                //ISudokuBoard c = new Board("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                //ISudokuBoard c = new Board("500009:0800000000700005000000>80003000000000000:0000000000000000000000000000:0000000000000000000000000000000000000001000000000080000000000000000000000030000;000000000000010000000000000000000000000000:3000000000800=000000900;00000000090002000300900000>00000");
                // ISudokuBoard c = new Board("02>000@40001005000900:01000004000000=6000>00?<0@65:082000<000107905410020000<0700000;00=030000010000@000=:?9>040:00;048?000050=00;010000:74030060702>@6:000;0000@0000010<00>0000040:0000?002@50;;0<000>000=70962807900;000>:000000400000;02008000600<00098000>10");
                // ISudokuBoard c = new Board("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                // ISudokuBoard c = new Board("00<00010020008000003?=<001:4500000@>;007500=?30020=706800?>0410;000000?>23000000<02;=90@:05>1?07>50000000000003600180002;0009=0000?:00014000@<004;000000000000?8107<240;=0?83:0500000063<:000000@09?0<200;70=5030031500?>0027;0000057>;00<13800000;000@004000900");
                // ISudokuBoard c = new Board("0000000000000000000000000000000000000000000000000000800000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000400000000000000060000000000000002000000500000050000000000000000000000000?00000000000000000000000000500000");
                //ISudokuBoard c = new Board("00:00?0000;0000001000:000000000000000000000000000000=00000000000000000000000000000500000000000000000000000600@00000000000000000000000000000;00000000000000000>000000000=0000000000000000000000000080000006000000000000000000000000000000000>00000000000000000000");
                //ISudokuBoard c = new Board("0000010000000000000000400000000000000000000000000000000000000000000000002;000000000800>0300000<0=0000000000000000000000000000600000000080000000000007000000008000000000000000000000000000000000000000000000>0000000000000000000000000000000000000000000000000000");
                c.print();
                Stopwatch sw = new Stopwatch();

                int SolvedCellsBefore = c.sudokuSolver.countHowManySolvedCells();
                sw.Start();
                c.sudokuSolver.Solve();
               c = c.sudokuSolver.sudokuBoard;
                sw.Stop();
                int SolvedCellsAfter = c.sudokuSolver.countHowManySolvedCells();

                c.print();
                Console.WriteLine("solved {0} out of {1} in {2} seconds", SolvedCellsAfter - SolvedCellsBefore,c.sizeOfBoard*c.sizeOfBoard - SolvedCellsBefore, (double)sw.ElapsedMilliseconds/1000);
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

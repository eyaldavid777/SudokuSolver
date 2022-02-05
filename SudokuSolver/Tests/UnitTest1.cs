using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver;
using SudokuSolver.Exceptions;
using System;


namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        public bool solveBoardTest(string boardString)
        {
            try
            {
                ISudokuBoard c = new Board(boardString);
                c.sudokuSolver.Solve();
                c.boardIntegrity();
                return true;
            }
            catch(SudokuExceptions s)
            {
                return false;
            }
        }
        [TestMethod]
        public void easy()
        {


            Assert.IsTrue(solveBoardTest("0"));

        }
        [TestMethod]
        public void smallBoard()
        {


            Assert.IsTrue(solveBoardTest("006000007970000040520000800000700500400003170050008006000301002000805000603902000"));

        }
        [TestMethod]
        public void bigBoard()
        {


            Assert.IsTrue(solveBoardTest("030>0:060092040?5@00<00;006300070?09000000040;@007000010@;00000000010>0000000=00600;0000092=4001090008;00000207000040<0?0008050000>=160<0:700983000200001000;<?0<000;00:00@0=000@090>3070200:006000041?020050009>000070000;06030000060000>0:1@50?20000300000000:"));

        }
        [TestMethod]
        public void InvalidNumber()
        {
            // invalid number (2)

            Assert.IsFalse(solveBoardTest("2"));

        }
        [TestMethod]
        public void InvalidBoardSize()
        {
            Assert.IsFalse(solveBoardTest("1234"));

        }
        [TestMethod]
        public void SameNumberInACube()
        {
            // same number in a cube
            Assert.IsFalse(solveBoardTest("0000010000100001"));

        }
        [TestMethod]
        public void SameNumberInARow()
        {
            // same number in a row
            Assert.IsFalse(solveBoardTest("0000010000100001"));

        }
        [TestMethod]
        public void SameNumberInCol()
        {
            // same number in a col
            Assert.IsFalse(solveBoardTest("0000010000100001"));

        }
        [TestMethod]
        public void NoPlaceForANumberInACube()
        {
            // the cube at index 0 cannot conatin the number 7
            Assert.IsFalse(solveBoardTest("140000000250000000360000000000000000000000000000000000000000000007000000000000000"));

        }
        [TestMethod]
        public void NoPlaceForANumberInARow()
        {
            // the row at index 3 cannot conatin the number 2
            Assert.IsFalse(solveBoardTest("000000000000000000000000200000000031000200000002000000000000000000000000000000000"));

        }
        [TestMethod]
        public void NoPlaceForANumberInACol()
        {
            // the col at index 0 cannot conatin the number 1
            Assert.IsFalse(solveBoardTest("000000000000000000001000000000000000000000000010000000000010000000000001200000000"));

        }
        [TestMethod]
        public void UnSolvableCells()
        {
            //the indexes at 27 36 cannot contain all of these numbers: 4 3 2 1
            Assert.IsFalse(solveBoardTest("043000000002000000001000000000000000000000000000012340700000000600000000500000000"));

        }
        [TestMethod]
        public void UnSolvableCellException()
        {
            //the cell in the cube 2 at index 2 (in the cube) cannot conatin a valid number
            Assert.IsFalse(solveBoardTest("123456780000000090000000000000000000000000000000000000000000000000000000000000000"));

        }
    }
}

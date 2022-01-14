using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Board : IInitializeable
    {
        public Cube[] board;
        public static int sizeOfBoard;
        public static int SqrtOfSizeOfBoard;
        public Board(String numbersInBoard)
        {
            sizeOfBoard = (int)Math.Sqrt(numbersInBoard.Length);
            SqrtOfSizeOfBoard = (int)Math.Sqrt(sizeOfBoard);
            board = new Cube[sizeOfBoard];
            Initialize(numbersInBoard, 0);
        }
        public void Initialize(String numbersInBoard, int index)
        {
            for (int numOfCube = 0; numOfCube < board.Length; numOfCube++)
            {
                board[numOfCube] = new Cube(numbersInBoard, numOfCube);
            }
        }
        


    }

}

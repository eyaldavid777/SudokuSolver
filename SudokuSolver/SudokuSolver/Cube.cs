using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Cube : IInitializeable
    {
        public ICell[] cube;

        public Cube(String cubeNumbers, int numOfCube)
        {
            cube = new ICell[Board.sizeOfBoard];
            Initialize(cubeNumbers, numOfCube);
        }
        public int indexInBoard(int indexInCube, int numOfCube)
        {
            int sqrtOfBoardSize = Board.SqrtOfSizeOfBoard;
            return (numOfCube / sqrtOfBoardSize) * Board.sizeOfBoard * sqrtOfBoardSize
                         + indexInCube % sqrtOfBoardSize + Board.sizeOfBoard * (indexInCube / sqrtOfBoardSize)
                         + (numOfCube % sqrtOfBoardSize) * sqrtOfBoardSize;
        }
        public void Initialize(String numbersInBoard, int numOfCube)
        {
            for (int indexInCube = 0; indexInCube < cube.Length; indexInCube++)
            {
                int index = indexInBoard(indexInCube, numOfCube);

                if (numbersInBoard[index] == '0')
                    cube[indexInCube] = new UnsolvedCell(numbersInBoard[index], index);
                else
                    cube[indexInCube] = new SolvedCell(numbersInBoard[index], index);
            }
        }
       

    }
}

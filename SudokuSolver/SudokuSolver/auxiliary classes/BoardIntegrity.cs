using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public abstract class BoardIntegrity
    {
        // the abstract class BoardIntegrity responsible for 
        // checking the integrity of the board that it receives

        /*
         * Summary:
         * The function checks the integrity of the size  
         * of the string which represents the board, if 
         * the string length has third rood.
         * 
         * Input:
         * numbersInBoard - A string which represents the board.
         * 
         * OutPut:
         * None.
         * if the size of the board is invalid the function
         * throws InvalidBoardSizeException.
         */
        protected void SizeOfBoardIntegrity(string numbersInBoard)
        {
            double doubleSizeOfBoard = Math.Sqrt(numbersInBoard.Length);
            if (!Calculations.isInt(Math.Sqrt(doubleSizeOfBoard)))
                throw new InvalidBoardSizeException();
        }

        /*
         * Summary:
         * The function is calling to rowColCubeIntegrity twice.
         * One time for checking the integrity of the rows and 
         * cols (sends ture) and one time for checking the 
         * integrity of the cubes (sends false).
         * 
         * Input:
         * sudokuBoard - A sudoku board.
         * 
         * OutPut:
         * None.
         * if one of the rows or cols or cubes in invalid (has the same number more than once)
         * the function throws indirectly SameNumberInARowOrColOrCubeException.
         *  if the cell's number is invalid (not suitable for the board)
         * the function throws indirectly InvalidNumberException.
         */
        protected void boardIntegrity(Board sudokuBoard, bool checkCells)
        {
            rowColCubeIntegrity(true, sudokuBoard);
            rowColCubeIntegrity(false, sudokuBoard);
            if (checkCells)
                boardIntegrityOfCells(sudokuBoard);
        }


        /*
        * Summary:
        * The function is checking the integrity of the rows and 
        * cols if checkRowsOrCols = true and the
        * integrity of the cubes if checkRowsOrCols = false
        * in the board "sudokuBoard".
        * 
        * Input:
        * sudokuBoard - A sudoku board.
        * checkRowsOrCols -  checkRowsOrCols is true when we 
        * want to check integrity of rows or cols and
        * false when we want to check integrity of cubes.
        * 
        * OutPut:
        * None.
        * if one of the rows or cols or cubes in invalid (has the same number more than once)
        * the function throws indirectly SameNumberInARowOrColOrCubeException.
        */
        private void rowColCubeIntegrity(bool checkRowsOrCols, Board sudokuBoard)
        {
            // checkRowsOrCols is true when we want to check integrity of 
            // rows or cols and false when we want to check integrity of cubes.
            for (int RowOrColOfCube = 0; RowOrColOfCube < sudokuBoard.sqrtOfSizeOfBoard; RowOrColOfCube++)
            {
                for (int RowOrColInCube = 0; RowOrColInCube < sudokuBoard.sqrtOfSizeOfBoard; RowOrColInCube++)
                {
                    if (checkRowsOrCols)
                    {
                        boardIntegrityOfRowOrCol(Type.col, RowOrColOfCube, RowOrColInCube, sudokuBoard);
                        boardIntegrityOfRowOrCol(Type.row, RowOrColOfCube, RowOrColInCube, sudokuBoard);
                    }
                    else
                    {
                        int cubeNumber = RowOrColOfCube * sudokuBoard.sqrtOfSizeOfBoard + RowOrColInCube;
                        boardIntegrityOfCube(cubeNumber, sudokuBoard);
                    }
                }
            }
        }

        /*
       * Summary:
       * The function is checking the integrity
       * of the rows and cols in the board "sudokuBoard"
       * 
       * Input:
       * sudokuBoard - A sudoku board.
       * type - the type we want to check
       * in this case col or row
       * RowOrColOfCube - the row or col of the cube
       * we want to check in
       * RowOrColInCube - the row or col in the cube
       * we want to check in
       * 
       * OutPut:
       * None.
       * if one of the rows or cols in invalid (has the same number more than once)
       * the function throws indirectly SameNumberInARowOrColOrCubeException.
       */
        private void boardIntegrityOfRowOrCol(Type type, int RowOrColOfCube, int RowOrColInCube, Board sudokuBoard)
        {
            List<int> CountNumberInRowAndCol = new List<int>();

            int[] forParams = Calculations.forParameters(RowOrColOfCube, type == Type.col, sudokuBoard.sqrtOfSizeOfBoard);
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
            {
                sudokuBoard.board[forParams[1]].rowOrColOrCubeIntegrity(type, CountNumberInRowAndCol, RowOrColInCube, RowOrColOfCube);
            }
        }

        /*
         * Summary:
         * The function is checking the integrity
         * of the cubes in the board "sudokuBoard"
         *  
         * Input:
         * sudokuBoard - A sudoku board.
         * cubeNumber - the number of the 
         * cube we want to check
         * 
         * OutPut:
         * None.
         * if one of the cubes in invalid (has the same number more than once)
         * the function throws indirectly SameNumberInARowOrColOrCubeException.
         */
        private void boardIntegrityOfCube(int cubeNumber, Board sudokuBoard)
        {
            List<int> CountNumberInCube = new List<int>();
            sudokuBoard.board[cubeNumber].rowOrColOrCubeIntegrity(Type.cube, CountNumberInCube);
        }

        /*
        * Summary:
        * The function is checking the integrity of 
        * the cells in the board "sudokuBoard"
        *  
        * Input:
        * sudokuBoard - A sudoku board.
        * 
        * OutPut:
        * None.
        *  if the cell's number is invalid (not suitable for the board)
        * the function throws indirectly InvalidNumberException.
        */
        private void boardIntegrityOfCells( Board sudokuBoard)
        {
            for (int cubeNumber = 0; cubeNumber < sudokuBoard.sizeOfBoard; cubeNumber++)
            {
                sudokuBoard.board[cubeNumber].cellsInCubeIntegrity();
            }
        }
    }
}

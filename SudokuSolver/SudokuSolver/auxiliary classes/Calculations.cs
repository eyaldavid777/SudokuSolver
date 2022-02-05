using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public static class Calculations
    {
        //Calculations static class help us calculate long equations
        // of indexes in board or mathematical calculations.
        //

        /*
        * Summary:
        * The functionc calculates the cube number of the index.
        *  
        * Input:
        * index - index in board.
        * sizeOfBoard - the size of the board (size of row/col/cube)
        * 
        * OutPut:
        * The functions returns the cube number of the index.
        */
        public static int getCubeNumberByIndex(int index,int sizeOfBoard)
        {
            int SqrtOfSizeOfBoard = sqrt(sizeOfBoard);
            return index / sizeOfBoard / SqrtOfSizeOfBoard * SqrtOfSizeOfBoard + index % sizeOfBoard / SqrtOfSizeOfBoard;
        }

        /*
        * Summary:
        * The functionc calculates the row or col in cube
        * by index in board.
        *  
        * Input:
        * col - true for col and false for row.
        * It tells the function if we want to get
        * the row or the col of the index.
        * index - index in board.
        * sizeOfBoard - the size of the board (size of row/col/cube)
        * 
        * OutPut:
        * The functions returns the row or col in cube by index in board.
        */
        public static int getRowOrColInCubeByIndexInBoard(bool col, int index, int sizeOfBoard)
        {
            return getRowOrCol(col, index, sizeOfBoard) % sqrt(sizeOfBoard);
        }

        /*
        * Summary:
        * The functionc calculates the index in cube
        * by index in board.
        *  
        * Input:
        * indexInBoard - index in board.
        * sizeOfBoard - the size of the board (size of row/col/cube)
        * 
        * OutPut:
        * The functions returns the index in cube by index in board.
        */
        public static int getIndexInCubeByIndexInBoard(int indexInBoard, int sizeOfBoard)
        {
            return getRowOrColInCubeByIndexInBoard(false, indexInBoard, sizeOfBoard) * sqrt(sizeOfBoard) + getRowOrColInCubeByIndexInBoard(true, indexInBoard, sizeOfBoard);              
        }

        /*
        * Summary:
        * The functionc calculates row or col of index1.
        *  
        * Input:
        * col - true for col and false for row.
        * It tells the function if we want to get
        * the row or the col of the index1.
        * index1 - index in board.
        * size - sizeOfBoard or sqrtOfSizeOfBoard depends
        * on what you want to check.
        * 
        * OutPut:
        * The functions returns the row or col of number1.
        */
        public static int getRowOrCol(bool col, int index1, int size)
        {
            if (col)
                return index1 % size;
            return index1 / size;
        }

        /*
        * Summary:
        * The functionc calculates the sqrt of number.
        *  
        * Input:
        * number - a number.
        * 
        * OutPut:
        * The functions returns the sqrt of number.
        */
        public static int sqrt(int number)
        {
            return (int)Math.Sqrt(number);
        }

        /*
        * Summary:
        * The functionc calculates the square of number.
        *  
        * Input:
        * number - a number.
        * 
        * OutPut:
        * The functions returns the square of number.
        */
        public static int squared(int number)
        {
            return (int)Math.Pow(number,2);
        }

        /*
        * Summary:
        * The functionc checks if "number" is an integer.
        *  
        * Input:
        * number - a number.
        * 
        * OutPut:
        * The functions returns true if "number" is an integer.Otherwise it returns false.
        */
        public static bool isInt(double number)
        {
            if (number == Math.Truncate(number))
                return true;
            return false;
        }

        /*
       * Summary:
       * The functionc sorts "dict" by value - by the length of the list in ascending order
       *  
       * Input:
       * None.
       * 
       * OutPut:
       * The functions returns the sorted "dict" by value - by the length of the list in ascending order.
       */
        public static Dictionary<int, List<int>> sortByValue(Dictionary<int, List<int>> dict)
        {
            return dict.OrderBy(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value);
        }

        /*
        * Summary:
        * The functionc sorts "dict" by value - by the length of the list in descending order
        *  
        * Input:
        * None.
        * 
        * OutPut:
        * The functions returns the sorted "dict" by value - by the length of the list in descending order.
        */
        public static Dictionary<int, List<int>> sortByValueDescending(Dictionary<int, List<int>> dict)
        {
            return dict.OrderByDescending(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value);
        }

        /*
       * Summary:
       * The function compares the two lists it receives.
       * 
       * Input:
       * firstIndexsesList - A list of two indexses in the board.
       * secondIndexsesList - A list of two indexses in the board.
       * 
       * OutPut:
       * The function returns true if the two lists it receives are the same. Otherwise it returns false.
       */
        public static bool compareListsOfIndexes(List<int> firstIndexsesList, List<int> secondIndexsesList)
        {
            return (firstIndexsesList.ElementAt(0) == secondIndexsesList.ElementAt(0)) && (firstIndexsesList.ElementAt(1) == secondIndexsesList.ElementAt(1));
        }

        /*
      * Summary:
      * forParams helps us to scan cols and rows
      * in the board.
      *  
      * Input:
      * colOrRowIndex - cor or row we want to scan.
      * col -  true for col and false for row.
      * It tells the function if colOrRowIndex
      * means row in cube or col. 
      * sqrtOfSizeOfBoard - sqrt of the size of the board (size of a row/col of cube )
      * 
      * OutPut:
      * The functions returns addToIndexInCube,indexInCube,endOfColOrRow.
      */
        public static int[] forParameters(int colOrRowIndex, bool col, int sqrtOfSizeOfBoard)
        {
            // forParams - addToIndexInCube,indexInCube,endOfColOrRow
            int[] forParams = new int[3];
            if (col)
            {
                forParams[0] = sqrtOfSizeOfBoard;
                forParams[1] = colOrRowIndex;
                forParams[2] = colOrRowIndex + sqrtOfSizeOfBoard * (sqrtOfSizeOfBoard - 1) + 1;
                return forParams;
            }
            forParams[0] = 1;
            forParams[1] = colOrRowIndex * sqrtOfSizeOfBoard;
            forParams[2] = forParams[1] + sqrtOfSizeOfBoard;
            return forParams;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    // enum of the types we want to check
    public enum Type { row, col, cube }
    public class Board : BoardIntegrity,ISudokuBoard
    {
        // the class board responsible of print the board and
        // give information about the board to Solver and the Cube.

        // the board of the game - an array of "ISudokuCube" 
        public ISudokuCube[] board { get;  }
        // The dictionary placesOfNumbers contains keys of numbers and for each number
        //  (the value) it contains a list of indexes of where the number can be located
        // Each time we check the indexes of where the number can be located be a different type
        public Dictionary<int, List<int>> placesOfNumbers { get; set; }
        // the size of the board (size of row/col/cube)
        public int sizeOfBoard { get; }
        // sqrt of size of the board (size of a row/col of cube )
        public int sqrtOfSizeOfBoard { get; }
        // IntersectionNumbers contains two dictionaries one for row and 
        // one for col.Each dictionary contains indexes of rows or cols and 
        // for each index a list of numbers that you found out that
        //  in their cube they are in the same row or col (Intersection tactics)
        // we are useing intersectionNumbers in the first step
        public IntersectionNumbers intersectionNumbers { get; set; }
        // tells if we are in the first dtep of solving
        public bool firstStep { get; set; }
        // every Board has it's own sudokuSolver that solve the sudoku board
        public ISudokuSolver sudokuSolver { get; set; }
        // pointer to function the functions nakedSingle or hiddenSubsets
        // - used in the function repetition in this class
        public delegate bool Ptr();


        /*
         * Summary:
         * The function initialized an instance of the Board class.
         * 
         * Input:
         * numbersInBoard - A string which represents the board.
         * checkBoardIntegrity - telling the constructor whether
         * to check the integrity of the board or not.
         * (checkBoardIntegrity is by default true).
         * 
         * OutPut:
         * None.
         * if the size of the board is invalid the function
         * throws indirectly InvalidBoardSizeException.
         * if one of the cubes in invalid (has the same number more than once)
         * the function throws indirectly SameNumberInARowOrColOrCubeException.
         * if one of the chars in numbersInBoard is invalid (not suitable for the board)
         * the function throws indirectly InvalidNumberException.
         */
        public Board(string numbersInBoard, bool checkBoardIntegrity = true)
        {
            if (checkBoardIntegrity)
                SizeOfBoardIntegrity(numbersInBoard);
            sizeOfBoard = Calculations.sqrt(numbersInBoard.Length);
            sqrtOfSizeOfBoard = Calculations.sqrt(sizeOfBoard);
            sudokuSolver = new Solver(this);
            board = new Cube[sizeOfBoard];
            Initialize(numbersInBoard, checkBoardIntegrity);
            if (checkBoardIntegrity)
                boardIntegrity(false);
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
         */
        public void boardIntegrity(bool checkCells = true)
        {
            base.boardIntegrity(this, checkCells);
        }

        /*
         * Summary:
         * The function initialized some properties of the new instance.
         * 
         * Input:
         * numbersInBoard - A string which represents the board.
         * checkBoardIntegrity - telling the function whether
         * to check the integrity of the board or not.
         * 
         * OutPut:
         * None.
         * if one of the chars in numbersInBoard is invalid (not suitable for the board)
         * the function throws indirectly InvalidNumberException.
         */
        private void Initialize(string numbersInBoard, bool checkBoardIntegrity)
        {
            firstStep = true;
            placesOfNumbers = new Dictionary<int, List<int>>();
            intersectionNumbers = new IntersectionNumbers();
            InitializePlacesOfNumbers();
            InitializeBorad(numbersInBoard, checkBoardIntegrity);
        }

         /*
         * Summary:
         * The function initialized the board, makes
         * new instanses of cubes in the board.
         * 
         * Input:
         * numbersInBoard - A string which represents the board.
         * checkBoardIntegrity - telling the function whether
         * to check the integrity of the board or not.
         * 
         * OutPut:
         * None.
         * if one of the chars in numbersInBoard is invalid (not suitable for the board)
         * the function throws indirectly InvalidNumberException.
         */
        public void InitializeBorad(string numbersInBoard, bool checkBoardIntegrity)
        {
            for (int numOfCube = 0; numOfCube < board.Length; numOfCube++)
            {
                board[numOfCube] = new Cube(numbersInBoard, numOfCube, this , checkBoardIntegrity);
            }
        }

        /*
         * Summary:
         * The function initialized the dictionary
         * placesOfNumbers with keys from one to sizeOfBoard
         * and for each key (value) a new list.
         * 
         * Input:
         * None.
         * 
         * OutPut:
         * None.
         */
        public void InitializePlacesOfNumbers()
        {
            for (int numOfCube = 0; numOfCube < sizeOfBoard; numOfCube++)
            {
                if (placesOfNumbers.ContainsKey(numOfCube + 1))
                    placesOfNumbers[numOfCube + 1] = new List<int>();
                else
                    placesOfNumbers.Add(numOfCube + 1, new List<int>());
            }
        }

        /*
         * Summary:
         * The function initialized the dictionary placesOfNumbers like that:
         * The keys are optional numbers in the row or col (rowOrColInBoard) and for each optional 
         * number it contains a list of indexes of where the optional number can be located (value).
         * 
         * Input:
         * col - true for col and false for row.
         * It tells the function if rowOrColInBoard
         * means row in board or col in board.
         * rowOrColInBoard - a row or col int 
         * the board.
         * 
         * OutPut:
         * None.
         */
        public void initializePlacesOfNumbersFromRowOrCol(bool col, int rowOrColInBoard)
        {
           // forParams structure: addToIndexInCube,indexInCube,endOfColOrRow
            int[] forParams = Calculations.forParameters(rowOrColInBoard / sqrtOfSizeOfBoard, col, sqrtOfSizeOfBoard);
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
                board[forParams[1]].copyOptionsOfARowOrAColInCube(col, rowOrColInBoard % sqrtOfSizeOfBoard);
        }

        /*
         * Summary:
         * The function copy the Board (this) into
         * a new instance of Board.
         * 
         * Input:
         * None.
         * 
         * OutPut:
         * The function returns the cloned Board.
         */
        public Board copyBoard()
        {
            string numbersInBoard = boardString();
            Board cloneBoard = new Board(numbersInBoard, false);
            return cloneBoard;
        }

        /*
        * Summary:
        * The function prints something according to the parameters it recives.
        *  
        * Input:
        * DarkBlue - if DarkBlue true, print what you want in dark blue.
        * col -  true for col and false for row.
        * It tells the function if you want to print "|" (true) or "_" (false) 
        * withSpaces - tells the function if to print spaces before "|" or "|"
        * 
        * OutPut:
        * None.
        */
        public void printAColOrRow(bool col, bool DarkBlue, bool withSpaces)
        {
            if (DarkBlue)
                Console.ForegroundColor = ConsoleColor.DarkBlue;
            if (col)
                if (withSpaces)
                    System.Console.Write("   |");
                else
                    System.Console.Write("|");
            else
                 if (withSpaces)
                System.Console.Write(" ___");
            else
                System.Console.Write("___");
            if (DarkBlue)
                Console.ForegroundColor = ConsoleColor.White;
        }

        /*
         * Summary:
         * The function creates a string of the board.
         * 
         * Input:
         * None.
         * 
         * OutPut:
         * The function returns the string of the board.
         */
        public string boardString()
        {
            string boardString = string.Empty;
            for (int rowIndex =0; rowIndex < sizeOfBoard; rowIndex++)
            {
                for(int cubeCol =0; cubeCol < sqrtOfSizeOfBoard; cubeCol++)
                {
                    int cubeNumber = rowIndex / sqrtOfSizeOfBoard * sqrtOfSizeOfBoard + cubeCol;
                    boardString += board[cubeNumber].rowOrCulIncubeString(rowIndex % sqrtOfSizeOfBoard);
                }
            }
            return boardString;
        }

        /*
        * Summary:
        * The function prints a dark blue line of "_" to the console.
        * 
        * Input:
        * None.
        * 
        * OutPut:
        * None.
        */
        private void printALine()
        {
            //printAColOrCol parameters: bool col, bool DarkBlue, bool withSpaces
            System.Console.Write("   ");
            for (int i = 0; i < sizeOfBoard; i++)
                printAColOrRow(false, true, true);
            System.Console.WriteLine();
        }

        /*
        * Summary:
        * The function prints a line of cols to the console.
        * 
        * Input:
        * Line - Line is true if this line has "_" between "|"
        * and false if this line has spaces between "|".
        * 
        * OutPut:
        * None.
        */
        private void printCols(bool Line, bool DarkBlue)
        {
            //printAColOrCol parameters: bool col, bool DarkBlue, bool withSpaces
            for (int i = 0; i < sizeOfBoard + 1; i++)
            {
                if (i % sqrtOfSizeOfBoard == 0)
                {
                    if (i == 0)
                    {
                        // print the first "   |" in the line in DarkBlue
                        printAColOrRow(true, true, true);
                    }
                    else
                    {
                        if (Line)
                        {
                            // print "___" (in DarkBlue if DarkBlue is true)
                            printAColOrRow(false, DarkBlue, false);
                            // print  "|"  in DarkBlue
                            printAColOrRow(true, true, false);
                        }
                        else
                        {
                            // print  "   |"  in DarkBlue
                            printAColOrRow(true, true, true);
                        }
                    }
                }
                else
                {
                    if (Line)
                    {
                        // print "___" (in DarkBlue if DarkBlue is true)
                        printAColOrRow(false, DarkBlue, false);
                        // print "|"
                        printAColOrRow(true, false, false);
                    }
                    else
                    {
                        // print "   |" 
                        printAColOrRow(true, false, true);
                    }
                }
            }
            System.Console.WriteLine();
        }

        /*
        * Summary:
        * The function prints the board to the console.
        * 
        * Input:
        * None
        * 
        * OutPut:
        * None.
        */
        public void print()
        {
            printALine();
            for (int rowOfCubes = 0; rowOfCubes < sqrtOfSizeOfBoard; rowOfCubes++)
            {
                for (int rowInCube = 0; rowInCube < sqrtOfSizeOfBoard; rowInCube++)
                {
                    printCols(false, false);
                    printAColOrRow(true, true, true);
                    for (int colOfCubes = 0; colOfCubes < sqrtOfSizeOfBoard; colOfCubes++)
                    {
                        // print a line with information on the cells
                        board[rowOfCubes * sqrtOfSizeOfBoard + colOfCubes].print(rowInCube);
                    }
                    System.Console.WriteLine();
                    if ((rowInCube + 1) % sqrtOfSizeOfBoard == 0)
                        printCols(true, true);
                    else
                        printCols(true, false);
                }
            }
            Console.WriteLine("\n");
        }

        /*
        * Summary:
        * The function checks if the cube is worth checking
        * for the number mostCommonNumber.
        * A cube is worth checking if their is at least one other cube with the same
        * row or col of the cube that contains the same number as mostCommonNumber 
        * and at the col or row of that number the cube is not full. 
        * Otherwise the board id not worth checking,
        *  
        * 
        * Input:
        * cubeNumber - the cube number we want to check.
        * mostCommonNumber - the number on which we want 
        * to check if the cube is worth checking.
        * 
        * OutPut:
        * The function returns true if the cube is worth checking.Otherwise it returns false.
        */
        public bool isTheCubeWorthChecking(int cubeNumber, int mostCommonNumber)
        {
            foreach (int indexInBoard in placesOfNumbers[mostCommonNumber])
                if (Calculations.getCubeNumberByIndex(indexInBoard, sizeOfBoard) / sqrtOfSizeOfBoard == cubeNumber / sqrtOfSizeOfBoard)
                {
                   if (!board[cubeNumber].isRowOrColFull(false, Calculations.getRowOrColInCubeByIndexInBoard(false,indexInBoard,sizeOfBoard)))
                        return true;
                }
                else
                {
                    if (Calculations.getCubeNumberByIndex(indexInBoard, sizeOfBoard) % sqrtOfSizeOfBoard == cubeNumber % sqrtOfSizeOfBoard)
                        if (!board[cubeNumber].isRowOrColFull(true, Calculations.getRowOrColInCubeByIndexInBoard(true,indexInBoard, sizeOfBoard)))
                            return true;
                }
            return false;
        }

        /*
        * Summary:
        * The function checks if the number "number"
        * appears at the "colOrRowIndexInCube" (depending on "col")
        * in the cubes at the same row or col of "cubeNumber"
        * at least one time, not including his cube (cubeNumber).
        *  
        * Input:
        * cubeNumber - the cube where the "number" is located.
        * col -  true for col and false for row.
        * It tells the function if colOrRowIndexInCube
        * means row in cube or col in cube.
        * colOrRowIndexInCube - the row or col in cube
        * we want to check.
        * forParams - helps us to scan cols and rows
        * in the board.
        * Its structure: addToIndexInCube,indexInCube,endOfColOrRow
        * number - the number we want to check.
        * 
        * OutPut:
        * The function returns true if the condition in Summary is true. Otherwise it returns false.
        */
        private bool isNumberInRowOrCol(int cubeNumber, bool col, int colOrRowIndexInCube, int[] forParams, int number)
        {
            int colOrRowIndexInBoard = Calculations.getRowOrCol(col, cubeNumber, sqrtOfSizeOfBoard) * sqrtOfSizeOfBoard + colOrRowIndexInCube;
            if (intersectionNumbers.isNumberInRowOrColInNumbersNotInBoard(col, colOrRowIndexInBoard, number))
                return true;
            // forParams - addToIndexInCube,indexInCube,endOfColOrRow
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
                if (forParams[1] != cubeNumber)
                    if (board[forParams[1]].isNumberInRowOrColInCube(col, colOrRowIndexInCube, number))
                        return true;
            return false;
        }

        /*
        * Summary:
        * The function checks if the index "indexOfNumberInBoard"
        * can contains the number "number"
        *  
        * Input:
        * indexOfNumberInBoard - the index where we want to 
        * check if the number "number" can be placed at
        * number - the number we want to check.
        * 
        * OutPut:
        * The function returns true if the index "indexOfNumberInBoard" can contains the number "number". Otherwise it returns false.
        */
        public bool isPossibleIndexToNumber(int indexOfNumberInBoard, int number)
        {
            int indexOfNumberInCube = Calculations.getIndexInCubeByIndexInBoard(indexOfNumberInBoard, sizeOfBoard);
            int cubeNumber = Calculations.getCubeNumberByIndex(indexOfNumberInBoard,sizeOfBoard);
            int rowOfCubeNumber = Calculations.getRowOrCol(false, cubeNumber , sqrtOfSizeOfBoard);
            int colOfCubeNumber = Calculations.getRowOrCol(true,cubeNumber , sqrtOfSizeOfBoard);
            int[] forParams = Calculations.forParameters(rowOfCubeNumber, false, sqrtOfSizeOfBoard);

            if (isNumberInRowOrCol(cubeNumber, false, Calculations.getRowOrCol(false,indexOfNumberInCube, sqrtOfSizeOfBoard), forParams, number))
                return false;

            forParams = Calculations.forParameters(colOfCubeNumber, true, sqrtOfSizeOfBoard);

            if (isNumberInRowOrCol(cubeNumber, true, Calculations.getRowOrCol(true,indexOfNumberInCube,sqrtOfSizeOfBoard), forParams, number))
                return false;
            return true;
        }

        /*
       * Summary:
       * The function delete the number "number" from
       * row or col (depending on "col")
       *  
       * Input:
       * col - true for col and false for row.
       * It tells the function from where
       * to delete "number" , row or col.
       * indexOfNumberInBoard - the index where number is at.
       * number - the number we want to delete from row or col.
       * 
       * OutPut:
       * None.
       */
        public void deleteNumberFromRowOrCol(bool col, int indexOfNumberInBoard, int number)
        {
            int cubeNumber =Calculations.getCubeNumberByIndex(indexOfNumberInBoard,sizeOfBoard);
            int rowOrColOfCube = Calculations.getRowOrCol(col, cubeNumber, sqrtOfSizeOfBoard);
            int rowOrColInCube = Calculations.getRowOrColInCubeByIndexInBoard(col, indexOfNumberInBoard, sizeOfBoard);
            int[] forParams = Calculations.forParameters(rowOrColOfCube, col, sqrtOfSizeOfBoard);

            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
                if (forParams[1] != cubeNumber)
                    board[forParams[1]].deleteNumberFromRowOrColInCube(col, rowOrColInCube, number);
        }

        /*
       * Summary:
       * The function put the number "number" 
       * at "indexInBoard" and delte his other options from row 
       * and col (depending on "deletFromRow" and "deletFromCol")
       *  
       * Input:
       * indexInBoard - the index in the board where we wiil put the number at.
       * number - the number we solved.
       * deletFromRow - tells the function whether to delete the other options of "number"
       * from his row.
       * deletFromCol - tells the function whether to delete the other options of "number"
       * from his col.
       * 
       * OutPut:
       * None.
       */
        public void putKnownNumberAndDeletOptions(int indexInBoard, int number, bool deletFromRow, bool deletFromCol)
        {
            int indexInCube = Calculations.getIndexInCubeByIndexInBoard(indexInBoard,sizeOfBoard);
            board[Calculations.getCubeNumberByIndex(indexInBoard, sizeOfBoard)].putTheNumberAndDeletOptions(indexInCube, number, deletFromRow, deletFromCol);
            // only if we are in the "firstStepOfSolving", there is a chance that mostCommonNumber
            // will already be removed from placesOfNumbers so we need to check if placesOfNumbers contains it
            if (firstStep && placesOfNumbers.ContainsKey(number))
            {
                // add to mostCommonNumber in placesOfNumbers his new place
                placesOfNumbers[number].Add(indexInBoard);
            }
        }

        /*
        * Summary:
        * The function checks how many options the number "number"
        * has in a cube.
        * If the number has one option - put it.
        * else check Intersection tactics on his options
        *  
        * Input:
        * optionsInCubeByBoardIndex - list of options (indexes) of
        * the number "number" in a cube.
        * number - the number we are checking.
        * 
        * OutPut:
        * None.
        */
        public void hiddenSingleOfCube(List<int> optionsInCubeByBoardIndex, int number)
        {
            if (optionsInCubeByBoardIndex.Count == 1)
            {
                // put the number in it's only place and delete the options of the number
                // in the cells with the same row and col
                putKnownNumberAndDeletOptions(optionsInCubeByBoardIndex.ElementAt(0), number, true, true);
            }
            else
            {
                // if the options of 'mostCommonNumber' in the same row or col you can
                //  delete mostCommonNumber's options from the rest of the row or col
                checkIntersection(optionsInCubeByBoardIndex, number);
            }
        }


        /*
        * Summary:
        * The function checks if the options of the number "number"
        * in the same row or col by "isOptionsInTheSameRowOrCol" below
        * and responds accordingly. 
        *  
        * Input:
        * optionsInCubeByBoardIndex - list of options (indexes) of
        * the number "number" in a cube.
        * number - the number we are checking.
        * 
        * OutPut:
        * None.
        */
        private void checkIntersection(List<int> optionsInCubeByBoardIndex, int number)
        {
            if (optionsInCubeByBoardIndex.Count <= sqrtOfSizeOfBoard && optionsInCubeByBoardIndex.Count > 0)
                switch (isOptionsInTheSameRowOrCol(optionsInCubeByBoardIndex))
                {
                    case -1: // the options dont have the same row or col
                        break;
                    case 1:  // the options have the same row
                        theOptionsInTheSameRowOrCol(false, optionsInCubeByBoardIndex, number,Calculations.getRowOrCol(false,optionsInCubeByBoardIndex.ElementAt(0) ,sizeOfBoard));
                        break;
                    case 2: // the options have the same col
                        theOptionsInTheSameRowOrCol(true, optionsInCubeByBoardIndex, number, Calculations.getRowOrCol(true,optionsInCubeByBoardIndex.ElementAt(0),sizeOfBoard));
                        break;
                }
        }

        /*
        * Summary:
        * The function checks if the options of the number "number"
        * in the same row or col.
        *  
        * Input:
        * optionsInCubeByBoardIndex - list of options (indexes) of
        * the number "number" in a cube.
        * 
        * OutPut:
        * The functions returns:
        * 1 - the options have the same row.
        * 2 - the options have the same col.
        * -1 - the options dont have the same row or col.
        */
        private int isOptionsInTheSameRowOrCol(List<int> numberOfOptionsInCube)
        {
            // comparing all the rows and cols to the row and col of the first option
            int rowOfFirst = Calculations.getRowOrCol(false,numberOfOptionsInCube.ElementAt(0) ,sqrtOfSizeOfBoard);
            int colOfFirst = numberOfOptionsInCube.ElementAt(0) % sqrtOfSizeOfBoard;
            bool sameRow = true;
            bool sameCol = true;
            for (int i = 1; i < numberOfOptionsInCube.Count; i++)
            {
                if (sameRow && numberOfOptionsInCube.ElementAt(i) / sqrtOfSizeOfBoard != rowOfFirst)
                {
                    // There is at least one option with a diffrent row
                    sameRow = false;
                }
                if (sameCol && numberOfOptionsInCube.ElementAt(i) % sqrtOfSizeOfBoard != colOfFirst)
                {
                    // There is at least one option with a diffrent col
                    sameCol = false;
                }
                if (!sameRow && !sameCol)
                {
                    // There is at least one option with a diffrent row
                    // and at least one option with a diffrent col
                    return -1;
                }
            }
            return sameRow ? 1 : 2;
        }

        /*
        * Summary:
        * The function call "deleteNumberFromRowOrCol" because we
        * found out the options of "number" are in the same col or row.
        * And if we are in the first step - 
        * Add to IntersectionNumbers the new "known number 
        * not in board" we found.
        *  
        * Input:
        * col - true for col and false for row.
        * It tells the function if the options
        * in the same row or col.
        * optionsInCubeByBoardIndex - list of options (indexes) of
        * the number "number" in a cube.
        * number - the number we are checking.
        * theSameRowOrCol - the same row or col all the options are.
        * 
        * OutPut:
        * None.
        */
        private void theOptionsInTheSameRowOrCol(bool col, List<int> optionsInCubeByBoardIndex, int number, int theSameRowOrCol)
        {
            if (firstStep)
            {
                Dictionary<int, List<int>> numbersInRowOrCol;
                if (col)
                    numbersInRowOrCol = intersectionNumbers.numbersInCols;
                else
                    numbersInRowOrCol = intersectionNumbers.numbersInRows;
                // add to IntersectionNumbers the new "known number not in board" we found.

                if (!numbersInRowOrCol.ContainsKey(theSameRowOrCol))
                {
                    numbersInRowOrCol.Add(theSameRowOrCol, new List<int>());
                }
                numbersInRowOrCol[theSameRowOrCol].Add(number);
            }
            int firstIndexInBoard = optionsInCubeByBoardIndex.ElementAt(0);
            deleteNumberFromRowOrCol(col, firstIndexInBoard, number);
        }

        /*
       * Summary:
       * The function checks if "rowOrColInBoard" has a number
       * with one option in it.
       *  
       * Input:
       * type - the type we want to check
       * in this case col or row.
       * rowOrColInBoard - A row or col in the board.
       * 
       * OutPut:
       * The function returns true if it finds a number with one option
       * in "rowOrColInBoard".Otherwise it returns false.
       * if a number has zero options in "rowOrColInBoard"
       * the function throws indirectly NoPlaceForANumberInARowOrColOrCubeException.
       */
        public bool hiddenSingleOfRowAndCol(Type type, int rowOrColInBoard)
        {
            bool theBoardHasChanged = false;
            for (int indexOfMissingNumberInDic = 0; indexOfMissingNumberInDic < placesOfNumbers.Keys.Count; indexOfMissingNumberInDic++)
            {
                int MissingNumberInDic = placesOfNumbers.Keys.ElementAt(indexOfMissingNumberInDic);
                if (isHiddenSingleOfRowOrCol(type, MissingNumberInDic, rowOrColInBoard))
                {
                    theBoardHasChanged = true;
                    indexOfMissingNumberInDic = -1;
                }
            }
            return theBoardHasChanged;
        }

        /*
       * Summary:
       * The function checks if MissingNumberInDic has one option in "rowOrColInBoard".
       *  
       * Input:
       * type - the type we want to check
       * in this case col or row.
       * missingNumberInDic - An unsolved number in "rowOrColInBoard". 
       * rowOrColInBoard - A row or col in the board.
       * 
       * OutPut:
       * The function returns true if missingNumberInDic has one option in 
       * "rowOrColInBoard".Otherwise it returns false.
       * if missingNumberInDic has zero options in "rowOrColInBoard"
       * the function throws NoPlaceForANumberInARowOrColOrCubeException.
       */
        private bool isHiddenSingleOfRowOrCol(Type type, int missingNumberInDic, int rowOrColInBoard)
        {
            // if MissingNumberInDic has only one place in rowOrColInBoard then
            if (placesOfNumbers[missingNumberInDic].Count == 1)
            {
                // indexInBoard = only option of MissingNumberInDic in rowOrColInBoard
                int indexInBoard = placesOfNumbers[missingNumberInDic].ElementAt(0);
                int indexInCube = Calculations.getIndexInCubeByIndexInBoard(indexInBoard, sizeOfBoard);
                int indexOfCube = Calculations.getCubeNumberByIndex(indexInBoard, sizeOfBoard);

                if (board[indexOfCube].getOptionalNumbers(indexInCube).Count == 1)
                    return false;

                List<int> remainingOptions = new List<int>();
                remainingOptions.Add(missingNumberInDic);
                board[indexOfCube].leaveInCellOnlyTheListOptions(indexInCube, remainingOptions);
                return true;
            }
            else
                if (placesOfNumbers[missingNumberInDic].Count == 0)
                    throw new NoPlaceForANumberInARowOrColOrCubeException("the " + type + " at index " + rowOrColInBoard + " cannot conatin the number " + missingNumberInDic);
            return false;
        }

        /*
        * Summary:
        * The function repeats the function repetitionContent 
        * until repetitionContent returns false.
        *  
        * Input:
        * repetitionContent - The functions nakedSingle or hiddenSubsets
        * 
        * OutPut:
        * The function returns true if the function repetitionContent
        * returned true at least one time.Otherwise it returns false.
        */
        public bool repetition(Ptr repetitionContent)
        {
            int numbersOfLopps = -1;
            do
            {
                numbersOfLopps++;
            } while (repetitionContent());
            return numbersOfLopps > 0;
        }     
    }
}

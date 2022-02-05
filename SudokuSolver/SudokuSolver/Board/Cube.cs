using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Cube : ISudokuCube
    {
        // the class cube responsible of print a line with information
        // and giving information about the cube to Solver and to the board.

        //a cube in the board - an array of "ICell"
        public ICell[] cube { get; set; }

        // the board where the cube is
        public ISudokuBoard inBoard { get; }

        //the number of the cube
        public int cubeNumber { get; }

        /*
         * Summary:
         * The function initialized an instance of the Board class.
         * 
         * Input:
         * numbersInBoard - A string which represents the board.
         * numOfCube - the number of the cube.
         * InBoard - the board where the cube is.
         * checkBoardIntegrity - telling the constructor whether
         * to check the integrity of the cells in the cube or not.
         * 
         * OutPut:
         * None.
         * if one of the chars in numbersInBoard is invalid (not suitable for the board)
         * the function throws indirectly InvalidNumberException.
         */
        public Cube(string numbersInBoard, int numOfCube, ISudokuBoard InBoard,bool checkCellIntegrity)
        {
            inBoard = InBoard;
            cubeNumber = numOfCube;
            cube = new ICell[inBoard.sizeOfBoard];
            Initialize(numbersInBoard, checkCellIntegrity);
        }

        /*
         * Summary:
         * The function initialized the cube, makes
         * new instanses of cells in the cube.
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
        private void Initialize(string numbersInBoard, bool checkCellIntegrity)
        {
            for (int indexInCube = 0; indexInCube < cube.Length; indexInCube++)
            {
                int index = indexInBoard(indexInCube);
                if (numbersInBoard[index] == '0')
                {
                    // adding the empty cell to the list of empty cells indexes in sudokuSolver
                    inBoard.sudokuSolver.emptyCellsIndexes.Add(index);
                    cube[indexInCube] = new Cell(index);
                }
                else
                    cube[indexInCube] = new Cell(inBoard.placesOfNumbers, numbersInBoard[index], index, checkCellIntegrity,inBoard.sizeOfBoard);
            }
        }

        /*
         * Summary:
         * The function clalculates the index in board bt the index
         * in cube. Because the function is complicated, I used it only one
         * time when I initialized the cube.
         * 
         * Input:
         * indexInCube - An index in "this" cube.
         * 
         * OutPut:
         * The function returns the index in board by the index in cube.
         */
        private int indexInBoard(int indexInCube)
        {
            int sqrtOfBoardSize = inBoard.sqrtOfSizeOfBoard;
            return cubeNumber / sqrtOfBoardSize * inBoard.sizeOfBoard * sqrtOfBoardSize
                         + indexInCube % sqrtOfBoardSize + inBoard.sizeOfBoard * (indexInCube / sqrtOfBoardSize)
                         + cubeNumber % sqrtOfBoardSize * sqrtOfBoardSize;
        }

        /*
        * Summary:
        * The function prints the importannt lines in the board print.
        * it prints th lines with the information on the cells (the numbers).
        * 
        * Input:
        * None
        * 
        * OutPut:
        * None.
        */
        public void print(int rowInCube)
        {
            //printAColOrCol parameters: bool col, bool DarkBlue, bool withSpaces
            for (int colInCube = 0; colInCube < inBoard.sqrtOfSizeOfBoard; colInCube++)
            {
                bool withDarkBlue = colInCube == inBoard.sqrtOfSizeOfBoard - 1;
                if (cube[rowInCube * inBoard.sqrtOfSizeOfBoard + colInCube].isSolved())
                {
                    char number = (char)(cube[rowInCube * inBoard.sqrtOfSizeOfBoard + colInCube].number + '0');
                    System.Console.Write(" {0} ", number);
                    //print  "|" (in DarkBlue if withDarkBlue is true)
                   inBoard.printAColOrRow(true, withDarkBlue, false);
                }
                else
                {
                    //print  "   |" (in DarkBlue if withDarkBlue is true)
                    inBoard.printAColOrRow(true, withDarkBlue, true);
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        /*
         * Summary:
         * The function initialized the dictionary placesOfNumbers like that:
         * The keys are optional numbers in the cube we at and for each optional 
         * number it contains a list of indexes of where the optional number can be located (value).
         * 
         * Input:
         * None.
         * 
         * OutPut:
         * None.
         */
        public void initializePlacesOfNumbersFromCube()
        {
            // The dictionary placesOfNumbers contains after this for below 
            // keys of optional numbers in the cube (cubeNumber) and for each optional 
            // number it contains an indexes list of where the optional number can be located
            for(int indexInCube =0; indexInCube < inBoard.sizeOfBoard; indexInCube++)
            {
                if (cube[indexInCube].isSolved())
                {
                    // if the cell is solved remove the number in it from inBoard.placesOfNumbers
                    // because the cube contains it (the number) so we dont need to look at this place optional numbers
                    inBoard.placesOfNumbers.Remove(cube[indexInCube].number);
                }
                else
                {
                    copyOptionsInIndex(indexInCube);
                }
            }
        }

        /*
         * Summary:
         * Only the cubes can access the value of the cells.
         * So if you want to get a cell you can you this function.
         * It gets the cell at "indexInCube" in "this" cube.
         * 
         * Input:
         * indexInCube - An index in "this" cube.
         * 
         * OutPut:
         * The cell at "indexInCube" in "this" cube.
         */
        public ICell getCell(int indexInCube)
        {
            return cube[indexInCube];
        }

        /*
         * Summary:
         * Only the cubes can access the value of the cells.
         * So if you want to get a cell's options you can you this function.
         * It gets the cell's options at "indexInCube" in "this" cube.
         * 
         * Input:
         * indexInCube - An index in "this" cube.
         * 
         * OutPut:
         * The cell's options at "indexInCube" in "this" cube.
         */
        public List<int> getOptionalNumbers(int indexInCube)
        {
            return cube[indexInCube].optionalNumbers;
        }

        /*
         * Summary:
         * The function creates a string of a row in "this" cube.
         * 
         * Input:
         * None.
         * 
         * OutPut:
         * The function returns the string of a row in "this" cube.
         */
        public string rowOrCulIncubeString(int rowInCube)
        {
            string rowOrCulIncubeString = string.Empty;
            int[] forParams = Calculations.forParameters(rowInCube, false, inBoard.sqrtOfSizeOfBoard);
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
            {
                char number = (char)(cube[forParams[1]].number + '0');
                rowOrCulIncubeString += number;
            }
            return rowOrCulIncubeString;
        }

        /*
         * Summary:
         * The function fills the empty cells that
         * "number" can be placed at with number.
         * It is checking the validation only if 
         * checkTheOptions is true.
         * 
         * Input:
         * number - The number we are checking.
         * checkTheOptions - tells the function if it needs
         * to check if the number can be placed in each place.
         * If it false, the function puts "number" in all of the empty cells.
         * 
         * 
         * OutPut:
         * The function returns a list of places the 
         * number "number" can be located in the cube.
         */
        public List<int> fillOptionsInCube(int number, bool checkTheOptions)
        {
            List<int> optionsInCubeByBoardIndex = new List<int>();
            for (int indexInCube = 0; indexInCube < cube.Length; indexInCube++)
            {
                if (!cube[indexInCube].isSolved())
                    if (!checkTheOptions || inBoard.isPossibleIndexToNumber(cube[indexInCube].index, number))
                    {
                        cube[indexInCube].optionalNumbers.Add(number);
                        optionsInCubeByBoardIndex.Add(cube[indexInCube].index);
                    }
            }
            return optionsInCubeByBoardIndex;
        }

        /*
          * Summary:
          * The function checks if the number "number"
          * is in the row or col in "this" cube.
          * 
          * Input:
          * number - The number we are checking.
          * forParams - addition,index,last index
          * forParams helps us to scan cols and rows
          * in the board.
          * Its structure: addToIndexInCube,indexInCube,endOfColOrRow
          * 
          * OutPut:
          * The function returns true if the number "number" is in the row or col in cube.Otherwise it returns false.
          */
        private bool isNumberInRowOrColInCubeFor( int[] forParams, int number)
        {
            // forParams - addToIndexInCube,indexInCube,endOfColOrRow
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
            {
                if (cube[forParams[1]].isSolved())
                    if (cube[forParams[1]].number == number)
                        return true;
            }
            return false;
        }

        /*
          * Summary:
          * The function checks if the number "number"
          * is in "colOrRowIndex" in "this" cube.
          * 
          * Input:
          * col - true for col and false for row.
          * It tells the function what you want to
          * scan , row or col.
          * colOrRowIndex - the row or col we search
          * the number "number" in.
          * number - The number we are checking.
          * 
          * OutPut:
          * The function returns true if the number "number" is in "colOrRowIndex" in "this" cube.Otherwise it returns false.
          */
        public bool isNumberInRowOrColInCube(bool col, int colOrRowIndex, int number)
        {
            int[] forParams = Calculations.forParameters(colOrRowIndex, col, inBoard.sqrtOfSizeOfBoard);
            if (isNumberInRowOrColInCubeFor(forParams, number))
                return true;
            return false;
        }

        /*
           * Summary:
           * The function checks if the "colOrRowIndexInCube"
           * is full with numbers or not.
           * 
           * Input:
           * col - true for col and false for row.
           * It tells the function what you want to
           * scan , row or col.
           * colOrRowIndexInCube - the row or col we want
           * to check if it is full.
           * 
           * OutPut:
           * The function returns true if the "colOrRowIndexInCube" is full with numbers.Otherwise it returns false.
           */
        public bool isRowOrColFull(bool col, int colOrRowIndexInCube)
        {
            // forParams - addToIndexInCube,indexInCube,endOfColOrRow
            int[] forParams = Calculations.forParameters(colOrRowIndexInCube, col, inBoard.sqrtOfSizeOfBoard);
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
                if (!cube[forParams[1]].isSolved())
                    return false;
            return true;
        }

         /*
           * Summary:
           * The function checks how many options the number
           * "number" has in "this" cube.
           * The function calls in the end to "hiddenSingleOfCube"
           * in Board.
           * 
           * Input:
           * number - the number we want to check.
           * 
           * OutPut:
           * None.
           */
        private void placesOfNumberInCube(int number)
        {
            //countNumberInCube - list of options (indexes) of the number "number" in the cube.
            List<int> countNumberInCube = new List<int>();
            for (int indexInCube = 0; indexInCube < inBoard.sizeOfBoard; indexInCube++)
                if (!cube[indexInCube].isSolved())
                {
                    if (cube[indexInCube].optionalNumbers.Contains(number))
                    {
                        // if the cell contains an option of "number" add the index to countNumberInCube
                        countNumberInCube.Add(cube[indexInCube].index);
                    }
                }
                else
                {
                    // there is a chance the number is already in the cube
                    if (cube[indexInCube].number == number)
                        return;
                }
            inBoard.hiddenSingleOfCube(countNumberInCube, number);
        }

        /*
          * Summary:
          * The function delete the number "number"
          * from the "colOrRowIndexInCube" in "this" cube.
          * 
          * Input:
          * col - true for col and false for row.
          * It tells the function what you want to
          * scan , row or col.
          * colOrRowIndexInCube - the row or col we want
           * to delete "number" from.
          * number - the number we want to check.
          * 
          * OutPut:
          * None.
          */
        public void deleteNumberFromRowOrColInCube(bool col, int colOrRowIndexInCube, int number)
        {
            int[] forParams = Calculations.forParameters(colOrRowIndexInCube, col, inBoard.sqrtOfSizeOfBoard);
            bool removeNumberFromCube = false;
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
                if (!cube[forParams[1]].isSolved())
                    if (cube[forParams[1]].optionalNumbers.Remove(number))
                        removeNumberFromCube = true;
            if (removeNumberFromCube)
                placesOfNumberInCube(number);
        }

        /*
          * Summary:
          * The function checks how many optional numbers
          * the cell at "indexInCube" in "this" cube has.
          * If the cell has one options - put it.
          * If the cell has zero options - throw UnSolvableCellException.
          * 
          * Input:
          * indexInCube - an index in "this" cube.
          * 
          * OutPut:
          * The function returns true if the cell has only one options.Otherwise it returns false.
          * if the cell at "indexInCube" in "this" cube has zero optional 
          * numbers the function throws UnSolvableCellException.
          */
        public bool checkCountOfOptionsInIndex(int indexInCube)
        {
            // countOfOptionalNumbers = how many optional numbers the cell at "indexInCube" in "this" cube has.
            int countOfOptionalNumbers = cube[indexInCube].optionalNumbers.Count;
            if (countOfOptionalNumbers == 0)
            {
                int cubeNumber = Calculations.getCubeNumberByIndex(cube[indexInCube].index, inBoard.sizeOfBoard);
                throw new UnSolvableCellException("the cell in the cube " + cubeNumber + " at index " + indexInCube + " (in the cube) cannot conatin a valid number");
            }
            if (countOfOptionalNumbers == 1)
            {
                // knownNumber = the only option in the in the cell
                int knownNumber = cube[indexInCube].optionalNumbers[0];
                putTheNumberAndDeletOptions(indexInCube, knownNumber, true, true);
                deleteNumberFromCube(knownNumber,new List<int>());
                return true;
            }
            return false;
        }

        /*
          * Summary:
          * The function delete from all "this" cube except 
          * the places in "PlacesNotToDelete" the options of 
          * the number "number"
          * 
          * Input:
          * number - the number we want to delete from the cube.
          * PlacesNotToDelete - A list of places we dont want to delete 
          * "number" from.
          * 
          * OutPut:
          * None
          */
        public void deleteNumberFromCube(int number, List<int> PlacesNotToDelete)
        {
            //deleteNumberFromCube(MissingNumberInDic, inBoard.placesOfNumbers,col, rowOrColInCube)
            // this function accurs only after each empty cell in the board contains all its options 
            for (int indexInCube = 0; indexInCube < inBoard.sizeOfBoard; indexInCube++)
                if (!cube[indexInCube].isSolved())
                    if (!PlacesNotToDelete.Contains(indexInCube))
                        cube[indexInCube].optionalNumbers.Remove(number);
        }

        /*
       * Summary:
       * The function put the number "number" 
       * at "indexInCube" in "this" cube" and delte his other options from row 
       * and col (depending on "deletFromRow" and "deletFromCol")
       *  
       * Input:
       * indexInCube - the index in the cube where we wiil put the number at.
       * knownNumber - the number we solved.
       * deletFromRow - tells the function whether to delete the other 
       * options of "knownNumber" from his row.
       * deletFromCol - tells the function whether to delete the other
       * options of ""knownNumber" from his col.
       * 
       * OutPut:
       * None.
       */
        public void putTheNumberAndDeletOptions(int indexInCube, int knownNumber, bool deletFromRow, bool deletFromCol)
        {
            List<int> optionalNumbersOfPlace = cube[indexInCube].optionalNumbers;
            cube[indexInCube].solvedTheCell(knownNumber);
            optionalNumbersOfPlace.Remove(knownNumber);

            // checking for the optional numbers that were at "indexInCube"
            // in "this" cube how many options each "number" has in "this" cube
            // after putting "knownNumber".
            foreach (int number in optionalNumbersOfPlace)
                placesOfNumberInCube(number);

            if (deletFromCol)
                inBoard.deleteNumberFromRowOrCol(true, cube[indexInCube].index, knownNumber);
            if (deletFromRow)
                inBoard.deleteNumberFromRowOrCol(false, cube[indexInCube].index, knownNumber);
            // removes the empty cell from emptyCellsIndexes list because the cell is solved.
            inBoard.sudokuSolver.emptyCellsIndexes.Remove(cube[indexInCube].index);
        }

        /*
       * Summary:
       * The function is checking the integrity
       * of a row or col in this cube ,or of this cube.
       * 
       * Input:
       * type - the type we want to check, row/col/cube.
       * CountNumber - list of the numbers in the row/col/cube.
       * RowOrColInCube - the row or col in the cube
       * we want to check in.
       * RowOrColOfCube - the row or col of the cube
       * we want to check in.
       * 
       * (RowOrColOfCube and rowOrColInCube are by default -1
       * because if we want to check the cube we dont need them)
       * 
       * OutPut:
       * None.
       * if one of the row/col/cube in invalid (has the same number more than once)
       * the function throws SameNumberInARowOrColOrCubeException.
       */
        public void rowOrColOrCubeIntegrity(Type type, List<int> CountNumber, int rowOrColInCube = -1, int rowOrColOfCube = -1)
        {
            //forParams structure - addToIndexInCube,indexInCube,endOfColOrRow
            // forParams of a cube 
            int[] forParams = { 1, 0, inBoard.sizeOfBoard };
            if (type != Type.cube)
            {
                // forParams of a row or col
                forParams = Calculations.forParameters(rowOrColInCube, type == Type.col, inBoard.sqrtOfSizeOfBoard);
            }
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
                if (cube[forParams[1]].isSolved())
                {
                    int numberInCell = cube[forParams[1]].number;
                    if (CountNumber.Contains(numberInCell))
                    {
                        // if the type already contains this number throw SameNumberInARowOrColOrCubeException
                        int typeNumber = cubeNumber;
                        if (type != Type.cube)
                        {
                            typeNumber = rowOrColOfCube * inBoard.sqrtOfSizeOfBoard + rowOrColInCube;
                        }
                        throw new SameNumberInARowOrColOrCubeException("The number " + numberInCell + " appears more than once in the " + type + " " + typeNumber);
                    }
                    CountNumber.Add(numberInCell);
                }
        }

        /*
      * Summary:
      * The function is checking the integrity
      * of of the cells in the cube.
      * 
      * Input:
      * None.
      * 
      * OutPut:
      * None.
      *  if the cell's number is invalid (not suitable for the board)
      * the function throws indirectly InvalidNumberException.
      */
        public void cellsInCubeIntegrity()
        {
            foreach(ICell cell in cube)
            {
                cell.cellIntegrity(inBoard.sizeOfBoard);
            }
        }

        /*
          * Summary:
          * The function copies all the optional numbers at "indexInCube"
          * to the dictionary inBoard.placesOfNumbers.
          * the key is an optional number and the value is the indexes list
          * of where the optional number is located.
          * 
          * Input:
          * indexInCube - An index in "this" cube
          * that from it we wnat to copy the optional numbers
          * 
          * OutPut:
          * None.
          */
        private void copyOptionsInIndex(int indexInCube)
        {
            // getting the indexInBoard
            int indexInBoard = cube[indexInCube].index;
            // copy the options
            foreach (int option in cube[indexInCube].optionalNumbers)
                inBoard.placesOfNumbers[option].Add(indexInBoard);
        }

        /*
           * Summary:
           * The function copies all the optional numbers in the "rowOrColInCube"
           * to the dictionary inBoard.placesOfNumbers.
           * the key is an optional number and the value is the indexes list
           * of where the optional number is located.
           * 
           * Input:
           * col - true for col and false for row.
           * It tells the function what you want to
           * scan , row or col.
           * rowOrColInCube - the row or col we want
           * to check if it is full.
           * 
           * OutPut:
           * None.
           */
        public void copyOptionsOfARowOrAColInCube(bool col, int rowOrColInCube)
        {
            int[] forParams = Calculations.forParameters(rowOrColInCube, col, inBoard.sqrtOfSizeOfBoard);
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
            {
                if (cube[forParams[1]].isSolved())
                {
                    // if the cell is solved remove the number in it from inBoard.placesOfNumbers
                    // because the row or col contains it (the number) so we dont need to look at this place optional numbers
                    inBoard.placesOfNumbers.Remove(cube[forParams[1]].number);
                }
                else
                    copyOptionsInIndex(forParams[1]);
            }
        }

        /*
           * Summary:
           * The function leaves in the cell at "indexInCube" in "this" cube
           * only the tow options in remainingOptions.
           * 
           * Input:
           * indexInCube - An index in "this" cube.
           * remainingOptions - A list of two options 
           * that we want to leave in the cell at "indexInCube".
           * 
           * OutPut:
           * The function returns true if the cell didnt have two options before the function.Otherwise it returns false.
           */
        public bool leaveInCellOnlyThePairs(int indexInCube,List<int> remainingOptions)
        {           
            if (cube[indexInCube].optionalNumbers.Count == 2)
            {
                //the cell already has only two options so you dont need to change it
                return false;
            }
            cube[indexInCube].optionalNumbers.Clear();
            cube[indexInCube].optionalNumbers.Add(remainingOptions.ElementAt(0));
            cube[indexInCube].optionalNumbers.Add(remainingOptions.ElementAt(1));
            deleteOptionsFromPlacesOfNumbers(cube[indexInCube].index, remainingOptions);
            return true;
        }


        /*
           * Summary:
           * The function delete the "indexInBoard" index value from
           * all the keys in placesOfNumbers except of the 
           * numbers (keys) in remainingOptions.
           * 
           * Input:
           * indexInBoard - An index in th board we want to delete it's options.
           * remainingOptions - A list of two options 
           * that we dont want to delete from them the "indexInBoard" index value.
           * 
           * OutPut:
           * None.
           */
        private void deleteOptionsFromPlacesOfNumbers(int indexInBoard,List<int> remainingOptions)
        {
            foreach(int number in inBoard.placesOfNumbers.Keys)
            {
                if (!remainingOptions.Contains(number))
                {
                    inBoard.placesOfNumbers[number].Remove(indexInBoard);
                }
            }
        }

        /*
           * Summary:
           * The function leaves in the cell at "indexInCube" in "this" cube
           * only the options in remainingOptions.
           *
           * Input:
           * indexInCube - An index in "this" cube.
           * remainingOptions - A list of options
           * that we want to leave in the cell at "indexInCube".
           * 
           * OutPut:
           * None.
           */
        public void leaveInCellOnlyTheListOptions(int indexInCube, List<int> remainingOptions)
        {
            cube[indexInCube].optionalNumbers.Clear();
            foreach (int option in remainingOptions)
            {
                cube[indexInCube].optionalNumbers.Add(option);
            }
        }

    }
}

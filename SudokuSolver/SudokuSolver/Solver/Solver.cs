using SudokuSolver.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Solver : ISudokuSolver
    {
        // The class Solver responsible of solving the board in sudokuBoard

        // the Board Solver needs to solve
        public ISudokuBoard sudokuBoard { get; set; }
        // a list of the indexes of the empty cells in the board in sudokuBoard
        public List<int> emptyCellsIndexes { get; set; }

        /*
         * Summary:
         * The function initialized an instance of the Solver class.
         * 
         * Input:
         * SudokuBoard - the Board Solver needs to solve
         * 
         * OutPut:
         * None.
         */
        public Solver(ISudokuBoard SudokuBoard)
        {
            sudokuBoard = SudokuBoard;
            emptyCellsIndexes = new List<int>();
        }

        /*
         * Summary:
         * The function that solves the board.
         * 
         * Input:
         * None.
         * 
         * OutPut:
         * None.
         * If a pair is invalid (explain in invalidGroup function below)
         * the function throws indirectly UnSolvableCellException.
         * If the board is unsolvable  (explain in thirdStepOfSolving function below)
         * the function throws indirectly UnSolvableBordException.
         */
        public void Solve()
        {
            firstStepOfSolving();
            secondStepOfSolving();
            thirdStepOfSolving(sudokuBoard);
        }

        /*
         * Summary:
         * The function copies all the empty cells in src Board to des Board.
         * 
         * Input:
         * src - the source Board.
         * des - the destination Board.
         * 
         * OutPut:
         * None.
         */
        private void copyEmptyCellsOptions(Board src, Board des)
        {
            foreach(int emptyCellIndexe in emptyCellsIndexes)
            {
                int cubeOfCell = Calculations.getCubeNumberByIndex(emptyCellIndexe, sudokuBoard.sizeOfBoard);
                int indexOfCellInCube = Calculations.getIndexInCubeByIndexInBoard(emptyCellIndexe, sudokuBoard.sizeOfBoard);
                des.board[cubeOfCell].leaveInCellOnlyTheListOptions(indexOfCellInCube, src.board[cubeOfCell].getOptionalNumbers(indexOfCellInCube));
            }
        }

        /*
        * Summary:
        * The function checks how many unSolved cell the board have.
        * 
        * Input:
        * None.
        * 
        * OutPut:
        * The function returns how many unSolved cell the board have.
        */
        public int countHowManyUnSolvedCells()
        {
            return emptyCellsIndexes.Count;
        }

        /*
         * Summary:
         * The function responsible of filling all the cells
         * in the board with their optional numbers.
         * It also solves the board by the hiddenSingleOfCube method.
         * 
         * Input:
         * None.
         * 
         * OutPut:
         * None.
         */
        private void firstStepOfSolving()
        {
            int mostCommonNumber;
            // sorts 'placesOfNumbers' by value - by the length of the list in descending order
            sudokuBoard.placesOfNumbers = Calculations.sortByValueDescending(sudokuBoard.placesOfNumbers);
            sudokuBoard.placesOfNumbers.Reverse();
            while (sudokuBoard.placesOfNumbers.Count != 0)
            {
                // mostCommonNumber = the element with the longest length of the value in 'placesOfNumbers',
                // (the number that exists most times in the board and it's in 'placesOfNumbers')
                mostCommonNumber = sudokuBoard.placesOfNumbers.Keys.ElementAt(0);
                for (int indexOfCube = 0; indexOfCube < sudokuBoard.sizeOfBoard; indexOfCube++)
                {
                    if (!isplacesOfNumberContainsTheCube(indexOfCube, mostCommonNumber))
                    {
                        bool checkTheOptions = sudokuBoard.isTheCubeWorthChecking(indexOfCube, mostCommonNumber);
                        // a list that contains the indexes in the cube (by the indexes of the board) of where  
                        // the number mostCommonNumber can be
                        List<int> optionsInCubeByBoardIndex = sudokuBoard.board[indexOfCube].fillOptionsInCube(mostCommonNumber, checkTheOptions);
                         sudokuBoard.hiddenSingleOfCube(optionsInCubeByBoardIndex, mostCommonNumber);
                    }
                }
                sudokuBoard.placesOfNumbers.Remove(mostCommonNumber);
            }
            // the first step is over
            sudokuBoard.firstStep = false;
            // we are not useing intersectionNumbers in the other steps
            sudokuBoard.intersectionNumbers = null;
        }

        /*
         * Summary:
         * The function checks if the number "number" has an 
         * option in the cube "cubeNumber".
         * 
         * Input:
         * cubeNumber - A cube number.
         * number - A number we want to check.
         * 
         * OutPut:
         * The function returns true if the number "number" has an option in the cube "cubeNumber".Otherwise it returns false.
         */
        private bool isplacesOfNumberContainsTheCube(int cubeNumber, int number)
        {
            foreach (int indexInBoard in sudokuBoard.placesOfNumbers[number])
                if (cubeNumber == Calculations.getCubeNumberByIndex(indexInBoard, sudokuBoard.sizeOfBoard)) 
                    return true;
            return false;
        }

        /*
          * Summary:
          * The function solves the board with 
          * nakedSingle and hiddenSubsets methods.
          * It is calling the methods until they both return false.
          * 
          * Input:
          * None.
          * 
          * OutPut:
          * None.
          * if a pair is invalid (explain in invalidGroup function below)
          * the function throws indirectly UnSolvableCellException.
          */
        public void secondStepOfSolving()
        {
            bool doNakedSingle = true;
            bool doHiddneSubsets = true;
            while (doNakedSingle || doHiddneSubsets)
            {
                if (doNakedSingle)
                {
                    if (sudokuBoard.repetition(nakedSingle))
                    {
                        doHiddneSubsets = true;
                    }
                    doNakedSingle = false;
                }
                if (doHiddneSubsets)
                {
                    if (sudokuBoard.repetition(hiddenSubsets)) 
                    {
                        doNakedSingle = true;
                    }
                    doHiddneSubsets = false;
                }
            }
        }

        /*
          * Summary:
          * The function solves the board with nakedSingle tactics -
          * if a cell has only one options - put it.
          * It is calling the method until it returns false.
          * 
          * Input:
          * None.
          * 
          * OutPut:
          * The function returns true if the board has change while running this method.Otherwise it returns false.
          */
        private bool nakedSingle()
        {
            bool theBoardHasChanged = false;
            for(int emptyCellsIndex =  0; emptyCellsIndex < emptyCellsIndexes.Count; emptyCellsIndex++)
            {
                int indexInBoard = emptyCellsIndexes[emptyCellsIndex];
                int indexOfCube = Calculations.getCubeNumberByIndex(indexInBoard, sudokuBoard.sizeOfBoard);
                int indexInCube = Calculations.getIndexInCubeByIndexInBoard(indexInBoard, sudokuBoard.sizeOfBoard);
                if (sudokuBoard.board[indexOfCube].checkCountOfOptionsInIndex(indexInCube))
                    theBoardHasChanged = true;
            }
            return theBoardHasChanged;
        }

        /*
           * Summary:
           * The function solves the board with hiddenSubsets tactics -
           * if a number has only one options in a row/col/cube - put it.
           * if two numbers have only tow options in a row/col/cube and the options in the same indexes -
           * leave in these indexes only the two numbers.
           * It is calling the methods (row/col/cube) until all of them return false.
           * 
           * Input:
           * None.
           * 
           * OutPut:
           * The function returns true if the board has change while running this method.Otherwise it returns false.
           *  if the pair is invalid (explain in invalidGroup function below)
           * the function throws indirectly UnSolvableCellException.
           */
        private bool hiddenSubsets()
        {
            bool theBoardHasChanged = false;
            bool dohiddenSubsetsOfCol = true;
            bool dohiddenSubsetsOfRow = true;
            bool dohiddenSubsetsOfCube = true;

            if (dohiddenSubsetsOfCol)
            {
                dohiddenSubsetsOfCol = false;
                if (hiddenSubsetsOfTypeFor(Type.col))
                {
                    dohiddenSubsetsOfRow = true;
                    dohiddenSubsetsOfCube = true;
                    theBoardHasChanged = true;
                }
            }
            if (dohiddenSubsetsOfRow)
            {
                dohiddenSubsetsOfRow = false;
                if (hiddenSubsetsOfTypeFor(Type.row))
                {
                    dohiddenSubsetsOfCol = true;
                    dohiddenSubsetsOfCube = true;
                    theBoardHasChanged = true;
                }
            }
            if (dohiddenSubsetsOfCube)
            {
                dohiddenSubsetsOfCube = false;
                if (hiddenSubsetsOfTypeFor(Type.cube))
                {
                    dohiddenSubsetsOfRow = true;
                    dohiddenSubsetsOfCol = true;
                    theBoardHasChanged = true;
                }
            }
            return theBoardHasChanged;
        }

        /*
          * Summary:
          * The function runs hiddenSubsetsOfType sizeOfBoard times,
          * on all the rows/cols/cubes.
          * 
          * Input:
          * type - the tyoe we want to check (row/col/cube).
          * 
          * OutPut:
          * The function returns true if the board has change while running this method.Otherwise it returns false.
          */
        private bool hiddenSubsetsOfTypeFor(Type type)
        {
            bool theBoardHasChanged = false;
            for (int indexOfType = 0; indexOfType < sudokuBoard.sizeOfBoard; indexOfType++)
            {
                if (hiddenSubsetsOfType(type, indexOfType))
                    theBoardHasChanged = true;
            }
            return theBoardHasChanged;
        }

        /*
         * Summary:
         * The function runs findPairsAndDeleteTheirOtherOptions
         * and hiddenSingleOfRowAndCol until they both return false,
         * 
         * Input:
         * type - the tyoe we want to check (row/col/cube).
         * typeIndexInBoard - the row/col/cube number.
         * 
         * OutPut:
         * The function returns true if the board has change while running this method.Otherwise it returns false.
         * if the pair is invalid (explain in invalidGroup function below)
         * the function throws indirectly UnSolvableCellException.
         */
        private bool hiddenSubsetsOfType(Type type, int typeIndexInBoard)
        {
            int numbersOfLopps = -1;
            sudokuBoard.InitializePlacesOfNumbers();
            if (type != Type.cube)
                sudokuBoard.initializePlacesOfNumbersFromRowOrCol(type == Type.col, typeIndexInBoard);
            else
                sudokuBoard.board[typeIndexInBoard].initializePlacesOfNumbersFromCube();
            do
            {
                numbersOfLopps++;                 
            } while (findPairsAndDeleteTheirOtherOptions(type) || sudokuBoard.hiddenSingleOfRowAndCol(type, typeIndexInBoard));
            return numbersOfLopps > 0;
        }

        /*
         * Summary:
         * The function finds pairs of numbers - 
         * two numbers that have only tow options in a row/col/cube and the options in the same indexes.
         * If it finds a pair it leaves in the indexes of the pair only the two numbers.
         * 
         * Input:
         * type - the tyoe we want to check (row/col/cube).
         * 
         * OutPut:
         * The function returns true if the board has change while running this method.Otherwise it returns false.
         * if the pair is invalid (explain in invalidGroup function below)
         * the function throws indirectly UnSolvableCellException.
         */
        private bool findPairsAndDeleteTheirOtherOptions(Type type)
        {
            Dictionary<int, List<int>> groupOptionalPairsNumbers = findSubsets();
            bool findAPair = false;
            foreach (int key in groupOptionalPairsNumbers.Keys)
            {
                // The keys are consecutive numbers from 0 and up 
                if (groupOptionalPairsNumbers[key].Count == 2)
                {
                    // there is a chance that two pairs or more will be at
                    // the same locations so we need to check if the numbers of the 
                    // pair can still be in two places in the type ( row/col/cube)
                    if (sudokuBoard.placesOfNumbers[groupOptionalPairsNumbers[key][0]].Count != 2)
                        invalidGroup(groupOptionalPairsNumbers, key);
                    List<int> FirstOptionInfo = gettingInfoOnTheOptions(groupOptionalPairsNumbers, key,0);
                    // FirstOptionInfo = firstPairNumber,firstIndexInBoard,firstIndexInCube,firstCubeNumber
                    List<int> SecondOptionInfo = gettingInfoOnTheOptions(groupOptionalPairsNumbers, key, 1);
                    // SecondOptionInfo = secondPairNumber,secondIndexInBoard,secondIndexInCube,secondCubeNumber
                    List<int> remainingOptions = new List<int>();
                    remainingOptions.Add(FirstOptionInfo[0]);
                    remainingOptions.Add(SecondOptionInfo[0]);
                    // remainingOptions = firstPairNumber,secondPairNumber
                    //leave in the cells of the pair only the numbers of the pair
                    if (sudokuBoard.board[FirstOptionInfo[3]].leaveInCellOnlyThePairs(FirstOptionInfo[2], remainingOptions) ||
                        sudokuBoard.board[SecondOptionInfo[3]].leaveInCellOnlyThePairs(SecondOptionInfo[2], remainingOptions))
                            findAPair = true;
                    isItNecessaryTodeleteNumberFromCube( type, FirstOptionInfo, SecondOptionInfo);
                }
                else
                {
                    // if there is more then two numbers that rlated to the group it is
                    //  a proplem because there is not enough places for all the numbers
                    if (groupOptionalPairsNumbers[key].Count > 2)
                        invalidGroup(groupOptionalPairsNumbers, key);
                }
            }
            return findAPair;
        }

        /*
          * Summary:
          * The function check if the pair is in the same row or col and in the same cube and we didn't check  
          * for pairs in cube (type != Type.cube) so it deletes all the options of the pair's  
          * numbers from any other place in the cube except from the places of the pair.
          * 
          * Input:
          * type - the tyoe we want to check (row/col/cube).
          * FirstOptionInfo - info on the first number in the pair.
          * FirstOptionInfo = firstPairNumber,firstIndexInBoard,firstIndexInCube,firstCubeNumber
          * SecondOptionInfo - info on the second number in the pair.
          * SecondOptionInfo = secondPairNumber,secondIndexInBoard,secondIndexInCube,secondCubeNumber
          * 
          * OutPut:
          * None.
          */
        private void isItNecessaryTodeleteNumberFromCube(Type type, List<int> FirstOptionInfo, List<int> SecondOptionInfo)
        {
            if (type != Type.cube && FirstOptionInfo[3] == SecondOptionInfo[3])
            {
                List<int> PlacesNotToDelete = new List<int>();
                PlacesNotToDelete.Add(FirstOptionInfo[2]);
                PlacesNotToDelete.Add(SecondOptionInfo[2]);
                //PlacesNotToDelete = firstIndexInCube,secondIndexInCube
                sudokuBoard.board[FirstOptionInfo[3]].deleteNumberFromCube(FirstOptionInfo[0], PlacesNotToDelete);
            }
        }

        /*
          * Summary:
          * The function is getting information about the options in 
          * "groupOptionalPairsNumbers" in the key "key" at  numOfOption.
          * 
          * Input:
          * groupOptionalPairsNumbers - a dictionary that it's keys are 
          * consecutive numbers from 0 and up.
          * Each key has a list of numbers with the same indexses of options (pairs or maby if the board is invalid
          * their will be more than teo numbers in the value).
          * key - a number of the key we want to check.
          * numOfOption - the number one or tow.
          * It tells on eich number of the pair you want to get 
          * 
          * OutPut:
          * The function returns a list with the options it founs on information.
          * infoOnOption = PairNumber,IndexInBoard,IndexInCube,CubeNumber
          */
        private List<int> gettingInfoOnTheOptions(Dictionary<int, List<int>> groupOptionalPairsNumbers, int key,int numOfOption)
        {
            // infoOnOption = PairNumber,IndexInBoard,IndexInCube,CubeNumber
            List<int> infoOnOption = new List<int>();
            infoOnOption.Add(groupOptionalPairsNumbers[key].ElementAt(numOfOption));
            infoOnOption.Add(sudokuBoard.placesOfNumbers[infoOnOption[0]].ElementAt(numOfOption));
            infoOnOption.Add(Calculations.getIndexInCubeByIndexInBoard(infoOnOption[1], sudokuBoard.sizeOfBoard));
            infoOnOption.Add(Calculations.getCubeNumberByIndex(infoOnOption[1], sudokuBoard.sizeOfBoard));
            return infoOnOption;
        }

        /*
          * Summary:
          * If a group of numbers with the same indexes is invalid -
          * 1. there is a chance that two pairs or more will be at
          *    the same locations so we need to check if the numbers of the 
          *    pair can still be in two places in the type ( row/col/cube)
          * 2. if there is more then two numbers that rlated to the group it is
          * a proplem because there is not enough places for all the numbers.
          * 
          * Input:
          * groupOptionalPairsNumbers - a dictionary that it's keys are 
          * consecutive numbers from 0 and up.
          * Each key has a list of numbers with the same indexses of options (pairs or maby if the board is invalid
          * their will be more than teo numbers in the value).
          * key - a number of the key we want to check.
          * 
          * OutPut:
          * None.
          * The function throws UnSolvableCellException.
          */
        private void invalidGroup(Dictionary<int, List<int>> groupOptionalPairsNumbers, int key)
        {
                // it is taking to match time to check for trios,quatrets and more... so 
                // they will get the excption message (if the board is unsolvable) in the backtracking
                string numbers = string.Empty;
                foreach (int number in groupOptionalPairsNumbers[key])
                {
                    char charNumber = (char)(number + '0');
                    numbers += charNumber + " ";
                }
                string indexes = string.Empty;
                foreach (int index in sudokuBoard.placesOfNumbers[groupOptionalPairsNumbers[key].ElementAt(0)])
                {
                    int charIndex = (char)index;
                    indexes += charIndex + " ";
                }
                throw new UnSolvableCellException("the indexes at " + indexes + "cannot contain all of these numbers: " + numbers);
        }

        /*
          * Summary:
          * The function finds subsets of groups.
          * groupOptionalPairsNumbers - a dictionary that it's keys are 
          * consecutive numbers from 0 and up.
          * Each key has a list of numbers with the same indexses of options (pairs or maby if the board is invalid
          * their will be more than teo numbers in the value).
          * 
          * Input:
          * None.
          * 
          * OutPut:
          * The function returns groupOptionalPairsNumbers (explain in Summary)
          */
        private Dictionary<int, List<int>> findSubsets()
        {
            // sorts 'placesOfNumbers' by value - by the length of the list in ascending order
            sudokuBoard.placesOfNumbers = Calculations.sortByValue(sudokuBoard.placesOfNumbers);
            List<List<int>> groupOptionalPairsIndexes = new List<List<int>>();
            Dictionary<int, List<int>> groupOptionalPairsNumbers = new Dictionary<int, List<int>>();
            int indexOfMissingNumber = 0;
            int sizeOfPlacesOfNumbers = sudokuBoard.placesOfNumbers.Count;
            while (indexOfMissingNumber < sizeOfPlacesOfNumbers && sudokuBoard.placesOfNumbers.Values.ElementAt(indexOfMissingNumber).Count == 2)
            {
                int indexInGroupOptionalPairsIndexes = placeInGroupOptionalPairsIndexes(groupOptionalPairsIndexes, sudokuBoard.placesOfNumbers.Values.ElementAt(indexOfMissingNumber));
                if (indexInGroupOptionalPairsIndexes != -1)
                {
                    int numberWithSameIndexes = sudokuBoard.placesOfNumbers.Keys.ElementAt(indexOfMissingNumber);
                    // there is a number with the same indexes like "numberWithSameIndexes" in groupOptionalPairsNumbers
                    // at index indexInGroupOptionalPairsIndexes.
                    // So you can add the new number to groupOptionalPairsNumbers at index indexInGroupOptionalPairsIndexes.
                    groupOptionalPairsNumbers[indexInGroupOptionalPairsIndexes].Add(numberWithSameIndexes);
                }
                else
                {
                    int numberWithDifferentIndexes = sudokuBoard.placesOfNumbers.Keys.ElementAt(indexOfMissingNumber);

                    //There is not any number with the same indexes like "numberWithSameIndexes" in groupOptionalPairsNumbers 
                    //at index indexInGroupOptionalPairsIndexes.
                    //So you need to add a new number to groupOptionalPairsNumbers at index groupOptionalPairsIndexes.Count.
                    groupOptionalPairsNumbers.Add(groupOptionalPairsIndexes.Count, new List<int>());
                    groupOptionalPairsNumbers[groupOptionalPairsIndexes.Count].Add(numberWithDifferentIndexes);
                    // add to the indexes list the new indexes of the number you found (numberWithDifferentIndexes)
                    groupOptionalPairsIndexes.Add(sudokuBoard.placesOfNumbers.Values.ElementAt(indexOfMissingNumber));
                }
                indexOfMissingNumber++;
            }
            return groupOptionalPairsNumbers;
        }

        /*
         * Summary:
         * The function finds if there is a List of indexes in groupOptionalPairsIndexes
         * with the same indexes like "indexsesList"
         * 
         * Input:
         * groupOptionalPairsIndexes - A list that contains lists of indexes of the numbers in
         * the type that we are checking (the type in findPairsAndDeleteTheirOtherOptions).
         * indexsesList - A list of indexes of a number in placesOfNumbers.
         * 
         * OutPut:
         * The function returns the index in groupOptionalPairsIndexes if there is a List of indexes in groupOptionalPairsIndexes
         * with the same indexes like "indexsesList". Otherwise it returns -1.
         */
        private int placeInGroupOptionalPairsIndexes(List<List<int>> groupOptionalPairsIndexes, List<int> indexsesList)
        {
            for (int optionalPairsIndex = 0; optionalPairsIndex < groupOptionalPairsIndexes.Count; optionalPairsIndex++)
                if (Calculations.compareListsOfIndexes(indexsesList, groupOptionalPairsIndexes.ElementAt(optionalPairsIndex)))
                    return optionalPairsIndex;
            return -1;
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
         * None.
         * if backTracking(sudokuBoard) returns null it means 
         * that the board is UnSolvableBordException and 
         * the function will throw UnSolvableBordException.
         */
        private void thirdStepOfSolving(ISudokuBoard board)
        {
            sudokuBoard = backTracking(sudokuBoard);
            if (sudokuBoard == null)
                throw new UnSolvableBordException();
        }

        /*
          * Summary:
          * The function solves the board in a backTracking way- 
          * trying all the options of the board.
          * 
          * Input:
          * board - The current board we want to solve.
          * 
          * OutPut:
          * The functions returns the solved board.
          * if the function finds out that the board is unsolvable  
          * the function returns null.
          */
        public ISudokuBoard backTracking(ISudokuBoard board)
        {
            // finds empty cell with the least amount of options.
            int cellWithLeastOptions = findUnSolvedCellWithLeastOptions();
            if (cellWithLeastOptions == -1)
            {
                // all the cells in the board are not empty.The board is solved.
                return board;
            }
            // get information on the "cellWithLeastOptions"
            //--------------------------------------------------------------
            int cubeNumberOfCell = Calculations.getCubeNumberByIndex(cellWithLeastOptions, sudokuBoard.sizeOfBoard);
            int indexInCubeOfCell = Calculations.getIndexInCubeByIndexInBoard(cellWithLeastOptions, sudokuBoard.sizeOfBoard);
            List<int> optionsOfCell = sudokuBoard.board[cubeNumberOfCell].getOptionalNumbers(indexInCubeOfCell);
            //--------------------------------------------------------------
            foreach (int option in optionsOfCell)
            {
                try
                {
                    // copy the "board" to a new board
                    ISudokuBoard tryBoard = board.copyBoard();
                    copyEmptyCellsOptions((Board)board, (Board)tryBoard);
                    // put in the new board the option
                    tryBoard.putKnownNumberAndDeletOptions(cellWithLeastOptions, option, true, true);
                    tryBoard.board[Calculations.getCubeNumberByIndex(cellWithLeastOptions,sudokuBoard.sizeOfBoard)].deleteNumberFromCube(option, new List<int>());
                    // solve the new board with secondStepOfSolving
                    tryBoard.sudokuSolver.secondStepOfSolving();
                    //call again to the backTracking with the new board
                    ISudokuBoard thirdStepOfSolvingBoard = tryBoard.sudokuSolver.backTracking(tryBoard);
                    if (thirdStepOfSolvingBoard != null)
                    {
                        return thirdStepOfSolvingBoard;
                    }
                }
                catch (SudokuExceptions s)
                {
                    // the option we put was not appropriate to the board
                    // try the next option
                }
            }
            //the board is unsolvable
            return null;
        }

        /*
         * Summary:
         * The function finds empty cell with the least amount of options.
         * 
         * Input:
         * None.
         * 
         * OutPut:
         * The functions returns the empty cell with the least amount of options.
         * If all the cells in the board are not empty it returns -1.The board is solved.
         */
        private int findUnSolvedCellWithLeastOptions()
        {
            int cellWithLeastOptions = -1;
            int leastOptions = sudokuBoard.sizeOfBoard + 1;
            foreach (int emptyCellIndexe in emptyCellsIndexes)
            {
                int cubeOfCell = Calculations.getCubeNumberByIndex(emptyCellIndexe, sudokuBoard.sizeOfBoard);
                int indexOfCellInCube = Calculations.getIndexInCubeByIndexInBoard(emptyCellIndexe, sudokuBoard.sizeOfBoard);
                int currentOptionsCount = -1;
                if (!sudokuBoard.board[cubeOfCell].getCell(indexOfCellInCube).isSolved())
                {
                    currentOptionsCount = sudokuBoard.board[cubeOfCell].getOptionalNumbers(indexOfCellInCube).Count;
                    if (currentOptionsCount == 2)
                        return emptyCellIndexe;
                    if (currentOptionsCount < leastOptions)
                    {
                        leastOptions = currentOptionsCount;
                        cellWithLeastOptions = emptyCellIndexe;
                    }
                }
            }
            return cellWithLeastOptions;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Solver : ISudokuSolver
    {

        public ISudokuBoard sudokuBoard { get; set; }
        public List<int> emptyCellsIndexes { get; set; }

        public Solver(ISudokuBoard SudokuBoard)
        {
            sudokuBoard = SudokuBoard;
            emptyCellsIndexes = new List<int>();
        }
        public void Solve()
        {
            firstStepOfSolving();
            sudokuBoard.step = 2;
            secondStepOfSolving();
            sudokuBoard.step = 3;
            thirdStepOfSolving(sudokuBoard);
        }
        private void copyEmptyCellsOptions(Board src, Board des)
        {
            foreach(int emptyCellIndexe in emptyCellsIndexes)
            {
                int cubeOfCell = Calculations.getCubeNumberByIndex(emptyCellIndexe, sudokuBoard.sizeOfBoard);
                int indexOfCellInCube = Calculations.getIndexInCubeByIndexInBoard(emptyCellIndexe, sudokuBoard.sizeOfBoard);
                des.board[cubeOfCell].leaveInCellOnlyTheListOptions(indexOfCellInCube, src.board[cubeOfCell].getOptionalNumbers(indexOfCellInCube));
            }
        }
        public int countHowManySolvedCells()
        {
            int count = 0;
            for (int indexOfCube = 0; indexOfCube < sudokuBoard.sizeOfBoard; indexOfCube++)
                count += sudokuBoard.board[indexOfCube].countHowManySolvedCells();
            return count;
        }

        private void firstStepOfSolving()
        {
            int mostCommonNumber;
            // sorts 'placesOfNumbers' by value - by the length of the list in descending order
            sudokuBoard.placesOfNumbers = StaticMethods.sortByValueDescending(sudokuBoard.placesOfNumbers);
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
                         sudokuBoard.checkNumberOfOptions(optionsInCubeByBoardIndex, mostCommonNumber);
                    }
                }
                sudokuBoard.placesOfNumbers.Remove(mostCommonNumber);
            }
        }
        private bool isplacesOfNumberContainsTheCube(int cubeNumber, int number)
        {
            foreach (int indexInBoard in sudokuBoard.placesOfNumbers[number])
                if (cubeNumber == Calculations.getCubeNumberByIndex(indexInBoard, sudokuBoard.sizeOfBoard)) 
                    return true;
            return false;
        }

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
        private bool nakedSingle()
        {
            bool theBoardHasChanged = false;
            for (int indexOfCube = 0; indexOfCube < sudokuBoard.sizeOfBoard; indexOfCube++)
            {
                if (sudokuBoard.repetition(sudokuBoard.board[indexOfCube].nakedSingleOfACube))
                    theBoardHasChanged = true;
            }
            return theBoardHasChanged;
        }

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

        private bool hiddenSubsetsOfType(Type type, int typeIndexInBoard)
        {
            int numbersOfLopps = -1;
            do
            {
                numbersOfLopps++;
                sudokuBoard.InitializePlacesOfNumbers();
                if (type != Type.cube)
                    sudokuBoard.initializePlacesOfNumbersFromRowOrCol(type == Type.col, typeIndexInBoard);
                else
                {
                    sudokuBoard.board[typeIndexInBoard].initializePlacesOfNumbersFromCube();                   
                }
            } while (findPairsAndDeleteTheirOtherOptions(type) || sudokuBoard.checkplacesOfNumbers(type == Type.col, typeIndexInBoard));
            return numbersOfLopps > 0;
        }

        private bool findPairsAndDeleteTheirOtherOptions(Type type)
        {
            Dictionary<int, List<int>> groupOptionalPairsNumbers = findSubsets();
            bool findAPair = false;
            foreach (int key in groupOptionalPairsNumbers.Keys)
            {
                if (groupOptionalPairsNumbers[key].Count == 2)
                {
                    int firstPairNumber = groupOptionalPairsNumbers[key].ElementAt(0);
                    int secondPairNumber = groupOptionalPairsNumbers[key].ElementAt(1);

                    int firstIndexInBoard = sudokuBoard.placesOfNumbers[firstPairNumber].ElementAt(0);
                    int firstIndexInCube = Calculations.getIndexInCubeByIndexInBoard(firstIndexInBoard, sudokuBoard.sizeOfBoard);
                    int firstCubeNumber = Calculations.getCubeNumberByIndex(firstIndexInBoard, sudokuBoard.sizeOfBoard);
                    int secondIndexInBoard = sudokuBoard.placesOfNumbers[firstPairNumber].ElementAt(1);
                    int secondIndexInCube = Calculations.getIndexInCubeByIndexInBoard(secondIndexInBoard, sudokuBoard.sizeOfBoard);
                    int secondCubeNumber = Calculations.getCubeNumberByIndex(secondIndexInBoard, sudokuBoard.sizeOfBoard);

                    List<int> remainingOptions = new List<int>();
                    remainingOptions.Add(firstPairNumber);
                    remainingOptions.Add(secondPairNumber);

                    if (sudokuBoard.board[firstCubeNumber].leaveInCellOnlyThePairs(firstIndexInCube, remainingOptions) ||
                    sudokuBoard.board[secondCubeNumber].leaveInCellOnlyThePairs(secondIndexInCube, remainingOptions))
                            findAPair = true;

                    if (type != Type.cube && firstCubeNumber == secondCubeNumber)
                    {
                        List<int> PlacesNotToDelete = new List<int>();
                        PlacesNotToDelete.Add(firstIndexInCube);
                        PlacesNotToDelete.Add(secondIndexInCube);
                        sudokuBoard.board[firstCubeNumber].deleteNumberFromCube(firstPairNumber, PlacesNotToDelete);
                    }
                }
                else
                {
                    if (groupOptionalPairsNumbers[key].Count > 2)
                    {
                        // it is taking to match time to check for trios,quatrets and more... so 
                        // they will get the excption message (if the board is unsolvable) in the backtracking
                        string numbers = string.Empty;
                        foreach(int number in groupOptionalPairsNumbers[key])
                        {
                            int charNumber = (char)(number + '0');
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
                }
            }
            return findAPair;

        }

        private Dictionary<int, List<int>> findSubsets()
        {
            // sorts 'placesOfNumbers' by value - by the length of the list in ascending order
            sudokuBoard.placesOfNumbers = StaticMethods.sortByValue(sudokuBoard.placesOfNumbers);
            List<List<int>> groupOptionalPairsIndexes = new List<List<int>>();
            Dictionary<int, List<int>> groupOptionalPairsNumbers = new Dictionary<int, List<int>>();
            int indexOfMissingNumber = 0;
            int sizeOfPlacesOfNumbers = sudokuBoard.placesOfNumbers.Count;
            while (indexOfMissingNumber < sizeOfPlacesOfNumbers && sudokuBoard.placesOfNumbers.Values.ElementAt(indexOfMissingNumber).Count == 2)
            {
                int indexInGroupOptionalPairsIndexes = placeInGroupOptionalPairsIndexes(groupOptionalPairsIndexes, sudokuBoard.placesOfNumbers.Values.ElementAt(indexOfMissingNumber));
                if (indexInGroupOptionalPairsIndexes != -1)
                {
                    groupOptionalPairsNumbers[indexInGroupOptionalPairsIndexes].Add(sudokuBoard.placesOfNumbers.Keys.ElementAt(indexOfMissingNumber));
                }
                else
                {
                    groupOptionalPairsNumbers.Add(groupOptionalPairsIndexes.Count, new List<int>());
                    groupOptionalPairsNumbers[groupOptionalPairsIndexes.Count].Add(sudokuBoard.placesOfNumbers.Keys.ElementAt(indexOfMissingNumber));
                    groupOptionalPairsIndexes.Add(sudokuBoard.placesOfNumbers.Values.ElementAt(indexOfMissingNumber));
                }
                indexOfMissingNumber++;
            }
            return groupOptionalPairsNumbers;
        }

        private int placeInGroupOptionalPairsIndexes(List<List<int>> groupOptionalPairsIndexes, List<int> indexsesList)
        {
            for (int optionalPairsIndex = 0; optionalPairsIndex < groupOptionalPairsIndexes.Count; optionalPairsIndex++)
                if (compareListsOfIndexes(indexsesList, groupOptionalPairsIndexes.ElementAt(optionalPairsIndex)))
                    return optionalPairsIndex;
            return -1;
        }

        private bool compareListsOfIndexes(List<int> firstIndexsesList, List<int> secondIndexsesList)
        {
            return (firstIndexsesList.ElementAt(0) == secondIndexsesList.ElementAt(0)) && (firstIndexsesList.ElementAt(1) == secondIndexsesList.ElementAt(1));
        }


        private void thirdStepOfSolving(ISudokuBoard board)
        {
            sudokuBoard = backTracking(sudokuBoard);
            if (sudokuBoard == null)
                throw new UnSolvableBordException();
        }
        public ISudokuBoard backTracking(ISudokuBoard board)
        {
            int cellWithLeastOptions = findUnSolvedCellWithLeastOptions();
            if (cellWithLeastOptions == -1)
            {
                return board;
            }

            int cubeNumberOfCell = Calculations.getCubeNumberByIndex(cellWithLeastOptions, sudokuBoard.sizeOfBoard);
            int indexInCubeOfCell = Calculations.getIndexInCubeByIndexInBoard(cellWithLeastOptions, sudokuBoard.sizeOfBoard);
            List<int> optionsOfCell = sudokuBoard.board[cubeNumberOfCell].getOptionalNumbers(indexInCubeOfCell);
            foreach (int option in optionsOfCell)
            {
                ISudokuBoard tryBoard = null;
                try
                {
                    tryBoard = board.copyBoard();
                    copyEmptyCellsOptions((Board)board, (Board)tryBoard);
                    tryBoard.putKnownNumberAndDeletOptions(cellWithLeastOptions, option, true, true);
                    tryBoard.board[Calculations.getCubeNumberByIndex(cellWithLeastOptions,sudokuBoard.sizeOfBoard)].deleteNumberFromCube(option, new List<int>());
                    tryBoard.sudokuSolver.secondStepOfSolving();
                    ISudokuBoard thirdStepOfSolvingBoard = tryBoard.sudokuSolver.backTracking(tryBoard);
                    if (thirdStepOfSolvingBoard != null)
                    {
                        return thirdStepOfSolvingBoard;
                    }
                }
                catch (SudokuExceptions s)
                {
                    // Console.WriteLine(s.Message);
                    // tryBoard = (Board)board.Clone();
                }
            }
            return null;
        }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Board : ISudokuBoard
    {

        private ISudokuCube[] board;
        public Dictionary<int, List<int>> placesOfNumbers { get; set; }
        public int sizeOfBoard { get; }
        public int SqrtOfSizeOfBoard { get; }
        public KnownNumbersNotInBoard knownNumbersNotInBoard { get; set; }

        public Board(String numbersInBoard)
        {
            SizeOfBoardIntegrity(numbersInBoard);
            sizeOfBoard = (int)Math.Sqrt(numbersInBoard.Length);
            SqrtOfSizeOfBoard = (int)Math.Sqrt(sizeOfBoard);
            Initialize(numbersInBoard, 0);
        }
        public void SizeOfBoardIntegrity(string numbersInBoard)
        {
            double doubleSizeOfBoard = Math.Sqrt(numbersInBoard.Length);
            if (!StaticMethods.isInt(Math.Sqrt(doubleSizeOfBoard)))
                throw new InvalidBoardSizeException();
        }
        private void Initialize(String numbersInBoard, int index)
        {
            placesOfNumbers = new Dictionary<int, List<int>>();
            InitializeADictionaryOfNumbersInBoard(placesOfNumbers);
            knownNumbersNotInBoard = new KnownNumbersNotInBoard();
            board = new Cube[sizeOfBoard];
            for (int numOfCube = 0; numOfCube < board.Length; numOfCube++)
            {
                board[numOfCube] = new Cube(numbersInBoard, numOfCube, this);
            }
        }
        private void InitializeADictionaryOfNumbersInBoard(Dictionary<int, List<int>> dictionaryOfNumbersInBoard)
        {
            for (int numOfCube = 0; numOfCube < sizeOfBoard; numOfCube++)
            {
                if (dictionaryOfNumbersInBoard.ContainsKey(numOfCube + 1))
                    dictionaryOfNumbersInBoard[numOfCube + 1] = new List<int>();
                else
                    dictionaryOfNumbersInBoard.Add(numOfCube + 1, new List<int>());
            }
        }
        private void printALine()
        {
            System.Console.Write("       ");
            for (int i = 0; i < sizeOfBoard; i++)
                StaticMethods.printAColOrCol(false, true, true);
            System.Console.WriteLine();
        }
        private void printCols(bool Line, bool DarkBlue)
        {
            for (int i = 0; i < sizeOfBoard + 1; i++)
                if (i % SqrtOfSizeOfBoard == 0)
                    if (i == 0)
                        StaticMethods.printAColOrCol(true, true, true);
                    else
                    {
                        if (Line)
                        {
                            StaticMethods.printAColOrCol(false, DarkBlue, false);
                            StaticMethods.printAColOrCol(true, true, false);
                        }
                        else
                            StaticMethods.printAColOrCol(true, true, true);
                    }
                else
                    if (Line)
                {
                    StaticMethods.printAColOrCol(false, DarkBlue, false);
                    StaticMethods.printAColOrCol(true, false, false);
                }
                else
                    StaticMethods.printAColOrCol(true, false, true);
            System.Console.WriteLine();
        }
        public void print()
        {
            printALine();
            for (int rowOfCubes = 0; rowOfCubes < SqrtOfSizeOfBoard; rowOfCubes++)
            {
                for (int rowInCube = 0; rowInCube < SqrtOfSizeOfBoard; rowInCube++)
                {
                    printCols(false, false);
                    StaticMethods.printAColOrCol(true, true, true);
                    for (int colOfCubes = 0; colOfCubes < SqrtOfSizeOfBoard; colOfCubes++)
                    {
                        board[rowOfCubes * SqrtOfSizeOfBoard + colOfCubes].print(rowInCube);
                    }
                    System.Console.WriteLine();
                    if ((rowInCube + 1) % SqrtOfSizeOfBoard == 0)
                        printCols(true, true);
                    else
                        printCols(true, false);
                }
            }
            Console.WriteLine("\n");
        }
        private int getCubeNumberByIndex(int index)
        {
            return ((index / sizeOfBoard) / SqrtOfSizeOfBoard) * SqrtOfSizeOfBoard +
                    (index % sizeOfBoard) / SqrtOfSizeOfBoard;
        }
        private int getRowOrColInCubeByIndexInBoard(int index,bool col)
        {
            return StaticMethods.getRowOcCol(col, index, sizeOfBoard) % SqrtOfSizeOfBoard;
        }
        private int getIndexInCubeByIndexInBoard(int indexInBoard)
        {
            return getRowOrColInCubeByIndexInBoard(indexInBoard, false) * SqrtOfSizeOfBoard + getRowOrColInCubeByIndexInBoard(indexInBoard, true);
        }
        private bool isplacesOfNumberContainsACube(int cubeNumber, int number)
        {
            foreach (int indexInBoard in placesOfNumbers[number])
                if (getCubeNumberByIndex(indexInBoard) == cubeNumber)
                    return true;
            return false;
        }
        private bool isCubeWorthChecking(int cubeNumber, int mostCommonNumber)
        {
            foreach (int indexInBoard in placesOfNumbers[mostCommonNumber])
                if (getCubeNumberByIndex(indexInBoard) / SqrtOfSizeOfBoard == cubeNumber / SqrtOfSizeOfBoard)
                {
                    if (!board[cubeNumber].isRowOrColFull(false, getRowOrColInCubeByIndexInBoard(indexInBoard, false)))
                        return true;
                }
                else
                    if (getCubeNumberByIndex(indexInBoard) % SqrtOfSizeOfBoard == cubeNumber % SqrtOfSizeOfBoard)
                        if (!board[cubeNumber].isRowOrColFull(true, getRowOrColInCubeByIndexInBoard(indexInBoard, true)))
                            return true;
            return false;
        }
        private bool isNumberInRowOrColOfCubes(int cubeNumber,bool col,int colOrRowIndexInCube,int[] forParams, int number)
        {
            int colOrRowIndexInBoard = StaticMethods.getRowOcCol(col, cubeNumber, SqrtOfSizeOfBoard) * SqrtOfSizeOfBoard + colOrRowIndexInCube; ;
            if (knownNumbersNotInBoard.isNumberInRowOrColInNumbersNotInBoard(col, colOrRowIndexInBoard, number))
                return true;
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
                if (forParams[1] != cubeNumber)
                    if (board[forParams[1]].isNumberInRowOrColInCube(col, colOrRowIndexInCube, number))
                        return true;
            return false;
        }
        public bool isPossibleIndexToNumber(int indexOfNumberInBoard, int indexOfNumberInCube, int number)
        {
            int cubeNumber = getCubeNumberByIndex(indexOfNumberInBoard);
            int rowOfCubeNumber = cubeNumber / SqrtOfSizeOfBoard;
            int colOfCubeNumber = cubeNumber % SqrtOfSizeOfBoard;
            int[] forParams = StaticMethods.forParameters(rowOfCubeNumber, false, SqrtOfSizeOfBoard);

            if (isNumberInRowOrColOfCubes(cubeNumber, false, indexOfNumberInCube / SqrtOfSizeOfBoard, forParams, number))
                return false;

            forParams = StaticMethods.forParameters(colOfCubeNumber, true, SqrtOfSizeOfBoard);

            if (isNumberInRowOrColOfCubes(cubeNumber, true, indexOfNumberInCube % SqrtOfSizeOfBoard, forParams, number))
                return false;
            return true;
        }
        public void deleteNumberFromRowOrCol(bool col,int indexOfNumberInBoard, int indexOfNumberInCube, int number)
        {
            int cubeNumber = getCubeNumberByIndex(indexOfNumberInBoard);
            int rowOrColOfCube = StaticMethods.getRowOcCol(col, cubeNumber, SqrtOfSizeOfBoard);
            int rowOrColInCube = StaticMethods.getRowOcCol(col, indexOfNumberInCube, SqrtOfSizeOfBoard);
            int[] forParams = StaticMethods.forParameters(rowOrColOfCube, col, SqrtOfSizeOfBoard);

            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
                if (forParams[1] != cubeNumber)
                    board[forParams[1]].deleteNumberFromRowOrColInCube(col, rowOrColInCube, number);
        }
        private int isOptionsInTheSameRowOrCol(List<int> numberOfOptionsInCube)
        {
            // comparing all the rows and cols to the row and col of the first option
            int rowOfFirst = numberOfOptionsInCube.ElementAt(0) / SqrtOfSizeOfBoard;
            int colOfFirst = numberOfOptionsInCube.ElementAt(0) % SqrtOfSizeOfBoard;
            bool sameRow = true;
            bool sameCol = true;
            for (int i =1;i< numberOfOptionsInCube.Count; i++)
            {
                if (sameRow && numberOfOptionsInCube.ElementAt(i) / SqrtOfSizeOfBoard != rowOfFirst)
                {
                    // There is at least one option with a diffrent row
                    sameRow = false;
                }
                if (sameCol && numberOfOptionsInCube.ElementAt(i) % SqrtOfSizeOfBoard != colOfFirst)
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
        private void putKnownNumberAndDeletOptions(bool firstStepOfSolving,int indexInBoard,int mostCommonNumber, bool deletFromRow, bool deletFromCol)
        {
            int indexInCube = getIndexInCubeByIndexInBoard(indexInBoard);
            board[getCubeNumberByIndex(indexInBoard)].putTheNumberAndDeletOptions(indexInCube, mostCommonNumber, deletFromRow, deletFromCol);
            if(firstStepOfSolving)
                placesOfNumbers[mostCommonNumber].Add(indexInBoard);
        }
        private void theOptionsInTheSameRowOrCol(bool col,List<int> optionsInCubeByBoardIndex, int mostCommonNumber, int theSameRowOrCol, Dictionary<int, List<int>> numbersInRowOrCol)
        {
            if (!numbersInRowOrCol.ContainsKey(theSameRowOrCol))
            {
                numbersInRowOrCol.Add(theSameRowOrCol, new List<int>());
            }
            numbersInRowOrCol[theSameRowOrCol].Add(mostCommonNumber);
            int firstIndexInBoard = optionsInCubeByBoardIndex.ElementAt(0);
            int indexInCube = getIndexInCubeByIndexInBoard(firstIndexInBoard);
            deleteNumberFromRowOrCol(col, optionsInCubeByBoardIndex.ElementAt(0), indexInCube, mostCommonNumber);
        }  
        
        private void checkIfOptionsInTheSameRowOrCol(List<int> optionsInCubeByBoardIndex, int mostCommonNumber)
        {
            if (optionsInCubeByBoardIndex.Count <= SqrtOfSizeOfBoard)
                switch (isOptionsInTheSameRowOrCol(optionsInCubeByBoardIndex))
                {
                    case -1: // the options dont have the same row or col
                        break;
                    case 1:  // the options have the same row
                        theOptionsInTheSameRowOrCol(false, optionsInCubeByBoardIndex, mostCommonNumber, optionsInCubeByBoardIndex.ElementAt(0) / sizeOfBoard, knownNumbersNotInBoard.numbersInRows);
                        break;
                    case 2: // the options have the same col
                        theOptionsInTheSameRowOrCol(true, optionsInCubeByBoardIndex, mostCommonNumber, optionsInCubeByBoardIndex.ElementAt(0) % sizeOfBoard, knownNumbersNotInBoard.numbersInCols);
                        break;
                }
        }
        private void checkNumberOfOptions(List<int> optionsInCubeByBoardIndex, int mostCommonNumber)
        {
            switch (optionsInCubeByBoardIndex.Count)
            {
                case 0:
                    // in the cube indexOfCube you can't put the number mostCommonNumber (exception)
                    break;
                case 1:
                    // put the number in it's only place and delete the options of the number
                    // in the cells with the same row and col
                    putKnownNumberAndDeletOptions(true,optionsInCubeByBoardIndex.ElementAt(0), mostCommonNumber,true,true);
                    break;
                default:
                    // if the options of 'mostCommonNumber' in the same row or col you can
                    //  delete mostCommonNumber's options from the rest of the row or col
                    checkIfOptionsInTheSameRowOrCol(optionsInCubeByBoardIndex,mostCommonNumber);
                    break;
            }
        }
        private void firstStepOfSolving()
        {
            int mostCommonNumber;
            // sorts 'placesOfNumbers' by value - by the length of the list
            StaticMethods.SortByValue(placesOfNumbers);                
            while (placesOfNumbers.Count != 0)
            {
                // mostCommonNumber = the element with the longest length of the value in 'placesOfNumbers',
                // (the number that exists most times in the board and it's in 'placesOfNumbers')
                mostCommonNumber = placesOfNumbers.Keys.ElementAt(0);
                for (int indexOfCube = 0; indexOfCube < sizeOfBoard; indexOfCube++)
                {
                    if (!isplacesOfNumberContainsACube(indexOfCube, mostCommonNumber))
                    {
                        bool  checkTheOptions = isCubeWorthChecking(indexOfCube, mostCommonNumber);
                        // a list that contains the indexes in the cube (by the indexes of the board) of where  
                        // the number mostCommonNumber can be
                        List<int> optionsInCubeByBoardIndex = board[indexOfCube].fillOptionsInCube(mostCommonNumber, checkTheOptions);
                        checkNumberOfOptions(optionsInCubeByBoardIndex,mostCommonNumber);
                    }
                }
                placesOfNumbers.Remove(mostCommonNumber);
            }
        }     
        private bool isNumberHasOnePlaceInRowOrCo(bool col,int MissingNumberInDic, int rowOrColInBoard,Dictionary<int, List<int>> optionsOfMissingNumbersInRowOrCol)
        {
            if (optionsOfMissingNumbersInRowOrCol[MissingNumberInDic].Count == 1)
            {
                int indexInBoard = optionsOfMissingNumbersInRowOrCol[MissingNumberInDic].ElementAt(0);
                int indexInCube = getIndexInCubeByIndexInBoard(indexInBoard);
                List<int> optionalNumbersOfPlace = board[getCubeNumberByIndex(indexInBoard)].getOptionalNumbers(indexInCube);
                optionalNumbersOfPlace.Remove(MissingNumberInDic);
                putKnownNumberAndDeletOptions(false, indexInBoard, MissingNumberInDic, !col, col);
                board[getCubeNumberByIndex(indexInBoard)].deleteNumberFromCube(MissingNumberInDic, optionsOfMissingNumbersInRowOrCol);
                // remove from optionsOfMissingNumbersInRowOrCol the MissingNumberInDic that you found
                optionsOfMissingNumbersInRowOrCol.Remove(MissingNumberInDic);
                foreach (int optionalNumber in optionalNumbersOfPlace)
                {
                    optionsOfMissingNumbersInRowOrCol[optionalNumber].Remove(indexInBoard);
                    if (optionsOfMissingNumbersInRowOrCol[optionalNumber].Count == 0)
                    {
                       // the row/col getRowOcCol(col,indexInBoard,SqrtOfSizeOfBoard) can not contains
                       // both of the numbers optionalNumber,MissingNumberInDic because the only place
                       // they can be in the row/col is in the same cell indexInBoard
                    }
                    else
                    {
                        isNumberHasOnePlaceInRowOrCo( col,optionalNumber, rowOrColInBoard, optionsOfMissingNumbersInRowOrCol);
                    }
                }
                return true;
            }
            else
            {
                if (optionsOfMissingNumbersInRowOrCol[MissingNumberInDic].Count == 0)
                {
                    // the row or col rowOrColInBoard can not conatins the number MissingNumberInDic                    
                }
            }
            return false;
        }

        public void checkOptionsOfMissingNumbersInRowOrCol(bool col,int rowOrColInBoard,Dictionary<int, List<int>> optionsOfMissingNumbersInRowOrCol)
        {
            for (int indexOfMissingNumberInDic = 0; indexOfMissingNumberInDic < optionsOfMissingNumbersInRowOrCol.Keys.Count; indexOfMissingNumberInDic++)
            {
                int MissingNumberInDic = optionsOfMissingNumbersInRowOrCol.Keys.ElementAt(indexOfMissingNumberInDic);
                if(isNumberHasOnePlaceInRowOrCo(col, MissingNumberInDic, rowOrColInBoard, optionsOfMissingNumbersInRowOrCol))
                    indexOfMissingNumberInDic = -1;
            }
        }
        private void secondStepOfSolving(bool col)
        {

            Dictionary<int, List<int>> optionsOfMissingNumbersInRowOrCol = new Dictionary<int, List<int>>();
            InitializeADictionaryOfNumbersInBoard(optionsOfMissingNumbersInRowOrCol);
            for (int rowOrColInBoard =0; rowOrColInBoard < sizeOfBoard; rowOrColInBoard++) 
            {
                // The dictionary optionsOfMissingNumbersInRowOrCol contains after this for below 
                // keys of optional numbers in the row or col (rowOrColInBoard) and for each optional 
                // number it contains an indexes list of where the optional number is located
                int[] forParams = StaticMethods.forParameters(rowOrColInBoard/ SqrtOfSizeOfBoard, col, SqrtOfSizeOfBoard);
                for (; forParams[1] < forParams[2]; forParams[1] += forParams[0]) 
                    board[forParams[1]].checksOptionsOfARowOrAColInCube(col, rowOrColInBoard% SqrtOfSizeOfBoard, optionsOfMissingNumbersInRowOrCol);
                checkOptionsOfMissingNumbersInRowOrCol(col, rowOrColInBoard, optionsOfMissingNumbersInRowOrCol);
                InitializeADictionaryOfNumbersInBoard(optionsOfMissingNumbersInRowOrCol);

               // print();
            }
        }
        private void BoardIntegrityOfRowOrCol(bool col, List<int> CountNumberInRowAndCol,int RowOrColOfCube, int RowOrColInCube)
        {
            int[] forParams = StaticMethods.forParameters(RowOrColOfCube, col, SqrtOfSizeOfBoard);
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
            {
                board[forParams[1]].rowOrColIntegrity(col, RowOrColInCube, RowOrColOfCube, CountNumberInRowAndCol);
            }
            CountNumberInRowAndCol.Clear();
        }
        private void BoardIntegrityOfCube(List<int> CountNumberInCube, int cubeNumber)
        {
            board[cubeNumber].cubeIntegrity(CountNumberInCube);
            CountNumberInCube.Clear();
        }
        private void ScanCubesInBoard(bool checkRowsOrCols)
        {
            // checkRowsOrCols is true when we want to check integrity of 
            // rows or cols and false when we want to check integrity of cubes.
            List<int> CountNumberInRowAndColOrCube = new  List<int>();
            for (int RowOrColOfCube = 0; RowOrColOfCube < SqrtOfSizeOfBoard; RowOrColOfCube++)
            {
                for (int RowOrColInCube = 0; RowOrColInCube < SqrtOfSizeOfBoard; RowOrColInCube++)
                {
                    if (checkRowsOrCols)
                    {
                        BoardIntegrityOfRowOrCol(true, CountNumberInRowAndColOrCube, RowOrColOfCube, RowOrColInCube);
                        BoardIntegrityOfRowOrCol(false, CountNumberInRowAndColOrCube, RowOrColOfCube, RowOrColInCube);
                    }
                    else
                        BoardIntegrityOfCube(CountNumberInRowAndColOrCube, RowOrColOfCube * SqrtOfSizeOfBoard + RowOrColInCube);
                }
            }
        }
        private void BoardIntegrity()
        {
            ScanCubesInBoard(true);
            ScanCubesInBoard(false);
        }
        private int countHowManySolvedCells()
        {
            int count = 0;
            for (int indexOfCube = 0; indexOfCube < sizeOfBoard; indexOfCube++)
                count += board[indexOfCube].countHowManySolvedCells();
            return count;
        }


        public void Solve()
        {
            BoardIntegrity();


            firstStepOfSolving();
            knownNumbersNotInBoard = null;

            int howManySolvedCells = countHowManySolvedCells();
            if (howManySolvedCells == sizeOfBoard * sizeOfBoard)
            {
                Console.WriteLine("solved");
                return;
            }
            Console.WriteLine("did not solve");

            print();

            secondStepOfSolving(true);

            print();

            secondStepOfSolving(false);

        }

    }
}

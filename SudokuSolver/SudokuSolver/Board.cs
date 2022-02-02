using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    enum Type{row,col,cube}
    class Board : ISudokuBoard
    {

        private ISudokuCube[] board;
        public Dictionary<int, List<int>> placesOfNumbers { get; set; }
        public int sizeOfBoard { get; }
        public int SqrtOfSizeOfBoard { get; }
        public KnownNumbersNotInBoard knownNumbersNotInBoard { get; set; }


        public int step { get; set; }

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
            step = 1;
            placesOfNumbers = new Dictionary<int, List<int>>();
            InitializePlacesOfNumbers();
            knownNumbersNotInBoard = new KnownNumbersNotInBoard();
            board = new Cube[sizeOfBoard];
            for (int numOfCube = 0; numOfCube < board.Length; numOfCube++)
            {
                board[numOfCube] = new Cube(numbersInBoard, numOfCube, this);
            }
        }
        private void InitializePlacesOfNumbers()
        {
            for (int numOfCube = 0; numOfCube < sizeOfBoard; numOfCube++)
            {
                if (placesOfNumbers.ContainsKey(numOfCube + 1))
                    placesOfNumbers[numOfCube + 1] = new List<int>();
                else
                    placesOfNumbers.Add(numOfCube + 1, new List<int>());
            }
        }
        private void printALine()
        {
            System.Console.Write("   ");
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
            int colOrRowIndexInBoard = StaticMethods.getRowOcCol(col, cubeNumber, SqrtOfSizeOfBoard) * SqrtOfSizeOfBoard + colOrRowIndexInCube; 
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
        private void putKnownNumberAndDeletOptions(int indexInBoard,int mostCommonNumber, bool deletFromRow, bool deletFromCol)
        {
            int indexInCube = getIndexInCubeByIndexInBoard(indexInBoard);
            board[getCubeNumberByIndex(indexInBoard)].putTheNumberAndDeletOptions(indexInCube, mostCommonNumber, deletFromRow, deletFromCol);
            if(step == 1)
                placesOfNumbers[mostCommonNumber].Add(indexInBoard);
        }
        private void theOptionsInTheSameRowOrCol(bool col,List<int> optionsInCubeByBoardIndex, int mostCommonNumber, int theSameRowOrCol)
        {
            Dictionary<int, List<int>> numbersInRowOrCol;
            if (col)
                numbersInRowOrCol = knownNumbersNotInBoard.numbersInCols;
            else
                numbersInRowOrCol = knownNumbersNotInBoard.numbersInRows;

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
                        theOptionsInTheSameRowOrCol(false, optionsInCubeByBoardIndex, mostCommonNumber, optionsInCubeByBoardIndex.ElementAt(0) / sizeOfBoard);
                        break;
                    case 2: // the options have the same col
                        theOptionsInTheSameRowOrCol(true, optionsInCubeByBoardIndex, mostCommonNumber, optionsInCubeByBoardIndex.ElementAt(0) % sizeOfBoard);
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
                    putKnownNumberAndDeletOptions(optionsInCubeByBoardIndex.ElementAt(0), mostCommonNumber,true,true);
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
        private bool isNumberHasOnePlaceInRowOrCol(bool col,int MissingNumberInDic, int rowOrColInCube)
        {
            // if MissingNumberInDic has only one place in rowOrColInBoard then
            if (placesOfNumbers[MissingNumberInDic].Count == 1)
            {
                // indexInBoard = only option of MissingNumberInDic in rowOrColInBoard
                int indexInBoard = placesOfNumbers[MissingNumberInDic].ElementAt(0);
                int indexInCube = getIndexInCubeByIndexInBoard(indexInBoard);
                int indexOfCube = getCubeNumberByIndex(indexInBoard);
                // optionalNumbersOfPlace = save the options at index 'indexInBoard'
                List<int> optionalNumbersOfPlace = board[indexOfCube].getOptionalNumbers(indexInCube);
                // remove from optionalNumbersOfPlace the MissingNumberInDic that you found
                optionalNumbersOfPlace.Remove(MissingNumberInDic);
                putKnownNumberAndDeletOptions( indexInBoard, MissingNumberInDic, !col, col);
                // delete all the options of MissingNumberInDic from the cube we found it at
                board[indexOfCube].deleteNumberFromCube(MissingNumberInDic,col, rowOrColInCube);
                // remove from placesOfNumbers the MissingNumberInDic that you found
                placesOfNumbers.Remove(MissingNumberInDic);

                InitializePlacesOfNumbers();
                initializePlacesOfNumbersWithOptions(col, StaticMethods.getRowOcCol(col, indexInBoard, sizeOfBoard));

                foreach (int optionalNumber in optionalNumbersOfPlace)
                {
                    if (!placesOfNumbers.ContainsKey(optionalNumber))
                        continue;
                    placesOfNumbers[optionalNumber].Remove(indexInBoard);
                    if (placesOfNumbers[optionalNumber].Count == 0)
                    {
                       // the row/col getRowOcCol(col,indexInBoard,SqrtOfSizeOfBoard) can not contains
                       // both of the numbers optionalNumber,MissingNumberInDic because the only place
                       // they can be in the row/col is in the same cell indexInBoard
                    }
                    else
                    {
                        isNumberHasOnePlaceInRowOrCol( col,optionalNumber, rowOrColInCube);
                    }
                }
                return true;
            }
            else
            {
                if (placesOfNumbers[MissingNumberInDic].Count == 0)
                {
                    // the row or col rowOrColInBoard can not conatins the number MissingNumberInDic
                    // 
                    // אם למשתנה יש יותר מאופציה אחת תבדוק האם האופציות באותה שורה ועמודה

                }
            }
            return false;
        }
        
        
        private bool compareListsOfIndexes(List<int> firstIndexsesList, List<int> secondIndexsesList)
        {
            return (firstIndexsesList.ElementAt(0) == secondIndexsesList.ElementAt(0)) && (firstIndexsesList.ElementAt(1) == secondIndexsesList.ElementAt(1));
        }

        private int placeInGroupOptionalPairsIndexes(List<List<int>> groupOptionalPairsIndexes, List<int> indexsesList)
        {
            for(int optionalPairsIndex =0; optionalPairsIndex < groupOptionalPairsIndexes.Count; optionalPairsIndex++)
                if (compareListsOfIndexes(indexsesList, groupOptionalPairsIndexes.ElementAt(optionalPairsIndex)))
                    return optionalPairsIndex;
            return - 1;
        }
        private bool findPairsAndDeleteTheirOtherOptions(bool col, int rowOrColInBoard)
        {
            Dictionary<int, List<int>> groupOptionalPairsNumbers = findSubsets();
            bool findAPair = false;
            foreach(int key in groupOptionalPairsNumbers.Keys)
            {
                if(groupOptionalPairsNumbers[key].Count == 2)
                {
                    int firstPairNumber = groupOptionalPairsNumbers[key].ElementAt(0);
                    int secondPairNumber = groupOptionalPairsNumbers[key].ElementAt(1);

                    int firstIndexInBoard = placesOfNumbers[firstPairNumber].ElementAt(0);
                    int firstCubeNumber = getCubeNumberByIndex(firstIndexInBoard);
                    int secondIndexInBoard = placesOfNumbers[firstPairNumber].ElementAt(1);
                    int secondCubeNumber = getCubeNumberByIndex(secondIndexInBoard);

                    if(board[firstCubeNumber].leaveOnlyThePairNumbers(getIndexInCubeByIndexInBoard(firstIndexInBoard), firstPairNumber, secondPairNumber) ||
                    board[secondCubeNumber].leaveOnlyThePairNumbers(getIndexInCubeByIndexInBoard(secondIndexInBoard), firstPairNumber, secondPairNumber))
                        findAPair = true;

                    // for cube
                        //for (int rowOrColInCube = 0; rowOrColInCube < SqrtOfSizeOfBoard; rowOrColInCube++)
                        //{
                        //    if (rowOrColInCube != getRowOrColInCubeByIndexInBoard(firstIndexInBoard, col))
                        //    {
                        //        board[firstCubeNumber].deleteNumberFromRowOrColInCube(col, rowOrColInCube, firstPairNumber);
                        //        board[firstCubeNumber].deleteNumberFromRowOrColInCube(col, rowOrColInCube, secondPairNumber);

                        //        if (secondCubeNumber != firstCubeNumber)
                        //        {
                        //            board[secondCubeNumber].deleteNumberFromRowOrColInCube(col, rowOrColInCube, firstPairNumber);
                        //            board[secondCubeNumber].deleteNumberFromRowOrColInCube(col, rowOrColInCube, secondPairNumber);
                        //        }
                        //    }
                        //}
                }
                else
                {
                    if (groupOptionalPairsNumbers[key].Count > 2)
                    {
                        // it is taking to match time to check for trios,quatrets and more... so 
                        // they will get the excption message (if the board is unsolvable) in the backtracking

                        //excption - the indexes in groupOptionalPairsIndexes.ElementAt(key) cant contain all the numbers in groupOptionalPairsNumbers[key]
                    }
                }
            }
            return findAPair;

        }
        private Dictionary<int, List<int>> findSubsets()
        {
            // sorts 'placesOfNumbers' by value - by the length of the list
            StaticMethods.SortByValue(placesOfNumbers);
            List<List<int>> groupOptionalPairsIndexes = new List<List<int>>();
            Dictionary<int, List<int>> groupOptionalPairsNumbers = new Dictionary<int, List<int>>();
            int indexOfMissingNumber = 0;
            int sizeOfPlacesOfNumbers = placesOfNumbers.Count;
            bool findAPair = false;
            while (indexOfMissingNumber < sizeOfPlacesOfNumbers && placesOfNumbers.Values.ElementAt(indexOfMissingNumber).Count == 2)
            {
                int indexInGroupOptionalPairsIndexes = placeInGroupOptionalPairsIndexes(groupOptionalPairsIndexes, placesOfNumbers.Values.ElementAt(indexOfMissingNumber));
                if (indexInGroupOptionalPairsIndexes != -1)
                {
                    groupOptionalPairsNumbers[indexInGroupOptionalPairsIndexes].Add(placesOfNumbers.Keys.ElementAt(indexOfMissingNumber));
                }
                else
                {
                    groupOptionalPairsNumbers.Add(groupOptionalPairsIndexes.Count, new List<int>());
                    groupOptionalPairsNumbers[groupOptionalPairsIndexes.Count].Add(placesOfNumbers.Keys.ElementAt(indexOfMissingNumber));
                    groupOptionalPairsIndexes.Add(placesOfNumbers.Values.ElementAt(indexOfMissingNumber));
                }
                indexOfMissingNumber++;
            }
            return groupOptionalPairsNumbers; 
        }
        private bool hiddenSubsets(bool col, int rowOrColInBoard)
        {
            int numbersOfLopps = -1;
            do
            {
                numbersOfLopps++;
                InitializePlacesOfNumbers();
                initializePlacesOfNumbersWithOptions(col, rowOrColInBoard);
            } while (findPairsAndDeleteTheirOtherOptions(col, rowOrColInBoard));
            return numbersOfLopps > 0;
        }


        //private bool compareListsOfIndexes(List<int> firstIndexsesList, List<int> secondIndexsesList)
        //{
        //    return (firstIndexsesList.ElementAt(0) == secondIndexsesList.ElementAt(0)) && (firstIndexsesList.ElementAt(1) == secondIndexsesList.ElementAt(1));
        //}

        //private int placeInGroupOptionalPairsIndexes(int startOfCountTwo, List<int> indexsesList)
        //{
        //    while (!compareListsOfIndexes(placesOfNumbers.Values.ElementAt(startOfCountTwo), indexsesList))
        //    {
        //        startOfCountTwo++;
        //    }
        //    return startOfCountTwo;
        //}
        //private void findPairsAndDeleteTheirOtherOptions()
        //{
        //    // sorts 'placesOfNumbers' by value - by the length of the list
        //    StaticMethods.SortByValue(placesOfNumbers);

        //    List<List<int>> groupOptionalPairsNumbers = new List<List<int>>();
        //    int sizeOfPlacesOfNumbers = placesOfNumbers.Count;
        //    int indexOfMissingNumber = 0;
        //    while (indexOfMissingNumber < sizeOfPlacesOfNumbers && placesOfNumbers.Values.ElementAt(indexOfMissingNumber).Count < 2)
        //    {
        //        indexOfMissingNumber++;
        //    }
        //    int startOfCountTwo = indexOfMissingNumber;
        //    while (indexOfMissingNumber < sizeOfPlacesOfNumbers && placesOfNumbers.Values.ElementAt(indexOfMissingNumber).Count == 2)
        //    {
        //        int indexInGroupOptionalPairsIndexes = placeInGroupOptionalPairsIndexes(startOfCountTwo, placesOfNumbers.Values.ElementAt(indexOfMissingNumber));
        //        if (indexInGroupOptionalPairsIndexes != indexOfMissingNumber)
        //        {
        //            groupOptionalPairsNumbers[indexInGroupOptionalPairsIndexes].Add(placesOfNumbers.Keys.ElementAt(indexOfMissingNumber));
        //        }
        //        else
        //        {
        //            groupOptionalPairsNumbers.Add(new List<int>());
        //            groupOptionalPairsNumbers.ElementAt(groupOptionalPairsNumbers.Count - 1).Add(placesOfNumbers.Keys.ElementAt(indexOfMissingNumber));
        //        }
        //        indexOfMissingNumber++;
        //    }
        //    foreach (int key in groupOptionalPairsNumbers.Keys)
        //    {
        //        if (groupOptionalPairsNumbers[key].Count == 2)
        //        {
        //            groupOptionalPairsIndexes.ElementAt(key)
        //        }
        //        else
        //        {
        //            if (groupOptionalPairsNumbers[key].Count > 2)
        //            {

        //            }
        //        }
        //    }

        //    //for (int indexOfMissingNumber = 0; indexOfMissingNumber < placesOfNumbers.Keys.Count; indexOfMissingNumber++)
        //    //{
        //    //    int MissingNumberInDic = placesOfNumbers.Keys.ElementAt(indexOfMissingNumber);
        //    //    if (placesOfNumbers[MissingNumberInDic].Count == group)
        //    //    {
        //    //        if ((group = placesOfNumbers[MissingNumberInDic].Count) > SqrtOfSizeOfBoard)
        //    //            break;



        //    //    }
        //    //}


        //    //    for (int indexOfMissingNumber = 0; indexOfMissingNumber < placesOfNumbers.Keys.Count; indexOfMissingNumber++)
        //    //{
        //    //    int MissingNumberInDic = placesOfNumbers.Keys.ElementAt(indexOfMissingNumber);
        //    //    if (placesOfNumbers[MissingNumberInDic].Count >= group) {
        //    //        if ((group = placesOfNumbers[MissingNumberInDic].Count) > SqrtOfSizeOfBoard)
        //    //            break;
        //    //        for (int indexOfNextMissingNumber = indexOfMissingNumber + 1; indexOfNextMissingNumber < placesOfNumbers.Keys.Count; indexOfNextMissingNumber++)
        //    //        {
        //    //            int NextMissingNumberInDic = placesOfNumbers.Keys.ElementAt(indexOfNextMissingNumber);
        //    //            if (placesOfNumbers[NextMissingNumberInDic].Count == group)
        //    //            {
        //    //                if (compareIndexesOfNumbers(placesOfNumbers[MissingNumberInDic], placesOfNumbers[NextMissingNumberInDic]))
        //    //                {

        //    //                }
        //    //            }
        //    //            else
        //    //            {
        //    //                break;
        //    //            }
        //    //        }
        //    //    }
        //    //}


        //}
        public void checkplacesOfNumbers(bool col, int rowOrColInCube )
        {
            for (int indexOfMissingNumberInDic = 0; indexOfMissingNumberInDic < placesOfNumbers.Keys.Count; indexOfMissingNumberInDic++)
            {
                int MissingNumberInDic = placesOfNumbers.Keys.ElementAt(indexOfMissingNumberInDic);
                if(isNumberHasOnePlaceInRowOrCol(col, MissingNumberInDic, rowOrColInCube))
                    indexOfMissingNumberInDic = -1;
            }
        }

        private void initializePlacesOfNumbersWithOptions(bool col,int rowOrColInBoard)
        {
            // The dictionary placesOfNumbers contains after this for below 
            // keys of optional numbers in the row or col (rowOrColInBoard) and for each optional 
            // number it contains an indexes list of where the optional number can be located
            int[] forParams = StaticMethods.forParameters(rowOrColInBoard / SqrtOfSizeOfBoard, col, SqrtOfSizeOfBoard);
            for (; forParams[1] < forParams[2]; forParams[1] += forParams[0])
                board[forParams[1]].checksOptionsOfARowOrAColInCube(col, rowOrColInBoard % SqrtOfSizeOfBoard);
        }


        private void secondStepOfSolving(bool col)
        {
            for (int rowOrColInBoard =0; rowOrColInBoard < sizeOfBoard; rowOrColInBoard++) 
            {
                hiddenSubsets(col, rowOrColInBoard);
                hiddenSubsets(col, rowOrColInBoard);
                checkplacesOfNumbers(col, rowOrColInBoard % SqrtOfSizeOfBoard);
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
            Console.WriteLine("after firstStepOfSolving:");
            print();
            int howManySolvedCells = countHowManySolvedCells();
            if (howManySolvedCells == sizeOfBoard * sizeOfBoard)
            {
                Console.WriteLine("solved");
                return;
            }
            Console.WriteLine("did not solve");

            // do a loop until you dont find more!
            step = 2;

            secondStepOfSolving(true);
            Console.WriteLine("after secondStepOfSolving(true):");
            print();

            secondStepOfSolving(false);
            Console.WriteLine("after secondStepOfSolving(false):");
            print();

            secondStepOfSolving(true);
            Console.WriteLine("after secondStepOfSolving(true):");
            print();

            secondStepOfSolving(false);
            Console.WriteLine("after secondStepOfSolving(false):");
            print();

            secondStepOfSolving(true);
            Console.WriteLine("after secondStepOfSolving(true):");
            print();

            secondStepOfSolving(false);
            Console.WriteLine("after secondStepOfSolving(false):");
            print();

            Console.WriteLine("after secondStepOfSolving:");
        }

    }
}

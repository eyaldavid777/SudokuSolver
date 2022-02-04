using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    interface ISudokuBoard 
    {
        Dictionary<int, List<int>> placesOfNumbers { get; set; }
        KnownNumbersNotInBoard knownNumbersNotInBoard { get; set; }

        ISudokuCube[] board { get; }
        int sizeOfBoard { get; }
        int SqrtOfSizeOfBoard { get; }
        int step { get; set; }

        ISudokuSolver sudokuSolver { get; set; }

        Board copyBoardWithNumber(int indexInBoard, char option);

        void putKnownNumberAndDeletOptions(int indexInBoard, int mostCommonNumber, bool deletFromRow, bool deletFromCol);

        Board copyBoard();

        void print();

        void hiddenSingleOfCube(List<int> optionsInCubeByBoardIndex, int mostCommonNumber);

        bool repetition(Board.Ptr repetitionContent);
        bool isPossibleIndexToNumber(int indexOfNumberInBoard, int indexOfNumberInCube, int number);

        void InitializePlacesOfNumbers();

        void initializePlacesOfNumbersFromRowOrCol(bool col, int rowOrColInBoard);

        bool hiddenSingleOfRowAndCol(Type type, int rowOrColInBoard);


        bool isTheCubeWorthChecking(int cubeNumber, int mostCommonNumber);

        void deleteNumberFromRowOrCol(bool col, int indexOfNumberInBoard, int indexOfNumberInCube, int number);

    }
}

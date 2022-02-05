using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public interface ISudokuBoard 
    {
        // the interface ISudokuBoard reveals to you what you are allowes to use in board.
        Dictionary<int, List<int>> placesOfNumbers { get; set; }
        IntersectionNumbers intersectionNumbers { get; set; }

        ISudokuCube[] board { get; }
        int sizeOfBoard { get; }
        int sqrtOfSizeOfBoard { get; }
        bool firstStep { get; set; }

        ISudokuSolver sudokuSolver { get; set; }

        void boardIntegrity(bool checkCells = true);

        string boardString();
        void putKnownNumberAndDeletOptions(int indexInBoard, int mostCommonNumber, bool deletFromRow, bool deletFromCol);

        Board copyBoard();

        void printAColOrRow(bool col, bool DarkBlue, bool withSpaces);

        void print();

        void hiddenSingleOfCube(List<int> optionsInCubeByBoardIndex, int mostCommonNumber);

        bool repetition(Board.Ptr repetitionContent);
        bool isPossibleIndexToNumber(int indexOfNumberInBoard, int number);

        void InitializePlacesOfNumbers();

        void initializePlacesOfNumbersFromRowOrCol(bool col, int rowOrColInBoard);

        bool hiddenSingleOfRowAndCol(Type type, int rowOrColInBoard);


        bool isTheCubeWorthChecking(int cubeNumber, int mostCommonNumber);

        void deleteNumberFromRowOrCol(bool col, int indexOfNumberInBoard, int number);

    }
}

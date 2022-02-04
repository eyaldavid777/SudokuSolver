using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    interface ISudokuCube 
    {
        void print(int rowInCube);

        List<int> fillOptionsInCube(int number, bool checkTheOptions);

        String rowOrCulIncubeString(bool col, int colOrRowIndex);


        ICell getCell(int indexInCube);

        bool isNumberInRowOrColInCube(bool col, int colOrRowIndex, int number);

        bool isRowOrColFull(bool col, int colOrRowIndex);

        void putTheNumberAndDeletOptions(int indexInCube, int knownNumber, bool deletFromRow, bool deletFromCol);

        void deleteNumberFromRowOrColInCube(bool col, int colOrRowIndexInCube, int number);

        void rowOrColIntegrity(bool col, int RowOrColInCube, int RowOrColOfCube, List<int> CountNumberInRowAndCol);

        void cubeIntegrity(List<int> CountNumberInRowAndCol);

        int countHowManySolvedCells();

        void checksOptionsOfARowOrAColInCube(bool col, int rowOrColInCube);

        void deleteNumberFromCube(int number, List<int> PlacesNotToDelete);

        List<int> getOptionalNumbers(int indexInCube); 

        bool nakedSingleOfACube();

        void initializePlacesOfNumbersFromCube();

        void leaveInCellOnlyTheListOptions(int indexInCube, List<int> options);

        bool leaveInCellOnlyThePairs(int indexInCube, List<int> remainingOptions);

    }
}

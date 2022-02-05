using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public interface ISudokuCube 
    {
        // the interface ISudokuCube reveals to you what you are allowes to use in cube.
        void print(int rowInCube);

        List<int> fillOptionsInCube(int number, bool checkTheOptions);

        string rowOrCulIncubeString(int rowInCube);


        ICell getCell(int indexInCube);

        void cellsInCubeIntegrity();

        bool isNumberInRowOrColInCube(bool col, int colOrRowIndex, int number);

        bool isRowOrColFull(bool col, int colOrRowIndex);

        void putTheNumberAndDeletOptions(int indexInCube, int knownNumber, bool deletFromRow, bool deletFromCol);

        void deleteNumberFromRowOrColInCube(bool col, int colOrRowIndexInCube, int number);

        void rowOrColOrCubeIntegrity(Type type, List<int> CountNumber, int rowOrColInCube = -1, int rowOrColOfCube = -1);

        void copyOptionsOfARowOrAColInCube(bool col, int rowOrColInCube);

        void deleteNumberFromCube(int number, List<int> PlacesNotToDelete);

        List<int> getOptionalNumbers(int indexInCube);


        bool checkCountOfOptionsInIndex(int indexInCube);

        void initializePlacesOfNumbersFromCube();

        void leaveInCellOnlyTheListOptions(int indexInCube, List<int> options);

        bool leaveInCellOnlyThePairs(int indexInCube, List<int> remainingOptions);

    }
}

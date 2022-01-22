﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public static class StaticMethods
    {
        public static int[] forParameters(int colOrRowIndex, bool col,int sqrtOfSizeOfBoard)
        {
            int[] forParams = new int[3];
            if (col)
            {
                forParams[0] = sqrtOfSizeOfBoard;
                forParams[1] = colOrRowIndex;
                forParams[2] = colOrRowIndex + sqrtOfSizeOfBoard * (sqrtOfSizeOfBoard - 1) + 1;
                return forParams;
            }
            forParams[0] = 1;
            forParams[1] = colOrRowIndex * sqrtOfSizeOfBoard;
            forParams[2] = forParams[1] + sqrtOfSizeOfBoard;
            return forParams;
        }
        public static void printAColOrCol(bool col, bool DarkBlue, bool withSpaces)
        {
            if (DarkBlue)
                Console.ForegroundColor = ConsoleColor.DarkBlue;
            if (col)
                if (withSpaces)
                    System.Console.Write("       |");
                else
                    System.Console.Write("|");
            else
                 if (withSpaces)
                System.Console.Write(" _______");
            else
                System.Console.Write("_______");
            if (DarkBlue)
                Console.ForegroundColor = ConsoleColor.White;
        }

       
    }
}
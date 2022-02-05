using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Security;

namespace SudokuSolver
{
    class FileReaderService : IReader
    {
        //The class FileReaderService responsible of reading the file and returs it content.

        /*
        * Summary:
        * The functionc gets the path of the file.
        *  
        * Input:
        * None.
        * 
        * OutPut:
        * The functions returns the path of the file.
        */
        public string GetPathOfFile()
        {
            Console.WriteLine("write file path: ");
            string filePath = Console.ReadLine();
            if(filePath.Length > 4 &&  filePath[filePath.Length -1] == 't' && filePath[filePath.Length - 2] == 'x' && filePath[filePath.Length - 3] == 't' && filePath[filePath.Length - 4] == '.')
            {
                return filePath;
            }
            return "";
        }

        /*
        * Summary:
        * The functionc gets the content (the string board) of the file GetPathOfFile().
        *  
        * Input:
        * None.
        * 
        * OutPut:
        * The functions returns the content (the string board) of the file GetPathOfFile().
        */
        public string ReadBoard()
        {
            string filePath = GetPathOfFile();
            string content = string.Empty;
            try
            {
                content = File.ReadAllText(filePath);
            }
            catch (IOException i)
            {
                content = "";
            }
            catch (ArgumentException a)
            {
                content = "";
            }
            catch (NotSupportedException n)
            {
                content = "";
            }
            catch (UnauthorizedAccessException u)
            {
                content = "";
            }
            catch (SecurityException s)
            {
                content = "";
            }
            return content;

        }

    }
}

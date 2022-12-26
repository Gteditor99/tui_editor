using System;
using System.IO;
using System.Threading;

namespace TuiEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "";
            if (args.Length > 0)
            {
                fileName = args[0];
                if (!File.Exists(fileName))
                {
                    Console.WriteLine("Error: file not found");
                    return;
                }
            }

            string[] lines = null;
            if (fileName != "")
            {
                lines = File.ReadAllLines(fileName);
            }
            else
            {
                lines = new string[1];
            }

            int cursorLine = 0;
            int cursorColumn = 0;

            bool running = true;
            while (running && cursorLine >= 0 && cursorLine < lines.Length)
            {
                Console.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    Console.WriteLine(lines[i]);
                }

                Console.SetCursorPosition(cursorColumn, cursorLine);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" ");
                Console.ResetColor();

                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.LeftArrow)
                {
                    cursorColumn--;
                    if (cursorColumn < 0)
                    {
                        cursorColumn = 0;
                    }
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    cursorColumn++;
                    if (cursorColumn >= lines[cursorLine].Length)
                    {
                        cursorColumn = lines[cursorLine].Length;
                    }
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    // Split the current line at the cursor position
                    string left = lines[cursorLine].Substring(0, cursorColumn);
                    string right = lines[cursorLine].Substring(cursorColumn);

                    // Insert a new line and move the cursor to the beginning of the new line
                    lines[cursorLine] = left;
                    cursorLine++;
                    cursorColumn = 0;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (cursorColumn > 0)
                    {
                        lines[cursorLine] = lines[cursorLine].Remove(cursorColumn - 1, 1);
                        cursorColumn--;
                    }
                }
                else if (key.Key == ConsoleKey.Delete)
                {
                    if (cursorColumn < lines[cursorLine].Length)
                    {
                        lines[cursorLine] = lines[cursorLine].Remove(cursorColumn, 1);
                    }
                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    cursorLine--;
                    if (cursorLine < 0)
                    {
                        cursorLine = 0;
                    }
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    cursorLine++;
                    if (cursorLine >= lines.Length)
                    {
                        cursorLine = lines.Length - 1;
                    }
                }

                else if (key.Key == ConsoleKey.Escape)
                {
                    running = false;
                }
                else if (key.Key == ConsoleKey.S && key.Modifiers == ConsoleModifiers.Control)
                {
                    File.WriteAllLines(fileName, lines);
                }
                else if (key.Key == ConsoleKey.S && key.Modifiers == ConsoleModifiers.Shift && key.Modifiers == ConsoleModifiers.Control)
                {
                    Console.WriteLine("Enter file name: ");
                    string newFileName = Console.ReadLine();
                    File.WriteAllLines(newFileName, lines);

                }


                else
                {
                    lines[cursorLine] = lines[cursorLine].Insert(cursorColumn, key.KeyChar.ToString());
                    cursorColumn++;
                }
            }
        }
    }
}


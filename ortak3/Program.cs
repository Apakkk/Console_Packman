
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Xml.Schema;

namespace CursorMovement
{
    class Program
    {
        static Random random = new Random();
        static string[,] map = new string[56, 26];
        static string[,] collectibles = new string[55, 25];
        static int[,,] walls = new int[10, 4, 4];

        static int mine = 3;
        static int energy = 200;
        static int score = 0;

        static enemies[] struct_enemies = new enemies[199]; //the position of enemies

        static int playerx = 0, playery = 0; //the position of player
        static void NewElements(int timer) // placement of mines and numbers
        {
            if (timer % 10 == 0 && timer != 0)
            {
                int odd = random.Next(1, 11);
                int x = random.Next(3, 45);
                int y = random.Next(4, 20);
                bool loccontrol = false;
                for (int i = 0; i < struct_enemies.Length; i++)
                {
                    if ((x == struct_enemies[i].xxx && y == struct_enemies[i].xyy) || x == struct_enemies[i].yxx && y == struct_enemies[i].yyy)
                        loccontrol = true;
                }
                if (map[x, y] == "#" || collectibles[x, y] != null || loccontrol == true || (playerx == x && playery == y))
                    NewElements(timer);
                if (map[x, y] != "#" && collectibles[x, y] == null && loccontrol == false && (playerx != x || playery != y))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(x, y);
                    if (odd < 7)
                    {
                        Console.WriteLine("1");
                        collectibles[x, y] = "1";
                    }
                    else if (odd < 10)
                    {
                        Console.WriteLine("2");
                        collectibles[x, y] = "2";
                    }
                    else
                    {
                        Console.WriteLine("3");
                        collectibles[x, y] = "3";
                    }
                }
            }
        }
        static void DeleteCore(int x, int y, int z)
        {
            if (z == 0)
            {

                Console.SetCursorPosition(3 + 5 * x, (5 * y) + 1);
                Console.Write(" ");
                map[5 * (x) + 3, 5 * y + 1] = null;
                Console.SetCursorPosition(3 + 5 * x, (5 * y) + 2);
                Console.Write(" ");
                map[3 + 5 * x, (5 * y) + 2] = null;

            }
            else if (z == 1)
            {

                Console.SetCursorPosition(5 * x, (5 * y) + 1);
                Console.Write(" ");
                map[(5 * x), (5 * y) + 1] = null;
                Console.SetCursorPosition(5 * x, (5 * y) + 2);
                Console.Write(" ");
                map[(5 * x), (5 * y) + 2] = null;



            }
            else if (z == 2)
            {
                Console.SetCursorPosition(5 * x + 1, 5 * y);
                Console.Write("  ");
                map[(5 * x) + 1, (5 * y)] = null;
                map[(5 * x) + 2, (5 * y)] = null;


            }
            else if (z == 3)
            {
                Console.SetCursorPosition(5 * x + 1, 3 + 5 * (y));
                Console.Write("  ");
                map[(5 * x) + 1, 5 * (y) + 3] = null;
                map[(5 * x) + 2, 5 * (y) + 3] = null;

            }

            walls[x - 1, y - 1, z] = 0;

            if (walls[x - 1, y - 1, 0] == 0 && walls[x - 1, y - 1, 2] == 0)
            {
                Console.SetCursorPosition(3 + 5 * x, 5 * y);
                Console.Write(" ");
                map[5 * (x) + 3, 5 * y] = null;

            }
            if (walls[x - 1, y - 1, 0] == 0 && walls[x - 1, y - 1, 3] == 0)
            {
                Console.SetCursorPosition(3 + 5 * x, (5 * y) + 3);
                Console.Write(" ");
                map[5 * (x) + 3, 5 * y + 3] = null;
            }


            if (walls[x - 1, y - 1, 1] == 0 && walls[x - 1, y - 1, 2] == 0)
            {
                Console.SetCursorPosition(5 * x, 5 * y);
                Console.Write(" ");
                map[(5 * x), (5 * y)] = null;

            }
            if (walls[x - 1, y - 1, 1] == 0 && walls[x - 1, y - 1, 3] == 0)
            {
                Console.SetCursorPosition(5 * x, (5 * y) + 3);
                Console.Write(" ");
                map[(5 * x), (5 * y + 3)] = null;

            }

        }
        static string[,] PrintCore(int x, int y, int z)
        {
            if (z == 0)
            {
                Console.SetCursorPosition(3 + 5 * x, (5 * y) + 1);
                Console.Write("#");
                map[5 * (x) + 3, 5 * y + 1] = "#";
                Console.SetCursorPosition(3 + 5 * x, (5 * y) + 2);
                Console.Write("#");
                map[5 * (x) + 3, 5 * y + 2] = "#";
            }
            else if (z == 1)
            {
                Console.SetCursorPosition(5 * x, (5 * y) + 1);
                Console.Write("#");
                map[(5 * x), (5 * y) + 1] = "#";
                Console.SetCursorPosition(5 * x, (5 * y) + 2);
                Console.Write("#");
                map[(5 * x), (5 * y) + 2] = "#";
            }
            else if (z == 2)
            {
                Console.SetCursorPosition(5 * x + 1, 5 * y);
                Console.Write("##");
                map[(5 * x) + 1, (5 * y)] = "#";
                map[(5 * x) + 2, (5 * y)] = "#";
            }
            else if (z == 3)
            {
                Console.SetCursorPosition(5 * x + 1, 3 + 5 * (y));
                Console.Write("##");
                map[(5 * x) + 1, 5 * (y) + 3] = "#";
                map[(5 * x) + 2, 5 * (y) + 3] = "#";
            }

            walls[x - 1, y - 1, z] = 1;

            if (walls[x - 1, y - 1, 0] == 1 || walls[x - 1, y - 1, 2] == 1)
            {
                Console.SetCursorPosition(3 + 5 * x, 5 * y);
                Console.Write("#");
                map[5 * (x) + 3, 5 * y] = "#";
            }
            if (walls[x - 1, y - 1, 0] == 1 || walls[x - 1, y - 1, 3] == 1)
            {
                Console.SetCursorPosition(3 + 5 * x, (5 * y) + 3);
                Console.Write("#");
                map[5 * (x) + 3, 5 * y + 3] = "#";
            }
            if (walls[x - 1, y - 1, 1] == 1 || walls[x - 1, y - 1, 2] == 1)
            {
                Console.SetCursorPosition(5 * x, 5 * y);
                Console.Write("#");
                map[(5 * x), (5 * y)] = "#";
            }
            if (walls[x - 1, y - 1, 1] == 1 || walls[x - 1, y - 1, 3] == 1)
            {
                Console.SetCursorPosition(5 * x, (5 * y) + 3);
                Console.Write("#");
                map[(5 * x), (5 * y) + 3] = "#";
            }
            return map;
        }
        static void Gameover(int score)
        {
            ConsoleKeyInfo cki;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(0, 3);
            Console.WriteLine("# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                   GGGGGGGGGGGGG         AAAA      MMMM       MMMM   EEEEEEEEEEEEEE                    #");
            Console.WriteLine("#                 GGGG                   AA  AA     MM M       M MM   EE                                #");
            Console.WriteLine("#                GG                     AA    AA    MM MM     MM MM   EE                                #");
            Console.WriteLine("#                GG                     AA    AA    MM  MM   MM  MM   EE                                #");
            Console.WriteLine("#                GG       GGGGGGG      AAAAAAAAAA   MM  MM   MM  MM   EEEEEEEEEEEEEE                    #");
            Console.WriteLine("#                GG            GG      AA      AA   MM   M   M   MM   EE                                #");
            Console.WriteLine("#                 GGGG         GG      AA      AA   MM   MM MM   MM   EE                                #");
            Console.WriteLine("#                   GGGGGGGGGGGGG      AA      AA   MM    MMM    MM   EEEEEEEEEEEEEE                    #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                OOOO   V       V EEEEE   RRRR                                          #");
            Console.WriteLine("#                               O    O   V     V  E       R   R                                         #");
            Console.WriteLine("#                               O    O    V   V   EEEEE   R   R                                         #");
            Console.WriteLine("#                               O    O     V V    E       RRRR                                          #");
            Console.WriteLine("#                                OOOO       V     EEEEE   R   R                                         #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #");
            Console.SetCursorPosition(86, 22);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Your score: " + score);
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.Escape || cki.Key == ConsoleKey.Enter)
                        start();
                }
            }
            Console.ResetColor();
        }
        static void ChangeWalls()
        {
            bool isThereObstacle = false;
            int a = random.Next(1, 11);
            int b = random.Next(1, 4);
            int count = 0;
            int c = random.Next(0, 4);


            for (int i = 0; i < 4; i++)
            {
                if (walls[a - 1, b - 1, i] == 1)
                    count++;

            }
            if (count == 3)
            {
                while (walls[a - 1, b - 1, c] != 1)
                {
                    c = random.Next(0, 4);
                }
                DeleteCore(a, b, c);
            }
            else if (count == 1)
            {
                while (walls[a - 1, b - 1, c] != 0)
                {
                    c = random.Next(0, 4);
                }
                if (c == 0)
                {
                    for (int i = 0; i < collectibles.GetLength(0); i++)
                    {
                        for (int j = 0; j < collectibles.GetLength(1); j++)
                        {
                            for (int m = 0; m < struct_enemies.Length; m++)
                            {
                                if ((struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b) || (struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b + 1) || (struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b + 2) || (struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b + 3))
                                {
                                    isThereObstacle = true;
                                    break;
                                }

                                else if ((playerx == 5 * a + 3 && playery == 5 * b) || (struct_enemies[m].yxx == 5 * a + 3 && struct_enemies[m].yyy == 5 * b) || (struct_enemies[m].yxx == 5 * a + 3 && struct_enemies[m].yyy == 5 * b + 1) || (struct_enemies[m].yxx == 5 * a + 3 && struct_enemies[m].yyy == 5 * b + 2) || (struct_enemies[m].yxx == 5 * a + 3 && struct_enemies[m].yyy == 5 * b + 3))
                                {
                                    isThereObstacle = true;
                                    break;
                                }
                                else if (collectibles[5 * a + 3, 5 * b] != null || collectibles[5 * a + 3, 5 * b + 1] != null || collectibles[5 * a + 3, 5 * b + 2] != null || collectibles[5 * a + 3, 5 * b + 3] != null)
                                {
                                    isThereObstacle = true;
                                    break;
                                }
                            }
                        }

                    }
                }
                else if (c == 1)
                {
                    for (int i = 0; i < collectibles.GetLength(0); i++)
                    {
                        for (int j = 0; j < collectibles.GetLength(1); j++)
                        {
                            for (int m = 0; m < struct_enemies.Length; m++)
                            {
                                if ((struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b) || (struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b + 1) || (struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b + 2) || (struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b + 3))
                                {
                                    isThereObstacle = true;
                                    break;
                                }

                                else if ((playerx == 5 * a && playery == 5 * b) || (struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b) || (struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b + 1) || (struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b + 2) || (struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b + 3))
                                {
                                    isThereObstacle = true;
                                    break;
                                }
                                else if (collectibles[5 * a, 5 * b] != null || collectibles[5 * a, 5 * b + 1] != null || collectibles[5 * a, 5 * b + 2] != null || collectibles[5 * a, 5 * b + 3] != null)
                                {
                                    isThereObstacle = true;
                                    break;
                                }
                            }
                        }

                    }
                }
                else if (c == 2)
                {
                    for (int i = 0; i < collectibles.GetLength(0); i++)
                    {
                        for (int j = 0; j < collectibles.GetLength(1); j++)
                        {
                            for (int m = 0; m < struct_enemies.Length; m++)
                            {
                                if ((struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b) || (struct_enemies[m].xxx == 5 * a + 1 && struct_enemies[m].xyy == 5 * b) || (struct_enemies[m].xxx == 5 * a + 2 && struct_enemies[m].xyy == 5 * b) || (struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b))
                                {
                                    isThereObstacle = true;
                                    break;
                                }

                                else if ((playerx == 5 * a && playery == 5 * b) || (struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b) || (struct_enemies[m].xxx == 5 * a + 1 && struct_enemies[m].xyy == 5 * b) || (struct_enemies[m].xxx == 5 * a + 2 && struct_enemies[m].xyy == 5 * b) || (struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b))
                                {
                                    isThereObstacle = true;
                                    break;
                                }
                                else if (collectibles[5 * a, 5 * b] != null || collectibles[5 * a + 1, 5 * b] != null || collectibles[5 * a + 2, 5 * b] != null || collectibles[5 * a + 3, 5 * b] != null)
                                {
                                    isThereObstacle = true;
                                    break;
                                }
                            }
                        }

                    }
                }
                else if (c == 3)
                {
                    for (int i = 0; i < collectibles.GetLength(0); i++)
                    {
                        for (int j = 0; j < collectibles.GetLength(1); j++)
                        {
                            for (int m = 0; m < struct_enemies.Length; m++)
                            {
                                if ((struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b + 3) || (struct_enemies[m].xxx == 5 * a + 1 && struct_enemies[m].xyy == 5 * b + 3) || (struct_enemies[m].xxx == 5 * a + 2 && struct_enemies[m].xyy == 5 * b + 3) || (struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b + 3))
                                {
                                    isThereObstacle = true;
                                    break;
                                }

                                else if ((playerx == 5 * a && playery == 5 * b + 3) || (struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b + 3) || (struct_enemies[m].xxx == 5 * a + 1 && struct_enemies[m].xyy == 5 * b + 3) || (struct_enemies[m].xxx == 5 * a + 2 && struct_enemies[m].xyy == 5 * b + 3) || (struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b + 3))
                                {
                                    isThereObstacle = true;
                                    break;
                                }
                                else if (collectibles[5 * a, 5 * b + 3] != null || collectibles[5 * a + 1, 5 * b + 3] != null || collectibles[5 * a + 2, 5 * b + 3] != null || collectibles[5 * a + 3, 5 * b + 3] != null)
                                {
                                    isThereObstacle = true;
                                    break;
                                }
                            }
                        }

                    }
                }
                if (!isThereObstacle)
                {
                    PrintCore(a, b, c);
                }
            }
            else
            {
                if (walls[a - 1, b - 1, c] == 1)
                    DeleteCore(a, b, c);
                else
                {
                    if (c == 0)
                    {
                        for (int i = 0; i < collectibles.GetLength(0); i++)
                        {
                            for (int j = 0; j < collectibles.GetLength(1); j++)
                            {
                                for (int m = 0; m < struct_enemies.Length; m++)
                                {
                                    if ((struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b) || (struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b + 1) || (struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b + 2) || (struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b + 3))
                                    {
                                        isThereObstacle = true;
                                        break;
                                    }

                                    else if ((playerx == 5 * a + 3 && playery == 5 * b) || (struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b) || (struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b + 1) || (struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b + 2) || (struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b + 3))
                                    {
                                        isThereObstacle = true;
                                        break;
                                    }
                                    else if (collectibles[5 * a + 3, 5 * b] != null || collectibles[5 * a + 3, 5 * b + 1] != null || collectibles[5 * a + 3, 5 * b + 2] != null || collectibles[5 * a + 3, 5 * b + 3] != null)
                                    {
                                        isThereObstacle = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (c == 1)
                    {
                        for (int i = 0; i < collectibles.GetLength(0); i++)
                        {
                            for (int j = 0; j < collectibles.GetLength(1); j++)
                            {
                                for (int m = 0; m < struct_enemies.Length; m++)
                                {
                                    if ((struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b) || (struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b + 1) || (struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b + 2) || (struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b + 3))
                                    {
                                        isThereObstacle = true;
                                        break;
                                    }

                                    else if ((playerx == 5 * a && playery == 5 * b) || (struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b) || (struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b + 1) || (struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b + 2) || (struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b + 3))
                                    {
                                        isThereObstacle = true;
                                        break;
                                    }
                                    else if (collectibles[5 * a, 5 * b] != null || collectibles[5 * a, 5 * b + 1] != null || collectibles[5 * a, 5 * b + 2] != null || collectibles[5 * a, 5 * b + 3] != null)
                                    {
                                        isThereObstacle = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    else if (c == 2)
                    {
                        for (int i = 0; i < collectibles.GetLength(0); i++)
                        {
                            for (int j = 0; j < collectibles.GetLength(1); j++)
                            {
                                for (int m = 0; m < struct_enemies.Length; m++)
                                {
                                    if ((struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b) || (struct_enemies[m].xxx == 5 * a + 1 && struct_enemies[m].xyy == 5 * b) || (struct_enemies[m].xxx == 5 * a + 2 && struct_enemies[m].xyy == 5 * b) || (struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b))
                                    {
                                        isThereObstacle = true;
                                        break;
                                    }

                                    else if ((playerx == 5 * a && playery == 5 * b) || (struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b) || (struct_enemies[m].xxx == 5 * a + 1 && struct_enemies[m].xyy == 5 * b) || (struct_enemies[m].xxx == 5 * a + 2 && struct_enemies[m].xyy == 5 * b) || (struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b))
                                    {
                                        isThereObstacle = true;
                                        break;
                                    }
                                    else if (collectibles[5 * a, 5 * b] != null || collectibles[5 * a + 1, 5 * b] != null || collectibles[5 * a + 2, 5 * b] != null || collectibles[5 * a + 3, 5 * b] != null)
                                    {
                                        isThereObstacle = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (c == 3)
                    {
                        for (int i = 0; i < collectibles.GetLength(0); i++)
                        {
                            for (int j = 0; j < collectibles.GetLength(1); j++)
                            {
                                for (int m = 0; m < struct_enemies.Length; m++)
                                {
                                    if ((struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b + 3) || (struct_enemies[m].xxx == 5 * a + 1 && struct_enemies[m].xyy == 5 * b + 3) || (struct_enemies[m].xxx == 5 * a + 2 && struct_enemies[m].xyy == 5 * b + 3) || (struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b + 3))
                                    {
                                        isThereObstacle = true;
                                        break;
                                    }

                                    else if ((playerx == 5 * a && playery == 5 * b + 3) || (struct_enemies[m].xxx == 5 * a && struct_enemies[m].xyy == 5 * b + 3) || (struct_enemies[m].xxx == 5 * a + 1 && struct_enemies[m].xyy == 5 * b + 3) || (struct_enemies[m].xxx == 5 * a + 2 && struct_enemies[m].xyy == 5 * b + 3) || (struct_enemies[m].xxx == 5 * a + 3 && struct_enemies[m].xyy == 5 * b + 3))
                                    {
                                        isThereObstacle = true;
                                        break;
                                    }
                                    else if (collectibles[5 * a, 5 * b + 3] != null || collectibles[5 * a + 1, 5 * b + 3] != null || collectibles[5 * a + 2, 5 * b + 3] != null || collectibles[5 * a + 3, 5 * b + 3] != null)
                                    {
                                        isThereObstacle = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (!isThereObstacle)
                    {
                        PrintCore(a, b, c);
                    }
                }
            }
        }
        static void start()
        {
            Array.Clear(walls,0,walls.Length);
            score = 0;
            mine = 3;
            energy = 200;
            Console.Clear();
            FileStream fs1 = File.Create("highscores.txt");
            fs1.Close();
            Console.SetWindowSize(150, 40);
            Console.CursorVisible = false;
            Random rand = new Random();
            int menux = 1;
            int menuy = 13;
            ConsoleKeyInfo cki;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(0, 3);
            Console.WriteLine("# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #                ");
            Console.WriteLine("#                                                                                                       #                ");
            Console.WriteLine("#                                                                                                       #                ");
            Console.WriteLine("#    Welcome to Walls and Mines!                                                                        #                ");
            Console.WriteLine("#                                                                                                       #                ");
            Console.WriteLine("#    M E N U                                                                                            #                ");
            Console.WriteLine("#    --------------------                                                                               #                ");
            Console.WriteLine("#    Use arrow keys.                                                                                    #                ");
            Console.WriteLine("#    Press Enter to select:                                                                             #                ");
            Console.WriteLine("#                                                                                                       #                ");
            Console.WriteLine("#    Play                                                                                               #                ");
            Console.WriteLine("#                                                                                                       #                ");
            Console.WriteLine("#    Options                                                                                            #                ");
            Console.WriteLine("#                                                                                                       #                ");
            Console.WriteLine("#    Help                                                                                               #                ");
            Console.WriteLine("#                                                                                                       #                ");
            Console.WriteLine("#    Credits                                                                                            #                ");
            Console.WriteLine("#                                                                                                       #                ");
            Console.WriteLine("#                                                                                                       #                ");
            Console.WriteLine("#                                                                                                       #                ");
            Console.WriteLine("#                                                                                                       #                ");
            Console.WriteLine("#                                                                                                       #                ");
            Console.WriteLine("# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #                ");
            Console.SetCursorPosition(menux, menuy);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("->");
            while (true)
            {
                Console.SetCursorPosition(menux, menuy);
                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey(true);
                    Console.Write("->");
                    if (cki.Key == ConsoleKey.DownArrow && menuy >= 13 && menuy < 19)
                    {
                        Console.SetCursorPosition(menux, menuy);
                        Console.WriteLine("  ");
                        menuy += 2;
                        Console.SetCursorPosition(menux, menuy);
                        Console.Write("->");
                    }
                    if (cki.Key == ConsoleKey.UpArrow && menuy <= 19 && menuy > 13)
                    {
                        Console.SetCursorPosition(menux, menuy);
                        Console.WriteLine("  ");
                        menuy -= 2;
                        Console.SetCursorPosition(menux, menuy);
                        Console.Write("->");
                    }
                    if (cki.Key == ConsoleKey.Enter)
                    {
                        switch (menuy)
                        {
                            case 13: Console.Clear(); Play(); break;
                            case 15: Console.Clear(); Options(); break;
                            case 17: Console.Clear(); Help(); break;
                            case 19: Console.Clear(); Credits(); break;
                        }
                    }
                    Console.ResetColor();
                    Thread.Sleep(10);
                }
            }
        }
        static void Options()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            ConsoleKeyInfo cki;
            Console.Clear();
            Console.SetCursorPosition(0, 3);
            Console.WriteLine("# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#       O P T I O N S - Walls and Mines                                                                 #");
            Console.WriteLine("#       _____________                                                                                   #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#       We are work on it.                                                                              #");
            Console.WriteLine("#       Coming soon.                                                                                    #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #");
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.Escape)
                        start();
                }
            }
        }
        static void Help()
        {
            ConsoleKeyInfo cki;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(0, 3);
            Console.WriteLine("# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#         How To Play - Walls and Mines                                                                 #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#         You are P, X and Y is your enemy. + is mine. If you press mine, you blow up.                  #");
            Console.WriteLine("#         Numbers are collectible items by P.                                                           #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#         1: 10 points                                                                                  #");
            Console.WriteLine("#         2: 30 points and 50 energy                                                                    #");
            Console.WriteLine("#         3: 90 points, 200 energy and 1 mine                                                           #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#         Your first energy is 200. If you run out of your energy. Your movement speed is halved.       #");
            Console.WriteLine("#         The game ends when P dies. If P press mine or P dies when he presses a mine or                #");
            Console.WriteLine("#         encounters x and y.                                                                           #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#         Do your best score!                                                                           #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#         Contact: wallsandmine@hotmail.com                                                             #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #");
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.Escape)
                        start();
                }
            }
        }
        static void Credits()
        {
            ConsoleKeyInfo cki;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(0, 3);
            Console.WriteLine("# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#       Credits - Walls and Mines                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("#                                                                                                       #");
            Console.WriteLine("# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #");
            Console.SetCursorPosition(8, 9);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Emir Karaismailoğlu");
            Console.SetCursorPosition(8, 11);
            Console.Write("Yusuf Apak");
            Console.SetCursorPosition(8, 13);
            Console.Write("Alperen Dönmez");
            Console.SetCursorPosition(8, 15);
            Console.Write("Ahmed Cengiz Yavuz");
            Console.ResetColor();
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.Escape)
                        start();
                }
            }
        }
        static void Play()
        {
            // Dış haritanın duvarları
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (x == 3 || x == 55 || y == 3 || y == 25)
                        map[x, y] = "#";
                }
            }
            // İç duvarların başlangıç konumu
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    for (int z = 0; z < 4; z++)
                    {
                        if (z == 3 && walls[x, y, 0] == 0 && walls[x, y, 1] == 0 && walls[x, y, 2] == 0)
                            walls[x, y, z] = 1;

                        else if (z == 3 && walls[x, y, 0] == 1 && walls[x, y, 1] == 1 && walls[x, y, 2] == 1)
                            walls[x, y, z] = 0;
                        else
                            walls[x, y, z] = random.Next(0, 2);
                    }
                }
            }
            ConsoleKeyInfo cki;               // required for readkey
            int index_struct = 0;
            int minedirection = 0;

            // --- Static screen parts
            Console.SetCursorPosition(3, 3);
            Console.WriteLine("#####################################################");
            for (int i = 0; i < 21; i++)
            {
                Console.SetCursorPosition(3, 3 + i + 1);
                Console.WriteLine("#                                                   #");
            }
            Console.SetCursorPosition(3, 25);
            Console.WriteLine("#####################################################");

            Console.ForegroundColor = ConsoleColor.Blue;

            for (int x = 1; x <= 10; x++)
            {
                for (int y = 1; y <= 4; y++)
                {
                    for (int z = 0; z < 4; z++)
                    {
                        if (walls[x - 1, y - 1, z] == 1)
                            PrintCore(x, y, z);
                    }
                }
            }

            int[] random_enemies = new int[199]; //I assign 0 or 1 to the array so that X and Y are random. I create X if 0 returns, Y if 1
            for (int i = 0; i < 199; i++)
            {
                int for_enemies = random.Next(0, 2);
                random_enemies[i] = for_enemies;
            }


            for (int i = 0; i < 55; i++)
            {
                playerx = random.Next(4, 55);
                playery = random.Next(4, 25);
                if (map[playerx, playery] != "#")
                {
                    break;
                }
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 0; i < 201; i += 10)
            {
                if (i != 0)
                    NewElements(i);
            }

            int time = 0;
            int count = 1;

            int thread_sleap = 200;
            while (true)
            {
                int a = random.Next(4, 55);
                int b = random.Next(4, 25);    // position of X
                if (map[a, b] != "#")
                {
                    struct_enemies[index_struct].xxx = a;
                    struct_enemies[index_struct].xyy = b;
                    index_struct++;
                    if (index_struct == 199)
                        break;
                }
            }
            index_struct = 0;
            for (int i = 0; i < 55; i++)
            {
                int a = random.Next(4, 55);
                int b = random.Next(4, 25);    // position of y
                if (map[a, b] != "#")
                {
                    struct_enemies[index_struct].yxx = a;
                    struct_enemies[index_struct].yyy = b;
                    index_struct++;
                    if (index_struct == 199)
                        break;
                }
            }

            while (true)
            {
                NewElements(time);
                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey(true);
                    if (energy > 0 || energy <= 0 && time % 2 == 0)
                    {
                        if ((cki.Key == ConsoleKey.RightArrow || cki.KeyChar == 'd' || cki.KeyChar == 'D') && map[playerx + 1, playery] != "#")
                        {
                            Console.SetCursorPosition(playerx, playery);
                            Console.WriteLine(" ");
                            playerx++;
                            minedirection = 1;
                            energy--;
                        }
                        if ((cki.Key == ConsoleKey.LeftArrow || cki.KeyChar == 'a' || cki.KeyChar == 'A') && map[playerx - 1, playery] != "#")
                        {
                            Console.SetCursorPosition(playerx, playery);
                            Console.WriteLine(" ");
                            playerx--;
                            minedirection = 2;
                            energy--;
                        }
                        if ((cki.Key == ConsoleKey.UpArrow || cki.KeyChar == 'w' || cki.KeyChar == 'W') && map[playerx, playery - 1] != "#")
                        {
                            Console.SetCursorPosition(playerx, playery);
                            Console.WriteLine(" ");
                            playery--;
                            minedirection = 3;
                            energy--;
                        }
                        if ((cki.Key == ConsoleKey.DownArrow || cki.KeyChar == 's' || cki.KeyChar == 'S') && map[playerx, playery + 1] != "#")
                        {
                            Console.SetCursorPosition(playerx, playery);
                            Console.WriteLine(" ");
                            playery++;
                            minedirection = 4;
                            energy--;
                        }
                        if (cki.Key == ConsoleKey.Spacebar && mine > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            if (minedirection == 1)
                            {
                                Console.SetCursorPosition(playerx - 1, playery);
                                Console.WriteLine("+");
                                mine--;
                                collectibles[playerx - 1, playery] = "+";
                            }
                            if (minedirection == 2)
                            {
                                Console.SetCursorPosition(playerx + 1, playery);
                                Console.WriteLine("+");
                                mine--;
                                collectibles[playerx + 1, playery] = "+";
                            }
                            if (minedirection == 3)
                            {
                                Console.SetCursorPosition(playerx, playery + 1);
                                Console.WriteLine("+");
                                mine--;
                                collectibles[playerx, playery + 1] = "+";
                            }
                            if (minedirection == 4)
                            {
                                Console.SetCursorPosition(playerx, playery - 1);
                                Console.WriteLine("+");
                                mine--;
                                collectibles[playerx, playery - 1] = "+";
                            }
                        }
                        if (cki.Key == ConsoleKey.Escape) start();
                    }
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(playerx, playery);
                Console.WriteLine("P");

                bool death_P = false;
                for (int i = 0; i < collectibles.GetLength(0); i++)
                {
                    for (int j = 0; j < collectibles.GetLength(1); j++)
                    {
                        if (collectibles[playerx, playery] == "+")
                            death_P = true;
                        else if (collectibles[playerx, playery] == "1")
                        {
                            score += 10;
                            collectibles[playerx, playery] = null;
                        }
                        else if (collectibles[playerx, playery] == "2")
                        {
                            score += 30;
                            energy += 50;
                            collectibles[playerx, playery] = null;
                        }
                        else if (collectibles[playerx, playery] == "3")
                        {
                            score += 90;
                            energy += 200;
                            mine += 1;
                            collectibles[playerx, playery] = null;
                        }
                    }
                    if (death_P)
                        Gameover(score);

                }
                if (death_P)
                    Gameover(score);

                if (time % 30 == 0)
                {
                    count += 1;
                }
                bool death = false;

                for (int i = 0; i < count; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    if (i == 0) //formation of 2X and 2Y at the beginning
                    {
                        enemiesx(0, count);
                        enemiesy(0, count);
                        enemiesx(1, count);
                        enemiesy(1, count);
                    }
                    else if (random_enemies[i] == 0) //increase in number of enemies over time
                    {
                        enemiesx(i, count);
                    }
                    else
                    {
                        enemiesy(i, count);
                    }
                    for (int j = 0; j < count; j++)
                    {
                        if (struct_enemies[j].xxx == playerx && struct_enemies[j].xyy == playery) death = true;
                        if (struct_enemies[j].yxx == playerx && struct_enemies[j].yyy == playery) death = true;
                    }
                    if (death)
                        Gameover(score);
                }
                if (death)
                    Gameover(score);

                Console.CursorVisible = false;

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(70, 6);
                Console.WriteLine("Time:" + time);
                Console.SetCursorPosition(70, 8);
                Console.WriteLine("Energy:" + energy);
                Console.SetCursorPosition(70, 10);
                Console.WriteLine("Score:" + score);
                Console.SetCursorPosition(70, 12);
                Console.WriteLine("Mine:" + mine);

                time += 1;
                if (energy == 0)
                    thread_sleap *= 2;
                Thread.Sleep(thread_sleap);     // sleep 200 ms
                Console.ForegroundColor = ConsoleColor.Blue;
                if (time % 2 == 0)
                    ChangeWalls();
            }
        }
        static void enemiesx(int x, int count)
        {
            int xxdir = 0, xydir = 0;
            bool flag = true;
            if (playerx < struct_enemies[x].xxx)
            {
                xxdir = -1;
            }
            else if (playerx > struct_enemies[x].xxx)
            {
                xxdir = 1;
            }
            else if (playerx == struct_enemies[x].xxx)
            {
                xxdir = 0;
                if (playery < struct_enemies[x].xyy) xydir = -1;
                if (playery > struct_enemies[x].xyy) xydir = 1;
                if (playery == struct_enemies[x].xyy) { }
            }
            for (int i = 0; i < count; i++)  //for prevent enemies from rising above each other
            {
                if (count != x && struct_enemies[x].xxx + xxdir == struct_enemies[i].xxx && struct_enemies[x].xyy + xydir == struct_enemies[i].xyy) flag = false;
                if (count != x && struct_enemies[x].xxx + xxdir == struct_enemies[i].yxx && struct_enemies[x].xyy + xydir == struct_enemies[i].yyy) flag = false;
                if (!flag || map[struct_enemies[x].xxx + xxdir, struct_enemies[x].xyy + xydir] == "#")
                {
                    xxdir = 0; xydir = 0;
                }
            }

            bool mine_control = false;

            //mine and number control
            if (collectibles[struct_enemies[x].xxx, struct_enemies[x].xyy] == "+")
            {
                mine_control = true;
                collectibles[struct_enemies[x].xxx, struct_enemies[x].xyy] = null;
            }
            else if (collectibles[struct_enemies[x].xxx, struct_enemies[x].xyy] != null)
            {
                collectibles[struct_enemies[x].xxx, struct_enemies[x].xyy] = null;
            }

            if (!mine_control && struct_enemies[x].xxx != 0 && struct_enemies[x].xyy != 0)
            {
                Console.SetCursorPosition(struct_enemies[x].xxx, struct_enemies[x].xyy);
                Console.WriteLine(" ");
                struct_enemies[x].xxx += xxdir;
                struct_enemies[x].xyy += xydir;
                Console.SetCursorPosition(struct_enemies[x].xxx, struct_enemies[x].xyy);
                Console.WriteLine("X");
            }
            else if (mine_control && struct_enemies[x].xxx != 0 && struct_enemies[x].xyy != 0)
            {
                Console.SetCursorPosition(struct_enemies[x].xxx, struct_enemies[x].xyy);
                Console.WriteLine(" ");
                struct_enemies[x].xxx = 0;
                struct_enemies[x].xyy = 0;
                score += 300;
            }
        }
        static void enemiesy(int x, int count)
        {
            int yxdir = 0, yydir = 0;
            bool flag = true;
            if (playery < struct_enemies[x].yyy)
            {
                yydir = -1;
            }
            else if (playery > struct_enemies[x].yyy)
            {
                yydir = 1;
            }
            else if (playery == struct_enemies[x].yyy)
            {
                if (playerx < struct_enemies[x].yxx)
                {
                    yxdir = -1;
                }
                if (playerx > struct_enemies[x].yxx)
                {
                    yxdir = 1;
                }
            }
            for (int i = 0; i < count; i++)
            {
                if (count != x && struct_enemies[x].yxx + yxdir == struct_enemies[i].yxx && struct_enemies[x].yyy + yydir == struct_enemies[i].yyy) flag = false;
                if (count != x && struct_enemies[x].yxx + yxdir == struct_enemies[i].xxx && struct_enemies[x].yyy + yydir == struct_enemies[i].xyy) flag = false;
            }
            if (!flag || map[struct_enemies[x].yxx + yxdir, struct_enemies[x].yyy + yydir] == "#")
            {
                yxdir = 0; yydir = 0;
            }

            bool mine_control2 = false;
            if (collectibles[struct_enemies[x].yxx, struct_enemies[x].yyy] == "+")
            {
                mine_control2 = true;
                collectibles[struct_enemies[x].yxx, struct_enemies[x].yyy] = null;
            }
            else if (collectibles[struct_enemies[x].yxx, struct_enemies[x].yyy] != null)
            {
                collectibles[struct_enemies[x].yxx, struct_enemies[x].yyy] = null;
            }

            if (!mine_control2 && struct_enemies[x].yxx != 0 && struct_enemies[x].yyy != 0)
            {
                Console.SetCursorPosition(struct_enemies[x].yxx, struct_enemies[x].yyy);
                Console.WriteLine(" ");
                struct_enemies[x].yxx += yxdir;
                struct_enemies[x].yyy += yydir;
                Console.SetCursorPosition(struct_enemies[x].yxx, struct_enemies[x].yyy);
                Console.WriteLine("Y");
            }
            else if (mine_control2 && struct_enemies[x].yxx != 0 && struct_enemies[x].yyy != 0)
            {
                Console.SetCursorPosition(struct_enemies[x].yxx, struct_enemies[x].yyy);
                Console.WriteLine(" ");
                struct_enemies[x].yxx = 0;
                struct_enemies[x].yyy = 0;
                score += 300;
            }
        }
        struct enemies
        {
            public int xxx;
            public int xyy;
            public int yxx;
            public int yyy;
        }
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            start();
        }
    }
}
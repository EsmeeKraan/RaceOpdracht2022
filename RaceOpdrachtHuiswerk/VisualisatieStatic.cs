using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceOpdrachtHuiswerk
{
    static class VisualisatieStatic
    {
        #region graphics
        private static string[] _finishHorizontal = { "═══════",
                                                      "  #    ",
                                                      "  #    ",
                                                      "═══════" };

        private static string[] _startHorizontal = { "═══════",
                                                     "  *    ",
                                                     "  *    ",
                                                     "═══════" };

        private static string[] _finishVertical = {"║      ║",
                                                   "║  #   ║",
                                                   "║  #   ║",
                                                   "║      ║" };

        private static string[] _startVertical = { "║      ║",
                                                   "║  *   ║",
                                                   "║  *   ║",
                                                   "║      ║" };

        private static string[] _straightHorizontal = { "═══════", 
                                                        "   1 ", 
                                                        "   2 ", 
                                                        "═══════" };

        private static string[] _straightVertical = { "║  1   ║", 
                                                      "║   2  ║", 
                                                      "║  1   ║", 
                                                      "║   2  ║" };

        private static string[] _rightDown = { "═══════╗"
                                             , "   1   ║"
                                             , "   2   ║"
                                             , "╗   2  ║" };

        private static string[] _leftDown = { "║   1  ╚", 
                                              "║ 2     ",
                                              "║   1   ",
                                              "╚═══════" };

        private static string[] _rightUp = { "╔═══════",
                                             "║    1  2",
                                             "║   2   ",
                                             "║      ╔" };

        private static string[] _leftUp = { "╝   1  ║", 
                                            "   1   ║", 
                                            "   2   ║", 
                                            "═══════╝" };

        #endregion

        public enum Directions
        {
            North,
            East,
            South,
            West,
        }
        public static void Initialize()
        {

        }

        public static void DrawTrack(Track track)
        {
            Console.WriteLine(track.Name);

            foreach (Section section in track.Sections)
            {
                Directions HuidigeDirection = Directions.East;
                Section.SectionTypes WatVoorBaanType = Section.SectionTypes.Straight;

                int startpositieX = 25;
                int startpositieY = 5;

                ConsoleWriteSection(_startHorizontal, startpositieX, startpositieY);


                if (HuidigeDirection == Directions.East)
                {
                    startpositieX += 7;
                    ConsoleWriteSection(_finishHorizontal, startpositieX, startpositieY);
                    startpositieX += 7;
                    ConsoleWriteSection(_straightHorizontal, startpositieX, startpositieY);
                    startpositieX += 7;
                    ConsoleWriteSection(_straightHorizontal, startpositieX, startpositieY);
                    HuidigeDirection = Directions.South;
                    WatVoorBaanType = Section.SectionTypes.RightCorner;
                }
                if (HuidigeDirection == Directions.South)
                {
                    startpositieX += 7;
                    ConsoleWriteSection(_rightDown, startpositieX, startpositieY);
                    startpositieY += 4;
                    ConsoleWriteSection(_straightVertical, startpositieX, startpositieY);
                    startpositieY += 4;
                    ConsoleWriteSection(_straightVertical, startpositieX, startpositieY);
                    startpositieY += 4;
                    ConsoleWriteSection(_straightVertical, startpositieX, startpositieY);
                    HuidigeDirection = Directions.West;
                }
                if (HuidigeDirection == Directions.West)
                {
                    startpositieY += 4;
                    ConsoleWriteSection(_leftUp, startpositieX, startpositieY);
                    startpositieX -= 7;
                    ConsoleWriteSection(_straightHorizontal, startpositieX, startpositieY);
                    startpositieX -= 7;
                    ConsoleWriteSection(_straightHorizontal, startpositieX, startpositieY);
                    startpositieX -= 7;
                    ConsoleWriteSection(_straightHorizontal, startpositieX, startpositieY);
                    startpositieX -= 7;
                    ConsoleWriteSection(_straightHorizontal, startpositieX, startpositieY);
                    HuidigeDirection = Directions.North;
                }
                if (HuidigeDirection == Directions.North)
                {
                    startpositieX -= 7;
                    ConsoleWriteSection(_leftDown, startpositieX, startpositieY);
                    startpositieY -= 4;
                    ConsoleWriteSection(_straightVertical, startpositieX, startpositieY);
                    startpositieY -= 4;
                    ConsoleWriteSection(_straightVertical, startpositieX, startpositieY);
                    startpositieY -= 4;
                    ConsoleWriteSection(_straightVertical, startpositieX, startpositieY);
                    startpositieY -= 4;
                    ConsoleWriteSection(_rightUp, startpositieX, startpositieY);
                }

            }
        }
        #region TekenMethodes

        public static void ConsoleWriteSection(string[] sectionStrings, int x, int y)
        {
            foreach (string s in sectionStrings)
            {
                Console.SetCursorPosition(x, y);
                Console.WriteLine(s);
                y++;
            }
        }

        #endregion
    }
}

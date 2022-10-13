using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RaceOpdrachtHuiswerk.VisualisatieStatic;

namespace RaceOpdrachtHuiswerk
{
    static class VisualisatieStatic
    {
        #region graphics
        private static string[] _finishHorizontal = { "═",
                                                      "#",
                                                      "#",
                                                      "═" };


        private static string[] _startHorizontal = { "═",
                                                     "L",
                                                     "R",
                                                     "═" };


        private static string[] _finishVertical = {"║ #  # ║", };


        private static string[] _startVertical = {"║ *  * ║", };


        private static string[] _straightHorizontal = { "═", 
                                                        "L", 
                                                        "R", 
                                                        "═" };


        private static string[] _straightVertical = { "║ L  R ║", };

        private static string[] _rightDown = { "═══════╗"
                                             , "    R  ║"
                                             , "  L    ║"
                                             , "╗      ║" };


        private static string[] _leftDown = { "║      ╚", 
                                              "║    L  ",
                                              "║  R    ",
                                              "╚═══════" };

        private static string[] _rightUp = { "╔═══════",
                                             "║  R    ",
                                             "║    L  ",
                                             "║      ╔" };


        private static string[] _leftUp = { "╝      ║", 
                                            "  L    ║", 
                                            "     R ║", 
                                            "═══════╝" };

        #endregion

        public enum Directions
        {
            North,  // verticaal
            East,   // horizontaal
            South,  // verticaal
            West,   // horizontaal
        }
        public static void Initialize()
        {

        }

        public static void DrawTrack(Race race)
        {
            Track track = race.Track;

            Console.WriteLine(track.Name);

            int startpositieX = 25;
            int startpositieY = 5;

            Directions HuidigeDirection = Directions.North;

            var it = track.Sections.First;

            while(it != null)
            {
                string[]? symbool = getSymbol(it.Value, HuidigeDirection);
                var sectionData = race.getSectionData(it.Value);
                it = it.Next;

                if(symbool == null)
                {
                    continue;
                } else
                {
                    ConsoleWriteSection(symbool, startpositieX, startpositieY, sectionData);

                    if(it == null)
                    {
                        break;
                    }

                    if(it.Previous != null && it.Previous.Value.SectionType == Section.SectionTypes.RightCorner)
                    {
                        switch (HuidigeDirection)
                        {
                            case Directions.North:
                                {
                                    HuidigeDirection = Directions.East;
                                }
                                break;
                            case Directions.East:
                                {
                                    HuidigeDirection = Directions.South;
                                }
                                break;
                            case Directions.South:
                                {
                                    HuidigeDirection = Directions.West;
                                }
                                break;
                            case Directions.West:
                                {
                                    HuidigeDirection = Directions.North;
                                }
                                break;
                        }
                    }

                    string[]? volgendeSymbool = getSymbol(it.Value, HuidigeDirection);
                    if(volgendeSymbool != null)
                    {
                        switch (HuidigeDirection)
                        {
                            case Directions.North:
                                startpositieY -= volgendeSymbool.Length;
                                break;
                            case Directions.South:
                                startpositieY += symbool.Length;
                                break;
                            case Directions.East:
                                startpositieX += symbool[0].Length;
                                break;
                            case Directions.West:
                                startpositieX -= volgendeSymbool[0].Length;
                                break;
                        }
                    }
                }
            } 
        }
        

        public static void ConsoleWriteSection(string[] sectionStrings, int x, int y, SectionData? sectionData)
        {
            char left = ' ';
            char right = ' ';

            if(sectionData != null)
            {
                if(sectionData.Left != null)
                {
                    left = sectionData.Left.Name[0];
                }
                if (sectionData.Right != null)
                {
                    right = sectionData.Right.Name[0];
                }
            }

            foreach (string s in sectionStrings)
            {
                Console.SetCursorPosition(x, y);
                Console.WriteLine(s.Replace('L', left).Replace('R', right));
                y++;
            }
        }

        public static string[] getSymbol(Section section, Directions HuidigeDirection)
        {
            switch (section.SectionType)
            {
                case Section.SectionTypes.RightCorner:
                    switch (HuidigeDirection)
                    {
                        case Directions.South:
                            return _leftUp;
                        case Directions.West:
                            return _leftDown;
                        case Directions.North:
                            return _rightUp;
                        case Directions.East:
                            return _rightDown;
                    }
                    break;
                case Section.SectionTypes.LeftCorner:
                    switch (HuidigeDirection)
                    {
                        case Directions.South:
                            return _leftDown;
                        case Directions.West:
                            return _leftUp;
                        case Directions.North:
                            return _rightDown;
                        case Directions.East:
                            return _rightUp;
                    }
                    break;
                case Section.SectionTypes.Straight:
                    {
                        switch (HuidigeDirection)
                        {
                            case Directions.South:
                            case Directions.North:
                                return _straightVertical;
                            case Directions.East:
                            case Directions.West:
                                return _straightHorizontal;
                        }
                        break;
                    }
                case Section.SectionTypes.StartGrid:
                    {
                        switch (HuidigeDirection)
                        {
                            case Directions.South:
                            case Directions.North:
                                return _startVertical;
                            case Directions.East:
                            case Directions.West:
                                return _startHorizontal;
                        }
                        break;
                    }
                case Section.SectionTypes.Finish:
                    {
                        switch (HuidigeDirection)
                        {
                            case Directions.South:
                            case Directions.North:
                                return _finishVertical;
                            case Directions.East:
                            case Directions.West:
                                return _finishHorizontal;
                        }
                        break;
                    }
            }
            return null;

        }


    }
}

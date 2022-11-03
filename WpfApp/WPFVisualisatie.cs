using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Model.IParticipant;
using static Model.Section;
using Track = Model.Track;

namespace WpfApp
{

    public enum Direction
    {
        North, 
        East, 
        South, 
        West
    }
    static class WPFVisualisatie
    {
        #region Graphics
        private const string _startHorizontal = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\startGridHorizontal.png";
        private const string _startVertical = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\startGridVertical.png";

        private const string _finishHorizontal = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\finishHorizontal.png";
        private const string _finishVertical = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\finishVertical.png";

        private const string _straightHorizontal = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\straightHorizontal.png";
        private const string _straightVertical = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\straightVertical.png";
        private const string _rightUp = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\rightUp.png";
        private const string _leftUp = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\leftUp.png";
        private const string _rightDown = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\rightDown.png";
        private const string _leftDown = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\leftDown.png";

        private const string _carBlue = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\teamcolor_BLUE.png";
        private const string _carBlueBroken = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\teamcolor_BLUE_BROKEN.png";

        private const string _carOrange = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\teamcolor_ORANGE.png";
        private const string _carOrangeBroken = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\teamcolor_ORANGE_BROKEN.png";

        private const string _carYellow = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\teamcolor_YELLOW.png";
        private const string _carYellowBroken = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\teamcolor_YELLOW_BROKEN.png";

        private const string _carRed = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\teamcolor_RED.png";
        private const string _carRedBroken = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\teamcolor_RED_BROKEN.png";

        private const string _carGrey = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\teamcolor_GRAY.png";
        private const string _carGreyBroken = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\teamcolor_GRAY_BROKEN.png";

        private const string _carGreen = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\teamcolor_GREEN.png";
        private const string _carGreenBroken = "C:\\Users\\MSI\\source\\repos\\RaceOpdrachtRecent2022\\WpfApp\\TypeSection\\teamcolor_GREEN_BROKEN.png";
        #endregion

        private const int _size = 128;
        private static int _xpos;
        private static int _ypos;
        private static int posLeft;
        private static int posRight;

        private static Direction _direction { get; set; }
        private static Race? _currentRace { get; set; }
        public static BitmapSource DrawTrack(Track track)
        {

            Bitmap emptyImage = createImage.CreateBitmap(1920, 1080);

            _xpos = (7 * _size);
            _ypos = 20;
            posLeft = _xpos + 50;
            posRight = _xpos + 100;

            Bitmap trackImage = PlaceSections(track, emptyImage);
            //Bitmap participantImage = DrawDriverOnTrack(track, trackImage);
            return createImage.CreateBitmapSourceFromGdiBitmap(trackImage);

            //return Images.CreateBitmapSourceFromGdiBitmap(Images.CreateBitmap(1920,1080));
        }

        public static Bitmap PlaceSections(Track track, Bitmap bitmap)
        {
            int startpositieX = 25;
            int startpositieY = 5;

            var it = track.Sections.First;
            Graphics graphics = Graphics.FromImage(bitmap);

            while (it != null)
            {
                string? symbool = getSymbol(it.Value, _direction);
                it = it.Next;

                if (symbool == null)
                {
                    continue;
                }
                else
                {
                    DrawDefaults(graphics, createImage.GetImageOutOfFolder(symbool),startpositieX, startpositieY, _direction);

                    if (it == null)
                    {
                        break;
                    }

                    if (it.Previous != null && it.Previous.Value.SectionType == Section.SectionTypes.RightCorner)
                    {
                        switch (_direction)
                        {
                            case Direction.North:
                                {
                                    _direction = Direction.East;
                                }
                                break;
                            case Direction.East:
                                {
                                    _direction = Direction.South;
                                }
                                break;
                            case Direction.South:
                                {
                                    _direction = Direction.West;
                                }
                                break;
                            case Direction.West:
                                {
                                    _direction = Direction.North;
                                }
                                break;
                        }
                    }

                    if (it.Previous != null && it.Previous.Value.SectionType == Section.SectionTypes.LeftCorner)
                    {
                        switch (_direction)
                        {
                            case Direction.North:
                                {
                                    _direction = Direction.West;
                                }
                                break;
                            case Direction.East:
                                {
                                    _direction = Direction.North;
                                }
                                break;
                            case Direction.South:
                                {
                                    _direction = Direction.East;
                                }
                                break;
                            case Direction.West:
                                {
                                    _direction = Direction.South;
                                }
                                break;
                        }
                    }
                    string? volgendeSymbool = getSymbol(it.Value, _direction);
                    if (volgendeSymbool != null)
                    {
                        switch (_direction)
                        {
                            case Direction.North:
                                startpositieY -= volgendeSymbool.Length;
                                break;
                            case Direction.South:
                                startpositieY += symbool.Length;
                                break;
                            case Direction.East:
                                startpositieX += symbool.Length;
                                break;
                            case Direction.West:
                                startpositieX -= volgendeSymbool.Length;
                                break;
                        }
                    }
                    _direction = Direction.North;
                    return bitmap;
                }
            }
        }

        private static string GetDriverImage(IParticipant driver)
        {
            switch (driver.TeamColor)
            {
                case TeamColors.Blue:
                    if (driver.Equipment.IsBroken == false) { return _carGreen; } else { return _carGreenBroken; }
                case TeamColors.Red:
                    if (driver.Equipment.IsBroken == false) { return _carRed; } else { return _carRedBroken; }
                case TeamColors.Yellow:
                    if (driver.Equipment.IsBroken == false) { return _carOrange; } else { return _carOrangeBroken; }
                case TeamColors.Grey:
                    if (driver.Equipment.IsBroken == false) { return _carBlue; } else { return _carBlueBroken; }
                case TeamColors.Green:
                    if (driver.Equipment.IsBroken == false) { return _carGrey; } else { return _carGreyBroken; }
                case TeamColors.Orange:
                    if (driver.Equipment.IsBroken == false) { return _carYellow; } else { return _carYellowBroken; }
                default:
                    return "";
            }
        }

        private static int XCorrection(int x, Direction direction, string side)
        {
            if (side.Equals("left"))
            {
                if (direction == Direction.East || direction == Direction.West)
                {
                    x = x + 10;
                }
                else x = x + 80;
            }
            if (side.Equals("right"))
            {
                if (direction == Direction.East || direction == Direction.West)
                {
                    //x stays x
                }
                else x = x + 10;
            }
            return x;
        }

        private static int YCorrection(int y, Direction direction, string side)
        {
            if (side.Equals("left"))
            {
                if (direction == Direction.East || direction == Direction.West)
                {
                    y = y + 10;
                }
                else y = y + 50;
            }
            if (side.Equals("right"))
            {
                if (direction == Direction.East || direction == Direction.West)
                {
                    y = y + 60;
                }
                else y = y + 20;
            }
            return y;
        }

        private static void DrawDriver(Graphics g, IParticipant p, SectionData sd, int x, int y, Direction direction)
        {
            if (p == sd.Left)
            {
                DrawDefaults(g, createImage.GetImageOutOfFolder(GetDriverImage(p)), XCorrection(x, direction, "left"), YCorrection(y, direction, "left"), direction);
            }
            if (p == sd.Right)
            {
                DrawDefaults(g, createImage.GetImageOutOfFolder(GetDriverImage(p)), XCorrection(x, direction, "right"), YCorrection(y, direction, "right"), direction);
            }
        }

        private static void DrawDefaults(Graphics g, Bitmap bitmap, int x, int y, Direction r)
        {
            Bitmap bitm = new Bitmap(bitmap);
            switch (r)
            {
                case Direction.North:
                    bitm.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    g.DrawImage(bitm, new Point(x, y));
                    break;
                case Direction.East:
                    g.DrawImage(bitm, new Point(x, y));
                    break;
                case Direction.South:
                    bitm.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    g.DrawImage(bitm, new Point(x, y));
                    break;
                case Direction.West:
                    bitm.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    g.DrawImage(bitm, new Point(x, y));
                    break;
            }
        }

        private static void CalculateDirection(SectionTypes sectionType)
        {
            switch (sectionType)
            {
                case SectionTypes.LeftCorner:
                    if (_direction == Direction.East) { _direction = Direction.North; break; }
                    if (_direction == Direction.North) { _direction = Direction.West; break; }
                    if (_direction == Direction.West) { _direction = Direction.South; break; }
                    else { _direction = Direction.East; break; }
                case SectionTypes.RightCorner:
                    if (_direction == Direction.East) { _direction = Direction.South; break; }
                    if (_direction == Direction.North) { _direction = Direction.East; break; }
                    if (_direction == Direction.West) { _direction = Direction.North; break; }
                    else { _direction = Direction.West; break; }
            }
        }

        public static string getSymbol(Section section, Direction HuidigeDirection)
        {
            switch (section.SectionType)
            {
                case Section.SectionTypes.RightCorner:
                    switch (HuidigeDirection)
                    {
                        case Direction.South:
                            return _leftUp;
                        case Direction.West:
                            return _leftDown;
                        case Direction.North:
                            return _rightUp;
                        case Direction.East:
                            return _rightDown;
                    }
                    break;
                case Section.SectionTypes.LeftCorner:
                    switch (HuidigeDirection)
                    {
                        case Direction.South:
                            return _leftDown;
                        case Direction.West:
                            return _rightUp;
                        case Direction.North:
                            return _rightDown;
                        case Direction.East:
                            return _leftUp;
                    }
                    break;
                case Section.SectionTypes.Straight:
                    {
                        switch (HuidigeDirection)
                        {
                            case Direction.South:
                            case Direction.North:
                                return _straightVertical;
                            case Direction.East:
                            case Direction.West:
                                return _straightHorizontal;
                        }
                        break;
                    }
                case Section.SectionTypes.StartGrid:
                    {
                        switch (HuidigeDirection)
                        {
                            case Direction.South:
                            case Direction.North:
                                return _startVertical;
                            case Direction.East:
                            case Direction.West:
                                return _startHorizontal;
                        }
                        break;
                    }
                case Section.SectionTypes.Finish:
                    {
                        switch (HuidigeDirection)
                        {
                            case Direction.South:
                            case Direction.North:
                                return _finishVertical;
                            case Direction.East:
                            case Direction.West:
                                return _finishHorizontal;
                        }
                        break;
                    }
            }
            return null;

        }

    }
}

using Controller;
using Model;
using System.Drawing;
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
        private const string _startHorizontal = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\startGridHorizontal.png";
        private const string _startVertical = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\startGridVertical.png";

        private const string _finishHorizontal = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\finishHorizontal.png";
        private const string _finishVertical = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\finishVertical.png";

        private const string _straightHorizontal = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\straightHorizontal.png";
        private const string _straightVertical = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\straightVertical.png";
        private const string _rightUp = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\rightUp.png";
        private const string _leftUp = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\leftUp.png";
        private const string _rightDown = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\rightDown.png";
        private const string _leftDown = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\leftDown.png";

        private const string _carBlue = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_BLUE.png";
        private const string _carBlueBroken = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_BLUE_BROKEN.png";

        private const string _carOrange = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_ORANGE.png";
        private const string _carOrangeBroken = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_ORANGE_BROKEN.png";

        private const string _carYellow = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_YELLOW.png";
        private const string _carYellowBroken = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_YELLOW_BROKEN.png";

        private const string _carRed = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_RED.png";
        private const string _carRedBroken = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_RED_BROKEN.png";

        private const string _carGrey = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_GRAY.png";
        private const string _carGreyBroken = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_GRAY_BROKEN.png";

        private const string _carGreen = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_GREEN.png";
        private const string _carGreenBroken = "C:\\Users\\MSI\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_GREEN_BROKEN.png";
        #endregion

        #region LaptopGraphics
        /*        private const string _startHorizontal = "C:\\Users\\esmee\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\startGridHorizontal.png";
                private const string _finishHorizontal = "C:\\Users\\esmee\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\finishHorizontal.png";

                private const string _straightHorizontal = "C:\\Users\\esmee\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\straightHorizontal.png";
                private const string _leftUp = "C:\\Users\\esmee\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\leftUp.png";
                private const string _rightDown = "C:\\Users\\esmee\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\rightDown.png";

                private const string _carBlue = "C:\\Users\\esmee\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_BLUE.png";
                private const string _carBlueBroken = "C:\\Users\\esmee\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_BLUE_BROKEN.png";

                private const string _carOrange = "C:\\Users\\esmee\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_ORANGE.png";
                private const string _carOrangeBroken = "C:\\Users\\esmee\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_ORANGE_BROKEN.png";

                private const string _carYellow = "C:\\Users\\esmee\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_YELLOW.png";
                private const string _carYellowBroken = "C:\\Users\\esmee\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_YELLOW_BROKEN.png";

                private const string _carRed = "C:\\Users\\esmee\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_RED.png";
                private const string _carRedBroken = "C:\\Users\\esmee\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_RED_BROKEN.png";

                private const string _carGrey = "C:\\Users\\esmee\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_GRAY.png";
                private const string _carGreyBroken = "C:\\Users\\esmee\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_GRAY_BROKEN.png";

                private const string _carGreen = "C:\\Users\\esmee\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_GREEN.png";
                private const string _carGreenBroken = "C:\\Users\\esmee\\source\\repos\\RaceOpdracht2022\\WpfApp\\TypeSection\\teamcolor_GREEN_BROKEN.png";*/
        #endregion
        private const float _factor = 0.2f;
        private static Direction _direction { get; set; }
        public static BitmapSource DrawTrack(Track track)
        {
            Bitmap emptyImage = createImage.CreateBitmap(1920, 1500, track);
            Bitmap trackImage = PlaceSections(track, emptyImage, true);
            return createImage.CreateBitmapSourceFromGdiBitmap(trackImage);
        }

        /// <summary>
        /// Draws the track and the participants
        /// </summary>
        /// <param name="t"></param>
        /// <param name="bitmap"></param>
        /// <param name="drawParticipants"></param>
        /// <returns></returns>
        public static Bitmap PlaceSections(Track t, Bitmap bitmap, bool drawParticipants)
        {

            float x = 32;
            float y = bitmap.Height * 0.2f;

            Graphics graphics = Graphics.FromImage(bitmap);
            foreach (Section section in t.Sections)
            {
                SectionData sd = Data.CurrentRace.GetSectionData(section);
                Bitmap bd = null;

                switch (section.SectionType) //switchcase which section to draw
                {
                    case SectionTypes.StartGrid:
                        bd = createImage.GetImageOutOfFolder(_startHorizontal);
                        if (!drawParticipants)
                        {
                            DrawDefaults(graphics, bd, x, y, _direction);
                        }
                        break;

                    case SectionTypes.Straight:
                        bd = createImage.GetImageOutOfFolder(_straightHorizontal);
                        if (!drawParticipants)
                        {
                            DrawDefaults(graphics, bd, x, y, _direction);
                        }
                        break;

                    case SectionTypes.Finish:
                        bd = createImage.GetImageOutOfFolder(_finishHorizontal);
                        if (!drawParticipants)
                        {
                            DrawDefaults(graphics, bd, x, y, _direction);
                        }
                        break;

                    case SectionTypes.LeftCorner:
                        bd = createImage.GetImageOutOfFolder(_leftUp);
                        switch (_direction)
                        {
                            case Direction.East:
                                y -= 98 * _factor;
                                break;
                            case Direction.North:
                                x -= 85 * _factor;
                                break;
                        }
                        if (!drawParticipants)
                        {
                            DrawDefaults(graphics, bd, x, y, _direction);
                        }
                        switch (_direction)
                        {
                            case Direction.South:
                                y += 98 * _factor;
                                break;
                            case Direction.East:
                                x += 85 * _factor;
                                y += 98 * _factor;
                                break;
                            case Direction.North:
                                x += 85 * _factor;
                                break;
                        }
                        CalculateDirection(SectionTypes.LeftCorner);
                        break;

                    case SectionTypes.RightCorner:
                        bd = createImage.GetImageOutOfFolder(_rightDown);
                        switch (_direction)
                        {
                            case Direction.South:
                                x -= 85 * _factor;
                                break;
                            case Direction.West:
                                y -= 98 * _factor;
                                break;
                        }
                        if (!drawParticipants)
                        {
                            DrawDefaults(graphics, bd, x, y, _direction);
                        }
                        switch (_direction)
                        {
                            case Direction.East:
                                x += 85 * _factor;
                                break;
                            case Direction.South:
                                x += 85 * _factor;
                                y += 98 * _factor;
                                break;
                            case Direction.West:
                                y += 98 * _factor;
                                break;

                        }
                        CalculateDirection(SectionTypes.RightCorner);

                        if (sd.Left != null)
                        {
                            IParticipant participant = sd.Left;
                            DrawDriver(graphics, participant, sd, x, y, _direction);
                        }
                        if (sd.Right != null)
                        {
                            IParticipant participant = sd.Right;
                            DrawDriver(graphics, participant, sd, x, y, _direction);
                        }

                        break;

                }
                if (drawParticipants)
                {
                    if (sd.Left is not null)
                    {
                        IParticipant participant = sd.Left;
                        DrawDriver(graphics, participant, sd, x, y, _direction);
                    }
                    if (sd.Right is not null)
                    {
                        IParticipant participant = sd.Right;
                        DrawDriver(graphics, participant, sd, x, y, _direction);
                    }
                }
                switch (_direction)
                {
                    case Direction.North:
                        y = y - bd.Height * _factor;
                        break;
                    case Direction.East:
                        x = x + bd.Width * _factor;
                        break;
                    case Direction.South:
                        y = y + bd.Height * _factor;
                        break;
                    case Direction.West:
                        x = x - bd.Width * _factor;
                        break;
                } // calculate xpos ypos

            }
            _direction = Direction.North;
            return bitmap;
        }

        /// <summary>
        /// Returns the driver image based on the teamcolor and the broken image when the status is set to broken
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
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

        /// <summary>
        /// The x offset of the section where the participant is supposed to be drawn on
        /// </summary>
        /// <param name="x"></param>
        /// <param name="direction"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        private static float XCorrection(float x, Direction direction, string side)
        {
            if (side.Equals("left"))
            {
                if (direction == Direction.East || direction == Direction.West)
                {
                    x = x + 10 * _factor;
                }
                else x = x + 80 * _factor;
            }
            if (side.Equals("right"))
            {
                if (direction == Direction.East || direction == Direction.West)
                {
                    //x stays x
                }
                else x = x + 10 * _factor;
            }
            return x;
        }
        /// <summary>
        /// The y offset of the section where the participant is supposed to be drawn on
        /// </summary>
        /// <param name="y"></param>
        /// <param name="direction"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        private static float YCorrection(float y, Direction direction, string side)
        {
            if (side.Equals("left"))
            {
                if (direction == Direction.East || direction == Direction.West)
                {
                    y = y + 10 * _factor;
                }
                else y = y + 50 * _factor;
            }
            if (side.Equals("right"))
            {
                if (direction == Direction.East || direction == Direction.West)
                {
                    y = y + 60 * _factor;
                }
                else y = y + 20 * _factor;
            }
            return y;
        }

        /// <summary>
        /// Draws the driver on the track
        /// </summary>
        /// <param name="g"></param>
        /// <param name="p"></param>
        /// <param name="sd"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="direction"></param>
        private static void DrawDriver(Graphics g, IParticipant p, SectionData sd, float x, float y, Direction direction)
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

        /// <summary>
        /// Draws the image and rotates the image if needed
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bitmap"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="r"></param>
        private static void DrawDefaults(Graphics g, Bitmap bitmap, float x, float y, Direction r)
        {
            Bitmap bitm = new Bitmap(bitmap);
            switch (r)
            {
                case Direction.North:
                    bitm.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    g.DrawImage(bitm, new RectangleF(x, y, bitmap.Height * _factor, bitmap.Width * _factor));
                    break;
                case Direction.East:
                    g.DrawImage(bitm, new RectangleF(x, y, bitmap.Width * _factor, bitmap.Height * _factor));
                    break;
                case Direction.South:
                    bitm.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    g.DrawImage(bitm, new RectangleF(x, y, bitmap.Width * _factor, bitmap.Height * _factor));
                    break;
                case Direction.West:
                    bitm.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    g.DrawImage(bitm, new RectangleF(x, y, bitmap.Width * _factor, bitmap.Height * _factor));
                    break;
            }
        }

        /// <summary>
        /// Updates the direction when a corner is reached
        /// </summary>
        /// <param name="sectionType"></param>
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
    }
}

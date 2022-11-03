using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;


namespace Controller
{
    public class Race
    {

        #region Properties
        public Track Track;
        public List<IParticipant> Participants;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        private Random _random = new();
        private Dictionary<Section, SectionData> _positions = new();
        public bool IsOver { get; private set; }

        private readonly System.Timers.Timer _timer;
        private const int _timerInterval = 20;
        private DateTime _lastSeenBroken = DateTime.Now;

        private const int _laps = 3;
        private Dictionary<IParticipant, int> _lapsDriven = new();

        private Dictionary<IParticipant, DateTime> _ParticipantLapTime = new();

        private List<IParticipant> _finishOrder = new();

        public event EventHandler<DriversChangedEventArgs> DriversChanged;

        public event EventHandler RaceFinished;
        #endregion

        #region Methods
        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            StartTime = DateTime.Now;
            _random = new(DateTime.Now.Millisecond);
            _positions = new();

            _timer = new System.Timers.Timer(_timerInterval);
            _timer.Elapsed += OnTimedEvent;

            SetDriverStartPosition(track, participants);
            RandomizeEquipment();
            InitializeFirstLap();
        }
        /// <summary>
        /// Loops through the sections until it finds a Start Grid
        /// Checks if there are any participants to place
        /// Returns the participants on the Start Grid
        /// </summary>
        /// <param name="track"></param>
        /// <param name="participants"></param>
        public void SetDriverStartPosition(Track track, List<IParticipant> participants)
        {
            Queue<IParticipant> participantsTemp = new Queue<IParticipant>(participants);

            foreach(Section section in track.Sections)
            {
                if(section.SectionType == Section.SectionTypes.StartGrid)
                {
                    SectionData? sectionData = GetSectionData(section);

                    if (participantsTemp.Count == 0)
                        return;
                    sectionData!.Left = participantsTemp.Dequeue();
                    if (participantsTemp.Count == 0)
                        return;
                    sectionData!.Right = participantsTemp.Dequeue();
                }
            }
        }
        
        /// <summary>
        /// Initialises first lap with the participant
        /// Used for keeping count of the amount of laps the participant has driven
        /// </summary>
        public void InitializeFirstLap()
        {
            _lapsDriven = new Dictionary<IParticipant, int>();
            foreach(IParticipant participant in Participants)
            {
                _lapsDriven.Add(participant, 1);
            }
        }

        /// <summary>
        /// Initialises a counter for tracking the lap time for each participant
        /// Used for keeping track of the lapTime
        /// </summary>
        public void initializeStartLapTime()
        {
            _lapsDriven = new Dictionary<IParticipant, int>();

            foreach(IParticipant participant in Participants)
            {
                /*_lapsDriven.Add(participant, StartTime);*/
            }
        }
        /// <summary>
        /// Updates a counter each participant has & tracks the lapTime
        /// Used for updating _lapsDriven and keeping track of _ParticipantLapTime 
        /// </summary>
        /// <param name="participant"></param>
        /// <param name="time"></param>
        public void UpdateLap(IParticipant participant, DateTime time)
        {
            _lapsDriven[participant]++;
            /*Console.Title = $"{participant.Name} just got to lap {_lapsDriven[participant]}";*/
            if (_lapsDriven[participant] <= 1) return;
            _ParticipantLapTime[participant] = time;
        }

        /// <summary>
        /// Checks if the participant has driven the amount of laps
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        public bool ParticipantFinished(IParticipant participant)
        {
            return _lapsDriven[participant] >= _laps;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        public void MoveParticipant(DateTime time)
        {
            LinkedListNode<Section> currentSectionNode = Track.Sections.Last;

            while (currentSectionNode != null)
            {
                MoveSectionData(currentSectionNode.Value, (currentSectionNode.Next?.Value ?? Track.Sections.First?.Value)!, time, currentSectionNode);

                currentSectionNode = currentSectionNode.Previous;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentSection"></param>
        /// <param name="nextSection"></param>
        /// <param name="time"></param>
        /// <param name="node"></param>
        public void MoveSectionData(Section currentSection, Section nextSection, DateTime time, LinkedListNode<Section> node)
        {
            //_timer.Stop();

                SectionData currentSectionData = GetSectionData(currentSection);
                SectionData nextSectionData = GetSectionData(nextSection);

            if (currentSectionData.Left != null && !currentSectionData.Left.Equipment.IsBroken)
            {
                currentSectionData.DistanceLeft += GetSpeedFromCompetitor(currentSectionData.Left);
                Side nextSide = Side.Left;
                while (true)
                {
                    Side? side = MoveLane(nextSide, currentSectionData, nextSectionData, time, currentSection.SectionType);
                    
                    if (side == null)
                        break;
                    

                    nextSide = (Side)side;
                    currentSectionData = nextSectionData;
                    nextSectionData = GetSectionData(node.Previous?.Value ?? Track.Sections.First!.Value);
                }
            }

            if (currentSectionData.Right != null && !currentSectionData.Right.Equipment.IsBroken)
            {
                currentSectionData.DistanceRight += GetSpeedFromCompetitor(currentSectionData.Right);
                Side nextSide = Side.Right;
                while (true)
                {
                    Side? side = MoveLane(nextSide, currentSectionData, nextSectionData, time, currentSection.SectionType);
                    if (side == null)
                        break;


                    nextSide = (Side)side;
                    currentSection = nextSection;
                    currentSectionData = nextSectionData;
                    nextSection = node.Previous?.Value ?? Track.Sections.First!.Value;
                    nextSectionData = GetSectionData(nextSection);
                }
            }

            

#if OldMethod
            if (currentSectionData.DistanceLeft >= 100 && currentSectionData.DistanceRight >= 100)
                {
                    #region Beide participants kunnen bewegen
                    int freePlace = CheckIfFreePlace(nextSectionData);

                    if(freePlace == 0)// links en rechts is geen plek voor drivers om te bewegen
                    {
                        currentSectionData.DistanceLeft = 99;
                        currentSectionData.DistanceRight = 99;
                    }
                    else if (freePlace == 3) // links en rechts is plek voor drivers
                    {
                        MoveSingleParticipant(currentSection, nextSection, Side.Left, Side.Left, false, time);
                        MoveSingleParticipant(currentSection, nextSection, Side.Right, Side.Right, false, time);
                    }
                    else
                    {
                        if (currentSectionData.DistanceLeft >= currentSectionData.DistanceRight)
                        {
                            // prefer left
                            if (freePlace == 1)
                                MoveSingleParticipant(currentSection, nextSection, Side.Left, Side.Left, true, time); // left to left
                            else if (freePlace == 2)
                                MoveSingleParticipant(currentSection, nextSection, Side.Left, Side.Right, true, time);
                        }
                        else
                        {
                            // choose right
                            if (freePlace == 1)
                                MoveSingleParticipant(currentSection, nextSection, Side.Right, Side.Left, true, time); // left to left
                            else if (freePlace == 2)
                                MoveSingleParticipant(currentSection, nextSection, Side.Right, Side.Right, true, time);
                        }
                    }
                    #endregion
                }
            else if (currentSectionData.DistanceLeft >= 100 && currentSectionData.Left != null)
            {
                #region Linker participant kan bewegen

                // for freesections, prefer same spot, otherwise take other
                int freePlaces = CheckIfFreePlace(nextSectionData);
                if (freePlaces == 0)
                    currentSectionData.DistanceLeft = 100;
                else if (freePlaces == 3 || freePlaces == 1)
                    // move from left to left
                    MoveSingleParticipant(currentSection, nextSection, Side.Left, Side.Left, false, time);
                else if (freePlaces == 2)
                    // move from left to right
                    MoveSingleParticipant(currentSection, nextSection, Side.Left, Side.Right, false, time);

                #endregion
            }
            else if (currentSectionData.DistanceRight >= 100 && currentSectionData.Right != null)
            {
                #region Rechter participant kan bewegen

                // for freesections, prefer same spot, otherwise take other
                int freePlaces = CheckIfFreePlace(nextSectionData);
                if (freePlaces == 0)
                    currentSectionData.DistanceRight = 100;
                else if (freePlaces == 3 || freePlaces == 2)
                    // move from right to right
                    MoveSingleParticipant(currentSection, nextSection, Side.Right, Side.Right, false, time);
                else if (freePlaces == 1)
                    // move from right to left
                    MoveSingleParticipant(currentSection, nextSection, Side.Right, Side.Left, false, time);

                #endregion
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="side"></param>
        /// <param name="currentSectionData"></param>
        /// <param name="nextSectionData"></param>
        /// <param name="time"></param>
        /// <param name="currentSectionType"></param>
        /// <returns></returns>
        private Side? MoveLane(Side side, SectionData currentSectionData, SectionData nextSectionData, DateTime time, Section.SectionTypes currentSectionType)
        {
            var distance = side == Side.Left
                         ? currentSectionData.DistanceLeft
                         : currentSectionData.DistanceRight;

            var participant = side == Side.Left
                            ? currentSectionData.Left
                            : currentSectionData.Right;

            if (distance < 100)
                return null;

            if (currentSectionType == Section.SectionTypes.Finish)
            {
                if (UpdateLapAndFinish(currentSectionData, (Side)side, time))
                    return null;
            }

            Side? nextSide;
            if (side == Side.Left)
            {
                nextSide = MoveToNextLeft(participant, distance, nextSectionData);
                if (nextSide == null)
                    nextSide = MoveToNextRight(participant, distance, nextSectionData);
                if (nextSide == null)
                    return null;
            }
            else
            {
                nextSide = MoveToNextRight(participant, distance, nextSectionData);
                if (nextSide == null)
                    nextSide = MoveToNextLeft(participant, distance, nextSectionData);
                if (nextSide == null)
                    return null;
            }

            if (side == Side.Left)
                currentSectionData.Left = null;
            else
                currentSectionData.Right = null;

            return nextSide;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="participant"></param>
        /// <param name="distance"></param>
        /// <param name="sectionData"></param>
        /// <returns></returns>
        private Side? MoveToNextLeft(IParticipant participant, int distance, SectionData sectionData)
        {
            if (sectionData.Left != null)
                return null;
            sectionData.Left = participant;
            sectionData.DistanceLeft = distance - 100;
            return Side.Left;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="participant"></param>
        /// <param name="distance"></param>
        /// <param name="sectionData"></param>
        /// <returns></returns>
        private Side? MoveToNextRight(IParticipant participant, int distance, SectionData sectionData)
        {
            if (sectionData.Right != null)
                return null;
            sectionData.Right = participant;
            sectionData.DistanceRight = distance - 100;
            return Side.Right;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectiondata"></param>
        /// <param name="side"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool UpdateLapAndFinish(SectionData sectiondata, Side side, DateTime time)
        {
            var participant = sectiondata.GetParticipant(side);
            if (participant != null)
            {
                UpdateLap(participant, time);
                if (!ParticipantFinished(participant))
                    return false;
                _finishOrder.Add(participant);

                Console.Title = $"{participant.Name} just finished!! :)";

                if (side == Side.Left)
                    sectiondata.Left = null;
                else
                    sectiondata.Right = null;
                return true;
            }
            return false;
        }


        public int GetSpeedFromCompetitor(IParticipant iParticipant) => Convert.ToInt32(Math.Ceiling(0.12 * (iParticipant.Equipment.Speed * 0.51) * iParticipant.Equipment.Performance + 15));

        #region Events
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            CheckIfEquipmentsShouldBreakOrBeRepaired();
            MoveParticipant(e.SignalTime);

            DriversChanged?.Invoke(this, new DriversChangedEventArgs() { Track = this.Track } );

            if (CheckIfFinished())
            {
                _timer.Stop();
                EndTime = e.SignalTime;
                IsOver = true;
                RaceFinished?.Invoke(this, new EventArgs());
            }
        }
        #endregion

        /// <summary>
        /// Randomly decides if the equipment should break and the participant will be 'isBroken'
        /// </summary>
        private void CheckIfEquipmentsShouldBreakOrBeRepaired()
        {
            var now = DateTime.Now;
            if ((now - _lastSeenBroken).TotalSeconds < 3)
                return;

            _lastSeenBroken = now;

            foreach(var participant in Participants)
            {
                // Equipment quality is randomizes in 'RandomizeEquipment'
                // Quality has a chance of being 60 to 100, which means each participant has a 1-40% chance of breaking
                participant.Equipment.IsBroken = _random.Next(0, 100) > participant.Equipment.Quality;
            }
        }

        /// <summary>
        /// Returns the current section
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public SectionData GetSectionData(Section section)
        {

            if (!_positions.ContainsKey(section)) _positions.Add(section, new SectionData());
            return _positions[section];
        }
            
        /// <summary>
        /// Randomizes the equipment quality, performance and speed
        /// </summary>
        public void RandomizeEquipment()
        {
            foreach (var participant in Participants)
            {
                participant.Equipment = new Car(_random.Next(60, 100), _random.Next(6, 50), _random.Next(6, 16));
            }
        }



        public bool CheckIfFinished() {
            return _positions.Values.FirstOrDefault(a => a.Left != null || a.Right != null) == null;
        }

        #region Timer & Cleanup
        public void Start()
        {
            StartTime = DateTime.Now;
            _timer.Start();
        }

        public void CleanUp()
        {
            _timer.Stop();
            DriversChanged = null;
            RaceFinished = null;
        }
        #endregion

        #endregion
    }
}

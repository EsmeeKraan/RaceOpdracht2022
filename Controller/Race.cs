using Model;
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

        public Dictionary<IParticipant, DateTime> ParticipantLapTime = new();

        private List<IParticipant> _finishOrder = new();

        private object _positionsLock = new object();

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

            foreach (Section section in track.Sections)
            {
                if (section.SectionType == Section.SectionTypes.StartGrid)
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
            foreach (IParticipant participant in Participants)
            {
                _lapsDriven.Add(participant, -1);
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
            if (!ParticipantFinished(participant)) return;
            ParticipantLapTime[participant] = time;
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
        /// Moves all the participants when they're not broken
        /// </summary>
        /// <param name="time"></param>
        public void MoveParticipants(DateTime time)
        {
            LinkedListNode<Section>? currentSectionNode = Track.Sections.Last;

            while (currentSectionNode != null)
            {
                MoveSectionData(currentSectionNode.Value, (currentSectionNode.Next?.Value ?? Track.Sections.First?.Value)!, time, currentSectionNode);

                currentSectionNode = currentSectionNode.Previous;
            }
        }

        /// <summary>
        /// Moves the participants on the left and right lane
        /// </summary>
        /// <param name="currentSection"></param>
        /// <param name="nextSection"></param>
        /// <param name="time"></param>
        /// <param name="node"></param>
        public void MoveSectionData(Section currentSection, Section nextSection, DateTime time, LinkedListNode<Section> node)
        {
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
        }

        /// <summary>
        /// Moves the participant from lane and also keeps in mind if a participant wants to surpass a competitor on the next lane
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
                         ? currentSectionData.DistanceLeft!
                         : currentSectionData.DistanceRight!;

            var participant = side == Side.Left
                            ? currentSectionData.Left!
                            : currentSectionData.Right!;

            if (distance < 100)
                return null;

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

            if (currentSectionType == Section.SectionTypes.Finish)
            {
                if (UpdateLapAndFinish(nextSectionData, (Side)nextSide, time))
                    return null;
            }

            return nextSide;
        }

        /// <summary>
        /// Increases the distance of the participant when they're on the left side of the track
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
        /// Increases the distance of the participant when they're on the right side of the track
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
        /// Checks if participant on a certain side and has finished
        /// Marks the participant finished so their ready to be removed from the track
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
                if (side == Side.Left)
                    sectiondata.Left = null;
                else
                    sectiondata.Right = null;
                return true;
            }
            return false;
        }


        public int GetSpeedFromCompetitor(IParticipant iParticipant) => Convert.ToInt32(Math.Ceiling(0.12 * (iParticipant.Equipment.Speed * 0.51) * iParticipant.Equipment.Performance + 15));

        /// <summary>
        /// Randomly decides if the equipment should break and the participant will be 'isBroken'
        /// </summary>
        private void CheckIfEquipmentsShouldBreakOrBeRepaired()
        {
            var now = DateTime.Now;
            if ((now - _lastSeenBroken).TotalSeconds < 3)
                return;

            _lastSeenBroken = now;

            foreach (var participant in Participants)
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
            if (!_positions.ContainsKey(section)) _positions.TryAdd(section, new SectionData());
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



        public bool CheckIfFinished()
        {
            return _positions.Values.FirstOrDefault(a => a.Left != null || a.Right != null) == null;
        }

        #endregion

        #region Timer & Cleanup
        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            StartTime = DateTime.Now;
            _timer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CleanUp()
        {
            _timer.Stop();
            DriversChanged = null;
            RaceFinished = null;
        }
        #endregion

        #region Events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            lock (_positionsLock)
            {
                CheckIfEquipmentsShouldBreakOrBeRepaired();
                MoveParticipants(DateTime.Now);

                DriversChanged?.Invoke(this, new DriversChangedEventArgs(Track) { Track = this.Track });

                if (CheckIfFinished())
                {
                    for (int i = 0; i < _finishOrder.Count; i++)
                    {
                        int points = 0;
                        switch (i)
                        {
                            case 0:
                                points = 15;
                                break;
                            case 1:
                                points = 12;
                                break;
                            case 2:
                                points = 10;
                                break;
                            case 3:
                                points = 8;
                                break;
                            case 4:
                                points = 7;
                                break;
                            case 5:
                                points = 5;
                                break;
                            case 6:
                                points = 3;
                                break;
                            case 7:
                                points = 1;
                                break;
                        }
                        _finishOrder[i].Points += points;
                    }
                    _timer.Stop();
                    EndTime = e.SignalTime;
                    IsOver = true;
                    RaceFinished?.Invoke(this, new EventArgs());
                }
            }

        }
        #endregion
    }
}

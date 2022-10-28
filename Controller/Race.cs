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
        
        public void InitializeFirstLap()
        {
            _lapsDriven = new Dictionary<IParticipant, int>();
            foreach(IParticipant participant in Participants)
            {
                _lapsDriven.Add(participant, -1);
            }
        }

/*        public void initializeStartLapTime()
        {
            _lapsDriven = new Dictionary<IParticipant, int>();

            foreach(IParticipant participant in Participants)
            {
                *//*_lapsDriven.Add(participant, StartTime);*//*
            }
        }*/

        public void UpdateLap(IParticipant participant, DateTime time)
        {
            _lapsDriven[participant]++;
            Console.Title = $"{participant.Name} just got to lap {_lapsDriven[participant]}";
            if (_lapsDriven[participant] <= 0) return;
            _ParticipantLapTime[participant] = time;
        }

        public bool ParticipantFinished(IParticipant participant)
        {
            return _lapsDriven[participant] >= _laps;
        }

        public bool CheckForFinishSection(Section section)
        {
            return section.SectionType == Section.SectionTypes.Finish;
        }

        public void MoveParticipant(DateTime time)
        {
            LinkedListNode<Section> currentSectionNode = Track.Sections.Last;

            while (currentSectionNode != null)
            {
                MoveSectionData(currentSectionNode.Value, (currentSectionNode.Next?.Value ?? Track.Sections.First?.Value)!, time, currentSectionNode);

                currentSectionNode = currentSectionNode.Previous;
            }
        }

/*        public void MoveSingleParticipant(Section currentSection, Section nextSection, Side start, Side end, bool correctOtherSide, DateTime time)
        {
            SectionData currentSectionData = GetSectionData(currentSection);
            SectionData nextSectionData = GetSectionData(nextSection);

            switch (start)
            {
                case Side.Right:
                    switch(end)
                    {
                        case Side.Right:
                        nextSectionData.Right = currentSectionData.Right;
                            nextSectionData.DistanceRight = currentSectionData.DistanceRight - 100;
                            if (CheckForFinishSection(nextSection))
                            {
                                UpdateLapAndFinish(nextSectionData, Side.Right, time);
                            }
                            break;
   
                         case Side.Left:
                            nextSectionData.Left = currentSectionData.Right;
                            nextSectionData.DistanceLeft = currentSectionData.DistanceLeft - 100;
                            if (CheckForFinishSection(nextSection))
                            {
                                UpdateLapAndFinish(nextSectionData, Side.Left, time);
                            }
                            break;
                    }
                    currentSectionData.Right = null;
                    currentSectionData.DistanceRight = 0;
                    break;
                case Side.Left:
                    switch (end)
                    {
                        case Side.Right:
                            nextSectionData.Right = currentSectionData.Left;
                            nextSectionData.DistanceRight = currentSectionData.DistanceRight - 100;
                            if (CheckForFinishSection(nextSection))
                            {
                                UpdateLapAndFinish(nextSectionData, Side.Right, time);
                            }
                            break;

                        case Side.Left:
                            nextSectionData.Left = currentSectionData.Left;
                            nextSectionData.DistanceLeft = currentSectionData.DistanceLeft - 100;
                            if (CheckForFinishSection(nextSection))
                            {
                                UpdateLapAndFinish(nextSectionData, Side.Left, time);
                            }
                            break;
                    }
                    currentSectionData.Left = null;
                    currentSectionData.DistanceRight = 0;
                    break;
            }
            if (start == Side.Right && correctOtherSide)
            {
                currentSectionData.DistanceLeft = 99;
            }
            else if (start == Side.Left && correctOtherSide)
            {
                currentSectionData.DistanceRight = 99;
            }
        }*/
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
                    Side? side = Tristan_MoveLane(nextSide, currentSectionData, nextSectionData, time, currentSection.SectionType);
                    
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
                    Side? side = Tristan_MoveLane(nextSide, currentSectionData, nextSectionData, time, currentSection.SectionType);
                    if (side == null)
                        break;


                    nextSide = (Side)side;
                    currentSection = nextSection;
                    currentSectionData = nextSectionData;
                    nextSection = node.Previous?.Value ?? Track.Sections.First!.Value;
                    nextSectionData = GetSectionData(nextSection);
                }
            }

            

#if no
2            if (currentSectionData.DistanceLeft >= 100 && currentSectionData.DistanceRight >= 100)
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

        // returns if moved or not
        private Side? Tristan_MoveLane(Side side, SectionData currentSectionData, SectionData nextSectionData, DateTime time, Section.SectionTypes currentSectionType)
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

        private Side? MoveToNextLeft(IParticipant participant, int distance, SectionData sectionData)
        {
            if (sectionData.Left != null)
                return null;
            sectionData.Left = participant;
            sectionData.DistanceLeft = distance - 100;
            return Side.Left;
        }

        private Side? MoveToNextRight(IParticipant participant, int distance, SectionData sectionData)
        {
            if (sectionData.Right != null)
                return null;
            sectionData.Right = participant;
            sectionData.DistanceRight = distance - 100;
            return Side.Right;
        }

        public bool UpdateLapAndFinish(SectionData sectiondata, Side side, DateTime time)
        {
            var participant = sectiondata.GetParticipant(side);
            if (participant != null)
            {
                UpdateLap(participant, time);
                if (!ParticipantFinished(participant))
                    return false;
                _finishOrder.Add(participant);

                Console.Title = $"{participant.Name} just finished!! :)  test l={sectiondata.Left?.Name} r={sectiondata.Right?.Name}  => {side}";

                if (side == Side.Left)
                    sectiondata.Left = null;
                else
                    sectiondata.Right = null;
                return true;
            }
            return false;
        }


        public int GetSpeedFromCompetitor(IParticipant iParticipant) => Convert.ToInt32(Math.Ceiling(0.12 * (iParticipant.Equipment.Speed * 0.51) * iParticipant.Equipment.Performance + 15));
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

        private void CheckIfEquipmentsShouldBreakOrBeRepaired()
        {
            var now = DateTime.Now;
            if ((now - _lastSeenBroken).TotalSeconds < 3)
                return;

            _lastSeenBroken = now;

            foreach(var participant in Participants)
            {
                participant.Equipment.IsBroken = _random.Next(0, 100) > participant.Equipment.Quality;
            }
        }

        public SectionData GetSectionData(Section section)
        {

            if (!_positions.ContainsKey(section)) _positions.Add(section, new SectionData());
            return _positions[section];
        }
        
        public void RandomizeEquipment()
        {
            foreach (var participant in Participants)
            {
                participant.Equipment = new Car(_random.Next(40, 100), _random.Next(6, 50), _random.Next(6, 16));
            }
        }

        public bool CheckIfFinished() {
            return _positions.Values.FirstOrDefault(a => a.Left != null || a.Right != null) == null;
        } 
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
    }
}

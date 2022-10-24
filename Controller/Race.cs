using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;


namespace Controller
{
    public class Race
    {

        public enum Side
        {
            left,
            right
        }
        public Track Track;
        public List<IParticipant> Participants;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        private Random _random;
        private Dictionary<Section, SectionData> _positions;

        private readonly System.Timers.Timer _timer;
        private const int _timerInterval = 500;

        private const int _laps = 3;
        private Dictionary<IParticipant, int> _lapsDriven;

        private Dictionary<IParticipant, DateTime> _ParticipantLapTime;

        private List<IParticipant> _finishOrder;

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

            setDriverStartPosition(track, participants);
            RandomizeEquipment();
            initializeFirstLap();
        }

        public void setDriverStartPosition(Track track, List<IParticipant> participants)
        {
            Queue<IParticipant> participantsTemp = new Queue<IParticipant>(participants);

            foreach(Section section in track.Sections)
            {
                if(section.SectionType == Section.SectionTypes.StartGrid)
                {
                    SectionData? sectionData = getSectionData(section);

                    if (participantsTemp.Count == 0)
                        return;
                    sectionData!.Left = participantsTemp.Dequeue();
                    if (participantsTemp.Count == 0)
                        return;
                    sectionData!.Right = participantsTemp.Dequeue();
                }
            }
        }
        
        public void initializeFirstLap()
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

        public void updateLap(IParticipant participant, DateTime time)
        {
            _lapsDriven[participant]++;
            if (_lapsDriven[participant] <= 0) return;
            _ParticipantLapTime[participant] = time;
        }

        public bool participantFinished(IParticipant participant)
        {
            return _lapsDriven[participant] >= _laps;
        }

        public bool checkForFinishSection(Section section)
        {
            return section.SectionType == Section.SectionTypes.Finish;
        }

        public void moveParticipant(DateTime time)
        {
            LinkedListNode<Section> currentSectionNode = Track.Sections.Last;

            while (currentSectionNode != null)
            {
                moveSectionData(currentSectionNode.Value, currentSectionNode.Next != null ? currentSectionNode.Next.Value : Track.Sections.First?.Value, time);

                currentSectionNode = currentSectionNode.Previous;
            }
        }
        public void moveSingleParticipant(Section currentSection, Section nextSection, Side start, Side end, bool correctOtherSide, DateTime elapsedDateTime)
        {

        }
        public void moveSectionData(Section currentSection, Section nextSection, DateTime time)
        {
            _timer.Stop();

            foreach(IParticipant participant in Participants)
            {
                SectionData currentSectionData = getSectionData(currentSection);
                SectionData nextSectionData = getSectionData(nextSection);

                if (currentSectionData.Left != null && !currentSectionData.Left.Equipment.IsBroken)
                {
                    currentSectionData.DistanceLeft += getSpeedFromCompetitor(currentSectionData.Left);
                }

                if (currentSectionData.Right != null && !currentSectionData.Right.Equipment.IsBroken)
                {
                    currentSectionData.DistanceRight += getSpeedFromCompetitor(currentSectionData.Right);
                }

                if(currentSectionData.DistanceLeft >= 100 && currentSectionData.DistanceRight >= 100)
                {
                    int freePlace = checkIfPlace(nextSectionData);

                    if(freePlace == 0)// links en rechts is plek voor drivers om te bewegen
                    {
                        currentSectionData.DistanceLeft = 99;
                        currentSectionData.DistanceRight = 99;
                    }
                    else if (freePlace == 3)
                    {

                    }
                }
            }
        }

        public int checkIfPlace(SectionData track)
        {
            int placeHolder = 0;
            if(track.Left == null)
            {
                return placeHolder += 1;
            }
            if(track.Right == null)
            {
                return placeHolder += 2;
            }
            return placeHolder;
        }

        // andere waardes gebruiken~!
        public int getSpeedFromCompetitor(IParticipant iParticipant) => Convert.ToInt32(Math.Ceiling(0.12 * (iParticipant.Equipment.Speed * 0.51) * iParticipant.Equipment.Performance + 15));
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (CheckIfFinished())
            {
                EndTime = e.SignalTime;
                RaceFinished?.Invoke(this, new EventArgs());
            }
        }

        public SectionData getSectionData(Section section)
        {
            if(_positions.TryGetValue(section, out SectionData? data))
            {
                return data;
            } else
            {
                SectionData nieuweSD = new SectionData();
                _positions.Add(section, nieuweSD);
                return nieuweSD;
            }
        }

        public void RandomizeEquipment()
        {
            foreach (var participant in Participants)
            {
                participant.Equipment.Quality = _random.Next(12, 21);
                participant.Equipment.Performance = _random.Next(6, 15);
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

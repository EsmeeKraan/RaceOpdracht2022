using Controller;
using RaceOpdrachtHuiswerk;

Data.Initialize();
Console.Clear();
Console.SetWindowSize(250, 80);
Console.SetBufferSize(Console.WindowLeft + Console.WindowWidth, Console.WindowTop + Console.WindowHeight);

Data.CompetitionEnded += (_, _) =>
{
    Console.Clear();
    Console.SetCursorPosition(0, 0);
    Console.Title = "Competition Over!";
    foreach (var participant in Data.Competition.Participants)
    {
        Console.WriteLine(participant.Name + " won!");
    }
};

Data.NextRaceEvent += VisualisatieStatic.OnNextRaceEvent;
Data.NextRace();

for (; ; )
{
    Thread.Sleep(100);
}
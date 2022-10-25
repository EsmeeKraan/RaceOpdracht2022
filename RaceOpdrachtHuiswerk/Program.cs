using Controller;
using Model;
using RaceOpdrachtHuiswerk;
using System;

Data.Initialize();
Data.NextRaceEvent += VisualisatieStatic.OnNextRaceEvent;
Data.NextRace();

/*Console.WriteLine(Data.CurrentRace.Track.Name);
Data.NextRace();
Console.WriteLine(Data.CurrentRace.Track.Name);
Data.NextRace();
Console.WriteLine(Data.CurrentRace.Track.Name);*/

for (; ; ) 
{
    Thread.Sleep(100);
}
using Controller;
using Model;
using RaceOpdrachtHuiswerk;
using System;

Data.Initialize();
Data.NextRace();

/*Console.WriteLine(Data.CurrentRace.Track.Name);
Data.NextRace();
Console.WriteLine(Data.CurrentRace.Track.Name);
Data.NextRace();
Console.WriteLine(Data.CurrentRace.Track.Name);*/

VisualisatieStatic.Initialize();
VisualisatieStatic.DrawTrack(Data.CurrentRace);



for (; ; ) 
{
    Thread.Sleep(100);
}
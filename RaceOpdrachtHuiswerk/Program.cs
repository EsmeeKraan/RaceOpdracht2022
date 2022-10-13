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
VisualisatieStatic.DrawTrack(new Track("Rainbow Road", new Section.SectionTypes[1]));



for (; ; ) 
{
    Thread.Sleep(100);
}
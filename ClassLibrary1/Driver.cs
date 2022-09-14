using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Driver : IParticipant
    {
        public string Name => throw new NotImplementedException();

        public int Points => throw new NotImplementedException();

        public IEquipment Equipment => throw new NotImplementedException();

        public IParticipant.TeamColors TeamColor => throw new NotImplementedException();
        public Driver(string Name, int Points)
        {
            Name = "Driver";
            Points = 0;
        }
    }
}

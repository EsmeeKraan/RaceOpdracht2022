using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IEquipment
    {
        int Quality {  get; }  
        int Performance { get; }   
        int Speed { get; }
        bool isBroken { get; }
    }
}

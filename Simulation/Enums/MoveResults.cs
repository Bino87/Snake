using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Enums
{
    public enum MoveResults
    {
        Ok,
        OutOfBounds,
        SelfCollision,
        EatFood
    }
}

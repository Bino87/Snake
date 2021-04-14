using System;
using Simulation.Enums;

namespace Simulation.Core
{
    public class VisionData : IComparable
    {
        public int X1 { get; }
        public int X2 { get; }
        public int Y1 { get; }
        public int Y2 { get; }

        public VisionData(VisionCollisionType visionCollisionType, int x1, int x2, int y1, int y2)
        {
            VisionCollisionType = visionCollisionType;
            X1 = x1;
            X2 = x2;
            Y1 = y1;
            Y2 = y2;
        }

        public VisionCollisionType VisionCollisionType { get; }
        public int CompareTo(object? obj)
        {
            if (obj is VisionData vd)
            {
                return VisionCollisionType.CompareTo(vd.VisionCollisionType);
            }


            return 0;
        }
    }
}

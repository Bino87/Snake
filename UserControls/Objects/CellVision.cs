using System.Diagnostics;
using Simulation.Core;
using Simulation.Enums;
using Simulation.Interfaces;
using UserControls.Core.Base;

namespace UserControls.Objects
{
    [DebuggerDisplay("X1:{X1} X1:{X2} Y1:{Y1} Y1:{Y2}")]
    public class CellVision : Observable, IMapItem
    {

        public MapItemType ItemType => MapItemType.Vision;
        public VisionCollisionType VisionCollisionType { get; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }

        public CellVision(VisionData visionData, double size, double cellSize)
        {
            VisionCollisionType = visionData.VisionCollisionType;
            X1 = size * visionData.X1 + cellSize;
            Y1 = size * visionData.Y1 + cellSize;
            X2 = size * visionData.X2 + cellSize;
            Y2 = size * visionData.Y2 + cellSize;
        }
    }
}
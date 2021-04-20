using System.Collections.Generic;
using System.Collections.ObjectModel;
using Simulation.Core;
using Simulation.Interfaces;
using UserControls.Constants;
using UserControls.Core.Base;
using UserControls.Objects;
using UserControls.Objects.SnakeDisplay;

namespace UserControls.Models
{
    public class SnakeMapViewModel : Observable
    {

        public ObservableCollection<IMapItem> MapItems { get; set; }

        public SnakeMapViewModel()
        {
            MapItems = new ObservableCollection<IMapItem>();
        }

        public void CreateVisionLines(IList<VisionData> visionData, int mapSize)
        {
            if (visionData is null)
                return;

            double cellSize = Cons.cMapSize / mapSize;   // size of each tile;
            double midPoint = cellSize / 2;                     //midpoint offset.

            for (int i = 0; i < visionData.Count; i++)
            {
                CellVision cv = new(visionData[i], cellSize, midPoint);
                MapItems.Add(cv);
            }
        }

        private void SetCells(IList<CellUpdateData> cellUpdateList, int mapSize)
        {
            double size = Cons.cMapSize / mapSize;

            for (int i = 0; i < cellUpdateList.Count; i++)
            {
                CellUpdateData item = cellUpdateList[i];
                MapCell mc = new(item.X * size, item.Y * size, size, size, item.CellType);

                MapItems.Add(mc);
            }
        }

        public void SetCells(IList<CellUpdateData> cellUpdateList, IList<VisionData> visionData, int mapSize)
        {
            MapItems.Clear();
            SetCells(cellUpdateList, mapSize);
            CreateVisionLines(visionData, mapSize);
        }
    }
}

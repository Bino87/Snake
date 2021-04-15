using System.Collections.Generic;
using System.Collections.ObjectModel;
using Simulation.Core;
using Simulation.Enums;
using Simulation.Interfaces;
using UserControls.Constants;
using UserControls.Core.Base;
using UserControls.Objects;
using UserControls.Objects.SnakeDisplay;

namespace UserControls.Models
{
    public class SnakeMapViewModel : Observable
    {
        public readonly int _numberOfTiles;

        public ObservableCollection<IMapItem> MapItems { get; set; }

        public SnakeMapViewModel(int numTiles)
        {
            _numberOfTiles = numTiles;
            MapItems = new ObservableCollection<IMapItem>();

        }
        
        public void CreateVisionLines(VisionData[] visionData)
        {
            if(visionData is null)
                return;

            double cellSize = Cons.cMapSize / _numberOfTiles;   // size of each tile;
            double midPoint = cellSize /2 ;                     //midpoint offset.

            for(int i = 0; i < visionData.Length; i++)
            {
                CellVision cv = new(visionData[i], cellSize, midPoint);
                MapItems.Add(cv);
            }
        }

        private void SetCells(List<(int X, int Y, MapCellType Status)> cellUpdateList)    
        {
            double size = Cons.cMapSize / _numberOfTiles ;

            for(int i = 0; i < cellUpdateList.Count; i++)
            {
                (int x, int y, MapCellType cellType) = cellUpdateList[i];
                MapCell mc = new(x * size , y * size, size, size, cellType);

                MapItems.Add(mc);
            }

        }

        public void SetCells(List<(int X, int Y, MapCellType Status)> cellUpdateList, VisionData[] visionData)
        {
            MapItems.Clear();
            SetCells(cellUpdateList);
            CreateVisionLines(visionData);
        }
    }
}

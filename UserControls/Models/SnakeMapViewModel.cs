using System.Collections.ObjectModel;
using Simulation.Core;
using Simulation.Interfaces;
using UserControls.Constants;
using UserControls.Core.Base;

namespace UserControls.Models
{
    public class SnakeMapViewModel : Observable
    {
        public readonly int _numberOfTiles;

        public ObservableCollection<IMapItem> MapItems { get; set; }
        public IMapCell[,] RectArr { get; }

        public SnakeMapViewModel(int numTiles)
        {
            _numberOfTiles = numTiles;
            RectArr = new IMapCell[numTiles, numTiles];
            CreateMap();
        }


        public void CreateVisionLines(VisionData[] visionData)
        {
            ClearVision();

            double cellSize = Cons.cMapSize / _numberOfTiles;   // size of each tile;
            double midPoint = cellSize /2 ;                     //midpoint offset.

            for(int i = 0; i < visionData.Length; i++)
            {
                CellVision cv = new(visionData[i], cellSize, midPoint);
                MapItems.Add(cv);
            }
            
        }

        private void ClearVision()
        {
            int count = _numberOfTiles * _numberOfTiles;

            while (MapItems.Count > count)
            {
                MapItems.RemoveAt(count);
            }
        }

        private void CreateMap()
        {
            ObservableCollection<IMapItem> res = new();

            double size = Cons.cMapSize / _numberOfTiles ;
            for (int y = 0; y < _numberOfTiles; y++)
            {
                for (int x = 0; x < _numberOfTiles; x++)
                {
                    MapCell mc = new(x * size , y * size, size, size);
                    RectArr[x, y] = mc;

                    res.Add(mc);
                }
            }

            MapItems = res;
        }
    }
}

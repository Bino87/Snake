using System.Collections.ObjectModel;
using Simulation.Interfaces;
using UserControls.Constants;
using UserControls.Core.Base;

namespace UserControls.Models
{
    public class SnakeMapViewModel : Observable
    {
        public readonly int _numberOfTiles;

        public ObservableCollection<MapCell> RectList { get; set; }
        public IMapCell[,] RectArr { get; }

        public SnakeMapViewModel(int numTiles)
        {
            _numberOfTiles = numTiles;
            RectArr = new MapCell[numTiles, numTiles];
            CreateMap();
        }

        public void CreateMap()
        {
            CreateMap(_numberOfTiles);
        }

        private void  CreateMap(int numTiles)
        {
            ObservableCollection<MapCell> res = new();

            double size = Cons.cMapSize / numTiles - 1;
            for (int y = 0; y < numTiles; y++)
            {
                for (int x = 0; x < numTiles; x++)
                {
                    MapCell mc = new(x * size + x, y * size + y, size, size);
                    RectArr[x, y] = mc;

                    res.Add(mc);
                }
            }

            RectList = res;
        }
    }
}

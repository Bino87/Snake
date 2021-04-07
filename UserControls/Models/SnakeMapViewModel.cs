using System.Collections.ObjectModel;
using UserControls.Constants;
using UserControls.Core.Base;

namespace UserControls.Models
{
    public class SnakeMapViewModel : Observable
    {
        public readonly int _numberOfTiles;

        public ObservableCollection<MapCell> Rects { get; set; }
        public SnakeMapViewModel(int numTiles)
        {
            _numberOfTiles = numTiles;
            CreateMap();
        }

        public void CreateMap()
        {
            Rects = CreateMap(_numberOfTiles);
        }

        private static ObservableCollection<MapCell> CreateMap(int numTiles)
        {
            ObservableCollection<MapCell> res = new();

            double size = Cons.cMapSize / numTiles - 1;
            for (int y = 0; y < numTiles; y++)
            {
                for (int x = 0; x < numTiles; x++)
                {

                    MapCell mc = new(x * size + x, y * size + y, size, size);

                    res.Add(mc);
                }
            }

            return res;
        }
    }
}

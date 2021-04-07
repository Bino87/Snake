using System.Collections.ObjectModel;
using UserControls.Constants;
using UserControls.Core.Base;

namespace UserControls.Models
{
    public class SnakeMapViewModel : Observable
    {
        public ObservableCollection<MapCell> Rects { get; set; }
        public SnakeMapViewModel(int numTiles)
        {
            Rects = CreateMap(numTiles);
        }

        private static ObservableCollection<MapCell> CreateMap(int numTiles)
        {
            ObservableCollection<MapCell> res = new();

            double SIZE = Cons.cMapSize / numTiles - 1;

            for (int x = 0; x < numTiles; x++)
            {
                for (int y = 0; y < numTiles; y++)
                {
                    MapCell mc = new(x * SIZE + x, y * SIZE + y, SIZE, SIZE);

                    res.Add(mc);
                }
            }

            return res;
        }
    }
}

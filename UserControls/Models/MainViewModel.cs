using System.Collections.ObjectModel;
using UserControls.Constants;
using UserControls.Core.Base;

namespace UserControls.Models
{
    public class MainViewModel : Observable
    {
        public ObservableCollection<MapCell> Rects { get; set; }

        public MainViewModel()
        {
            Rects = CreateMap();

        }

        static ObservableCollection<MapCell> CreateMap()
        {
            ObservableCollection<MapCell> res = new();

            const double SIZE = Cons.cMapSize / Cons.numTiles - 1;

            for (int x = 0; x < Cons.numTiles; x++)
            {
                for (int y = 0; y < Cons.numTiles; y++)
                {
                    MapCell mc = new(x * SIZE + x, y * SIZE + y, SIZE, SIZE);

                    res.Add(mc);
                }
            }

            return res;
        }
    }
}

using UserControls.Constants;
using UserControls.Core.Base;

namespace UserControls.Models
{
    public class MainViewModel : Observable
    {
        public SnakeMapViewModel SnakeMapViewModel {get; set; }
        public MainViewModel()
        {
            SnakeMapViewModel = new SnakeMapViewModel(Cons.cNumberOfTiles);
        }
    }
}

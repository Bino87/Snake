using UserControls.Constants;
using UserControls.Core.Base;

namespace UserControls.Models
{
    public class MainViewModel : Observable
    {
        public SnakeMapViewModel SnakeMapViewModel {get; set; }
        private int _delay;

        public int Deley
        {
            get => _delay;
            set => SetField(ref _delay, value);

        }

        public MainViewModel()
        {
            SnakeMapViewModel = new SnakeMapViewModel(Cons.cNumberOfTiles);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserControls.Core.Base;
using UserControls.Enums;

namespace UserControls.Models.NeuralNetDisplay
{
    public abstract class PrimitiveShape : Observable
    {
        private double _x;
        private double _y;
        public abstract ShapeType ShapeType { get; }
        public  double X
        {
            get => _x;
            set => SetField(ref _x, value);
        }

        public  double Y
        {
            get => _y;
            set => SetField(ref _y, value);
        }

        protected PrimitiveShape(double x, double y)
        {
            _x = x;
            _y = y;
        }

    }
}

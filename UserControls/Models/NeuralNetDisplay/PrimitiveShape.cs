﻿using System;
using UserControls.Core.Base;
using UserControls.Enums;

namespace UserControls.Models.NeuralNetDisplay
{
    public class PrimitiveShapeValueProvider : Observable
    {
        private double _value ;

        public PrimitiveShapeValueProvider() // indexes? :O
        {
            

        }
            
        public double Value
        {
            get => _value;
            set => SetField(ref _value, value);
        }
    }

    public abstract class PrimitiveShape : Observable
    {
        private double _x;
        private double _y;
        public abstract ShapeType ShapeType { get; }
        public PrimitiveShapeValueProvider ValueProvider { get; }

        public double X
        {
            get => _x;
            set => SetField(ref _x, value);
        }

        public double Y
        {
            get => _y;
            set => SetField(ref _y, value);
        }

        protected PrimitiveShape(PrimitiveShapeValueProvider valueProvider, double x, double y)
        {
            ValueProvider = valueProvider;
            _x = x;
            _y = y;
        }
    }
}